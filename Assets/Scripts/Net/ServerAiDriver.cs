using UnityEngine;

/// <summary>
/// Turns the AI decision into a paddle command on the host. The match then
/// moves that mallet with the same speed limit as a networked opponent.
/// </summary>
public class ServerAiDriver : MonoBehaviour
{
	public static AiDifficulty Difficulty = AiDifficulty.Medium;

	AiOpponent opponent;
	AiDifficulty builtFor;
	int seed = 1;
	Transform puck;

	void Awake()
	{
		AuthoritativeMatch.TopSlotIsRemote = true;
		Rebuild();
	}

	void FixedUpdate()
	{
		if (!AuthoritativeMatch.AiControlsTop || AuthoritativeMatch.Instance == null)
			return;
		if (AuthoritativeMatch.Instance.Role == MatchRole.Client)
			return;
		if (builtFor != Difficulty)
			Rebuild();
		if (puck == null)
		{
			GameObject puckObject = GameObject.FindGameObjectWithTag("Puck");
			if (puckObject == null)
				return;
			puck = puckObject.transform;
		}

		Rigidbody body = puck.GetComponent<Rigidbody>();
		Vector3 velocity = body != null ? body.linearVelocity : Vector3.zero;
		float time = Time.fixedTime;
		opponent.Observe(time, puck.position.x, puck.position.z, velocity.x, velocity.z);
		AuthoritativeMatch.SubmitAiPaddle(opponent.Decide(time, seed));
	}

	void Rebuild()
	{
		builtFor = Difficulty;
		opponent = new AiOpponent(AiTuning.For(builtFor));
		AuthoritativeMatch.ClearTopPaddle();
		if (AuthoritativeMatch.Instance != null)
			AuthoritativeMatch.SubmitAiPaddle(new PaddleCommand { Sequence = 0, X = 0f, Z = AiOpponent.DefenseZ, SpeedScale = opponent.SpeedScale });
	}
}
