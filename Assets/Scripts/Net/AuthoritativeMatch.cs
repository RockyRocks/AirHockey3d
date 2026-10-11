using System.Collections;
using UnityEngine;

public enum MatchRole
{
	Host = 0,
	Client = 1
}

/// <summary>
/// The machine that owns the rigidbodies is the only simulation. Clients
/// send paddle samples and render interpolated snapshots, so two devices
/// cannot disagree about the puck. A cloud stream runs as Host: input is
/// applied on the next tick and the picture is the server state.
/// </summary>
public class AuthoritativeMatch : MonoBehaviour
{
	public static AuthoritativeMatch Instance;
	public static MatchRole RequestedRole = MatchRole.Host;
	public static bool BottomSlotIsRemote;
	public static bool TopSlotIsRemote;
	public static bool AiControlsTop = true;

	public MatchRole Role;
	public float interpolationDelay = 0.05f;
	public float maxExtrapolate = 0.02f;

	readonly CommandIngress bottomIngress = new CommandIngress();
	readonly CommandIngress topIngress = new CommandIngress();
	readonly SnapshotBuffer buffer = new SnapshotBuffer();

	int tick;
	int localSequence;
	public PaddleCommand LatestLocalCommand;
	public bool HasOutboundCommand;
	PuckBehaviour puck;
	Transform bottomPaddle;
	Transform topPaddle;
	float snapshotTime;

	public static void SubmitRemotePaddle(int slot, int sequence, float x, float z)
	{
		if (Instance == null)
			return;
		PaddleCommand command = new PaddleCommand { Sequence = sequence, X = x, Z = z };
		if (slot == 0)
			Instance.bottomIngress.Push(command);
		else
		{
			if (AiControlsTop)
				Instance.topIngress.Reset();
			AiControlsTop = false;
			Instance.topIngress.Push(command);
		}
	}

	public static void ClearTopPaddle()
	{
		if (Instance != null)
			Instance.topIngress.Reset();
	}

	public static void SubmitAiPaddle(PaddleCommand command)
	{
		if (Instance == null || !AiControlsTop)
			return;
		TopSlotIsRemote = true;
		Instance.topIngress.Push(command);
	}

	public void SubmitLocalPaddle(float x, float z)
	{
		localSequence++;
		LatestLocalCommand = new PaddleCommand { Sequence = localSequence, X = x, Z = z };
		HasOutboundCommand = true;
	}

	public bool TryLatestSnapshot(out NetSnapshot snapshot)
	{
		return buffer.Sample(snapshotTime, 0f, out snapshot);
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static void Install()
	{
		if (Instance != null)
			return;
		GameObject puckObject = GameObject.FindGameObjectWithTag("Puck");
		if (puckObject == null)
			return;
		GameObject host = new GameObject("AuthoritativeMatch");
		host.AddComponent<AuthoritativeMatch>();
	}

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		Role = RequestedRole;
		DontDestroyOnLoad(gameObject);
	}

	void OnDestroy()
	{
		if (Instance == this)
			Instance = null;
	}

	void Start()
	{
		BindTable();
		if (Role == MatchRole.Client)
			SetPuckPresentationOnly(true);
		else if (AiControlsTop)
			gameObject.AddComponent<ServerAiDriver>();
		StartCoroutine(CaptureAfterPhysics());
	}

	void FixedUpdate()
	{
		if (Role == MatchRole.Client)
			return;
		if (BottomSlotIsRemote && bottomPaddle != null)
			ApplyIngress(bottomPaddle, bottomIngress, 0);
		if (TopSlotIsRemote && topPaddle != null)
			ApplyIngress(topPaddle, topIngress, 1);
	}

	void Update()
	{
		if (Role != MatchRole.Client)
			return;
		float renderTime = snapshotTime - interpolationDelay;
		NetSnapshot sample;
		if (!buffer.Sample(renderTime, maxExtrapolate, out sample))
			return;
		if (puck != null)
		{
			Vector3 position = puck.transform.position;
			position.x = sample.PuckX;
			position.z = sample.PuckZ;
			position.y = 0f;
			puck.transform.position = position;
		}
		if (topPaddle != null)
			topPaddle.position = new Vector3(sample.TopX, topPaddle.position.y, sample.TopZ);
	}

	public void ReceiveSnapshot(NetSnapshot snapshot)
	{
		buffer.Push(snapshot);
		snapshotTime = snapshot.Time;
		Properties.HumanplayerGoals = snapshot.BottomScore;
		Properties.AIPlayerGoals = snapshot.TopScore;
	}

	IEnumerator CaptureAfterPhysics()
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		while (true)
		{
			yield return wait;
			if (Role == MatchRole.Client || puck == null)
				continue;
			BindTable();
			tick++;
			Rigidbody body = puck.GetComponent<Rigidbody>();
			Vector3 velocity = body != null ? body.linearVelocity : Vector3.zero;
			NetSnapshot snapshot = new NetSnapshot();
			snapshot.Tick = tick;
			snapshot.Time = Time.fixedTime;
			snapshot.PuckX = puck.transform.position.x;
			snapshot.PuckZ = puck.transform.position.z;
			snapshot.PuckVx = velocity.x;
			snapshot.PuckVz = velocity.z;
			if (bottomPaddle != null)
			{
				snapshot.BottomX = bottomPaddle.position.x;
				snapshot.BottomZ = bottomPaddle.position.z;
			}
			if (topPaddle != null)
			{
				snapshot.TopX = topPaddle.position.x;
				snapshot.TopZ = topPaddle.position.z;
			}
			snapshot.BottomScore = Properties.HumanplayerGoals;
			snapshot.TopScore = Properties.AIPlayerGoals;
			buffer.Push(snapshot);
			snapshotTime = snapshot.Time;
		}
	}

	void ApplyIngress(Transform paddle, CommandIngress ingress, int slot)
	{
		if (!ingress.Has)
			return;
		float x = ingress.X;
		float z = ingress.Z;
		TableRules.ClampPaddle(slot, ref x, ref z);
		float nextX;
		float nextZ;
		float speed = TableRules.MaxPaddleSpeed * (ingress.SpeedScale <= 0f ? 1f : ingress.SpeedScale);
		TableRules.StepTowards(paddle.position.x, paddle.position.z, x, z, Time.fixedDeltaTime, speed, out nextX, out nextZ);
		Vector3 position = paddle.position;
		position.x = nextX;
		position.y = 0f;
		position.z = nextZ;
		paddle.position = position;
	}

	void BindTable()
	{
		if (puck == null)
		{
			GameObject puckObject = GameObject.FindGameObjectWithTag("Puck");
			if (puckObject != null)
				puck = puckObject.GetComponent<PuckBehaviour>();
		}
		if (bottomPaddle == null)
		{
			GameObject player = GameObject.FindGameObjectWithTag("Player");
			if (player != null)
				bottomPaddle = player.transform;
		}
		if (topPaddle == null)
		{
			GameObject ai = GameObject.FindGameObjectWithTag("AIPlayer");
			if (ai != null)
				topPaddle = ai.transform;
		}
	}

	void SetPuckPresentationOnly(bool presentationOnly)
	{
		if (puck == null)
			return;
		Rigidbody body = puck.GetComponent<Rigidbody>();
		if (body == null)
			return;
		body.isKinematic = presentationOnly;
		body.linearVelocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;
		puck.enabled = !presentationOnly;
	}
}
