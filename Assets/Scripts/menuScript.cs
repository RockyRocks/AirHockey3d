using UnityEngine;
using System.Collections;

public class menuScript : MonoBehaviour {

	public Texture2D backGround;
	public Texture2D frame;

	public GUIStyle Style;
	public GUIStyle arcadeStyle;
	public GUIStyle classicStyle;

	public Texture2D arcadeModeTex;
	public Texture2D TournamentMode;

	public Rect arcadeRect;
	public Rect TournamentRect;

	public AudioClip ReturnClick;
	public AudioClip CommonClick;

	// Use this for initialization
	void Start () {
		Properties.ParentScene = "ModesScene";
		Properties.PreviousScene = "MainMenu";
		arcadeRect = new Rect (Screen.width/2 - arcadeModeTex.width/2, 340, arcadeModeTex.width, arcadeModeTex.height);
		TournamentRect = new Rect (Screen.width/2 - TournamentMode.width/2, 210, TournamentMode.width, TournamentMode.height);
	}

//	TODO Add the Audio functionality
	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length );
		switch (buttonName) {
		case "Back":
			WorkAround.LoadLevelWorkaround(Properties.PreviousScene);
			break;

		case "Tournament":
			Properties.PlayMode = Properties.PlayForFunMode.TOURNAMENT;
			WorkAround.LoadLevelWorkaround("Countries");
			break;

		case "Arcade":
			Properties.PlayMode = Properties.PlayForFunMode.ARCADE;
			WorkAround.LoadLevelWorkaround("Countries");
			break;
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){
		GUI.depth = 0;
		if (GUI.Button (arcadeRect, "", arcadeStyle)) {
			StartCoroutine(PlayAudio(CommonClick, "Arcade"));
		}
		if (GUI.Button (TournamentRect, "", classicStyle)) {
			StartCoroutine(PlayAudio(CommonClick, "Tournament"));
		}
	}

	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}
}
