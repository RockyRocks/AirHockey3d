using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScript : MonoBehaviour {

//	public AudioClip buttonClip;
//	public AudioClip pinCollisionClip;
//	public AudioClip winClip;
//	public AudioClip loseClip;
    public AudioClip[] ChallengeWin_audio;
    public AudioClip[] Challengemiss_audio;
	public Texture2D helpButton;
	public Texture2D exitButton;
	public Texture2D pauseButton;
	public Texture2D winnerTexture;
	public Texture2D winnerTexture_ticket;
	public Texture2D exitPopupBG;
	public Texture2D exitPopupText;
	public Texture2D okButton;
	public Texture2D cancelButton;

	public Rect okButtonRect;
	public Rect cancelButtonRect;
	public Texture2D buttonBG;
	public GUIStyle helpStyle;
	public GUIStyle pauseStyle;
	public GUIStyle exitStyle;
	public GUIStyle buttonStyle;



	public Rect wonRect;
	public Rect wonRect_Tickets;
	

	//int player1Score;
	int totaltryCount;
	//int player2Score;
	//int pinsFallen;

	//int try1Count;
	//int try2Count;

	Properties.Modes gameMode;
	string challengeMode;
	bool isWinTexure = false;


	int buttonWidth = 48;
	int buttonHeight = 48;

	private static GameScript script;
    PuckBehaviour puck;
    PlayerController Aiplayer;
    System.DateTime Actualtime;
    System.DateTime Currenttime;

	public static GameScript Get(){
		return script;
	}

	public int totalFrames = 0;
	//bool showFrameText = false;
	// Use this for initialization
	void Start () {

		audio.Stop();

		okButtonRect = new Rect (360, 335, buttonBG.width, buttonBG.height);
		cancelButtonRect = new Rect (530, 335, buttonBG.width, buttonBG.height);

		script = this;
        puck = GameObject.FindGameObjectWithTag("Puck").GetComponent<PuckBehaviour>();
        Aiplayer = GameObject.FindGameObjectWithTag("AIPlayer").GetComponent<PlayerController>();
		wonRect = new Rect (470,340,50,50);
		wonRect_Tickets = new Rect (465,330,50,50);
		gameMode = Properties.GameType;
		challengeMode = Properties.ChallengeMode;
        Properties.GameWonState = "IsPlaying";
		Properties.isPopUpShown = false;
        Actualtime = System.DateTime.Now;

		totaltryCount = 1;

		List<int> playerScoreList = new List<int> ();
		for (int i = 0; i < 10; i++) {
			if(PlayerPrefs.HasKey("PlayerScore"+i))
				playerScoreList.Add(int.Parse(PlayerPrefs.GetString("PlayerScore"+i)));
		}

		if (playerScoreList.Count != 0) {
			Properties.playerScores = playerScoreList;
		}
	}

	//bool isLoadWonScene = false;

	public void YouWin(){
		//StartCoroutine (PlayAudio(null,"Win"));
	}

	public void YouLose(){
        StartCoroutine(PlayAudio(Challengemiss_audio[Random.Range(0,2)], "Lose"));
	}

	public void UpdateTickets(){

		ChallengeProperties val = null;
		val = Properties.winLoseDictionary[challengeMode];

		if (Properties.GameWonState == "Lose") {
			if(val!=null)
			{
				Properties.Tickets = val.LoseAmount;
				Properties.TotalTickets += Properties.Tickets;
			}
		} 
		else {
			if(val!=null){
				Properties.Tickets = val.WinAmount;
				Properties.TotalTickets += Properties.Tickets;
			}
		}
		PlayerPrefs.SetInt("Total Tickets", (int)Properties.TotalTickets);
		PlayerPrefs.Save ();
	}


	public bool isUpdate = false;

	public GUIStyle labelStyle;
	public GUIStyle wonrectStyle;

	//float timeElapsed = 0f;


	IEnumerator ChallengeSceneWait(){
		yield return new WaitForSeconds (1.8f);
		WorkAround.LoadLevelWorkaround("ChallengesScene");
	}

	//float timeOut = 0;

	IEnumerator ResultsWait(){
		yield return new WaitForSeconds (1f);
		WorkAround.LoadLevelWorkaround("Results");
	}

	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
        audio.volume = Properties.sfxVolume;
        audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length);
		switch (buttonName) {
		case "Help":
			Properties.PreviousScene = "Airhockey";
			WorkAround.LoadLevelWorkaround("Help");
			break;
		case "Options":
			Properties.PreviousScene = "Airhockey";
			WorkAround.LoadLevelWorkaround("Options");
			break;
		case "Back":
			break;
		case "Win":
			isWinTexure = true;
			Properties.GameWonState = "Win";
			BasicHandShake.GamePlayReveal_flag = true;
            if (Properties.GameType == Properties.Modes.PlayforMoney)
			UpdateTickets ();
			StartCoroutine (ResultsWait());
			break;

		case "Lose":
			Properties.GameWonState = "Lose";
                if(Properties.GameType==Properties.Modes.PlayforMoney)
			    UpdateTickets ();
			    WorkAround.LoadLevelWorkaround ("Results");
			break;

		case "Ok":
			    Properties.isPopUpShown = false;
			    WorkAround.LoadLevelWorkaround(Properties.ParentScene);
			break;
		}
	}

	public IEnumerator ShowScoreScene(){
		yield return new WaitForSeconds(1f);
		WorkAround.LoadLevelWorkaround("ScoreScene");
	}
	public void ResetPlayForFun(){

		Properties.CurrentScore = Scoring.Get().totalScore.ToString();
		if (Properties.playerScores.Count > 0) {
			Properties.HighestScore = Properties.playerScores [0].ToString ();
			if (int.Parse (Properties.CurrentScore) > int.Parse (Properties.HighestScore))
					Properties.isScoreHightest = true;
			else
					Properties.isScoreHightest = false;
		}
		else
			Properties.isScoreHightest = false;

		Properties.playerScores.Add (Scoring.Get().totalScore);
		Properties.playerScores.Sort();
		Properties.playerScores.Reverse ();

		Properties.HighestScore = Properties.playerScores [0].ToString();

		if (Properties.playerScores.Count > Properties.LeaderBoardSize) {
			Properties.playerScores.RemoveRange(Properties.LeaderBoardSize,Properties.playerScores.Count - Properties.LeaderBoardSize);
		}

		for(int i=0;i<Properties.playerScores.Count;i++)
		{
			PlayerPrefs.SetString("PlayerScore"+i,Properties.playerScores[i].ToString());
		}

		PlayerPrefs.Save();
		Scoring.Get ().ResetScore ();

		StartCoroutine (ShowScoreScene());

	}
	//bool frameNoShown = true;
	bool audioPinPlayed = false;

	public void PlayPinAudio(){

		if(!audioPinPlayed)
		{
			audio.volume = Properties.sfxVolume;
			audioPinPlayed = true;
		}
	}

	public void CheckEndOfFrames(){
		if(totalFrames >= (TOTALFRAMES)){
			totalFrames = 0;
			ResetPlayForFun();
		}
	}

	int TOTALFRAMES = 10;
	int bonusFrameCount = 0;
	bool bonusFrame = false;


	// Update is called once per frame
    // No Ai on challenges 1, 6, 7 and 9
	void Update () {
        switch (gameMode)
        {
            case Properties.Modes.PlayforFun:

                //CheckEndOfFrames();
                if (Properties.HumanplayerGoals == 7 && Properties.HumanplayerGoals>Properties.AIPlayerGoals)
                {
                    Properties.HumanplayerGoals = 0;
                    Properties.AIPlayerGoals = 0;
                    if (Properties.PlayMode == Properties.PlayForFunMode.ARCADE)
                    {
                        //this.YouWin();
                        StartCoroutine(PlayAudio(ChallengeWin_audio[0], "Win"));
                    }
                    else if (Properties.PlayMode == Properties.PlayForFunMode.TOURNAMENT)
                    {

                        Properties.Rounds++;
                        if (Properties.Rounds > 2)
                        {
                            Properties.HumanplayerGoals = 0;
                            Properties.AIPlayerGoals = 0;
                            Properties.Rounds = 0;
                            Properties.TempCountries.Clear();
                            WorkAround.LoadLevelWorkaround("LeaderBoard");
                        }
                        else
                        {
                            Properties.HumanplayerGoals = 0;
                            Properties.AIPlayerGoals = 0;
                            WorkAround.LoadLevelWorkaround("ladder");
                        }
                    }
                }
                else if (Properties.AIPlayerGoals > Properties.HumanplayerGoals && Properties.AIPlayerGoals == 7)
                {
                    Properties.HumanplayerGoals = 0;
                    Properties.AIPlayerGoals = 0;
                    Properties.Rounds = 0;
                    this.YouLose();
                }
                break;

            case Properties.Modes.PlayforMoney:
                switch (challengeMode)
                {
                    case "Bank Shot":
                        Aiplayer.gameObject.SetActive(false);
                        this.bankshot();
                        break;

                    case "Shutout":
                        Currenttime = System.DateTime.Now;
                        System.TimeSpan span = Currenttime.Subtract(Actualtime);
                        if (span.Minutes >= 5)
                        {
                            this.shutout();
                        }
                        else if (Properties.AIPlayerGoals > 0)
                        {
							Properties.AIPlayerGoals=0;
                            this.UpdateAttempts();
                        }
                        break;

                    case "Defender":
                        this.defender();
                        break;

                    case "The Wall":
                        this.thewall();
                        break;

                    case "Hat Trick":
                        this.hattrick();
                        break;

                    case "Penalty Shot":
                        Aiplayer.rigidbody.velocity=Vector3.zero;
                        this.penaltyshot();
                        break;

                    case "Ricochet Champ":
                        Aiplayer.gameObject.SetActive(false);
                        this.ricochetchamp();
                        break;

                    case "Destroyer":
                        this.destroyer();
                        break;

                    case "Off the Boards":
                        Aiplayer.gameObject.SetActive(false);
                        this.offtheboards();
                        break;

                    case "The Great Wall":
                        this.thegreatwall();
                        break;
                }
                break;
        }
	}

	 void bankshot(){
         if (puck.GoalScored)
         {
             if (Properties.NoofWalls >= 1)
             {
                 Properties.NoofWalls = 0;
                 puck.GoalScored = false;
                 Properties.HumanplayerGoals = 0;
                 Properties.AIPlayerGoals = 0;
                 //this.YouWin();
                 StartCoroutine(PlayAudio(ChallengeWin_audio[0], "Win"));
             }
             else
             {
                 puck.PlayerHits = 0;
                 puck.GoalScored = false;
                 Properties.HumanplayerGoals = 0;
                 Properties.AIPlayerGoals = 0;
                 //puck.Reset();
                 this.UpdateAttempts();
                 puck.RandomPuckReSpawn();
             }
         }
         else if (!puck.GoalScored && puck.PlayerHits > 1) 
         {
             puck.PlayerHits = 0;
             Properties.NoofWalls = 0;
             puck.GoalScored = false;
             Properties.HumanplayerGoals = 0;
             Properties.AIPlayerGoals = 0;
             //puck.Reset();
             this.UpdateAttempts();
             puck.RandomPuckReSpawn();
         }
	}
     void shutout() {
         if (Properties.HumanplayerGoals > 0 && Properties.AIPlayerGoals == 0)
         {
             Properties.HumanplayerGoals = 0;
             Properties.AIPlayerGoals = 0;
             //this.YouWin();
             StartCoroutine(PlayAudio(ChallengeWin_audio[1], "Win"));
         }
         else
         {
             Properties.HumanplayerGoals = 0;
             Properties.AIPlayerGoals = 0;
             this.YouLose();
         }
     }
     void defender() 
     {
         Currenttime = System.DateTime.Now;
         System.TimeSpan span = Currenttime.Subtract(Actualtime);
         if (Properties.NoofBlocks >= 5 && Properties.AIPlayerGoals == 0 && span.Minutes <= 5)
         {
			 Properties.NoofBlocks=0;
             //this.YouWin();
             StartCoroutine(PlayAudio(ChallengeWin_audio[2], "Win"));
         }
         else if (Properties.NoofBlocks<5 && Properties.AIPlayerGoals>0)
         {
			 Properties.AIPlayerGoals=0;
			 Properties.NoofBlocks=0;
             this.UpdateAttempts();
         }
     }
     void thewall() 
     {
         Currenttime = System.DateTime.Now;
         System.TimeSpan span = Currenttime.Subtract(Actualtime);
         if (Properties.NoofBlocks >= 10 && Properties.AIPlayerGoals == 0 && span.Minutes<=10)
         {
			 Properties.NoofBlocks = 0;
             //this.YouWin();
             StartCoroutine(PlayAudio(ChallengeWin_audio[3], "Win"));
             
         }
         else if(Properties.NoofBlocks<10 && Properties.AIPlayerGoals>0)
         {
			 Properties.AIPlayerGoals = 0;
             Properties.NoofBlocks = 0;
             this.UpdateAttempts();
         }
     }
     void hattrick() 
     {
         if (Properties.HumanplayerGoals >= 3 && Properties.AIPlayerGoals == 0)
         {
             Properties.HumanplayerGoals = 0;
             Properties.AIPlayerGoals = 0;
             //this.YouWin();
             StartCoroutine(PlayAudio(ChallengeWin_audio[4], "Win"));
         }
         else if (Properties.AIPlayerGoals > 0)
         {
             Properties.AIPlayerGoals = 0;
             Properties.HumanplayerGoals = 0;
             this.UpdateAttempts();
         }
     }
     void penaltyshot() {
         if(Properties.HumanplayerGoals>0 && puck.PlayerHits>0){
			 Properties.HumanplayerGoals=0;
			 puck.PlayerHits=0;
             //this.YouWin();
             StartCoroutine(PlayAudio(ChallengeWin_audio[5], "Win"));
         }else if (Properties.HumanplayerGoals == 0 && puck.PlayerHits > 1){
             Properties.HumanplayerGoals = 0;
             puck.PlayerHits = 0;
             this.UpdateAttempts();
             puck.RandomPuckReSpawn();
         }
     }
     void ricochetchamp() {
         if (puck.GoalScored){
             if (Properties.NoofWalls >= 4 && puck.GoalScored){
				 Properties.NoofWalls=0;
				 puck.GoalScored=false;
                 //this.YouWin();
                 StartCoroutine(PlayAudio(ChallengeWin_audio[6], "Win"));
             }else if (Properties.NoofWalls < 4 && puck.GoalScored){
                 puck.PlayerHits = 0;
				 Properties.NoofWalls=0;
				 puck.GoalScored=false;
                 //puck.Reset();
                 this.UpdateAttempts();
                 puck.RandomPuckReSpawn();
             }
         }else if(Properties.NoofWalls > 4 && !puck.GoalScored){
             puck.PlayerHits = 0;
			 Properties.NoofWalls=0;
             //puck.Reset();
             this.UpdateAttempts();
             puck.RandomPuckReSpawn();
         }
     }
     void destroyer() 
     {
         if (Properties.HumanplayerGoals >= 10 && Properties.AIPlayerGoals == 0){
             Properties.HumanplayerGoals = 0;
             Properties.AIPlayerGoals = 0;
             //this.YouWin();
             StartCoroutine(PlayAudio(ChallengeWin_audio[7], "Win"));
         }else if (Properties.AIPlayerGoals > 0){
             Properties.HumanplayerGoals = 0;
             Properties.AIPlayerGoals = 0;
             this.UpdateAttempts();
         }
     }
     void offtheboards() 
     {
         if (puck.Topposthit){
             if (puck.bottomposthit){
                 if (Properties.HumanplayerGoals > 0 && puck.PlayerHits==1){
                     puck.Topposthit = false;
                     puck.bottomposthit = false;
					 puck.PlayerHits=0;
                     Properties.HumanplayerGoals = 0;
                     //this.YouWin();
                     StartCoroutine(PlayAudio(ChallengeWin_audio[8], "Win"));
                 }else if(puck.PlayerHits>2){
                     puck.bottomposthit = false;
                     puck.Topposthit = false;
					 puck.PlayerHits=0;
                     Properties.HumanplayerGoals = 0;
                     this.UpdateAttempts();
                     puck.RandomPuckReSpawn();
                 }
             }
         }
         else if (!puck.Topposthit && puck.bottomposthit){
             puck.Topposthit = false;
             puck.bottomposthit = false;
             Properties.HumanplayerGoals = 0;
             this.UpdateAttempts();
             puck.RandomPuckReSpawn();
         }
         else if (puck.Topposthit && !puck.bottomposthit && Properties.HumanplayerGoals>0){
             puck.Topposthit = false;
             puck.bottomposthit = false;
             Properties.HumanplayerGoals = 0;
             this.UpdateAttempts();
             puck.RandomPuckReSpawn();
         }
     }
     void thegreatwall() {
         Currenttime = System.DateTime.Now;
         System.TimeSpan span = Currenttime.Subtract(Actualtime);
             if (Properties.NoofBlocks >= 20 && Properties.AIPlayerGoals == 0 && span.Minutes<=20){
			     Properties.NoofBlocks = 0;
                 //this.YouWin();
                 StartCoroutine(PlayAudio(ChallengeWin_audio[9], "Win"));
             }else if (Properties.NoofBlocks < 20 && Properties.AIPlayerGoals > 0){
                 Properties.NoofBlocks = 0;
				 Properties.AIPlayerGoals=0;
                 this.UpdateAttempts();
             }
     }

	public void UpdateFrame(){

        //if (totaltryCount >= 2) {
        //    totalFrames++;
        //    if(totalFrames == (TOTALFRAMES-1) && bonusFrame)
        //        //Scoring.Get().UpdateFrame(bonusFrameCount, "Bonus Frame ");	
        //    else
        //        if(totalFrames < TOTALFRAMES)
        //            //Scoring.Get().UpdateFrame(totalFrames, "Frame ");

        //    totaltryCount = 1;

        //} else
        //    totaltryCount++;
	}

	public void UpdateScore(){

	}
    int attempt = 0;
    public void UpdateAttempts()
    {
        if (Properties.TotalAttempts <= 1){
            Properties.Attempts = 0;
            YouLose();
        }else{
            Properties.TotalAttempts--;
            Properties.Attempts++;
            Scoring.Get().UpdateAttemptTexture();
        }
    }

    //public void CalculateFramePoints(int pinsDown){

    //    //if (totaltryCount == 1) {
    //    //    try1Count = pinsDown;
    //    //}
    //    //else
    //    //    if (totaltryCount == 2)
    //    //        try2Count = pinsDown;
    //}

    //void UpdateScore(int noOfPinsFall, int frameCount, int hitType){
    //    Scoring.Get ().UpdateScore (noOfPinsFall, frameCount, hitType);
    //}

	void OnGUI(){

        //if(GUI.Button (new Rect(0, Screen.height - buttonHeight, buttonWidth, buttonHeight), "", exitStyle)){
        //    Properties.isPopUpShown = true;
        //    StartCoroutine(PlayAudio(null, "Back"));
        //}

        //if(GUI.Button (new Rect (0, 0, buttonWidth, buttonHeight), "", pauseStyle)){
        //    StartCoroutine(PlayAudio(null, "Options"));
        //}

        //if(GUI.Button (new Rect (Screen.width - buttonWidth,0, buttonWidth,buttonHeight), "", helpStyle)){
        //    StartCoroutine(PlayAudio(null, "Help"));
        //}
//		if (isWinTexure )
//		{
//			if(Properties.WonAmount!="0")
//			{
//				Debug.Log(Properties.WonAmount);
//				//GUILayout.BeginArea(new Rect(Screen.width / 2 - 256 / 2, Screen.height / 2 - 256 / 2, 256, 256/* winnerTexture.width, winnerTexture.height*/));
//				GUI.DrawTexture (new Rect (Screen.width / 2 - winnerTexture.width / 2, Screen.height / 2 - winnerTexture.height / 2,  winnerTexture.width, winnerTexture.height), winnerTexture);
//				GUI.Label (wonRect,Properties.WonAmount, labelStyle);
//			}
//			else
//			{
//				Debug.Log(Properties.WonAmount);
//				GUI.DrawTexture (new Rect (Screen.width / 2 - winnerTexture.width / 2, Screen.height / 2 - winnerTexture.height / 2,  winnerTexture.width, winnerTexture.height), winnerTexture_ticket);
//				GUI.Label (wonRect_Tickets,Properties.TotalTickets.ToString(), wonrectStyle);
//			}
//           // GUILayout.EndArea();
//		}
        //if (Properties.isPopUpShown) {
        //    GUI.DrawTexture(new Rect(Screen.width/2 - exitPopupBG.width/2,Screen.height/2 - exitPopupBG.height/2,exitPopupBG.width,exitPopupBG.height),exitPopupBG);
        //    GUI.DrawTexture(new Rect(Screen.width/2 - exitPopupText.width/2,Screen.height/2 - exitPopupText.height/2,exitPopupText.width,exitPopupText.height),exitPopupText);

        //    if(GUI.Button(okButtonRect, okButton, buttonStyle)){
        //        StartCoroutine(PlayAudio(null, "Ok"));
        //    }
        //    if(GUI.Button(cancelButtonRect, cancelButton, buttonStyle)){
        //        StartCoroutine(PlayAudio(null, "Cancel"));
        //        Properties.isPopUpShown = false;
        //    }
        //}
	}

	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}
}
