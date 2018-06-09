using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour {

	public GUIStyle label;
	
	public Texture2D rightBg;
	public Texture2D ATBg;
    public Texture2D leftBg;

	public Texture2D Home;
	public Texture2D Visitor;

	public Texture2D stakes;
	public Texture2D attempts;

    //public Texture2D challengeNumber;
    //public Texture2D challengeName;
    //public Texture2D challengeDescp;
    //public Texture2D ChallengeImage;

	//public Rect ChallengeImageRect;
	public Rect rightBgRect;
    public Rect leftBgRect;
	public Rect ATBgRect;
	public Rect stakesRect;
	public Rect attemptsRect;
    public Rect HomeScore;
    public Rect VisitorScore;
	//public Rect challengeNumberRect;
	//public Rect challengeNameRect;
	//public Rect challengeDescpRect;
	public Rect attemptsLeftRect;
	public Rect stakesleftRect;
	public Rect HomeRect;
	public Rect VisitorRect;

	public GUIStyle labelStyle;
	// Use this for initialization
	void Start () {

		Properties.CurrentScene = "Airhockey";

        //if (Properties.GameType == Properties.Modes.PlayforMoney)
        {
            //int selectedChallengeNumber = Properties.winLoseDictionary[Properties.SelectedChallenge].Index;
            //challengeNumber = Resources.Load<Texture2D>("Challenges/Challenge" + (selectedChallengeNumber + 1) + "/Challenge");
            //challengeName = Resources.Load<Texture2D>("Challenges/Challenge" + (selectedChallengeNumber + 1) + "/Name");
            //challengeDescp = Resources.Load<Texture2D>("Challenges/Challenge" + (selectedChallengeNumber + 1) + "/Description");
            //ChallengeImage = Resources.Load<Texture2D>("Challenges/Challenge" + (selectedChallengeNumber + 1) + "/Image");
            //ChallengeImageRect = new Rect(rightBgRect.width / 2 - ChallengeImage.width / 2, 90, ChallengeImage.width, ChallengeImage.height);
           // challengeNumberRect = new Rect(rightBgRect.width / 2 - challengeNumber.width / 2, 30, challengeNumber.width, challengeNumber.height);
			//challengeNameRect = new Rect(rightBgRect.width / 2 - challengeName.width / 2, 80, challengeName.width, challengeName.height);
			//challengeDescpRect = new Rect(rightBgRect.width / 2 - challengeDescp.width / 2, 140, challengeDescp.width, challengeDescp.height);

            rightBgRect = new Rect(Screen.width - rightBg.width+10, 0, rightBg.width, rightBg.height);
            leftBgRect = new Rect(0, 0, leftBg.width, leftBg.height);

            stakesRect = new Rect(20, 18, stakes.width, stakes.height);
            attemptsRect = new Rect(102, 18, attempts.width, attempts.height);
            HomeRect = new Rect(20, 18, Home.width, Home.height);
            VisitorRect = new Rect(120, 18, Visitor.width, Visitor.height);
        }

        if (Properties.GameType == Properties.Modes.PlayforFun){
            ATBgRect = new Rect(183, 0, ATBg.width, 85);
            HomeRect = new Rect(340, 18, Home.width, Home.height);
            VisitorRect = new Rect(540, 18, Visitor.width, Visitor.height);

            // need to initialize ui for arcade mode here.
            if (Properties.PlayMode == Properties.PlayForFunMode.ARCADE) {

            }
            else if (Properties.PlayMode == Properties.PlayForFunMode.TOURNAMENT){

            }
        }

	}
	
	void OnGUI(){
		GUI.depth = 1;

		if (Properties.GameType == Properties.Modes.PlayforMoney) {
            GUI.DrawTexture(rightBgRect, rightBg);

            GUI.BeginGroup(rightBgRect);{
                var rect = new Rect(stakesRect.x, stakesRect.y + 30, stakesRect.width, stakesRect.height);
                Vector2 sizeOfLabel;
                GUI.BeginGroup(rect);{
                    string stakesString = "$" + Properties.SelectedDenomination;
                    sizeOfLabel = label.CalcSize(new GUIContent(stakesString));
                    GUI.Label(new Rect(rect.width / 2 - sizeOfLabel.x / 2, rect.height / 2 - sizeOfLabel.y / 2, sizeOfLabel.x, sizeOfLabel.y), stakesString, label);
                }
                GUI.EndGroup();
                //TODO adding the attempts from challenges
                var rect1 = new Rect(attemptsRect.x-20, attemptsRect.y + 30, attemptsRect.width, attemptsRect.height);
                GUI.BeginGroup(rect1);{
                    string attemptsString = Properties.Attempts.ToString() + " of "+ Properties.winLoseDictionary[Properties.SelectedChallenge].Attempts.ToString();
                    sizeOfLabel = label.CalcSize(new GUIContent(attemptsString));
                    GUI.Label(new Rect(rect1.width / 2 - sizeOfLabel.x / 2, rect1.height / 2 - sizeOfLabel.y / 2, sizeOfLabel.x, sizeOfLabel.y), attemptsString, label);
                }
                GUI.EndGroup();

                GUI.DrawTexture(stakesRect, stakes);
                GUI.DrawTexture(attemptsRect, attempts);
            }
            GUI.EndGroup();

            GUI.DrawTexture(leftBgRect, leftBg);
            GUI.BeginGroup(leftBgRect);{
                HomeScore = new Rect(HomeRect.x, HomeRect.y + 25, HomeRect.width, HomeRect.height);
                VisitorScore = new Rect(VisitorRect.x, VisitorRect.y + 25, VisitorRect.width, VisitorRect.height);
                GUI.DrawTexture(HomeRect, Home);
                GUI.DrawTexture(VisitorRect, Visitor);

                Vector2 sizeOfLabel;
                GUI.BeginGroup(HomeScore);{
                    string ScoreString = Properties.HumanplayerGoals.ToString();
                    sizeOfLabel = label.CalcSize(new GUIContent(ScoreString));
                    GUI.Label(new Rect(HomeScore.width / 2 - sizeOfLabel.x / 2, HomeScore.height / 2 - sizeOfLabel.y / 2, sizeOfLabel.x, sizeOfLabel.y), ScoreString, label);
                }
                GUI.EndGroup();

                GUI.BeginGroup(VisitorScore);{
                    string ScoreString = Properties.AIPlayerGoals.ToString();
                    sizeOfLabel = label.CalcSize(new GUIContent(ScoreString));
                    GUI.Label(new Rect(VisitorScore.width / 2 - sizeOfLabel.x / 2, HomeScore.height / 2 - sizeOfLabel.y / 2, sizeOfLabel.x, sizeOfLabel.y), ScoreString, label);
                }
                GUI.EndGroup();

            }
            GUI.EndGroup();
		}

        if (Properties.GameType == Properties.Modes.PlayforFun)
        {
            GUI.DrawTexture(ATBgRect, ATBg);
            GUI.DrawTexture(HomeRect, Home);
            GUI.DrawTexture(VisitorRect, Visitor);
            var sizeOflabel = labelStyle.CalcSize(new GUIContent(Properties.HumanplayerGoals.ToString() + "            " + Properties.AIPlayerGoals.ToString()));
            GUI.Label(new Rect(ATBgRect.width / 2 - sizeOflabel.x / 2 + 110, ATBgRect.height / 2 - sizeOflabel.y / 2, sizeOflabel.x, sizeOflabel.y),
                      Properties.HumanplayerGoals.ToString() + "                                 " + Properties.AIPlayerGoals.ToString(), labelStyle);

            // need to put ui for arcade mode here.
            if (Properties.PlayMode == Properties.PlayForFunMode.ARCADE){

            }
            else if (Properties.PlayMode == Properties.PlayForFunMode.TOURNAMENT){

            }
        }
	}
}
