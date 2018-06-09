using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {
	public Transform		m_target;
	public float 			m_Speed;
	Vector3 newPosition = new Vector3 (0f, 1.27f, 2.93f);

	public bool canFollow;
	public bool zoomOut;
    public bool SliderFollow;

	// Use this for initialization
	void Start () {
		ResetPosition ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (canFollow) {
			transform.position = Vector3.Lerp (transform.position, m_target.position + new Vector3 (0, 0.4f, 1.48f), Time.deltaTime * m_Speed);
			Camera.main.GetComponent<FollowCam>().SliderFollow = false;
			SliderFollow=false;
		}
		if (zoomOut) {
			transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * m_Speed); 
		}
        if (SliderFollow) {
            transform.position = Vector3.Lerp(transform.position, newPosition+new Vector3(m_target.position.x,0,0), Time.deltaTime * m_Speed); 
        }
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "DeletePinTrigger") {
			canFollow = false;
		}
	}

	public void ResetPosition(){
		transform.position = newPosition;
	}
}
