using UnityEngine;
using System.Collections;
using System.IO;

public class UnLockChallenge : MonoBehaviour {

	public AudioClip ReturnClick;
	public AudioClip CommonClick;

	//Texture2D texture;

	public Texture2D bg;
	public Texture2D hud;
	public Texture2D frame;
	public Texture2D bspotLoggedIn;
	public Texture2D bspotLoggedOut;
	
	public Texture2D challengeLockedTitle;
	public Texture2D challengeLockedTexture;
	public Texture2D challengeUnLockedTexture;
	public Texture2D challengeUnlockedIcon;

	public Texture2D optionsButton;
	public Texture2D ticketsBG;
	public Texture2D help;
	public Texture2D challengeTitle;
	public Texture2D gamePlayButtonbg;
	public Texture2D gamePlayButton;
	public Texture2D unlockChallenge;
	public Texture2D playAnotherChallenge;
	
	public Rect bspotRect;
	public Rect frameRect;
	public Rect hudRect;
	public Rect challengeRect;
	public Rect upArrowRect;
	public Rect downArrowRect;
	public Rect gamePlayButtonRect;
	public Rect challengeLockedRect;
	public Rect challengeLockedTextureRect;
	public Rect playAnotherChallengeRect;
	public Rect unlockChallengeRect;

	public Rect challengeUnlockedIconRect;
	
	public Rect optionsButtonRect;
	public Rect ticketsBGRect;
	public Rect helpRect;
	public Rect ticketsRect;
	public Rect gameplayBalanceRect;

	public GUIStyle labelStyle;
	public GUIStyle optionsStyle;
	public GUIStyle helpStyle;
	public GUIStyle challengeStyle;
	public GUIStyle logStyle;

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

	// Use this for initialization
	void Start () {
		//texture = challengeLockedTexture;

		unlockChallengeRect = new Rect (Screen.width/2 - unlockChallenge.width/2,345,unlockChallenge.width,unlockChallenge.height);

		playAnotherChallengeRect = new Rect (Screen.width/2 - playAnotherChallenge.width/2,445,playAnotherChallenge.width,playAnotherChallenge.height);
		
		challengeLockedRect = new Rect (Screen.width/2 - challengeLockedTitle.width/2,220,challengeLockedTitle.width,challengeLockedTitle.height);
		challengeLockedTextureRect = new Rect (307,125,challengeLockedTexture.width,challengeLockedTexture.height);
		challengeRect = new Rect (Screen.width/2 - challengeTitle.width/2,80,challengeTitle.width,challengeTitle.height);

		optionsButtonRect = new Rect (0, 0, buttonWidth, buttonHeight);
		helpRect = new Rect (Screen.width - buttonWidth,0, buttonWidth, buttonHeight);

		frameRect = new Rect (Screen.width/2 - frame.width/2, Screen.height/2 - frame.height/2,Screen.width, Screen.height);
		hudRect = new Rect (0,568, hud.width, hud.height);

		gameplayBalanceRect = new Rect(112, 568, 0, 0);
		ticketsRect = new Rect (858,567,0,0);

		okButtonRect = new Rect (360, 335, buttonBG.width/2, buttonBG.height/2);
		cancelButtonRect = new Rect (530, 335, buttonBG.width/2, buttonBG.height/2);

		gamePlayButtonRect = new Rect (-20,Screen.height- gamePlayButtonbg.height+8, gamePlayButtonbg.width, gamePlayButtonbg.height);
		ticketsBGRect = new Rect (724, Screen.height- ticketsBG.height+8,ticketsBG.width,ticketsBG.height);

		bspotRect = new Rect (Screen.width/2 - buttonWidth/2, Screen.height- buttonHeight,buttonWidth, buttonWidth);

		challengeUnlockedIconRect =  new Rect(Screen.width/2 - challengeUnlockedIcon.width/2,250, challengeUnlockedIcon.width, challengeUnlockedIcon.height);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}

	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length );
		switch (buttonName) {
		case "Help":
			Properties.PreviousScene = "UnLockChallenge";
			WorkAround.LoadLevelWorkaround("Help");
			break;
		case "Options":
			Properties.PreviousScene = "UnLockChallenge";
			WorkAround.LoadLevelWorkaround("Options");
			break;
		case "PlayAnotherChallenge":
			WorkAround.LoadLevelWorkaround ("ChallengesScene");
			break;
		case "UnLockChallenge":
			var challengeProperties = Properties.winLoseDictionary[Properties.ChallengeMode];
			int unLockAmount = challengeProperties.UnLockAmount; 
			if(unLockAmount	<= Properties.TotalTickets){
				
				PlayerPrefsX.SetBool(Properties.winLoseDictionary[Properties.ChallengeMode].Name,true);
				
				Properties.TotalTickets -= unLockAmount;
				
				PlayerPrefs.SetInt("Total Tickets", (int)Properties.TotalTickets);
				
				PlayerPrefs.Save();
				//show unlockedScreen
				//texture = challengeUnLockedTexture;
				challengeUnlocked = true;
				StartCoroutine("ChangeScreen");
//					WorkAround.LoadLevelWorkaround("UnLockedChallenge");
			}
			
			else{
				Properties.MoreTickets = unLockAmount - (int)Properties.TotalTickets;
				WorkAround.LoadLevelWorkaround("MoreTickets");
			}
			break;
		}
	}

	IEnumerator ChangeScreen(){
		yield return new WaitForSeconds (2f);
		WorkAround.LoadLevelWorkaround ("ChallengesScene");
	}

	bool challengeUnlocked = false;

	void OnGUI(){

				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), bg);
		if(!Properties.isPopUpShown)
		{
				GUI.DrawTexture (hudRect, hud);
				GUI.DrawTexture (frameRect, frame);
				GUI.DrawTexture (challengeRect, challengeTitle);

				if (GUI.Button (helpRect, "", helpStyle)) {
						StartCoroutine (PlayAudio (CommonClick, "Help"));
				}
				if (GUI.Button (optionsButtonRect, "", optionsStyle)) {	
						StartCoroutine (PlayAudio (CommonClick, "Options"));
				}

				if (challengeUnlocked) {

						Vector2 sizeOfLabel1 = challengeStyle.CalcSize (new GUIContent ("Challenge UnLocked"));
						GUI.BeginGroup (new Rect (challengeLockedTextureRect.x - 20, challengeLockedTextureRect.y, sizeOfLabel1.x * 2, 600));
						var sizeOfLabel = challengeStyle.CalcSize (new GUIContent (Properties.SelectedChallenge));
						GUI.Label (new Rect (sizeOfLabel1.x / 2, 0, 100, 100), Properties.SelectedChallenge, challengeStyle);
						sizeOfLabel = challengeStyle.CalcSize (new GUIContent ("Challenge UnLocked"));
						GUI.Label (new Rect (sizeOfLabel1.x / 2, sizeOfLabel.y, 100, 100), "Challenge UnLocked", challengeStyle);
						GUI.EndGroup ();

						GUI.DrawTexture (challengeUnlockedIconRect, challengeUnlockedIcon);

				} else {

						Vector2 sizeOfLabel1 = challengeStyle.CalcSize (new GUIContent ("Challenge Locked"));
						GUI.BeginGroup (new Rect (challengeLockedTextureRect.x, challengeLockedTextureRect.y, sizeOfLabel1.x * 2, 600));
						var sizeOfLabel = challengeStyle.CalcSize (new GUIContent (Properties.SelectedChallenge));
						GUI.Label (new Rect (sizeOfLabel1.x / 2, 0, 100, 100), Properties.SelectedChallenge, challengeStyle);
						sizeOfLabel = challengeStyle.CalcSize (new GUIContent ("Challenge Locked"));
						GUI.Label (new Rect (sizeOfLabel.x / 2, sizeOfLabel.y, 100, 100), "Challenge Locked", challengeStyle);
						GUI.EndGroup ();

						GUI.DrawTexture (challengeLockedRect, challengeLockedTitle);

						if (GUI.Button (playAnotherChallengeRect, playAnotherChallenge, labelStyle)) {	
								StartCoroutine (PlayAudio (CommonClick, "PlayAnotherChallenge"));
						}

						if (GUI.Button (unlockChallengeRect, unlockChallenge, labelStyle)) {	
								StartCoroutine (PlayAudio (CommonClick, "UnLockChallenge"));
						}

				}
		
				GUI.DrawTexture (gamePlayButtonRect, gamePlayButtonbg);
				GUI.DrawTexture (ticketsBGRect, ticketsBG);
				GUI.Label (gameplayBalanceRect, Properties.GamePlayBalance.ToString (), labelStyle);
				GUI.Label (ticketsRect, Properties.TotalTickets.ToString (), labelStyle);
				//GUI.DrawTexture (gamePlayButtonRect, gamePlayButton);
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
		

		
		//		GUI.Label (challangeScreenRect, "Challenge Screen", style1);
		//		GUI.Label (challangeLockedRect, "Challenge Locked", style);
		//		GUI.Label(gamePlayLabelRect, "GamePlay : $"+ Properties.GamePlayBalance, style2);
		//		GUI.Label(TicketsLabelRect, "Tickets : "+ Properties.TotalTickets, style2);
		//
		//		GUI.DrawTexture (lockedChallengeBGRect, lockedChallengeBG);
		//		if (GUI.Button (playAnotherChallengeRect, playAnotherChallenge, style)) {
		//			WorkAround.LoadLevelWorkaround("ChallengesScene");
		//		}
		//
		//		if (GUI.Button (unLockChallengeRect, unLockChallenge, style)) {
		//			var challengeProperties = Properties.winLoseDictionary[Properties.ChallengeMode];
		//			int unLockAmount = challengeProperties.UnLockAmount; 
		//			if(unLockAmount	<= Properties.TotalTickets){
		//
		//				PlayerPrefsX.SetBool(Properties.winLoseDictionary[Properties.ChallengeMode].Name,true);
		//
		//				Properties.TotalTickets -= unLockAmount;
		//
		//				PlayerPrefs.SetInt("Total Tickets", (int)Properties.TotalTickets);
		//
		//				PlayerPrefs.Save();
		//				//show unlockedScreen
		//				WorkAround.LoadLevelWorkaround("UnLockedChallenge");
		//			}
		//
		//			else{
		//				Properties.MoreTickets = unLockAmount - (int)Properties.TotalTickets;
		//				WorkAround.LoadLevelWorkaround("MoreTickets");
		//			}
		//		}
	}
}
