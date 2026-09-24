using System.Numerics;

/// <summary>
/// Planar air-hockey collision response.
/// The puck rides an air cushion, so motion stays on the table plane.
/// Rails return most of the normal speed (hard plastic on a rail) and only
/// a little tangential speed (light contact friction). A mallet is treated as
/// much heavier than the puck, so the puck leaves the face with the mallet's
/// approach speed plus a restitution term.
/// </summary>
public static class PuckCollisionMath
{
	public const float RailRestitution = 0.82f;
	public const float RailFriction = 0.10f;
	public const float MalletRestitution = 0.50f;
	public const float AirDragPerSecond = 0.06f;

	public static Vector3 ResolveRail(Vector3 incomingVelocity, Vector3 contactNormal, float restitution, float friction)
	{
		Vector3 normal = HorizontalUnit(contactNormal);
		if (normal.LengthSquared() < 1e-8f)
			return Planar(incomingVelocity);

		Vector3 incoming = Planar(incomingVelocity);
		float approach = Vector3.Dot(incoming, normal);
		// Normal points out of the rail, toward the puck. A negative approach
		// means the puck is still moving into the rail.
		if (approach >= 0f)
			return incoming;

		Vector3 normalVelocity = approach * normal;
		Vector3 tangentVelocity = incoming - normalVelocity;
		float tangentSpeed = tangentVelocity.Length();
		if (tangentSpeed > 1e-5f)
		{
			float retained = System.Math.Max(0f, tangentSpeed - friction * System.Math.Abs(approach));
			tangentVelocity *= retained / tangentSpeed;
		}

		return tangentVelocity - restitution * normalVelocity;
	}

	public static Vector3 ResolveMalletHit(Vector3 puckVelocity, Vector3 malletVelocity, Vector3 contactNormal, float restitution, float minimumStrikeSpeed)
	{
		Vector3 normal = HorizontalUnit(contactNormal);
		if (normal.LengthSquared() < 1e-8f)
			return Planar(puckVelocity);

		Vector3 puck = Planar(puckVelocity);
		float puckNormal = Vector3.Dot(puck, normal);
		float malletNormal = Vector3.Dot(Planar(malletVelocity), normal);
		if (malletNormal < minimumStrikeSpeed)
			malletNormal = minimumStrikeSpeed;

		// Infinite-mass mallet: v' = v_mallet + e * (v_mallet - v_puck) along the face normal.
		float outgoingNormal = (1f + restitution) * malletNormal - restitution * puckNormal;
		if (outgoingNormal < malletNormal)
			outgoingNormal = malletNormal;

		Vector3 tangential = puck - puckNormal * normal;
		return tangential + outgoingNormal * normal;
	}

	public static Vector3 ApplyAirDrag(Vector3 velocity, float dragPerSecond, float deltaTime)
	{
		float keep = 1f - dragPerSecond * deltaTime;
		if (keep < 0f)
			keep = 0f;
		Vector3 planar = Planar(velocity);
		return planar * keep;
	}

	public static Vector3 ClampPlanarSpeed(Vector3 velocity, float maxSpeed)
	{
		Vector3 planar = Planar(velocity);
		float speed = planar.Length();
		if (maxSpeed > 0f && speed > maxSpeed)
			return planar * (maxSpeed / speed);
		return planar;
	}

	public static Vector3 Planar(Vector3 velocity)
	{
		return new Vector3(velocity.X, 0f, velocity.Z);
	}

	static Vector3 HorizontalUnit(Vector3 normal)
	{
		Vector3 planar = Planar(normal);
		float length = planar.Length();
		if (length < 1e-8f)
			return Vector3.Zero;
		return planar / length;
	}
}
