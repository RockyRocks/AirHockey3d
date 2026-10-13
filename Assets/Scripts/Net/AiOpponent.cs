using System;

public enum AiDifficulty
{
	Easy = 0,
	Medium = 1,
	Hard = 2
}

public struct AiTuning
{
	public float ReactionSeconds;
	public float AimError;
	public float SpeedScale;
	public float Lead;

	public static AiTuning For(AiDifficulty difficulty)
	{
		if (difficulty == AiDifficulty.Hard)
			return new AiTuning { ReactionSeconds = 0.05f, AimError = 0.04f, SpeedScale = 1f, Lead = 0.9f };
		if (difficulty == AiDifficulty.Medium)
			return new AiTuning { ReactionSeconds = 0.16f, AimError = 0.18f, SpeedScale = 0.72f, Lead = 0.55f };
		return new AiTuning { ReactionSeconds = 0.34f, AimError = 0.5f, SpeedScale = 0.45f, Lead = 0.2f };
	}
}

/// <summary>
/// Chooses a mallet target from what it has had time to see. It never
/// integrates the puck. The server applies the target with the same step
/// used for a remote player.
/// </summary>
public sealed class AiOpponent
{
	public const float DefenseZ = 2.4f;
	const int Capacity = 16;

	readonly AiTuning tuning;
	readonly Sample[] samples = new Sample[Capacity];
	int count;
	int sequence;

	public AiOpponent(AiTuning tuning)
	{
		this.tuning = tuning;
	}

	public float SpeedScale
	{
		get { return tuning.SpeedScale; }
	}

	public void Observe(float time, float puckX, float puckZ, float puckVx, float puckVz)
	{
		samples[count % Capacity] = new Sample
		{
			Time = time,
			X = puckX,
			Z = puckZ,
			Vx = puckVx,
			Vz = puckVz
		};
		count++;
	}

	public PaddleCommand Decide(float time, int seed)
	{
		float x = 0f;
		float z = DefenseZ;
		Sample seen;
		if (TrySeen(time, out seen))
		{
			x = seen.X;
			if (seen.Vz > 0.2f)
			{
				float travel = (DefenseZ - seen.Z) / seen.Vz;
				if (travel < 0f)
					travel = 0f;
				if (travel > 1.2f)
					travel = 1.2f;
				x = seen.X + seen.Vx * travel * tuning.Lead;
			}
			else
			{
				x = seen.X * tuning.Lead;
			}
			x += tuning.AimError * Error(seed, sequence);
		}

		TableRules.ClampPaddle(1, ref x, ref z);
		sequence++;
		return new PaddleCommand
		{
			Sequence = sequence,
			X = x,
			Z = z,
			SpeedScale = tuning.SpeedScale
		};
	}

	bool TrySeen(float time, out Sample seen)
	{
		seen = default(Sample);
		float cutoff = time - tuning.ReactionSeconds;
		bool found = false;
		int available = count < Capacity ? count : Capacity;
		for (int i = 0; i < available; i++)
		{
			Sample sample = samples[i];
			if (sample.Time > cutoff)
				continue;
			if (!found || sample.Time >= seen.Time)
			{
				seen = sample;
				found = true;
			}
		}
		return found;
	}

	static float Error(int seed, int step)
	{
		int mixed = seed * 73856093 ^ step * 19349663;
		if (mixed < 0)
			mixed = -mixed;
		return ((mixed % 100) / 50f) - 1f;
	}

	struct Sample
	{
		public float Time;
		public float X;
		public float Z;
		public float Vx;
		public float Vz;
	}
}
