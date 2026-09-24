using System;
using System.Numerics;

internal static class Program
{
	static int failures;

	static void Main()
	{
		HeadOnRailReversesAndKeepsMostSpeed();
		GlancingRailKeepsTheOutgoingAngle();
		RailDoesNotPullAPuckAlreadyLeaving();
		MalletStrikeSendsThePuckAwayFasterThanTheSwing();
		SlowMalletStillSeparatesThePuck();
		AirDragStaysOnTheTableAndBarelySlowsThePuck();
		PlanarSpeedCapKeepsDirection();

		if (failures > 0)
		{
			Console.Error.WriteLine(failures + " physics checks failed.");
			Environment.Exit(1);
		}
		Console.WriteLine("Puck physics checks passed.");
	}

	static void HeadOnRailReversesAndKeepsMostSpeed()
	{
		Vector3 incoming = new Vector3(-6f, 1.5f, 0f);
		Vector3 normal = new Vector3(1f, 0f, 0f);
		Vector3 outgoing = PuckCollisionMath.ResolveRail(incoming, normal, PuckCollisionMath.RailRestitution, PuckCollisionMath.RailFriction);
		Near(outgoing.X, 6f * PuckCollisionMath.RailRestitution, 0.02f, "head-on normal speed");
		Near(outgoing.Y, 0f, 0.001f, "head-on stays on the table");
		Near(outgoing.Z, 0f, 0.001f, "head-on has no side speed");
	}

	static void GlancingRailKeepsTheOutgoingAngle()
	{
		Vector3 incoming = new Vector3(-4f, 0f, 4f);
		Vector3 normal = new Vector3(1f, 0f, 0f);
		Vector3 outgoing = PuckCollisionMath.ResolveRail(incoming, normal, 1f, 0f);
		Near(outgoing.X, 4f, 0.02f, "glance normal");
		Near(outgoing.Z, 4f, 0.02f, "glance tangent");
		float incomingAngle = MathF.Atan2(4f, 4f);
		float outgoingAngle = MathF.Atan2(MathF.Abs(outgoing.Z), MathF.Abs(outgoing.X));
		Near(outgoingAngle, incomingAngle, 0.08f, "angle of incidence matches reflection when friction is zero");
	}

	static void RailDoesNotPullAPuckAlreadyLeaving()
	{
		Vector3 incoming = new Vector3(3f, 0f, 1f);
		Vector3 normal = new Vector3(1f, 0f, 0f);
		Vector3 outgoing = PuckCollisionMath.ResolveRail(incoming, normal, PuckCollisionMath.RailRestitution, PuckCollisionMath.RailFriction);
		Near(outgoing.X, 3f, 0.001f, "separating puck is not pulled back");
		Near(outgoing.Z, 1f, 0.001f, "separating tangent is unchanged");
	}

	static void MalletStrikeSendsThePuckAwayFasterThanTheSwing()
	{
		Vector3 puck = new Vector3(0f, 2f, -1f);
		Vector3 mallet = new Vector3(0f, 0f, 4f);
		Vector3 normal = new Vector3(0f, 0f, 1f);
		Vector3 outgoing = PuckCollisionMath.ResolveMalletHit(puck, mallet, normal, PuckCollisionMath.MalletRestitution, 2f);
		float expectedNormal = (1f + PuckCollisionMath.MalletRestitution) * 4f - PuckCollisionMath.MalletRestitution * (-1f);
		Near(outgoing.Z, expectedNormal, 0.02f, "mallet normal impulse");
		Check(outgoing.Z > mallet.Z, "puck leaves faster than the mallet along the hit");
		Near(outgoing.Y, 0f, 0.001f, "mallet hit stays on the table");
	}

	static void SlowMalletStillSeparatesThePuck()
	{
		Vector3 puck = new Vector3(0f, 0f, -3f);
		Vector3 mallet = new Vector3(0.1f, 0f, 0.1f);
		Vector3 normal = Vector3.Normalize(new Vector3(0f, 0.2f, 1f));
		Vector3 outgoing = PuckCollisionMath.ResolveMalletHit(puck, mallet, normal, PuckCollisionMath.MalletRestitution, 2f);
		float approach = Vector3.Dot(outgoing, new Vector3(normal.X, 0f, normal.Z));
		Check(approach >= 2f, "slow swing still pushes the puck off the mallet");
	}

	static void AirDragStaysOnTheTableAndBarelySlowsThePuck()
	{
		Vector3 velocity = new Vector3(3f, 5f, 4f);
		Vector3 dragged = PuckCollisionMath.ApplyAirDrag(velocity, PuckCollisionMath.AirDragPerSecond, 0.02f);
		Near(dragged.Y, 0f, 0.001f, "drag removes vertical speed");
		float before = new Vector3(3f, 0f, 4f).Length();
		float after = dragged.Length();
		Check(after > before * 0.99f && after < before, "air cushion only trims a little speed");
	}

	static void PlanarSpeedCapKeepsDirection()
	{
		Vector3 velocity = new Vector3(6f, 9f, 8f);
		Vector3 capped = PuckCollisionMath.ClampPlanarSpeed(velocity, 8f);
		Near(capped.Length(), 8f, 0.02f, "speed cap");
		Near(capped.Y, 0f, 0.001f, "cap stays planar");
		Near(capped.X / capped.Z, 6f / 8f, 0.02f, "cap keeps the shot direction");
	}

	static void Near(float actual, float expected, float tolerance, string label)
	{
		if (MathF.Abs(actual - expected) > tolerance)
			Fail(label + " expected " + expected + " got " + actual);
	}

	static void Check(bool condition, string label)
	{
		if (!condition)
			Fail(label);
	}

	static void Fail(string label)
	{
		failures++;
		Console.Error.WriteLine("FAIL " + label);
	}
}
