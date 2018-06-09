using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	public Texture2D yes;
	public Texture2D no;

	public Rect yesRect;
	public Rect noRect;

	public GUIStyle style;
	public GUIStyle yesStyle;
	public GUIStyle noStyle;

	// Use this for initialization
	void Start () {
		yesRect = new Rect (320,300,yes.width,yes.height);	
		noRect = new Rect (500,300,no.width,no.height);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		if(GUI.Button(yesRect, "", yesStyle)){
			if(Properties.GameType == Properties.Modes.PlayforFun)
				WorkAround.LoadLevelWorkaround("ModesScene");
			else
				WorkAround.LoadLevelWorkaround("ChallengesScene");
		}
		if(GUI.Button(noRect, "", noStyle)){
			WorkAround.LoadLevelWorkaround(Properties.PreviousScene);
		}

	}
}
