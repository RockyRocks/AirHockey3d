using UnityEngine;
using System.Collections;

public class PuckResetHack : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit(Collider other)
	{
		//Hackey way to reset puck if it gone out of the table
		//This is not final we have to fix the PUCK going out of table bug.
		if(other.name.Contains ("Puck"))
		{
			Debug.LogError ("PUCK gone out of table, \nVelocity: " + other.rigidbody.velocity.magnitude);
			Debug.LogError ("\nForce: " + other.rigidbody.velocity + "resetting it (it's a HACK, dont forget to Fix!!)");
			
			Puck.Get ().ResetPuck ();
		}
	}
}
