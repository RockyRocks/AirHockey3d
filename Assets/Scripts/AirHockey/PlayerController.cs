using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	[HideInInspector]
	public bool IsPuckHit=false;
	public float smooth=3f;
	//private Ray ray;
	//private RaycastHit rayHit;
	//private float depth=6;
	public Vector3 InitialPosition;
	private RaycastHit hit;
	public GameObject plane;
	public Vector3 AiPlayerHitPoint;
	public Vector3 Last_Position=Vector3.zero;
	public float speed=0;
	public SpringJoint springJoint;
	public float spring=1500;
	public float damper=3;
	public float distance=0.2f;
	// Use this for initialization.
	void Start()
	{
		this.InitialPosition = new Vector3 (0f, 0f, -4f);
		this.transform.position = InitialPosition;
		// new spring joint
		springJoint.spring = spring;
		springJoint.damper = damper;
		springJoint.maxDistance = distance;
		springJoint.connectedBody = hit.rigidbody;
        // ignoring collision with Walls and Players else it bounces while we touch the wall
        Physics.IgnoreLayerCollision(8, 9);
	}
	// Update is called once per frame
	void Update () {
		// Need to check for physics updates in FixedUpdate. Have to refer the API
		// this is to move the Player object using cursor/touch
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        if (Physics.Raycast(ray,out hit))
        {
            var Mouse_World = new Vector3 (hit.point.x, 0, hit.point.z);
			//springJoint.transform.position = hit.point;
			//springJoint.transform.position=Mouse_World;
			var Lock_Player = Vector3.Lerp (springJoint.transform.position, Mouse_World, Time.smoothDeltaTime*smooth);
//			if(Lock_Player.x<-2.65f || Lock_Player.x>2.65f)
//			{
//				
//			}   
//			if(Lock_Player.z<-4.05f || Lock_Player.z < -0.47f)
//			{
//
//			}
            Lock_Player.x = Mathf.Clamp (Lock_Player.x, -2.7f, 2.7f);
            Lock_Player.z=Mathf.Clamp(Lock_Player.z,-4.1f,-0.5f);
            //this.transform.position = Lock_Player;
			springJoint.transform.position=Lock_Player;
        }
	}
	
	// for physics update!

	void FixedUpdate()
	{
		if (IsPuckHit == true || rigidbody.velocity!=Vector3.zero ) 
		{
			rigidbody.velocity = Vector3.zero;
			IsPuckHit=false;
		}
		speed = (this.transform.rigidbody.position - this.Last_Position).magnitude / Time.deltaTime;
		this.Last_Position = this.transform.rigidbody.position;
	}
	
	// to Add force to puck and giving more momentum for the puddle to simulate close to real life situation..
	void OnCollisionEnter(Collision col)
	{
		if (col.collider.tag == "Puck") {
			if(IsPuckHit==false)
			{
				IsPuckHit=true;
			}
			//if (col.collider.tag == "Plane"||col.collider.tag=="Board")
				
			//col.collider.rigidbody.velocity = col.relativeVelocity / 1.1f;
		}
	}
	public void Reset()
	{
		// have to change this form of code to reflect AIplayer too
		this.transform.position = InitialPosition;
		this.rigidbody.velocity = Vector3.zero;
	}
	public Vector3 HumanPlayer
	{
		get
		{
			return GameObject.FindGameObjectWithTag("Player").transform.position;
		}
	}

	public float Speed
	{
		get
		{
			return speed;
		}
		set
		{
			speed =value;
		}
	}
}
