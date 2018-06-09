using UnityEngine;
using System.Collections;

public class Back : MonoBehaviour {

	public Texture2D back;
	public Rect backRect;
	public GUIStyle backStyle;


	//int buttonWidth = 64;
	//int buttonHeight = 64;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (GUI.Button(backRect, "", backStyle))
		{
			//StartCoroutine(PlayAudio(buttonClip, "Back"));
		}
	}
}
