using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChallengesScript : MonoBehaviour
{
	
	int buttonWidth = 64;
	int buttonHeight = 64;
	
	public AudioClip buttonClip;
    public AudioClip challenges_audio;
	
	public Texture2D upArrow;
	public Texture2D downArrow;
    public Texture2D Select;

	public Texture2D challengeBG;

	Texture2D bspotTexture;
	
	public Rect upArrowRect;
	public Rect downArrowRect;
    public Rect SelectRect;

	public Rect challengesBGRect;

	public Rect ticketsBGRect;
	public Rect ticketsRect;

	/******************/
	public Texture2D exitPopupBG;
	public Texture2D exitPopupText;

	public Texture2D okButton;
	public Texture2D cancelButton;
	
	public Rect okButtonRect;
	public Rect cancelButtonRect;

	public GUIStyle buttonStyle;
	public GUIStyle logStyle;
    public GUIStyle SelectStyle;
	/******************/
	
	int challengesSize;
	
	
	public GameObject challenge;
	GameObject challenges;
	
	int count = 0;
	
	public GameObject challengesList;
	
	int totalTickets;
	
	List<Vector3> challengePositions = new List<Vector3>();
	
	public GUIStyle upArrowStyle;
	public GUIStyle downArrowStyle;

	public GUIStyle labelStyle;
	public Vector3 pos;
	// Use this for initialization
	void Start()
	{
		challengesBGRect = new Rect(Screen.width / 2 - challengeBG.width / 2, 83, challengeBG.width, challengeBG.height);
		upArrowRect = new Rect(760, Screen.height / 2 - upArrow.height / 2, upArrow.width, upArrow.height);
		downArrowRect = new Rect(100, Screen.height / 2 - downArrow.height / 2, downArrow.width, downArrow.height);
        SelectRect = new Rect(Screen.width / 2 - Select.width / 2, Screen.height - (Select.height+10), Select.width, Select.height);
		
		challengesSize = Properties.challengeDictionary.Count;
		
		Properties.TotalTickets = PlayerPrefs.GetInt("Total Tickets");
		
		Properties.ParentScene = "ModesScene";
		Properties.PreviousScene = "ChallengesScene";
		
		pos = new Vector3(Screen.width / 2 - challengeBG.width / 2 + 15, Screen.height / 2 - challengeBG.height / 2 + 100, 0);
		
		challenges = GameObject.Find("Challenges");
		
		int totalChallengeSlots = 19;
		
		int startPos = -9;

        for (int i = 0; i < totalChallengeSlots; i++){
			Vector3 position = new Vector3(pos.x + startPos * 500, pos.y , 0);
			challengePositions.Add(position);
			startPos++;
		}
		
		for (int i = 0; i < Properties.ChallengesList.Length; i++){
			int num = i + 1;
			var val = startElement + num;
			var challengeObject = Instantiate(challenge, challengePositions[val], Quaternion.identity) as GameObject;	
			challengeObject.name = "Challenge" + num;
			
			var challengeScript = challengeObject.GetComponent<Challenge>();
			challengeScript.name = Properties.challengeDictionary[Properties.ChallengesList[i]];
			challengeScript.challengeNumber = challengeScript.name;
			challengeScript.challengeName = Properties.ChallengesList[i];
			challengeScript.challengeNumberTexture = Resources.Load<Texture2D>("Challenges/Challenge" + ( i + 1 ) +"/Challenge");
			challengeScript.challengeNameTexture = Resources.Load<Texture2D>("Challenges/Challenge" + ( i + 1 ) +"/Name");
			challengeScript.challengeDescTexture = Resources.Load<Texture2D>("Challenges/Challenge" + ( i + 1 ) +"/Description");
			challengeScript.ChallengeImage = Resources.Load<Texture2D>("Challenges/Challenge" + ( i + 1 ) +"/Image");
            challengeScript.ticketsAngledTexure = Resources.Load<Texture2D>("Challenges/Challenge" + (i + 1) + "/bticket");
            challengeScript.position = Properties.challengesNamePositions[i];

			if (PlayerPrefs.HasKey(Properties.challengeDictionary[challengeScript.challengeName])){
				challengeScript.isLocked = PlayerPrefsX.GetBool(Properties.challengeDictionary[challengeScript.challengeName]);
			}else{
				challengeScript.isLocked = Properties.winLoseDictionary[Properties.challengeDictionary[challengeScript.challengeName]].Locked;
			}

			challengeObject.transform.parent = challengesList.transform;
			if (i < 1)
				challengeObject.SetActive(true);
			else
				challengeObject.SetActive(false);
		}
	}
	
	int startElement = 8;
	
	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length );
		switch (buttonName) {
		case "Help":
			WorkAround.LoadLevelWorkaround("help");
			break;
		case "Options":
			WorkAround.LoadLevelWorkaround("Options");
			break;
		case "Back":
			WorkAround.LoadLevelWorkaround("ModesScene");
			break;
        case "Select":
            Challenge.Select_flag = true;
            break;
			
		}
	}
	
	void UpdateChallengePosition(int count, bool direction)
	{
		Transform[] childElements = challengesList.transform.GetComponentsInChildren<Transform>();
		int i = 0;
		
		foreach (Transform tr in challengesList.transform){
			if (direction){
				var name = "Challenge" + (count + 1);
				if (tr.name == name)
					tr.gameObject.SetActive(true);
				name = "Challenge" + (count + 2);
				if (tr.name == name)
					tr.gameObject.SetActive(false);
			}else{
				var name = "Challenge" + count;
				if (tr.name == name)
					tr.gameObject.SetActive(false);
				name = "Challenge" + (count + 1);
				if (tr.name == name)
					tr.gameObject.SetActive(true);
				
			}
			var val = startElement - count + 1 + i;
			tr.gameObject.transform.position = challengePositions[val];
			i++;
		}
	}

	void OnGUI()
	{
		GUI.depth = 0;
		if (!Properties.isPopUpShown) {
            if (count != challengesSize - 1)
			if (GUI.Button (upArrowRect, "", upArrowStyle)) {					
					if (count <= challengesSize - 1) {
						if (count != challengesSize-1)
							count++;
						UpdateChallengePosition (count, false);
						StartCoroutine (PlayAudio (buttonClip, ""));
					}
			}
            if (count > 0)
			if (GUI.Button (downArrowRect, "", downArrowStyle)) {
				if (count >= 0) {
					if (count != 0)
						count--;
					UpdateChallengePosition (count, true);
					StartCoroutine (PlayAudio (buttonClip, ""));
				}
			}
            if (GUI.Button(SelectRect, "", SelectStyle)){
                StartCoroutine(PlayAudio(challenges_audio, "Select"));
            }
		} 
		else {
			GUI.DrawTexture (new Rect (Screen.width / 2 - exitPopupBG.width / 2, Screen.height / 2 - exitPopupBG.height / 2, exitPopupBG.width, exitPopupBG.height), exitPopupBG);
			GUI.DrawTexture (new Rect (Screen.width / 2 - exitPopupText.width / 2, Screen.height / 2 - exitPopupText.height / 2, exitPopupText.width, exitPopupText.height), exitPopupText);
			if (GUI.Button (okButtonRect, okButton, buttonStyle)) {
				StartCoroutine(PlayAudio(buttonClip, "Ok"));
				Properties.DeleteSession = true;
			}
			if (GUI.Button (cancelButtonRect, cancelButton, buttonStyle)) {
				StartCoroutine(PlayAudio(buttonClip, "Cancel"));
				Properties.isPopUpShown = false;
			}
		}
	}
	
	void OnApplicationQuit()
	{
		PlayerPrefs.Save();
	}
}
