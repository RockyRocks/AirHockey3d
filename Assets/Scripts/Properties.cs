using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Properties {

	public static bool isPopUpShown = false;
	public static List<int>playerScores = new List<int>();
	public static int LeaderBoardSize = 10;
	
	public static string BaseUrl="https://sb.oddz.com/api/";
	public static string Version="v1.2";
	public static string WonAmount = "";
	public static string ChallengeMode;

	public static string ParentScene;
	public static string PreviousScene;
    public static string CurrentScene;

	public static float SelectedDenomination;
    public static double TotalWinnings = 0;
	public static double GamePlayBalance = 0;
	public static double CashBalance = 0;
	public static string SelectedChallenge;
	public static string CurrentScore;
	public static bool isScoreHightest = false;
    public static bool isChallengeFinished = false;
	public static string HighestScore;

	//number of more tickets needed to unlock the challenge
	public static int MoreTickets;
	public static float Tickets;
	public static float TotalTickets;
    public static bool S_CantakeInputs = false;
    public static bool S_GoingtoPlay = false;
	public static string GameWonState;
	public static int TotalFrames;
	public static string SessionKey;
    public static bool IsLoggedIn = false;
    public static bool DeleteSession=false;
	public static string AcessKey;

	public static float sfxVolume = 0.6f;
	public static float musicVolume = 0.6f;
    public static bool IsLogoTransitionsAllowed = false;
    public static float voiceOverVolumer = 0f;

    // air hockey static properties
    public static int HumanplayerGoals = 0;
    public static int NoofBlocks = 0;
    public static int AIPlayerGoals = 0;
    public static int NoofWalls = 0;
    public static int Attempts=0;
    public static int TotalAttempts = 0;
    public static int Rounds = 0;
	public static int semis_1;
	public static int semis_2;
	public static int semis_3;
	public static int finals_1;

	public enum RequestType{
		none,
	    Session,
		Account,
		GamePlay,
        GamePlay_Events,
		Play,
		Balances,
		WithDraw,
		DeleteSession,
		CancelTokens,
	};
    
    public enum Modes{
        PlayforFun,
        PlayforMoney
    };

    public static Modes GameType;
    public enum PlayForFunMode{
        ARCADE,
        TOURNAMENT
    };
    public static PlayForFunMode PlayMode;
    public enum Countries{
        GreatBritian,
        Germany,
        USA,
        CzechRepublic,
        Finland,
        Russia,
        Canada,
        Sweden
    };
    public static Countries countries;
	public static Countries AiCountries;

    public static Vector2[] challengesNamePositions = new Vector2[] {
        new Vector2(130f,0),
         new Vector2(130f,0),
          new Vector2(132f,0),
           new Vector2(130f,0),
            new Vector2(130f,0),
             new Vector2(94f,0),
              new Vector2(57f,0),
               new Vector2(120f,0),
                new Vector2(55f,0),
                 new Vector2(75f,0)
    };

    public static Vector2[] LadderPositions=new Vector2[]{
        //new Vector2(84f,117f),
        new Vector2(84f,173f),
         new Vector2(84f,217f),
          new Vector2(84f,264f),
           new Vector2(84f,310f),
            new Vector2(84f,361f),
             new Vector2(84f,411f),
               new Vector2(84f,464f),// quarters positions
                                   new Vector2(327f,137f),  // Player position in quarters
                                   new Vector2(327f,238f),
                                   new Vector2(327f,341f),
                                   new Vector2(327f,441f), // semis positions
                                                           new Vector2(570f,198f),//player position in semis
                                                           new Vector2(570f,375f), // final position/player position
                                                                                   new Vector2(796f,262f)

    };

    public static Dictionary<Countries, Texture2D> LoadLadderCoutries = new Dictionary<Countries, Texture2D>(){
        {Countries.GreatBritian,Resources.Load<Texture2D>("flags/eng_flag")},
         {Countries.Canada,Resources.Load<Texture2D>("flags/canada_flag")},
          {Countries.CzechRepublic,Resources.Load<Texture2D>("flags/croatia_flag")},
           {Countries.Finland,Resources.Load<Texture2D>("flags/finland_falg")},
            {Countries.Germany,Resources.Load<Texture2D>("flags/germany_flag")},
             {Countries.Russia,Resources.Load<Texture2D>("flags/russia_flag")},
              {Countries.Sweden,Resources.Load<Texture2D>("flags/sweden_flag")},
               {Countries.USA,Resources.Load<Texture2D>("flags/usa_flag")},
    };
    public static List<Texture2D> TempCountries= new List<Texture2D>();

    public static string[] ChallengesList = new string[]{
		"Score a goal after bouncing the puck off of at least one wall.",
		"Successfully defeat opponent without giving up a goal.",
		"Successfully block 5 shots on your goal",
		"Successfully block 10 shots on your goal",
		"Score three consecutive goals without being scored on.",
		"Score on opponent goal with the opponents immobile mallet in front of the goal",
		"Score a goal after bouncing the puck off of at least 4 walls.",
		"Score ten goals without being scored on",
		"Shoot the puck off your opponent's back boards, ricochet to your back boards and go into opponent's goal.",
		"Successfully block twenty shots on your goal"
	};


    public static Dictionary<string, string> challengeDictionary = new Dictionary<string, string>(){
		{ChallengesList[0], "Bank Shot"},
		{ChallengesList[1], "Shutout"},
		{ChallengesList[2], "Defender"},
		{ChallengesList[3], "The Wall"}, 
		{ChallengesList[4], "Hat Trick"},
		{ChallengesList[5], "Penalty Shot"},
		{ChallengesList[6], "Ricochet Champ"},
		{ChallengesList[7], "Destroyer"},
		{ChallengesList[8], "Off the Boards"},
		{ChallengesList[9], "The Great Wall"}
	};

	//Dictionary contains challengeName,[win,lose,unlock value] 
	public static Dictionary<string,ChallengeProperties> winLoseDictionary = new Dictionary<string, ChallengeProperties>(){
		{challengeDictionary[ChallengesList[0]] , new ChallengeProperties(0,challengeDictionary[ChallengesList[0]],100,50,00,false,2)},
		{challengeDictionary[ChallengesList[1]] , new ChallengeProperties(1,challengeDictionary[ChallengesList[1]],250,100,00,false,1)},
		{challengeDictionary[ChallengesList[2]] , new ChallengeProperties(2,challengeDictionary[ChallengesList[2]],350,100,150,false,3)},
		{challengeDictionary[ChallengesList[3]] , new ChallengeProperties(3,challengeDictionary[ChallengesList[3]],550,150,200,false,3)},
		{challengeDictionary[ChallengesList[4]] , new ChallengeProperties(4,challengeDictionary[ChallengesList[4]],750,150,200,false,5)},
		{challengeDictionary[ChallengesList[5]] , new ChallengeProperties(5,challengeDictionary[ChallengesList[5]],1000,150,200,false,5)},
		{challengeDictionary[ChallengesList[6]] , new ChallengeProperties(6,challengeDictionary[ChallengesList[6]],1250,250,250,false,5)},
		{challengeDictionary[ChallengesList[7]] , new ChallengeProperties(7,challengeDictionary[ChallengesList[7]],1500,250,275,false,5)},
		{challengeDictionary[ChallengesList[8]] , new ChallengeProperties(8,challengeDictionary[ChallengesList[8]],2000,500,275,false,3)},
		{challengeDictionary[ChallengesList[9]] , new ChallengeProperties(9,challengeDictionary[ChallengesList[9]],3000,125,275,false,2)}

	};    
}
