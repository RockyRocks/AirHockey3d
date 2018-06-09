using UnityEngine;
using System.Collections;

public class ButtonIcons : MonoBehaviour {

	public AudioClip buttonClick;

	public Texture2D BG;
	public Texture2D back;
	public Texture2D settings;
	public Texture2D audioTexture;

	public Rect backRect;
	public Rect settingsRect;
	public Rect audioRect;

	public GUIStyle backStyle;
	public GUIStyle settingsStyle;
	public GUIStyle audioStyle;

	int buttonWidth = 64;
	int buttonHeight = 64;

	// Use this for initialization
	void Start () {
		backRect = new Rect (15, 525, buttonWidth, buttonHeight);
		audioRect = new Rect (800, 525, buttonWidth,buttonHeight);
		settingsRect = new Rect (880, 525, buttonWidth, buttonHeight);
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length );
		switch (buttonName) {
		case "Back":
			if(Properties.CurrentScene =="MainGame"){
				WorkAround.LoadLevelWorkaround("ExitScene");
			}
			else
				WorkAround.LoadLevelWorkaround(Properties.ParentScene);
			break;
		case "Settings":
			WorkAround.LoadLevelWorkaround("SettingsScene");
			break;
		case "Audio":
			WorkAround.LoadLevelWorkaround("Options");
			break;
		}
	}

	void OnGUI(){
		GUI.depth = 1;
	
		if (GUI.Button (backRect, "", backStyle)) {
			StartCoroutine(PlayAudio(buttonClick, "Back"));
		}
		if (GUI.Button (settingsRect, "", settingsStyle)) {
			StartCoroutine(PlayAudio(buttonClick, "Settings"));
		}
		if (GUI.Button (audioRect, "", audioStyle)) {
			StartCoroutine(PlayAudio(buttonClick, "Audio"));
		}
	}
}
