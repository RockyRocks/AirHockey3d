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
	public Vector3 PlanarVelocity;
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
		if (springJoint != null)
		{
			springJoint.spring = 0f;
			springJoint.damper = 0f;
			springJoint.connectedBody = null;
		}
		var body = GetComponent<Rigidbody>();
		if (body != null)
			AirHockeyPhysics.ConfigurePaddle(body);
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
			Vector3 from = springJoint != null ? springJoint.transform.position : transform.position;
			var Lock_Player = Vector3.Lerp (from, Mouse_World, Time.smoothDeltaTime*smooth);
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
			desiredPosition = Lock_Player;
			hasDesiredPosition = true;
        }
	}

	private Vector3 desiredPosition;
	private bool hasDesiredPosition;
	
	// for physics update!

	void FixedUpdate()
	{
		var body = GetComponent<Rigidbody>();
		if (hasDesiredPosition && body != null)
		{
			const float maxMalletSpeed = 12f;
			Vector3 next = Vector3.MoveTowards(body.position, desiredPosition, maxMalletSpeed * Time.fixedDeltaTime);
			next.y = body.position.y;
			PlanarVelocity = (next - body.position) / Time.fixedDeltaTime;
			PlanarVelocity.y = 0f;
			body.MovePosition(next);
		}
		IsPuckHit = false;
		speed = PlanarVelocity.magnitude;
		this.Last_Position = body != null ? body.position : transform.position;
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
				
			//col.collider.GetComponent<Rigidbody>().velocity = col.relativeVelocity / 1.1f;
		}
	}
	public void Reset()
	{
		// have to change this form of code to reflect AIplayer too
		this.transform.position = InitialPosition;
		this.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
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




