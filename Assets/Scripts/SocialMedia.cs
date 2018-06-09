using UnityEngine;
using System.Collections;

public class SocialMedia : MonoBehaviour {

	public Texture2D facebookTexture;
	public Texture2D twitterTexture;

	public Rect facebookRect;
	public Rect twitterRect;

	int buttonWidth = 64;
	int buttonHeight = 64;
	int border = 15;

	public GUIStyle twitterStyle;
	public GUIStyle facebookStyle;

	// Use this for initialization
	void Start () {
		twitterRect = new Rect (885,14, buttonWidth, buttonHeight);
		facebookRect = new Rect (805,14, buttonWidth, buttonHeight);
	}
	
	void OnGUI(){
		GUI.depth = 0;
		if (GUI.Button (twitterRect, "", twitterStyle)) {
			//StartCoroutine(PlayAudio(buttonClick, "Twitter"));
		}
		
		if (GUI.Button (facebookRect, "", facebookStyle)) {
			//StartCoroutine(PlayAudio(buttonClick, "Facebook"));
		}
	}
}
