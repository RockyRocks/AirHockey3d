using UnityEngine;
using System.Collections;

public class AnimatedTextScript : MonoBehaviour {

	private static AnimatedTextScript instance;
	public static AnimatedTextScript Get ()
	{
		return instance;
	}

	Vector3 scale = new Vector3 (1f, 1f, 1f);

	// Use this for initialization
	void Start () {
		instance = this;
		transform.localScale = scale;
	}

	Vector3 targetScale = new Vector3(2f,2f,1f);


	public bool zoomIn = false;
	public bool zoomOut = false;

	// Update is called once per frame
	void Update () {
		transform.LookAt (Camera.main.transform);
		gameObject.transform.localEulerAngles = new Vector3 (0, 180, 0);

		if (zoomIn) {
			//show ();
			if (transform.localScale.x + 0.01f <= targetScale.x) {
				transform.localScale = Vector3.Lerp (transform.localScale, targetScale, 0.1f);
			} else{
				zoomIn = false;
				zoomOut = true;
			}
		}
		if(zoomOut && !zoomIn) {
			if (transform.localScale.x - 0.01f> scale.x) {
				transform.localScale = Vector3.Lerp (transform.localScale, scale, 0.1f);
			}
			else{
				zoomIn = false;
				zoomOut = false;
				//Hide ();
			}
		}
	}

	public void show ()
	{
		var c = Color.white;
		c.a = 255f;
		this.gameObject.GetComponent<TextMesh> ().color = c;
	}

	public void Hide ()
	{
		var c = Color.white;
		c.a = 0f;
		this.gameObject.GetComponent<TextMesh> ().color = c;
	}
}
