using UnityEngine;
using System.Collections;

public class Challenge : MonoBehaviour
{

    public Texture2D challengeBG;
    public Texture2D lockTexture;
    public Texture2D unLockTexture;
	public Texture2D pinPlacementTexture;
	public Texture2D attemptsTexture;
	public Texture2D ticketsAngledTexure;
    //public Texture2D Select;
	public GameObject pinsCollection;

	public Texture2D challengeNameTexture;
	public Texture2D challengeNumberTexture;
	public Texture2D challengeDescTexture;
	public Texture2D ChallengeImage;

    //public Rect SelectRect;
	public Rect ChallengeImageRect;
	public Rect challengeDescRect;

	public string challengeName = "";
	public string challengeNumber = "";
    public string ticketLabel = "";

    public Rect challengeNumberRect;
    public Rect lockTextureRect;
    public Rect challengeNameRect;
    public Rect challengeRect;
    public Rect challenge1Rect;
	public Rect label1;
	public Rect label2;
	public Rect attemptsTextureRect;
	public Rect ticketsAngledRect;

    public GUIStyle SelectStyle;
    public GUIStyle style;
	public GUIStyle unlockStyle;
	public GUIStyle attemptStyle;
	public string name;
    public bool isLocked;
    public Vector2 position;

	string label_unlock;
	string label_to_unlock;
    public static bool Select_flag=false;
	int attempts = 0;
	int unLockTickets;
	// Use this for initialization
    void Start()
    {
		challengeNumberRect = new Rect(0, 0, challengeNumberTexture.width, challengeNumberTexture.height);
        challengeNameRect = new Rect( position.x, 0, challengeNameTexture.width, challengeNameTexture.height);
		challengeDescRect = new Rect (10, 45, challengeDescTexture.width, challengeDescTexture.height);
		ChallengeImageRect = new Rect (400, 40, ChallengeImage.width/2,ChallengeImage.height);

		challengeRect = new Rect(0, -30, challengeBG.width, challengeBG.height);

        lockTextureRect = new Rect(210, 32, lockTexture.width, lockTexture.height);

		attemptsTextureRect = new Rect (340, 436, attemptsTexture.width, attemptsTexture.height);
		ticketsAngledRect = new Rect (600, 108, ticketsAngledTexure.width, ticketsAngledTexure.height);
       // SelectRect = new Rect(Screen.width / 2 - Select.width / 2, Screen.height - (Select.height + 10), Select.width, Select.height);
		unLockTickets = Properties.winLoseDictionary[Properties.challengeDictionary[challengeName]].UnLockAmount;
		attempts = Properties.winLoseDictionary [Properties.challengeDictionary [challengeName]].Attempts;
	}

	int width = 170;
	int height= 100;
	 
	public GUIStyle challengeBGStyle;

    void OnGUI()
    {
		if (!Properties.isPopUpShown) {

			GUI.depth = 1;

			GUI.BeginGroup (new Rect (transform.position.x, transform.position.y, challengeBG.width , challengeBG.height));

			Vector2 sizeOfLabel;

			if(isLocked){

                if (GUI.Button(new Rect(challengeRect.width / 2 - lockTextureRect.width / 2, 
                                        challengeRect.height / 2 - lockTextureRect.height / 2, 
                                        lockTextureRect.width, lockTextureRect.height), lockTexture, style) || Select_flag){
                    Select_flag = false;
					Properties.ChallengeMode = Properties.challengeDictionary [challengeName];
					
					Properties.SelectedChallenge = name;

					if(Properties.TotalTickets >= 	unLockTickets){
						PlayerPrefsX.SetBool(Properties.winLoseDictionary[Properties.ChallengeMode].Name,false);
						
						Properties.TotalTickets -= unLockTickets;
						
						PlayerPrefs.SetInt("Total Tickets", (int)Properties.TotalTickets);
						
						PlayerPrefs.Save();

						isLocked = false;

						//pinPositionScript.isLocked = isLocked;

						//WorkAround.LoadLevelWorkaround ("UnLockChallenge");
					}else{
						WorkAround.LoadLevelWorkaround ("TicketsNeeded");
					}
				}

				GUI.Button(challengeRect, challengeBG, style);
				
				label_unlock = unLockTickets + " b TICKETS TO UNLOCK";
				sizeOfLabel = style.CalcSize (new GUIContent (label_unlock));
				label1 = new Rect(0,0,challengeRect.width,challengeRect.height);

				GUI.Label(label1, label_unlock, unlockStyle);
			}else{
				if(GUI.Button(challengeRect, challengeBG, style) || Select_flag){
                    Select_flag = false;
					Properties.isChallengeFinished = false;
					Properties.ChallengeMode = Properties.challengeDictionary [challengeName];
					Properties.TotalAttempts = Properties.winLoseDictionary[Properties.ChallengeMode].Attempts;
					Properties.SelectedChallenge = challengeNumber;
					WorkAround.LoadLevelWorkaround ("WagerScene");
				}

//				//title of the challenge
//				sizeOfLabel = style.CalcSize (new GUIContent (challengeNumber));
//				label1 = new Rect(challengeRect.width/2 - sizeOfLabel.x/2, 5, sizeOfLabel.x, sizeOfLabel.y);
//				GUI.Label(label1, challengeNumber, style);
//
//				//description of the challenge
//				sizeOfLabel = style.CalcSize (new GUIContent (challengeName));
//				label2 = new Rect(30,65,width,height);
//				GUI.Label(label2, challengeName, style);

				GUI.DrawTexture(challengeNameRect, challengeNameTexture);

				GUI.DrawTexture(challengeDescRect,challengeDescTexture);

				GUI.DrawTexture(ChallengeImageRect, ChallengeImage);

			}

			GUI.EndGroup ();

			//GUI.DrawTexture(challengeNumberRect, challengeNumberTexture);

			GUI.DrawTexture(ticketsAngledRect,ticketsAngledTexure);

			if(!isLocked)
			{
				GUI.DrawTexture(attemptsTextureRect,attemptsTexture);
			
				sizeOfLabel = style.CalcSize (new GUIContent (attempts.ToString()));
				GUI.Label(new Rect( attemptsTextureRect.xMax + 20, attemptsTextureRect.yMax - sizeOfLabel.y, sizeOfLabel.x,sizeOfLabel.y),attempts.ToString(),attemptStyle);
			}
		}
	}
}
