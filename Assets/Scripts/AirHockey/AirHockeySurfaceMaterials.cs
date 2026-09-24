using UnityEngine;

/// <summary>
/// Builds the surface materials used by the puck, rails, table and mallets.
/// Created in code so the binary game scene still gets real bounce values
/// when the imported physic-material assets are not assigned.
/// </summary>
public static class AirHockeySurfaceMaterials
{
	static PhysicsMaterial puck;
	static PhysicsMaterial rail;
	static PhysicsMaterial table;
	static PhysicsMaterial mallet;

	public static PhysicsMaterial Puck
	{
		get
		{
			if (puck == null)
			{
				puck = new PhysicsMaterial("AirHockeyPuck");
				puck.bounciness = PuckCollisionMath.RailRestitution;
				puck.dynamicFriction = 0.02f;
				puck.staticFriction = 0.02f;
				puck.bounceCombine = PhysicsMaterialCombine.Maximum;
				puck.frictionCombine = PhysicsMaterialCombine.Minimum;
			}
			return puck;
		}
	}

	public static PhysicsMaterial Rail
	{
		get
		{
			if (rail == null)
			{
				rail = new PhysicsMaterial("AirHockeyRail");
				rail.bounciness = PuckCollisionMath.RailRestitution;
				rail.dynamicFriction = 0.04f;
				rail.staticFriction = 0.04f;
				rail.bounceCombine = PhysicsMaterialCombine.Maximum;
				rail.frictionCombine = PhysicsMaterialCombine.Minimum;
			}
			return rail;
		}
	}

	public static PhysicsMaterial Table
	{
		get
		{
			if (table == null)
			{
				table = new PhysicsMaterial("AirHockeyTable");
				table.bounciness = 0.02f;
				table.dynamicFriction = 0.015f;
				table.staticFriction = 0.02f;
				table.bounceCombine = PhysicsMaterialCombine.Minimum;
				table.frictionCombine = PhysicsMaterialCombine.Minimum;
			}
			return table;
		}
	}

	public static PhysicsMaterial Mallet
	{
		get
		{
			if (mallet == null)
			{
				mallet = new PhysicsMaterial("AirHockeyMallet");
				mallet.bounciness = PuckCollisionMath.MalletRestitution;
				mallet.dynamicFriction = 0.08f;
				mallet.staticFriction = 0.1f;
				mallet.bounceCombine = PhysicsMaterialCombine.Average;
				mallet.frictionCombine = PhysicsMaterialCombine.Average;
			}
			return mallet;
		}
	}

	public static void ApplyToScene()
	{
		int wallLayer = LayerMask.NameToLayer("Walls");
		Collider[] colliders = Object.FindObjectsByType<Collider>(FindObjectsSortMode.None);
		for (int i = 0; i < colliders.Length; i++)
		{
			Collider collider = colliders[i];
			if (collider == null || collider.isTrigger)
				continue;

			string name = collider.gameObject.name;
			string tag = collider.gameObject.tag;
			if (tag == "Puck")
				collider.material = Puck;
			else if (tag == "Player" || tag == "AIPlayer")
				collider.material = Mallet;
			else if (collider.gameObject.layer == wallLayer || NameLooksLikeRail(name))
				collider.material = Rail;
			else if (NameLooksLikeTable(name))
				collider.material = Table;
		}
	}

	static bool NameLooksLikeRail(string name)
	{
		return name.IndexOf("Wall", System.StringComparison.OrdinalIgnoreCase) >= 0
			|| name.IndexOf("Rail", System.StringComparison.OrdinalIgnoreCase) >= 0
			|| name.IndexOf("Board", System.StringComparison.OrdinalIgnoreCase) >= 0;
	}

	static bool NameLooksLikeTable(string name)
	{
		return name.IndexOf("Plane", System.StringComparison.OrdinalIgnoreCase) >= 0
			|| name.IndexOf("Table", System.StringComparison.OrdinalIgnoreCase) >= 0
			|| name.IndexOf("Surface", System.StringComparison.OrdinalIgnoreCase) >= 0;
	}
}
