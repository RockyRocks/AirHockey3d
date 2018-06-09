using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeaderBoard : MonoBehaviour {

	public AudioClip ReturnClick;
	public AudioClip CommonClick;

	public Texture2D leaderBoardCash;
	public Texture2D leaderBoardFree;

	public Texture2D globalButton;
	public Texture2D localButton;
	public Texture2D classicButton;
	public Texture2D arcadeButton;

	public GUIStyle globalStyle;
	public GUIStyle localStyle;

	public GUIStyle classicStyle;
	public GUIStyle arcadeStyle;

	public Rect globalRect;
	public Rect classicRect;

	bool isGlobal = false;
	bool isClassic = false;

	// Use this for initialization
	void Start () {
		globalRect = new Rect (130, 7,globalButton.width,globalButton.height);
		classicRect = new Rect (675, 7,classicButton.width,classicButton.height);
        Properties.CurrentScene = "LeaderBoard";

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length );
		switch (buttonName) {
		case "Help":
			WorkAround.LoadLevelWorkaround("help");
			break;
		case "Options":
			WorkAround.LoadLevelWorkaround("Options");
			break;
		case "Back":
			WorkAround.LoadLevelWorkaround("ModesScene");
			break;
		}
	}

	void OnGUI(){

		if (isClassic){
			if (GUI.Button (classicRect, "", classicStyle)) {
				isClassic = !isClassic;
			}
		}
		else{
			if (GUI.Button (classicRect, "", arcadeStyle)) {
				isClassic = !isClassic;
			}
		}

		if (isGlobal) {
			if (GUI.Button (globalRect, "", globalStyle)) {
				isGlobal = !isGlobal;
			}
		} else {
			if (GUI.Button (globalRect, "", localStyle)) {
				isGlobal = !isGlobal;				
			}		
		}
	}
}
