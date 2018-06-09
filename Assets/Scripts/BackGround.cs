using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour {

	public Texture2D frame;
	public Texture2D BG;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.depth = 3;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), BG);
		GUI.DrawTexture (new Rect (Screen.width / 2 - frame.width / 2 + transform.position.x, Screen.height / 2 - frame.height / 2 + transform.position.y, frame.width, frame.height), frame);
	}
}
