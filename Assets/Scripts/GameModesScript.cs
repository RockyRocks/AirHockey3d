using UnityEngine;
using System.Collections;

public class GameModesScript : MonoBehaviour {

    public Texture2D playForFun;
    public Texture2D playForCash;
    public Texture2D options;
    public Texture2D Bg;
    public Texture2D BSpot;
	public Texture2D frame;	
	public Texture2D help;
	public Texture2D ReturnLogo;
	public Texture2D PopupLogo;
	public Texture2D Ok;

	public AudioClip ReturnClick;
	public AudioClip CommonClick;

	public Texture2D Logo;
	public Texture2D podium;
	public Texture2D NotEnoughBalance;

	public Rect logoRect;
	public Rect bSpotRect;
    public Rect playForFunRect;
    public Rect playForCashRect;
    public Rect optionsRect;
	public Rect helpRect;
	public Rect podiumRect;
	public Rect Return;
	public Rect Popuprect;
	public Rect OkRect;

    public Rect storeRect;


    int startHeight = 0;
    int startWidth = 0;
    
	int buttonWidth = 64;
    int buttonHeight = 64;
	int border = 15;
	public GUIStyle style;
	public GUIStyle helpStyle;
	public GUIStyle optionsStyle;
	public GUIStyle podiumStyle;
	public GUIStyle PlayForFunStyle;
	public GUIStyle PlayForMoneyStyle;
	public GUIStyle OkStyle;

	bool showpopup=false;

    // Use this for initialization
    void Start () {

		Properties.ParentScene = "ModesScene";
		Properties.PreviousScene = "ModesScene";
        Properties.CurrentScene = "ModesScene";

		playForFunRect = new Rect (Screen.width/2 - playForFun.width/2, 325, playForFun.width, playForFun.height);
		playForCashRect = new Rect (Screen.width/2 - playForCash.width/2, 400, playForCash.width, playForCash.height);

		Return = new Rect (40,520, buttonWidth, buttonHeight);
		podiumRect = new Rect ( 125, 520, buttonWidth, buttonWidth);
		helpRect = new Rect (760,520, buttonWidth,buttonHeight);
		bSpotRect = new Rect (Screen.width/2 - BSpot.width/2, 120, BSpot.width, BSpot.height);
        logoRect = new Rect(Screen.width / 2 - Logo.width/4+80, 180, Logo.width/4, Logo.height/4);
		optionsRect = new Rect (840,520, buttonWidth, buttonHeight);
		Popuprect = new Rect (Screen.width/2-PopupLogo.width/2,Screen.height/2-PopupLogo.height/2,PopupLogo.width,PopupLogo.height);
		OkRect = new Rect (Popuprect.width-100,Popuprect.height-50,Ok.width,Ok.height);
		if (!showpopup)
			showpopup = true;
	}
    
	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length );
		switch (buttonName) {
		case "PlayForCash":
			break;
		case "PlayForFun":
			WorkAround.LoadLevelWorkaround("MainMenu");
			break;
		case "Help":
			WorkAround.LoadLevelWorkaround("Options");
			break;
		case "Settings":
			WorkAround.LoadLevelWorkaround("Settings");
			break;
		case "Back":

			break;
		case "LeaderBoard":
			WorkAround.LoadLevelWorkaround("LeaderBoard");
			break;
		}
	}
	int width = 480;
	int height = 300;
    void OnGUI(){

		GUI.depth = 0;

		GUI.DrawTexture (bSpotRect, BSpot);
        GUI.DrawTexture(logoRect, Logo);
        
		if (GUI.Button (Return, "", style)) {
			StartCoroutine(PlayAudio(ReturnClick, "Back"));
		}
		if (GUI.Button (podiumRect, "", podiumStyle)) {
			StartCoroutine(PlayAudio(CommonClick, "LeaderBoard"));
		}
		if (!showpopup) 
		{
				if (GUI.Button (playForFunRect, "", PlayForFunStyle)) {
						StartCoroutine (PlayAudio (CommonClick, "PlayForFun"));
						Properties.GameType = Properties.Modes.PlayforFun;
						Properties.ChallengeMode = "";
				}
				if (GUI.Button (playForCashRect, "", PlayForMoneyStyle)) {

						Properties.GameType = Properties.Modes.PlayforMoney;
						StartCoroutine (PlayAudio (CommonClick, "PlayForCash"));
                        //if (!Properties.IsLoggedIn)
                        //{
                        //    Application.OpenURL(BasicHandShake.loginUrl + "?successUrl=" + BasicHandShake.thisPage + "&apiKey=" + BasicHandShake.apiKey);
                        //    Debug.Log("testing this: " + (BasicHandShake.loginUrl + "?successUrl=" + BasicHandShake.thisPage + "&apiKey=" + BasicHandShake.apiKey));
                        //}
                        //else 
                        {
								WorkAround.LoadLevelWorkaround ("ChallengesScene");
						}
				}
		}

		if (GUI.Button (optionsRect, "",optionsStyle)) {

			StartCoroutine(PlayAudio(CommonClick, "Help"));
		}

		if (GUI.Button (helpRect, "", helpStyle)) {
			StartCoroutine(PlayAudio(CommonClick, "Settings"));
		}

		if (showpopup) 
		{
			GUI.DrawTexture (Popuprect, PopupLogo);
			if (GUI.Button (OkRect, "", OkStyle)) {
				showpopup=false;
			}
		}
    }
}
