using UnityEngine;
using System.Collections;

public class Settings : MonoBehaviour {

	public Texture2D backButton;

	public Texture2D help;
	public Texture2D logOut;
	public Texture2D quitGame;

	public Rect helpRect;
	public Rect logOutRect;
	public Rect quitGameRect;

	public GUIStyle backStyle;
	public GUIStyle textStyle;
	public Rect backRect;

	int buttonWidth = 64;
	int buttonHeight = 64;

	public AudioClip ReturnClick;
	public AudioClip CommonClick;

	// Use this for initialization
	void Start () {
		backRect = new Rect (191, 406, buttonWidth, buttonHeight);
		helpRect = new Rect (Screen.width/2 - help.width/2, 200, help.width, help.height);
		logOutRect = new Rect (Screen.width/2 - logOut.width/2, 252, logOut.width, logOut.height);
		quitGameRect = new Rect (Screen.width/2 - quitGame.width/2, 304, quitGame.width, quitGame.height);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length);
		switch (buttonName) {
		case "Back":
			WorkAround.LoadLevelWorkaround(Properties.PreviousScene);
			break;
		case "Help":
//			WorkAround.LoadLevelWorkaround(Properties.PreviousScene);
			break;
		case "LogOut":
//				WorkAround.LoadLevelWorkaround(Properties.PreviousScene);
			break;
		case "Quit":
//				WorkAround.LoadLevelWorkaround(Properties.PreviousScene);
			break;
		}

	}

	void OnGUI(){
		GUI.depth = 0;
		if (GUI.Button(backRect, "", backStyle))
		{
			StartCoroutine(PlayAudio(ReturnClick, "Back"));
		}

//		if (GUI.Button (helpRect, help, textStyle)) {
//			StartCoroutine(PlayAudio(CommonClick, "Help"));
//		}
		if(Properties.IsLoggedIn)
		if (GUI.Button (logOutRect, logOut, textStyle)) {
			StartCoroutine(PlayAudio(CommonClick, "LogOut"));
		}
		if (GUI.Button (quitGameRect, quitGame, textStyle)) {
			StartCoroutine(PlayAudio(CommonClick, "Quit"));
		}
	}
}
