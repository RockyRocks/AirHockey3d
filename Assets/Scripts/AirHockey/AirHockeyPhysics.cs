using UnityEngine;

/// <summary>
/// Air-cushion table setup and planar collision response.
/// A real puck barely touches the table, so friction is tiny and it keeps sliding.
/// Rails give most of the speed back. A mallet is heavy and driven by the player,
/// so the puck takes the mallet's velocity instead of shoving the mallet away.
/// </summary>
[DefaultExecutionOrder(-200)]
public class AirHockeyPhysics : MonoBehaviour
{
	public const float RailRestitution = 0.9f;
	public const float RailFriction = 0.06f;
	public const float PaddleRestitution = 0.5f;
	public const float PaddleSweep = 0.55f;
	public const float MaxPlanarSpeed = 14f;
	public const float PuckMass = 0.05f;
	public const float PuckLinearDamping = 0.16f;
	public const float PuckAngularDamping = 0.45f;

	public static PhysicsMaterial PuckMaterial { get; private set; }
	public static PhysicsMaterial RailMaterial { get; private set; }
	public static PhysicsMaterial PaddleMaterial { get; private set; }
	public static PhysicsMaterial TableMaterial { get; private set; }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static void Bootstrap()
	{
		if (Object.FindAnyObjectByType<AirHockeyPhysics>() != null)
			return;
		var host = new GameObject("AirHockeyPhysics");
		host.AddComponent<AirHockeyPhysics>();
	}

	void Awake()
	{
		BuildMaterials();
		ConfigureScene();
	}

	public static void BuildMaterials()
	{
		if (PuckMaterial != null)
			return;

		PuckMaterial = Make("PuckSurface", 0.02f, 0.02f, 0.35f, PhysicsMaterialCombine.Minimum, PhysicsMaterialCombine.Average);
		RailMaterial = Make("RailSurface", 0.04f, 0.02f, RailRestitution, PhysicsMaterialCombine.Minimum, PhysicsMaterialCombine.Maximum);
		PaddleMaterial = Make("PaddleSurface", 0.08f, 0.04f, PaddleRestitution, PhysicsMaterialCombine.Minimum, PhysicsMaterialCombine.Average);
		TableMaterial = Make("TableSurface", 0.015f, 0.01f, 0f, PhysicsMaterialCombine.Minimum, PhysicsMaterialCombine.Minimum);
	}

	static PhysicsMaterial Make(string name, float staticFriction, float dynamicFriction, float bounciness, PhysicsMaterialCombine frictionCombine, PhysicsMaterialCombine bounceCombine)
	{
		var material = new PhysicsMaterial(name)
		{
			staticFriction = staticFriction,
			dynamicFriction = dynamicFriction,
			bounciness = bounciness,
			frictionCombine = frictionCombine,
			bounceCombine = bounceCombine
		};
		return material;
	}

	public static void ConfigureScene()
	{
		BuildMaterials();
		int wallLayer = LayerMask.NameToLayer("Walls");

		var puck = GameObject.FindGameObjectWithTag("Puck");
		if (puck != null)
			ConfigurePuck(puck);

		foreach (var body in Object.FindObjectsByType<Rigidbody>(FindObjectsSortMode.None))
		{
			if (body.CompareTag("Player") || body.CompareTag("AIPlayer"))
				ConfigurePaddle(body);
		}

		foreach (var collider in Object.FindObjectsByType<Collider>(FindObjectsSortMode.None))
		{
			if (collider.isTrigger)
				continue;
			if (wallLayer >= 0 && collider.gameObject.layer == wallLayer)
			{
				collider.sharedMaterial = RailMaterial;
				continue;
			}
			string name = collider.gameObject.name;
			if (name.IndexOf("Wall", System.StringComparison.OrdinalIgnoreCase) >= 0)
				collider.sharedMaterial = RailMaterial;
			else if (collider.CompareTag("Board") || name.IndexOf("table", System.StringComparison.OrdinalIgnoreCase) >= 0 || name.IndexOf("plane", System.StringComparison.OrdinalIgnoreCase) >= 0)
				collider.sharedMaterial = TableMaterial;
		}
	}

	public static void ConfigurePuck(GameObject puck)
	{
		BuildMaterials();
		var body = puck.GetComponent<Rigidbody>();
		if (body == null)
			return;
		body.mass = PuckMass;
		body.useGravity = false;
		body.linearDamping = PuckLinearDamping;
		body.angularDamping = PuckAngularDamping;
		body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		body.interpolation = RigidbodyInterpolation.Interpolate;
		body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		var collider = puck.GetComponent<Collider>();
		if (collider != null)
			collider.sharedMaterial = PuckMaterial;
		if (puck.GetComponent<DontGoThrough>() == null)
		{
			var guard = puck.AddComponent<DontGoThrough>();
			int wallLayer = LayerMask.NameToLayer("Walls");
			int playerLayer = LayerMask.NameToLayer("Players");
			int mask = 0;
			if (wallLayer >= 0)
				mask |= 1 << wallLayer;
			if (playerLayer >= 0)
				mask |= 1 << playerLayer;
			guard.layerMask = mask;
			guard.skinWidth = 0.05f;
		}
	}

	public static void ConfigurePaddle(Rigidbody body)
	{
		BuildMaterials();
		body.useGravity = false;
		body.isKinematic = true;
		body.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		body.interpolation = RigidbodyInterpolation.Interpolate;
		var collider = body.GetComponent<Collider>();
		if (collider != null)
			collider.sharedMaterial = PaddleMaterial;
	}

	/// <summary>
	/// Reflect a planar velocity off a rail. The contact normal points out of the rail,
	/// toward the puck. Restitution below 1 matches a lively but not perfectly elastic board.
	/// </summary>
	public static Vector3 ResolveRail(Vector3 velocity, Vector3 normal)
	{
		velocity.y = 0f;
		normal.y = 0f;
		if (normal.sqrMagnitude < 1e-8f)
			return ClampPlanar(velocity);
		normal.Normalize();

		float approach = Vector3.Dot(velocity, normal);
		if (approach < 0f)
			velocity -= (1f + RailRestitution) * approach * normal;

		Vector3 tangent = Vector3.ProjectOnPlane(velocity, normal);
		Vector3 normalPart = Vector3.Project(velocity, normal);
		velocity = normalPart + tangent * (1f - RailFriction);
		return ClampPlanar(velocity);
	}

	/// <summary>
	/// Infinite-mass mallet hit. Relative velocity along the contact normal is reflected
	/// with restitution, and a sideways mallet sweep carries the puck with it.
	/// </summary>
	public static Vector3 ResolvePaddle(Vector3 puckVelocity, Vector3 paddleVelocity, Vector3 normal)
	{
		puckVelocity.y = 0f;
		paddleVelocity.y = 0f;
		normal.y = 0f;
		if (normal.sqrMagnitude < 1e-8f)
			normal = paddleVelocity.sqrMagnitude > 1e-6f ? paddleVelocity.normalized : Vector3.forward;
		else
			normal.Normalize();

		Vector3 relative = puckVelocity - paddleVelocity;
		float approach = Vector3.Dot(relative, normal);
		if (approach < 0f)
			puckVelocity -= (1f + PaddleRestitution) * approach * normal;

		Vector3 sweep = Vector3.ProjectOnPlane(paddleVelocity, normal);
		puckVelocity += sweep * PaddleSweep;

		if (paddleVelocity.sqrMagnitude > 0.04f && puckVelocity.sqrMagnitude < 1f)
			puckVelocity += normal * Mathf.Max(1.5f, paddleVelocity.magnitude);

		return ClampPlanar(puckVelocity);
	}

	public static Vector3 ClampPlanar(Vector3 velocity)
	{
		velocity.y = 0f;
		float speed = velocity.magnitude;
		if (speed > MaxPlanarSpeed)
			velocity *= MaxPlanarSpeed / speed;
		if (speed < 0.05f)
			return Vector3.zero;
		return velocity;
	}
}
