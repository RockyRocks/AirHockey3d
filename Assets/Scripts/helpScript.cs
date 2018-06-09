using UnityEngine;
using System.Collections;

public class helpScript : MonoBehaviour {

	string[] helpTexts = new string[]{
		"Slide the Puck towards the Pins to score a STRIKE or a SPARE!",
		"Your objective for Play for Fun mode is to beat your own score!",
		"Beat your own score to get listed on the Local Leaderboard!",
		"You can rebound the Puck by sliding towards the side walls of the board!",
	};
	public AudioClip buttonClip;
	public AudioClip CommonClip;
	public Texture2D back;
	public Rect backRect;
	public GUIStyle backStyle;

	public Texture2D backButton;

	public Texture2D playForFun;
	public Texture2D playForMoney;
	public Texture2D setUp;
	public Texture2D classicMode;
	public Texture2D arcadeMode;
	public Texture2D faq;
	public Texture2D contact;

	public Rect playForFunRect;
	public Rect playForMoneyRect;
	public Rect setUpRect;
	public Rect classicModeRect;
	public Rect arcadeModeRect;
	public Rect faqRect;
	public Rect contactRect;
	
	public GUIStyle right;
	public GUIStyle left;
	public GUIStyle textStyle;

	int buttonWidth = 64;
	int buttonHeight = 64;

	// Use this for initialization
	void Start () {
		backRect = new Rect (191, 486, buttonWidth, buttonHeight);
		playForFunRect = new Rect (Screen.width/2 - playForFun.width/2, 172, playForFun.width, playForFun.height);
		playForMoneyRect = new Rect (Screen.width/2 - playForMoney.width/2, 224, playForMoney.width, playForMoney.height);
		setUpRect = new Rect (Screen.width/2 - setUp.width/2, 224, setUp.width, setUp.height);
		classicModeRect	= new Rect (Screen.width/2 - classicMode.width/2, 276, classicMode.width, classicMode.height);
		arcadeModeRect = new Rect (Screen.width/2 - arcadeMode.width/2, 328, arcadeMode.width, arcadeMode.height);
		faqRect	= new Rect (Screen.width/2 - faq.width/2, 380, faq.width, faq.height);
		contactRect	= new Rect (Screen.width/2 - contact.width/2, 276, contact.width, contact.height);
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
			WorkAround.LoadLevelWorkaround(Properties.PreviousScene);
			break;
		case "Fun":

			break;
		case "Money":

			break;
		case "SetUp":

			break;
		case "Tournament":

			break;
		case "Arcade":

			break;
		case "FAQ":

			break;
		case "Contact":

			break;
		}
	}

	void OnGUI(){
		GUI.depth = 0;

		if(GUI.Button (playForFunRect, playForFun,textStyle)){
			StartCoroutine(PlayAudio(CommonClip, "Fun"));
		}
		if (GUI.Button (playForMoneyRect, playForMoney, textStyle)) {
			StartCoroutine(PlayAudio(CommonClip, "Money"));
		}
//		if (GUI.Button (setUpRect, setUp, textStyle)) {
//			StartCoroutine(PlayAudio(CommonClip, "SetUp"));
//		}
//		if (GUI.Button (classicModeRect, classicMode, textStyle)) {
//			StartCoroutine(PlayAudio(CommonClip, "Tournament"));
//		}
//		if(GUI.Button (arcadeModeRect, arcadeMode, textStyle)){
//			StartCoroutine(PlayAudio(CommonClip, "Arcade"));
//		}
//		if(GUI.Button (faqRect, faq, textStyle)){
//			StartCoroutine(PlayAudio(CommonClip, "FAQ"));
//		}
		if (GUI.Button (contactRect, contact,textStyle)) {
			StartCoroutine(PlayAudio(CommonClip, "Contact"));
		}
		if (GUI.Button(backRect, "", backStyle))
		{
			StartCoroutine(PlayAudio(buttonClip, "Back"));
		}
	}
	
	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}
}
