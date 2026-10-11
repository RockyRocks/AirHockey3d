using System;

public struct PaddleCommand
{
	public int Sequence;
	public float X;
	public float Z;
}

public struct NetSnapshot
{
	public int Tick;
	public float Time;
	public float PuckX;
	public float PuckZ;
	public float PuckVx;
	public float PuckVz;
	public float BottomX;
	public float BottomZ;
	public float TopX;
	public float TopZ;
	public int BottomScore;
	public int TopScore;
}

/// <summary>
/// Keeps the newest paddle sample and drops anything older. A late or
/// duplicated packet therefore cannot rewind a mallet.
/// </summary>
public sealed class CommandIngress
{
	public int LastSequence = -1;
	public float X;
	public float Z;
	public bool Has;

	public bool Push(PaddleCommand command)
	{
		if (command.Sequence < LastSequence)
			return false;
		LastSequence = command.Sequence;
		X = command.X;
		Z = command.Z;
		Has = true;
		return true;
	}
}

public static class TableRules
{
	public const float MinX = -2.7f;
	public const float MaxX = 2.7f;
	public const float BottomMinZ = -4.1f;
	public const float BottomMaxZ = -0.5f;
	public const float TopMinZ = 0.5f;
	public const float TopMaxZ = 3.7f;
	public const float MaxPaddleSpeed = 12f;

	public static void ClampPaddle(int slot, ref float x, ref float z)
	{
		if (x < MinX)
			x = MinX;
		if (x > MaxX)
			x = MaxX;
		float minZ = slot == 0 ? BottomMinZ : TopMinZ;
		float maxZ = slot == 0 ? BottomMaxZ : TopMaxZ;
		if (z < minZ)
			z = minZ;
		if (z > maxZ)
			z = maxZ;
	}

	public static void StepTowards(float fromX, float fromZ, float toX, float toZ, float deltaTime, out float x, out float z)
	{
		float dx = toX - fromX;
		float dz = toZ - fromZ;
		float distance = (float)Math.Sqrt(dx * dx + dz * dz);
		float budget = MaxPaddleSpeed * deltaTime;
		if (distance <= budget || distance <= 0.00001f)
		{
			x = toX;
			z = toZ;
			return;
		}
		float scale = budget / distance;
		x = fromX + dx * scale;
		z = fromZ + dz * scale;
	}
}

/// <summary>
/// Plays server snapshots a short time behind the newest tick so a late
/// packet is filled by interpolation instead of a visible pop.
/// </summary>
public sealed class SnapshotBuffer
{
	readonly NetSnapshot[] data = new NetSnapshot[32];
	int count;

	public int Count
	{
		get { return count; }
	}

	public void Push(NetSnapshot snapshot)
	{
		if (count > 0 && snapshot.Tick <= data[count - 1].Tick)
			return;
		if (count == data.Length)
		{
			Array.Copy(data, 1, data, 0, data.Length - 1);
			count--;
		}
		data[count++] = snapshot;
	}

	public bool Sample(float renderTime, float maxExtrapolate, out NetSnapshot sample)
	{
		sample = default(NetSnapshot);
		if (count == 0)
			return false;
		if (count == 1 || renderTime <= data[0].Time)
		{
			sample = data[0];
			return true;
		}

		NetSnapshot last = data[count - 1];
		if (renderTime >= last.Time)
		{
			float extra = renderTime - last.Time;
			if (extra > maxExtrapolate)
				extra = maxExtrapolate;
			if (extra < 0f)
				extra = 0f;
			last.PuckX += last.PuckVx * extra;
			last.PuckZ += last.PuckVz * extra;
			sample = last;
			return true;
		}

		for (int i = 1; i < count; i++)
		{
			if (data[i].Time < renderTime)
				continue;
			sample = Lerp(data[i - 1], data[i], renderTime);
			return true;
		}

		sample = last;
		return true;
	}

	static NetSnapshot Lerp(NetSnapshot a, NetSnapshot b, float renderTime)
	{
		float span = b.Time - a.Time;
		float t = span <= 0.0001f ? 1f : (renderTime - a.Time) / span;
		NetSnapshot result = b;
		result.Time = renderTime;
		result.PuckX = a.PuckX + (b.PuckX - a.PuckX) * t;
		result.PuckZ = a.PuckZ + (b.PuckZ - a.PuckZ) * t;
		result.PuckVx = a.PuckVx + (b.PuckVx - a.PuckVx) * t;
		result.PuckVz = a.PuckVz + (b.PuckVz - a.PuckVz) * t;
		result.BottomX = a.BottomX + (b.BottomX - a.BottomX) * t;
		result.BottomZ = a.BottomZ + (b.BottomZ - a.BottomZ) * t;
		result.TopX = a.TopX + (b.TopX - a.TopX) * t;
		result.TopZ = a.TopZ + (b.TopZ - a.TopZ) * t;
		return result;
	}
}
