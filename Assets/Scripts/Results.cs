using UnityEngine;
using System.Collections;

public class Results : MonoBehaviour {

    public AudioClip WonAmount_audio;
    public AudioClip WonTickets_audio;
	public GUIStyle textStyle;

	public Texture2D youWin;
	public Texture2D angledTickets;
	public Texture2D tickets;
    //public Texture2D ok;

    //public Rect okRect;
	public Rect youWinRect;
	public Rect angledTicketsRect;
	public Rect ticketsRect;

    public GUIStyle labelStyle;


	public Texture2D tryAgain;
	public Texture2D newChallenge;
	public Texture2D challengeCompleted;
	public Texture2D challengeNotCompleted;

	public Rect tryAgainRect;
	public Rect newChallengeRect;
	public Rect challengeCompletedRect;
	public Rect challengeNotCompletedRect;

	// Use this for initialization
	void Start () {

		youWinRect = new Rect (200, 180,youWin.width,youWin.height);
		angledTicketsRect = new Rect (270, 380, angledTickets.width, angledTickets.height);
		ticketsRect = new Rect (400, 390,tickets.width,tickets.height);
        //okRect = new Rect(Screen.width / 2 - ok.width / 2, Screen.height - (ok.height + 10), ok.width, ok.height);
		Properties.PreviousScene = "ChallengeScene";

		tryAgainRect = new Rect (227, 350, tryAgain.width, tryAgain.height);
		newChallengeRect = new Rect (485, 350, newChallenge.width, newChallenge.height);

		challengeCompletedRect = new Rect (Screen.width / 2 - challengeCompleted.width / 2, 250, challengeCompleted.width, challengeCompleted.height);
		challengeNotCompletedRect = new Rect (Screen.width / 2 - challengeNotCompleted.width / 2, 250, challengeNotCompleted.width, challengeNotCompleted.height);
        if (Properties.WonAmount != ""){
            StartCoroutine(PlayAudio(WonAmount_audio));
        }
        else
            StartCoroutine(PlayAudio(WonTickets_audio));
	}
    IEnumerator PlayAudio(AudioClip audioClip){
        audio.volume = Properties.sfxVolume;
        audio.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
    }
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){

		GUI.DrawTexture (youWinRect, youWin);
		GUI.DrawTexture (ticketsRect, tickets);
		GUI.DrawTexture (angledTicketsRect, angledTickets);

        if (Properties.isChallengeFinished){
            var winAmount = Properties.WonAmount;
            Properties.WonAmount = "";
            var sizeOflabel = labelStyle.CalcSize(new GUIContent("$" + winAmount.ToString()));
            GUI.Label(new Rect(Screen.width / 2 - sizeOflabel.x / 2, Screen.height / 2 - sizeOflabel.y / 2, sizeOflabel.x, sizeOflabel.y), winAmount.ToString(), labelStyle);
            Properties.isChallengeFinished = false;
        }
//		if (Properties.isChallengeFinished)
//			GUI.DrawTexture (challengeCompletedRect, challengeCompleted);
//		else
//			GUI.DrawTexture (challengeNotCompletedRect, challengeNotCompleted);
//
//		if (GUI.Button (tryAgainRect, tryAgain, textStyle)) {
//			Properties.isChallengeFinished = false;
//			Properties.challengeAttempts = Properties.winLoseDictionary [Properties.ChallengeMode].Attempts;
//			WorkAround.LoadLevelWorkaround ("Airhockey");
//		}
//
//		if (GUI.Button (newChallengeRect, newChallenge, textStyle)) {
//			WorkAround.LoadLevelWorkaround ("ChallengesScene");
//		}
	}
}
