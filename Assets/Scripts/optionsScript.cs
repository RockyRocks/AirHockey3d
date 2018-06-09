using UnityEngine;
using System.Collections;

public class optionsScript : MonoBehaviour {

	public Texture2D back;
	public Rect backRect;
	public GUIStyle backStyle;
	public GUIStyle labelStyle;

	public Texture2D frame;
	public Texture2D BG;
	public Texture2D options;

	public Texture2D musicVolume;
	public Texture2D sfxVolume;
	public Texture2D voiceOver;

	public GameObject scrollerPrefab;

	public Rect optionsRect;

	public Rect sfxVolumeRect;
	public Rect voiceOverRect;

	public Vector3 scrollerPosition = new Vector3();

	public AudioClip ReturnClick;


	GameObject musicScroller;
	GameObject sfxScroller;
	GameObject voiceOverScroller;

	int buttonWidth = 64;
	int buttonHeight = 64;

	// Use this for initialization
	void Start () {


		backRect = new Rect(225, 448, buttonWidth, buttonHeight);

		optionsRect = new Rect (Screen.width/2-options.width/2,100,options.width,options.height);

		musicScroller = Instantiate (scrollerPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		musicScroller.name = "Music Volume";
		var script = musicScroller.GetComponent<Scroller> ();
		script.position = new Rect (Screen.width/2-512/2,150,512,128);
		script.volume = Properties.musicVolume;
		script.nameTexture = musicVolume;
		musicScroller.transform.parent = this.transform;

		sfxScroller = Instantiate (scrollerPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		sfxScroller.name = "SFX Volume";
		script = sfxScroller.GetComponent<Scroller> ();
		script.position = new Rect (Screen.width/2-512/2,250,512,128);
		script.volume = Properties.sfxVolume;
		script.nameTexture = sfxVolume;
		sfxScroller.transform.parent = this.transform;

		voiceOverScroller = Instantiate (scrollerPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		voiceOverScroller.name = "VoiceOver Volume";
		script = voiceOverScroller.GetComponent<Scroller> ();
		script.position = new Rect (Screen.width/2-512/2,350,512,128);
		script.volume = Properties.sfxVolume;
		script.nameTexture = voiceOver;
		voiceOverScroller.transform.parent = this.transform;

	}
	
	// Update is called once per frame
	void Update () {
		Properties.sfxVolume = sfxScroller.GetComponent<Scroller>().volume;
		Properties.musicVolume = musicScroller.GetComponent<Scroller>().volume;
		Properties.voiceOverVolumer = voiceOverScroller.GetComponent<Scroller>().volume;
	}

	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length );
		switch (buttonName) {
		case "Back":
			WorkAround.LoadLevelWorkaround(Properties.PreviousScene);
			break;
		case "Right":
			break;
		case "Left":
			break;

		}
	}

	void OnGUI(){
		
		GUI.DrawTexture (new Rect(0,0,Screen.width, Screen.height), BG);
		GUI.DrawTexture (new Rect (Screen.width/2 - frame.width/2,Screen.height/2 - frame.height/2,frame.width, frame.height), frame);

		if (GUI.Button(backRect, "", backStyle))
		{
			PlayerPrefs.SetFloat("MusicVolume",Properties.musicVolume);
			PlayerPrefs.SetFloat("SFXVolume",Properties.sfxVolume);
			PlayerPrefs.SetFloat("VoiceOverVolume",Properties.sfxVolume);
			PlayerPrefs.Save();
			StartCoroutine(PlayAudio(ReturnClick, "Back"));
		}

		//		if (GUI.Button (rect, backButton)) 
		//		{
		//			Debug.Log(Properties.PreviousScene);
		////			if(Properties.PreviousScene == "Airhockey"){
		////				WorkAround.LoadLevelWorkaround("Airhockey");
		////			}
		////			if(Properties.PreviousScene == "MainMenu")
		////				WorkAround.LoadLevelWorkaround("MainMenu");
		////
		////			if(Properties.PreviousScene == "ModesScene")
		////				WorkAround.LoadLevelWorkaround("ModesScene");
		////
		////			if(Properties.PreviousScene == "WagerScene")
		////				WorkAround.LoadLevelWorkaround("WagerScene");
		//
		//			WorkAround.LoadLevelWorkaround(Properties.PreviousScene);
		//		}
	}
	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}
}
