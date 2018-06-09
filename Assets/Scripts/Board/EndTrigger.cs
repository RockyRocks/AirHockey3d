using UnityEngine;
using System.Collections;

public class EndTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.name.Contains ("pin"))
		{
			other.transform.position = new Vector3(100,100,100);
			other.transform.rotation = Quaternion.Euler (Vector3.zero);
			other.rigidbody.isKinematic = true;
			//other.gameObject.GetComponent<Pin>().IsFalen = true;
		}
	}
}
