using UnityEngine;
using System.Collections;

public class LogOut : MonoBehaviour {

	public Texture2D yes;
	public Texture2D no;

	public Rect yesRect;
	public Rect noRect;

	public GUIStyle yesStyle;
	public GUIStyle noStyle;

	// Use this for initialization
	void Start () {
		yesRect = new Rect (320,320,yes.width,yes.height);	
		noRect = new Rect (500,320,no.width,no.height);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.depth = 0;

		if(GUI.Button(yesRect, "", yesStyle)){
			
		}
		if(GUI.Button(noRect, "", noStyle)){
			
		}
	}
}
