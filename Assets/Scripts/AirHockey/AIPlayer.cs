using UnityEngine;
using System.Collections;
using System.IO;
public class AIPlayer : PlayerController {


	private Vector3 PuckPosition;
	private Vector3 AiPlayerPosition;
	private Vector3 PuckVelocity;
	private Vector3 PuckInitialVelocity;
	private float PlayerVelocity;
	private  PuckBehaviour Puck;
	// Use this for initialization
	//  AI approach 
	//  1. Move to defensive position.
	//	2. Move to an attacking position.
	//	3. Strike the puck
	//  Have to move this code to a state manager for better display of what we are doing.
	void Start () {
	// AI Player Initial Spawning Position
		this.InitialPosition = new Vector3 (0f, 0f, 3.6f);
		this.transform.position = InitialPosition;
		Puck = GameObject.FindGameObjectWithTag ("Puck").GetComponent<PuckBehaviour> ();
	}
	// Update is called once per frame
	void Update () {
		// Lock the AI player to his boundaries 
		var pos = this.transform.position;
		pos.x = Mathf.Clamp(pos.x , -2.7f, 2.7f);
		pos.z = Mathf.Clamp(pos.z , 0.5f, 3.7f);
		this.transform.position = pos;

		AiPlayerPosition = this.transform.position;
		PuckPosition = Puck.transform.position;
		if (Puck.transform.position == Vector3.zero) {
			this.Reset();
				}
		//Physics.IgnoreLayerCollision(8,9);
	}
	void FixedUpdate (){
        if (Properties.SelectedChallenge == "Penalty Shot")
            return;

		if (IsPuckHit == true || this.rigidbody.velocity!=Vector3.zero) {
			rigidbody.velocity = Vector3.zero;
			IsPuckHit=false;
		}
		// Defense after Contact and delay between the Defense and attack about 2 to 8 sec.
        if (Puck.rigidbody.velocity.z > 4 || Puck.rigidbody.velocity.z < -3) {
            this.AI_Defense(AiPlayerPosition.x, AiPlayerPosition.z,
                             PuckPosition.x, PuckPosition.z, Puck.rigidbody.velocity.z);
            return;
        }
		// make decision
		this.AI_MakeDecision (AiPlayerPosition.x, AiPlayerPosition.z,
		                      PuckPosition.x, PuckPosition.z, Puck.rigidbody.velocity);
    
		PuckVelocity = Puck.rigidbody.velocity;


		speed = (this.transform.position - this.Last_Position).magnitude / Time.deltaTime;
		this.Last_Position = this.transform.position;

	}
	// Need to attach joints for predicting the pucks movement for attack 
	// or Defense.
	// Have to make decision based on the velocity and its current position inside the player half.
	// Here we are making a decision whether to hover or to attack.
	void AI_MakeDecision(float x, float z, float px,float pz,Vector3 Velocity){
		// damn this editor -> how to get the world position ? 
		// Have to add this since Puck in the corners is confusing the AI (Well a dumb AI really)
		//var CorneredPuck = (px < -2.5f && pz > 3.8f) || (px > 2.5f && pz > 3.8f);
        // if puck is in AI players corner
        if (pz > 0 && IsPuckHit == false || pz == 0) {
            this.AI_MoveToPuck(x, z, px, pz);
            IsPuckHit = true;
        }
		// need to alter the behavior of the puck from here			
		// if puck is in players corner...
		if ( pz < 0 ) {
		// Testing... basically this and Ai_defense is doing the same business 
		// alter it to mimic the aggressive behavior by Paddle while puck is in human players corner.
			this.AIHover();
		}
	}
	// hover around human players movement .
	//Huge potential to improve by using human movement to add a very realistic effect.{no Time}
	// Right it just hovers over the defense line with respect to mouse movement (Bad way of doing).
	void AIHover(){
		var HumanPosition = this.HumanPlayer;
		this.transform.position = Vector3.MoveTowards(this.transform.position,
		                                       new Vector3(HumanPosition.x, 0f, 3.7f),
		                                       Time.smoothDeltaTime);
	}

	// We do not need to check for collisions since these two are rigid bodies physics will handle these 
	// steps. so all i have to do is move the Ai_paddle towards the puck.
	void AI_MoveToPuck(float x, float z, float px,float pz){
		PlayerVelocity = Mathf.Abs(PuckVelocity.z+1) * Time.smoothDeltaTime;
		if (PuckVelocity.z == 0) {
            PlayerVelocity = Mathf.Abs(Random.Range(4, 7)/2) * Time.smoothDeltaTime;
			//Debug.Log("AI has become dumb because puck has no Z velocity ");
				}
        if (PuckVelocity.z < 0)
            PlayerVelocity = Mathf.Abs(PuckVelocity.z - 1) * Time.smoothDeltaTime;

		#region puckBehavior Joints
		#endregion
		// move Ai paddle to that new position;
		this.transform.position=Vector3.MoveTowards(this.transform.position,Puck.transform.position,PlayerVelocity);
	}

	// change the code weights are taking so much time 
	//make it simple hovering after and before the puck hit at a defense line just above the goal post.
	void AI_Defense(float x,float z,float px,float pz,float Velocity){
		Velocity=Mathf.Abs(Velocity) * Time.smoothDeltaTime;
		this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (px, 0f, 3.6f), Velocity);
	}

	void AI_EvadetheCorners(/*Vector3 PosA,Vector3 PosB*/){
		this.Puck.rigidbody.AddForce (-Puck.AiPlayerHitpoint *3f * Time.deltaTime);
//		float i = 0.0f;
//		float rate = 1.0f / 3.0f;
//		while (i < 1.0f) {
//			i += Time.deltaTime * rate;
//			this.transform.position = Vector3.Lerp(PosA, PosB, i); 
//		yield return null;
//				}
	}
}
