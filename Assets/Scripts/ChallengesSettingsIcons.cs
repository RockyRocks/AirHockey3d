using UnityEngine;
using System.Collections;

public class ChallengesSettingsIcons : MonoBehaviour {


	public AudioClip ReturnClick;
	public AudioClip CommonClick;


	int buttonWidth = 64;
	int buttonHeight = 64;
	int border = 12;

    public Texture2D ok;
	public GUIStyle optionsStyle;
	public GUIStyle helpStyle;
	public GUIStyle backStyle;
	public GUIStyle textStyle;
    public GUIStyle OkStyle;

	public Rect helpRect;
	public Rect optionsButtonRect;
	public Rect backRect;
	public Rect ticketRect;
	public Rect totalTicketsRect;
    public Rect okRect;

	public Texture2D ticketTexture;


	// Use this for initialization
	void Start () {
		backRect = new Rect(border, Screen.height - buttonHeight - border, buttonWidth, buttonHeight);

		helpRect = new Rect (Screen.width - buttonWidth - border, Screen.height - buttonHeight - border, buttonWidth, buttonHeight);

		optionsButtonRect = new Rect (helpRect.x - buttonWidth - border, Screen.height - buttonHeight - border, buttonWidth, buttonHeight);

		ticketRect = new Rect (backRect.xMax + buttonWidth + border + border, Screen.height - buttonHeight - border, ticketTexture.width, ticketTexture.height);

		totalTicketsRect = new Rect (backRect.xMax + border ,Screen.height - buttonHeight - border, ticketTexture.width, ticketTexture.height);
        //okRect= new Rect(Screen.width / 2 - ok.width / 2, Screen.height - (ok.height + 10), ok.width, ok.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length );
		switch (buttonName) {
		case "Help":
			WorkAround.LoadLevelWorkaround("help");
			break;
		case "Sound":
			WorkAround.LoadLevelWorkaround("Options");
			break;
		case "Back":
			WorkAround.LoadLevelWorkaround(Properties.ParentScene);
			break;
			
		}
	}

	void OnGUI(){
		if (GUI.Button (helpRect, "", helpStyle)) {
			StartCoroutine (PlayAudio (CommonClick, "Help"));
		}

		if (GUI.Button (optionsButtonRect, "", optionsStyle)) {
			StartCoroutine (PlayAudio (CommonClick, "Sound"));
		}

		if (GUI.Button (backRect, "", backStyle)) {
			StartCoroutine (PlayAudio (ReturnClick, "Back"));
		}

        if (Properties.PreviousScene == "ChallengeScene"){
            if (GUI.Button(okRect, "", OkStyle)){
                StartCoroutine(PlayAudio(ReturnClick, "Back"));
            }
        }
		var sizeOfLabel = textStyle.CalcSize (new GUIContent (Properties.TotalTickets.ToString()));

		GUI.Label (totalTicketsRect, Properties.TotalTickets.ToString(),textStyle);

		GUI.DrawTexture (new Rect(totalTicketsRect.x + sizeOfLabel.x + border ,Screen.height - buttonHeight - border, ticketTexture.width, ticketTexture.height), ticketTexture);
	}

}
