using UnityEngine;
using System.Collections;

public class GameInput : MonoBehaviour
{
	private static GameInput Instance;
	public static GameInput Get()
	{
		return Instance;
	}


	public GameObject[]	m_HUDItems;
	private Vector3 EasySlide;

	public bool	m_LineCrossed;
	private bool m_IsPuckSelected;
    public float Depth = 10f;

	private Vector3	m_MouseStartPos;

	//private Camera				m_Camera;

	private float m_ForceZ;
	private float m_ForceX;

	private bool _canTakeInputs;
	private Ray	ray;
	private RaycastHit hit;
	public float CorrectionforMouse=0.05f;

    //public bool CanTakeInputs {
    //    get {
    //        return _canTakeInputs;
    //    }
    //    set {
    //        _canTakeInputs = value;
    //    }
    //}

	void Awake()
	{
		Instance = this;
	}

	void Start	()
	{
		//m_Camera = GameObject.Find ("UICamera").GetComponent<Camera> ();
		m_LineCrossed = false;
		m_IsPuckSelected = false;
       // Properties.S_CantakeInputs = true;
	}

	void Update () 
	{
		if (Properties.isPopUpShown)
			return;

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (!Properties.S_CantakeInputs) 
		{
			if (m_IsPuckSelected && m_LineCrossed && easyslidehack) 
			{
				Debug.Log ("releasing the puck");

				Vector3 m_MouseEndPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Depth);
				Vector3 force = (m_MouseEndPos - EasySlide);
				var Zforce = force.magnitude/Time.deltaTime;
				force = new Vector3(-force.x,0,-Zforce);
				Debug.Log(force);
				if(force.magnitude > 0.3f)
						Puck.Get ().LaunchPuck (force);
				else 
				{
					Puck.Get ().rigidbody.velocity = Vector3.zero;
					Puck.Get().ResetPuck();
				}
				m_IsPuckSelected = false;
				easyslidehack=false;
			}
			return;
		}
        this.CheckInputs();
		if(Puck.Get()!=null)
		if((Puck.Get().transform.position.x>0.15f || Puck.Get().transform.position.x<-0.15f) 
		   && m_LineCrossed==false
           &&Input.GetMouseButtonUp(0)
		   &&Puck.Get().transform.position.z<1.90f)
		{
			StartCoroutine("Follow_camera");
		}
	}
	void FixedUpdate()
	{
        
	}

	//private Vector3 Previous_position;
	void CheckInputs()
	{
		if (Input.GetMouseButton (0)) 
		{
			//None of the UI Elements are selected.
			//So, check if the Puck is selected.
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) 
			{
				Debug.DrawLine (Puck.Get ().rigidbody.position, hit.point, Color.cyan);
				if (hit.collider.name.Contains ("Puck")) 
				{
					m_MouseStartPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Depth);
					if(!easyslidehack)
					{
						EasySlide= m_MouseStartPos;
						easyslidehack=true;
					}
					m_IsPuckSelected = true;
					var lock_puck = new Vector3 (hit.point.x,
			                            Puck.Get ().transform.position.y,
					                             hit.point.z - CorrectionforMouse);
					lock_puck.x = Mathf.Clamp (lock_puck.x, -0.37f, 0.37f);
					Puck.Get ().rigidbody.position = lock_puck;
                    //Previous_position = lock_puck;
				}
			}
		} 
		else if (Input.GetMouseButtonUp (0)) {
						this.Pucklaunch ();
				} 

	}
	bool easyslidehack=false;
	private void Pucklaunch()
	{
		if(m_IsPuckSelected == true)
		{
			Vector3 m_MouseEndPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Depth);
			Vector3 force = (m_MouseEndPos - m_MouseStartPos);
			var Zforce = force.magnitude/Time.deltaTime;
			force = new Vector3(-force.x,0,-Zforce);
			//Debug.Log(force);
			if(force.magnitude > 0.3f)
				Puck.Get ().LaunchPuck (force);
			else 
				Puck.Get ().rigidbody.velocity = Vector3.zero;
			m_IsPuckSelected = false;
			easyslidehack=false;
		}
	}

	IEnumerator Follow_camera()
	{
        Camera.main.GetComponent<FollowCam>().SliderFollow = true;
        //Camera.main.GetComponent<FollowCam>().zoomOut = false;
        yield return new WaitForSeconds(1.4f);
	}
	void CheckTouchInputs()
	{
	}
}
