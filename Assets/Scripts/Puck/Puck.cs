using UnityEngine;
using System.Collections;

public class Puck : MonoBehaviour
{
	private static Puck Instance;
	public AudioClip puckReleasedClip;

	public static Puck Get()
	{
		return Instance;
	}

	private Transform m_Transform;
	//private Rigidbody m_RigidBody;
	//private Vector3	m_DefaultPosition;
	private float m_ForwardSpeed;
	private float m_TimeInEnd;
    public float m_minSpeed=500f;
    public float m_maxSpeed=2000f;
	
	private Vector3	_basePos; //give base position to reset Puck
	private bool _isPuckLaunched;
	public float puckVelocity; //Will help to stop the puck.

    public float m_Velocity; //Speed with which Puck should move after launch.
	//private float _launchTime;

	public GameObject sliderQuad;

	public bool IsPuckLaunched {
		get {
			return _isPuckLaunched;
		}
		set {
			_isPuckLaunched = value;
		}
	}

	public bool isCollided = false;
	//float timeElapsed = 0f;

	void Awake()
	{
		Instance = this;
		_basePos = transform.position;
		m_Transform = gameObject.transform;
		//m_RigidBody = gameObject.GetComponent<Rigidbody> ();
		//m_DefaultPosition = gameObject.transform.position;
        //Physics.IgnoreLayerCollision(8, 10);
		//Properties.S_CantakeInputs = true;d
	}
	//private Vector3 fc;
	void FixedUpdate()
	{
		puckVelocity = rigidbody.velocity.magnitude;
//		if((_puckVelocity <= 0.00f && GameInput.Get().m_LineCrossed == true))
//		{
//		}
	}
    //Vector3 newForce =Vector3.zero;
	public void LaunchPuck(Vector3 force)
	{
        var LaunchForceZ = Mathf.Clamp(force.z * m_Velocity, -m_maxSpeed, -m_minSpeed);
        force = new Vector3(force.x * m_Velocity, force.y, LaunchForceZ);
		rigidbody.AddForce (force , ForceMode.Force);
        //newForce = force;
		IsPuckLaunched = true;
        try
        {
			audio.volume = Properties.sfxVolume;
            audio.PlayOneShot(puckReleasedClip);
        }
        catch (System.Exception)
        {
            throw;
        }
		
		//Set the camera to follow Puck.
		Camera.main.GetComponent<FollowCam>().canFollow = true;
		Camera.main.GetComponent<FollowCam>().zoomOut = false;
	}



	void Update(){
        if (this.rigidbody.position.z > 1.4f 
		            && this.rigidbody.position.z < 1.90f )
        {
            Properties.S_CantakeInputs = true;
			if(GameInput.Get()!=null)
            GameInput.Get().m_LineCrossed = false;
          //  Debug.Log("I am checking for puck position here" + this.rigidbody.position.z);
        }
        else
        {
          //  Debug.Log("Stopping the inputs since puck is not in the input zone" + this.rigidbody.position.z);
            Properties.S_CantakeInputs = false;
			if(GameInput.Get()!=null)
            GameInput.Get().m_LineCrossed = true;
        }
	}

    //void OnTriggerEnter(Collider hit)
    //{
    //    //if (hit.gameObject.name.Contains ("End")) {
    //    //    Properties.S_CantakeInputs = false;
    //    //}
    //    //else
    //    if (hit.gameObject.name.Contains ("Line")) {
    //        Properties.S_CantakeInputs = false;
    //        GameInput.Get ().m_LineCrossed = true;
    //    }
    //}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Pin") {
			if(!isCollided){
				var script = sliderQuad.GetComponent<SliderScript>();
				script.RePosition();
				script.Pause();
			}
			isCollided = true;
		}
//		if (collision.gameObject.tag == "EndBlock") {
//			var hitPoint=collision.contacts[0];
//			var newNormal= new Vector3(hitPoint.normal.x,0,-hitPoint.normal.z);
//			this.rigidbody.velocity=Vector3.Reflect(this.rigidbody.velocity,newNormal);
//			Debug.Log("Trying to reflect the velocity: "+this.rigidbody.velocity);
//				}
	}
	
	public void MoveTo(Vector3 position)
	{

	}

	public void ResetPuck()
	{ 
		rigidbody.velocity = Vector3.zero; //Zero the velocity
		transform.position = _basePos;
		GameInput.Get().m_LineCrossed = false;
		IsPuckLaunched = false;
        Properties.S_CantakeInputs = true;
        //_launchTime = 0f;

		Camera.main.GetComponent<FollowCam> ().zoomOut = true;
		Camera.main.GetComponent<FollowCam>().canFollow = false;

		var script = sliderQuad.GetComponent<SliderScript>();
		StartCoroutine(script.Reset());
	}
	
	public void MoveLeft()
	{
//		if (!m_TakeInputs)
//			return;
//
//		m_Transform.Translate (-(m_Transform.right) * Time.deltaTime);
	}

	public void MoveRight()
	{
//		if (!m_TakeInputs)
//			return;
//
//		m_Transform.Translate ((m_Transform.right) * Time.deltaTime);
	}

	public void TurnLeft()
	{
//		if (!m_TakeInputs)
//			return;
//
//		m_RotationAngle -= (m_TurnSpeed * Time.deltaTime);
//
//		if (m_RotationAngle > -20.0f)
//		{
//			m_Transform.Rotate(new Vector3(0.0f, -m_TurnSpeed * Time.deltaTime, 0.0f));
//			Camera.main.gameObject.transform.RotateAround (m_Transform.position, Vector3.up, -m_TurnSpeed * Time.deltaTime);
//		}
	}
	
	public void TurnRight()
	{
//		if (!m_TakeInputs)
//			return;
//
//		m_RotationAngle += (m_TurnSpeed * Time.deltaTime);
//
//		if (m_RotationAngle < 20.0f)
//		{
//			m_Transform.Rotate(new Vector3(0.0f, m_TurnSpeed * Time.deltaTime, 0.0f));
//			Camera.main.gameObject.transform.RotateAround (m_Transform.position, Vector3.up, m_TurnSpeed * Time.deltaTime);
//		}
	}

	public void Launch(float powerZ, float powerX)
	{
//		if (m_TakeInputs)
//		{
//			m_ForwardSpeed = (powerZ * 40.0f);
//			m_SideSpeed	   = (powerX * 40.0f);
//			Camera.main.GetComponent<FollowCam>().canFollow = true;
//
//			m_TakeInputs = false;
//		}
	}

	public void RegisterHit(string hitName)
	{
//		switch (hitName)
//		{
//		case "Left":
//
//			m_SideSpeed = -m_SideSpeed;
//
//			break;
//
//		case "Right":
//			
//			m_SideSpeed = -m_SideSpeed;
//			
//			break;
//
//		case "Top":
//			
//			m_ForwardSpeed = -m_ForwardSpeed;
//			
//			break;
//		}
	}

	public Transform GetTransform()
	{
		return m_Transform;
	}
}
