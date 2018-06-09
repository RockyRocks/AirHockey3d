using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	int buttonWidth = 48;
	int buttonHeight = 48;


	public AudioClip buttonClip;

	public Texture2D gameplayBal;
	public Texture2D cashBal;
	public Texture2D playAgain;
	public Texture2D mainMenu;
	public Texture2D personalBG;
	public Texture2D personalBest;
	public Texture2D awardBG;
	public Texture2D bspotLoggedIn;
	public Rect bspotRect;
	public Texture2D bSpotLoggedOut;
	public GUIStyle balanceStyle;
	public Rect gameplayBalValueRect;
	public Rect cashBalValueRect;
	public Rect gameplayBalRect;
	public Rect cashBalRect;
	public Rect personalBestRect;
	public Texture2D BG;
	public Texture2D frame;
	public Texture2D hud;
	public Texture2D back;
	
	public Texture2D backs;
	public Texture2D options;
	public Texture2D help;
	
	public Texture2D scoreTitle;
	public Texture2D scoreBg;
	public Rect currentScoreRect;
	public Rect bestScoreRect;
	public Rect scoreInfoRect;

	public Rect playButtonRect;
	public Rect mainMenuButtonRect;
	public Rect scoreBGRect;
	public Rect BGRect;
	public Rect frameRect;
	public Rect hudRect;
	public Rect scoreTitleRect;
	public Rect challengeInfoRect;
	public Rect admitCardRect;

	public Rect ticketsRect;
	public Rect awardBGRect;

	public Rect backsRect;
	public Rect optionsRect;
	public Rect helpRect;
	public Rect storeRect;
	public Rect personalBgRect;
	public GUIStyle labelStyle;
	public GUIStyle backsStyle;
	public GUIStyle optionsStyle;
	public GUIStyle helpStyle;
	
	public GUIStyle ticketsStyle;
	public GUIStyle playButtonStyle;
	public Rect scoreRect;
	// Use this for initialization
	void Start () {
		scoreRect = new Rect (460,370,100,100);
		awardBGRect = new Rect (Screen.width/2-awardBG.width/2,100, awardBG.width,awardBG.height);
		personalBgRect = new Rect (257,252, personalBG.width, personalBG.height);
		personalBestRect = new Rect (382,310, personalBest.width, personalBest.height);

		currentScoreRect = new Rect(0,0,100,100);
		bestScoreRect = new Rect(0,0,100,100);
		scoreInfoRect = new Rect(0,0,100,100);

		playButtonRect = new Rect (350 , 450, playAgain.width,playAgain.height);
		mainMenuButtonRect = new Rect (485 , 450, mainMenu.width,mainMenu.height);
		scoreBGRect = new Rect (Screen.width/2-scoreBg.width/2, 185,scoreBg.width, scoreBg.height);
		ticketsRect = new Rect (0,0, 100,100);
		bspotRect = new Rect (Screen.width/2-bSpotLoggedOut.width/2, 700, bSpotLoggedOut.width,bSpotLoggedOut.height);
		
		gameplayBalRect  = new Rect (98,700,gameplayBal.width,gameplayBal.height);
		cashBalRect = new Rect (660,700,cashBal.width,cashBal.height);
		
		gameplayBalValueRect  = new Rect (220,725,gameplayBal.width,gameplayBal.height);
		cashBalValueRect = new Rect (780,725,cashBal.width,cashBal.height);
		
		BGRect = new Rect (0, 0, Screen.width, Screen.height);
		frameRect = new Rect (Screen.width/2 - frame.width/2, Screen.height/2 - frame.height/2,Screen.width, Screen.height);
		hudRect = new Rect (0,568, hud.width, hud.height);
		
		scoreTitleRect = new Rect(Screen.width/2 - scoreTitle.width/2,100, scoreTitle.width, scoreTitle.height);
		challengeInfoRect = new Rect(Screen.width/2 - scoreBg.width/2,215, scoreBg.width, scoreBg.height);
		
		optionsRect = new Rect (0, 0, buttonWidth, buttonHeight);
		helpRect = new Rect (Screen.width - buttonWidth,0, buttonWidth, buttonHeight);
		backsRect = new Rect(buttonWidth, 0, buttonWidth, buttonHeight);
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
			WorkAround.LoadLevelWorkaround("ChallengesScene");
			break;
		case "Help":
			Properties.PreviousScene = "ScoreScene";
			WorkAround.LoadLevelWorkaround("Help");
			break;
		case "Options":
			Properties.PreviousScene = "ScoreScene";
			WorkAround.LoadLevelWorkaround("Options");
			break;
		case "PlayAgain":
			Properties.GameType = Properties.Modes.PlayforFun;
			WorkAround.LoadLevelWorkaround("Airhockey");
			break;
		case "MainMenu":
			WorkAround.LoadLevelWorkaround("ModesScene");
			break;
		}
	}
	
	void OnGUI(){
		
		GUI.DrawTexture (BGRect,BG);
		GUI.DrawTexture (frameRect, frame);
		GUI.DrawTexture (scoreTitleRect, scoreTitle);

		if (GUI.Button (playButtonRect, playAgain, playButtonStyle)) {
			StartCoroutine(PlayAudio(buttonClip,"PlayAgain"));
		}
		if (GUI.Button (mainMenuButtonRect, mainMenu, playButtonStyle)) {
			StartCoroutine(PlayAudio(buttonClip,"MainMenu"));
		}
		if (GUI.Button (backsRect,"",backsStyle)) {
			StartCoroutine(PlayAudio(buttonClip,"Back"));
		}
		if (GUI.Button (optionsRect,"",optionsStyle)) {
			StartCoroutine(PlayAudio(buttonClip,"Options"));
			
		}
		if (GUI.Button (helpRect,"",helpStyle)) {
			StartCoroutine(PlayAudio(buttonClip,"Help"));
		}

		if (!Properties.isScoreHightest)
		{
			GUI.BeginGroup (scoreBGRect);

//			string text = "CurrentScore: ";
//			Vector3 size = labelStyle.CalcSize (new GUIContent (text));
//			GUI.Label (new Rect(0,10,scoreBGRect.width, size.y),text,labelStyle);

			GUI.Label (new Rect (75 , 20 , 100, 100), Properties.CurrentScore, labelStyle);

//			var text = "Best Score: ";
//			var size = labelStyle.CalcSize (new GUIContent (text));
//			GUI.Label (new Rect(0,85,scoreBGRect.width, size.y),text,labelStyle);

			GUI.Label (new Rect (75, 100, 100, 100), Properties.HighestScore, labelStyle);

//			text = "Beat Your Best Score To Be On The LeaderBoard";
//			size = labelStyle.CalcSize (new GUIContent (text));
//			GUI.Label (new Rect (0,155,scoreBGRect.width,size.y*3),text,labelStyle);

			GUI.EndGroup ();
			GUI.DrawTexture (scoreBGRect, scoreBg);
		}
		else {
			GUI.DrawTexture (awardBGRect, awardBG);
			GUI.DrawTexture (personalBgRect, personalBG);
			GUI.DrawTexture (personalBestRect, personalBest);
			GUI.BeginGroup (personalBgRect);
			GUI.Label (new Rect (210, 105 ,  100, 100), Properties.HighestScore, labelStyle);
			GUI.EndGroup ();
		}
	}

	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}
}
