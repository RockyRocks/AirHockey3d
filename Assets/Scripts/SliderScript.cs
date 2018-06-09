using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SliderScript : MonoBehaviour {

	float speed = 0.2f;


	public GameObject trigger300;
	public GameObject trigger400;
	public GameObject trigger500;
	public GameObject trigger600;
	public GameObject trigger600_1;
	public GameObject trigger500_1;
	public GameObject trigger400_1;
	public GameObject trigger300_1;
	public GameObject trigger800;

	List<GameObject> TriggerColliders = new List<GameObject> ();

	private bool pause = false;

	Vector3 startPos = new Vector3 (0.276921f, 1.376187f,-0.718390f);
 	Vector3 direction = new Vector3 (-1,0,0);


	void Start(){
		TriggerColliders.Add (trigger300);
		TriggerColliders.Add (trigger400);
		TriggerColliders.Add (trigger500);
		TriggerColliders.Add (trigger600);
		TriggerColliders.Add (trigger600_1);
		TriggerColliders.Add (trigger500_1);
		TriggerColliders.Add (trigger400_1);
		TriggerColliders.Add (trigger300_1);
		TriggerColliders.Add (trigger800);

	}

	// Update is called once per frame
	void Update () {
		if(newCollider){
			var trigger = TriggerColliders.Find(item => item.tag == previousCollider);
			if(trigger!=null){
				foreach (Transform child in trigger.transform) 		
					child.gameObject.SetActive (false);
				newCollider = false;
			}
		}
	}

	void FixedUpdate(){
		if (!pause) {
			var pos = new Vector3 (transform.position.x + direction.x * speed * Time.deltaTime, transform.position.y + direction.y * Time.deltaTime, transform.position.z + direction.z * Time.deltaTime);
			transform.position = pos;
		}
	}

	string colliderName = "Trigger300";
	string previousCollider;
	bool newCollider = false;

	void OnTriggerEnter(Collider other){
	
		if (other.tag == "LeftSliderCollider") {
			direction = new Vector3(-1,0,0);
		} else
		if (other.tag == "RightSliderCollider") {
			transform.position = startPos;
		}else
		if (other.tag != colliderName && other.tag != "Untagged") {
			previousCollider = colliderName;
			foreach (Transform child in other.transform) 		
				child.gameObject.SetActive (true);
			colliderName = other.tag;
			newCollider = true;
		}
	}

	public IEnumerator Reset(){
		yield return new WaitForSeconds (1.8f);
		transform.position = startPos;
		pause = false;
	}

	public void Pause(){
		pause = true;
	}

	public int FindCollisionCollider(){

		int value = 0;

		if(transform.rigidbody.collider.bounds.Intersects(trigger300.collider.bounds)||(transform.rigidbody.collider.bounds.Intersects(trigger300_1.collider.bounds))){
			value = 300;
		}else
			if(transform.rigidbody.collider.bounds.Intersects(trigger400.collider.bounds)||((transform.rigidbody.collider.bounds.Intersects(trigger400_1.collider.bounds)))){
				value = 400;
			}else
				if(transform.rigidbody.collider.bounds.Intersects(trigger500.collider.bounds)||(transform.rigidbody.collider.bounds.Intersects(trigger500_1.collider.bounds))){
					value = 500;
				}else
					if(transform.rigidbody.collider.bounds.Intersects(trigger600.collider.bounds) || (transform.rigidbody.collider.bounds.Intersects(trigger600_1.collider.bounds))){
						value = 600;
					}else
						if(transform.rigidbody.collider.bounds.Intersects(trigger800.collider.bounds)){
							value = 800;
						}

		return value;
	}

	public void RePosition(){

		if(transform.rigidbody.collider.bounds.Intersects(trigger300.collider.bounds)){
			transform.position = trigger300.transform.position;
		}else
		if(transform.rigidbody.collider.bounds.Intersects(trigger400.collider.bounds)){
			transform.position = trigger400.transform.position;
		}else
		if(transform.rigidbody.collider.bounds.Intersects(trigger500.collider.bounds)){
			transform.position = trigger500.transform.position;
		}else
		if(transform.rigidbody.collider.bounds.Intersects(trigger600.collider.bounds)){
			transform.position = trigger600.transform.position;
		}else
		if(transform.rigidbody.collider.bounds.Intersects(trigger800.collider.bounds)){
			transform.position = trigger800.transform.position;
		}else
		if(transform.rigidbody.collider.bounds.Intersects(trigger600_1.collider.bounds)){
			transform.position = trigger300_1.transform.position;
		}
		else
		if(transform.rigidbody.collider.bounds.Intersects(trigger500_1.collider.bounds)){
			transform.position = trigger400_1.transform.position;
		}
		else
		if(transform.rigidbody.collider.bounds.Intersects(trigger400_1.collider.bounds)){
			transform.position = trigger500_1.transform.position;
		}
		else
		if(transform.rigidbody.collider.bounds.Intersects(trigger300_1.collider.bounds)){
			transform.position = trigger600_1.transform.position;
		}

	}
}
