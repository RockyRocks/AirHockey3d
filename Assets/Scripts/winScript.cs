using UnityEngine;
using System.Collections;

public class winScript : MonoBehaviour {

	public AudioClip ReturnClick;
	public AudioClip CommonClick;

	public Texture2D gameplayBal;
	public Texture2D cashBal;
	public Texture2D BG;
	public Texture2D frame;
	public Texture2D hud;
	public Texture2D back;
	public Texture2D title;
	public Texture2D play;

	public Texture2D resultAward;
	public Texture2D resultTextBox1;
	public Texture2D resultTextBox2;

	public Rect winningsRect;
	public Rect tickestAwardedRect;
	public Rect totalWinningsRect;

	public Rect titleRect;
	public Rect resultAwardRect;
	public Rect resultTextBoxRect;

	public Rect backRect;
	public GUIStyle backStyle;
	public Rect gameplayBalRect;
	public Rect cashBalRect;

	public Rect BGRect;
	public Rect frameRect;
	public Rect hudRect;

	//public Rect okButtonRect;
	public Rect ticketsRect;
	
	public GUIStyle labelStyle;
	public GUIStyle playStyle;
	public GUIStyle winningStyle;
    public GUIStyle logStyle;

	public Texture2D playButton;
	public Texture2D bspotLoggedIn;
	public Rect bspotRect;
	public Texture2D bSpotLoggedOut;
	public Rect playRect;
	public GUIStyle balanceStyle;
	public Rect gameplayBalValueRect;
	public Rect cashBalValueRect;

	/******************/
	public Texture2D exitPopupBG;
	public Texture2D exitPopupText;
	public Texture2D okButton;
	public Texture2D cancelButton;
	
	public Rect okButtonRect;
	public Rect cancelButtonRect;
	public GUIStyle buttonStyle;
	public Texture2D buttonBG;
	/******************/


	int buttonWidth = 48;
	int buttonHeight = 48;

	public Texture playText;
	// Use this for initialization	
	void Start () {	

		bspotRect = new Rect (Screen.width/2 - buttonWidth/2, Screen.height- buttonHeight,buttonWidth, buttonWidth);

		gameplayBalRect = new Rect (-20,Screen.height- gameplayBal.height+8, gameplayBal.width, gameplayBal.height);
		gameplayBalValueRect = new Rect(112, Screen.height- cashBal.height+32, 0, 0);
		
		cashBalRect = new Rect (724, Screen.height- cashBal.height+8,cashBal.width,cashBal.height);
		cashBalValueRect = new Rect (cashBalRect.x+130, Screen.height- cashBal.height+32,cashBal.width,cashBal.height);


		//backRect = new Rect(90, 660, back.width, back.height);
		BGRect = new Rect (0, 0, Screen.width, Screen.height);
		frameRect = new Rect (Screen.width/2 - frame.width/2, Screen.height/2 - frame.height/2,Screen.width, Screen.height);
		hudRect = new Rect (0,568, hud.width, hud.height);
		playRect = new Rect (Screen.width/2-playButton.width/2,465,playButton.width, playButton.height);
		titleRect = new Rect (Screen.width/2-title.width/2, 80,title.width, title.height);
		resultAwardRect = new Rect (Screen.width/2-resultAward.width/2, 80,resultAward.width, resultAward.height);
		resultTextBoxRect = new Rect (Screen.width/2-resultTextBox1.width/2, 246,resultTextBox1.width, resultTextBox1.height);

		okButtonRect = new Rect (360, 335, buttonBG.width/2, buttonBG.height/2);
		cancelButtonRect = new Rect (530, 335, buttonBG.width/2, buttonBG.height/2);

		ticketsRect = new Rect (490	,365,100,100);

		winningsRect = new Rect (540,290,100,100);
		totalWinningsRect = new Rect (540, 418,100,100);
		tickestAwardedRect = new Rect (540,360,100,100);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length );
		switch (buttonName) {
		case "OK":
			WorkAround.LoadLevelWorkaround ("ChallengesScene");
			break;
		}
	}
	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}
	
	void OnGUI(){
				GUI.DrawTexture (BGRect, BG);
				if (!Properties.isPopUpShown) {
						GUI.DrawTexture (hudRect, hud);
						GUI.DrawTexture (frameRect, frame);
						GUI.DrawTexture (titleRect, title);


						if (Properties.GameWonState == "Win") {
								resultTextBoxRect = new Rect (Screen.width / 2 - resultTextBox1.width / 2, 246, resultTextBox1.width, resultTextBox1.height);
								GUI.DrawTexture (resultAwardRect, resultAward);
								GUI.DrawTexture (resultTextBoxRect, resultTextBox1);
								GUI.Label (winningsRect, Properties.WonAmount, winningStyle);
								GUI.Label (totalWinningsRect, Properties.TotalWinnings.ToString (), winningStyle);
								GUI.Label (tickestAwardedRect, Properties.Tickets.ToString (), winningStyle);

						} else {
								resultTextBoxRect = new Rect (Screen.width / 2 - resultTextBox1.width / 2, 170, resultTextBox1.width, resultTextBox1.height);

								Vector2 sizeOfLabel = winningStyle.CalcSize (new GUIContent (Properties.Tickets.ToString ()));

								tickestAwardedRect = new Rect (Screen.width / 2 - sizeOfLabel.x / 2, 250, sizeOfLabel.x, sizeOfLabel.y);

								GUI.Label (tickestAwardedRect, Properties.Tickets.ToString (), winningStyle);

								sizeOfLabel = winningStyle.CalcSize (new GUIContent (Properties.TotalWinnings.ToString ()));

								tickestAwardedRect = new Rect (Screen.width / 2 - sizeOfLabel.x / 2, 337, sizeOfLabel.x, sizeOfLabel.y);

								GUI.Label (tickestAwardedRect, Properties.TotalWinnings.ToString (), winningStyle);

								GUI.DrawTexture (resultTextBoxRect, resultTextBox2);
						}

						if (GUI.Button (playRect, play, playStyle)) {
								StartCoroutine (PlayAudio (CommonClick, "OK"));
						}


						GUI.DrawTexture (gameplayBalRect, gameplayBal);

						GUI.DrawTexture (cashBalRect, cashBal);

						GUI.Label (gameplayBalValueRect, Properties.GamePlayBalance.ToString (), balanceStyle);

						GUI.Label (cashBalValueRect, Properties.CashBalance.ToString (), balanceStyle);
				} 
		else {
				GUI.DrawTexture (new Rect (Screen.width / 2 - exitPopupBG.width / 2, Screen.height / 2 - exitPopupBG.height / 2, exitPopupBG.width, exitPopupBG.height), exitPopupBG);
				GUI.DrawTexture (new Rect (Screen.width / 2 - exitPopupText.width / 2, Screen.height / 2 - exitPopupText.height / 2, exitPopupText.width, exitPopupText.height), exitPopupText);
				if (GUI.Button (okButtonRect, okButton, buttonStyle)) {
					StartCoroutine(PlayAudio(CommonClick, "Ok"));
					Properties.DeleteSession = true;
					
				}
				if (GUI.Button (cancelButtonRect, cancelButton, buttonStyle)) {
					StartCoroutine(PlayAudio(CommonClick, "Cancel"));
					Properties.isPopUpShown = false;
				}

			}
			//TODO CHange if the player is logged in or not
			if (GUI.Button (bspotRect, "", logStyle)) {
				Debug.Log ("Pressing the LogOut Button");
				Properties.isPopUpShown=true;
			}	
	}
}
