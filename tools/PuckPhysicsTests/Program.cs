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
		StalePaddleCommandIsIgnored();
		PaddleCannotCrossTheCenterLine();
		PaddleCannotTeleportThroughThePuck();
		SnapshotPlaybackFillsAGapAndDoesNotRunAhead();

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

	static void StalePaddleCommandIsIgnored()
	{
		CommandIngress ingress = new CommandIngress();
		Check(ingress.Push(new PaddleCommand { Sequence = 2, X = 1f, Z = -2f }), "first command is accepted");
		Check(!ingress.Push(new PaddleCommand { Sequence = 1, X = 9f, Z = 9f }), "older command is dropped");
		Near(ingress.X, 1f, 0.001f, "stale command does not move the mallet");
		Check(ingress.Push(new PaddleCommand { Sequence = 2, X = 0.5f, Z = -1f }), "same tick refresh is kept");
		Near(ingress.X, 0.5f, 0.001f, "newest sample at this sequence wins");
	}

	static void PaddleCannotCrossTheCenterLine()
	{
		float x = 4f;
		float z = 2f;
		TableRules.ClampPaddle(0, ref x, ref z);
		Near(x, TableRules.MaxX, 0.001f, "bottom mallet stays on the table width");
		Near(z, TableRules.BottomMaxZ, 0.001f, "bottom mallet stays in its own half");
		x = -4f;
		z = -2f;
		TableRules.ClampPaddle(1, ref x, ref z);
		Near(z, TableRules.TopMinZ, 0.001f, "top mallet stays in its own half");
	}

	static void PaddleCannotTeleportThroughThePuck()
	{
		float x;
		float z;
		TableRules.StepTowards(0f, -4f, 0f, -0.5f, 0.02f, out x, out z);
		float moved = MathF.Abs(z - (-4f));
		Near(moved, TableRules.MaxPaddleSpeed * 0.02f, 0.001f, "mallet travel is limited to one server tick");
		Check(z < -0.5f, "a late burst cannot jump the mallet onto the puck");
	}

	static void SnapshotPlaybackFillsAGapAndDoesNotRunAhead()
	{
		SnapshotBuffer buffer = new SnapshotBuffer();
		buffer.Push(new NetSnapshot { Tick = 1, Time = 0f, PuckX = 0f, PuckZ = 0f, PuckVx = 2f, PuckVz = 0f });
		buffer.Push(new NetSnapshot { Tick = 1, Time = 1f, PuckX = 9f, PuckZ = 9f });
		Check(buffer.Count == 1, "duplicate tick is ignored");
		buffer.Push(new NetSnapshot { Tick = 3, Time = 0.1f, PuckX = 2f, PuckZ = 0f, PuckVx = 2f, PuckVz = 0f });
		NetSnapshot mid;
		Check(buffer.Sample(0.05f, 0.02f, out mid), "gap sample exists");
		Near(mid.PuckX, 1f, 0.001f, "playback sits halfway between server snapshots");
		NetSnapshot ahead;
		Check(buffer.Sample(1f, 0.02f, out ahead), "future sample exists");
		Near(ahead.PuckX, 2f + 2f * 0.02f, 0.001f, "playback extrapolates at most one short tick");
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
