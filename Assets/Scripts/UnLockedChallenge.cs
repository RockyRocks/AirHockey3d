using UnityEngine;
using System.Collections;

public class UnLockedChallenge : MonoBehaviour {

	public Texture2D gameplayBal;
	public Texture2D cashBal;
	
	public Texture2D bspotLoggedIn;
	public Rect bspotRect;
	public Texture2D bSpotLoggedOut;
	public Rect playRect;
	public GUIStyle balanceStyle;
	public Rect gameplayBalValueRect;
	public Rect cashBalValueRect;
	public Rect gameplayBalRect;
	public Rect cashBalRect;
	
	public Texture2D BG;
	public Texture2D frame;
	public Texture2D hud;
	public Texture2D back;
	
	public Texture2D backs;
	public Texture2D options;
	public Texture2D help;
	public Texture2D store;
	
	public Texture2D challengeTitle;
	public Texture2D challengeInfo;
	public Texture2D admitCard;
	public Texture2D buyTickets;
	
	public Rect BGRect;
	public Rect frameRect;
	public Rect hudRect;
	public Rect challengeTitleRect;
	public Rect challengeInfoRect;
	public Rect admitCardRect;
	public Rect buyTicketsRect;
	
	public Rect backsRect;
	public Rect optionsRect;
	public Rect helpRect;
	public Rect storeRect;
	
	public GUIStyle button;
	
	public GUIStyle backsStyle;
	public GUIStyle optionsStyle;
	public GUIStyle helpStyle;
	public GUIStyle storeStyle;
	
	// Use this for initialization
	void Start () {
		
		bspotRect = new Rect (Screen.width/2-bSpotLoggedOut.width/2, 660, bSpotLoggedOut.width,bSpotLoggedOut.height);
		gameplayBalRect  = new Rect (170,660,gameplayBal.width,gameplayBal.height);
		cashBalRect = new Rect (600,660,cashBal.width,cashBal.height);
		gameplayBalValueRect  = new Rect (370,684,gameplayBal.width,gameplayBal.height);
		cashBalValueRect = new Rect (765,684,cashBal.width,cashBal.height);
		
		BGRect = new Rect (0, 0, Screen.width, Screen.height);
		frameRect = new Rect (0,-75,Screen.width, Screen.height);
		hudRect = new Rect (0,512, hud.width, hud.height);
		challengeTitleRect = new Rect(Screen.width/2 - challengeTitle.width/2,90, challengeTitle.width, challengeTitle.height);
		challengeInfoRect = new Rect(Screen.width/2 - challengeInfo.width/2,140, challengeInfo.width, challengeInfo.height);
		admitCardRect =  new Rect(Screen.width/2 - admitCard.width/2,230, admitCard.width, admitCard.height);
		buyTicketsRect =  new Rect(Screen.width/2 - buyTickets.width/2,250, buyTickets.width, buyTickets.height);
		
		backsRect = new Rect(15,0,back.width,back.height);
		optionsRect = new Rect(95,0,back.width,back.height);
		helpRect = new Rect(865,0,back.width,back.height);
		storeRect = new Rect(945,0,back.width,back.height);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI(){
		
		GUI.DrawTexture (BGRect,BG);
		GUI.DrawTexture(hudRect, hud);
		GUI.DrawTexture (frameRect, frame);
		GUI.DrawTexture (challengeTitleRect, challengeTitle);
		GUI.DrawTexture (challengeInfoRect, challengeInfo);	
		GUI.DrawTexture (buyTicketsRect, buyTickets);

		if (GUI.Button (backsRect,"",backsStyle)) {
			WorkAround.LoadLevelWorkaround("ChallengesScene");
		}
		if (GUI.Button (optionsRect,"",optionsStyle)) {
			Properties.PreviousScene = "UnLockedChallenge";
			WorkAround.LoadLevelWorkaround("Options");
			
		}
		if (GUI.Button (helpRect,"",helpStyle)) {
			Properties.PreviousScene = "UnLockedChallenge";
			WorkAround.LoadLevelWorkaround("Help");
		}
		if (GUI.Button (storeRect,"",storeStyle)) {
			
		}
		
		GUI.DrawTexture (bspotRect, bspotLoggedIn);
		GUI.DrawTexture (gameplayBalRect, gameplayBal);
		GUI.DrawTexture (cashBalRect, cashBal);
		GUI.Label (gameplayBalValueRect, Properties.GamePlayBalance.ToString(), balanceStyle);
		GUI.Label (cashBalValueRect, Properties.CashBalance.ToString(), balanceStyle);
		
		//		GUI.Label (ticketsRect, "Tickets :"+Properties.TotalTickets, style2);
		//		GUI.Label (gameplayRect, "GamePlay: $"+Properties.GamePlayBalance, style2);
		//
		//		GUI.Label (labelRect, "Challenge Screen", style);
		//		GUI.Label (ticketsLabelRect, ""+Properties.MoreTickets, style1);
		//		GUI.DrawTexture (textureRect,texture);
		//		if (GUI.Button (returnTextureRect, returnTexture)) {
		//			WorkAround.LoadLevelWorkaround("ChallengesScene");
		//		}
	}
	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}
}
