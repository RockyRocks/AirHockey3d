using UnityEngine;
using System.Collections;

public class LoginScript : MonoBehaviour {

	public Texture2D bg;
	public Texture2D frame;

	public Texture2D facebook;
	public Texture2D eMail;
	public Texture2D twitter;

	public GUIStyle facebookStyle;
	public GUIStyle eMailStyle;
	public GUIStyle twitterStyle;
	public GUIStyle buttonStyle;

	public Texture2D playOffline;
	public Rect playOfflineRect;
	public Rect facebookRect;
	public Rect eMailRect;
	public Rect twitterRect;

	public FaderScript faderScript;
	// Use this for initialization
	void Start () {
		playOfflineRect = new Rect (Screen.width/2- playOffline.width/2, 530, playOffline.width, playOffline.height);
		facebookRect = new Rect (460,420, 96, 96);
		twitterRect = new Rect (378,310, 96, 96);
		eMailRect = new Rect (548,310, 96, 96);	
	}
	
	// Update is called once per frame
	void Update () {
		//GUI.skin.button.c
	}

	void OnGUI(){

		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bg);
		GUI.DrawTexture (new Rect (0,0, frame.width, frame.height), frame);

		if (GUI.Button (playOfflineRect, playOffline,buttonStyle)) {
			//Application.OpenURL("https://sb.oddz.com/cas/login?service=https://s3.amazonaws.com/testtiki/webbuild.html");
			Properties.ParentScene = "Login";
			WorkAround.LoadLevelWorkaround ("ModesScene");
		}

		if (GUI.Button (facebookRect, "", facebookStyle)) {

		}

		if (GUI.Button (eMailRect, "", eMailStyle)) {

		}

		if (GUI.Button (twitterRect, "", twitterStyle)) {

		}
	}
	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}
}
