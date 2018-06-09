using UnityEngine;
using System.Collections;
using System.IO;

public class MoreTickets : MonoBehaviour {
	
	public AudioClip ReturnClick;
	public AudioClip CommonClick;
	
	public GUIStyle labelStyle;
	public GUIStyle yesStyle;
	public GUIStyle noStyle;
	
	public Texture2D yesTexture;
	public Texture2D noTexture;
	
	
	public Texture2D challengeBlackBG;
	
	public Texture2D textTexture;
	public Texture2D lockTexure;
	public Texture2D unLockTexure;
	
	
	public Rect yesTextureRect;
	public Rect noTextureRect;
	
	public Rect challengeBlackBGRect;
	public Rect textTextureRect;
	public Rect lockTexureRect;
	public Rect labelRect;
	
	Vector2 sizeOfLabel1;
	
	string labelString;
	// Use this for initialization
	void Start () {
		
		yesTextureRect = new Rect (0,0, yesTexture.width, yesTexture.height);
		
		noTextureRect = new Rect (0,0, noTexture.width, noTexture.height);
		
		challengeBlackBGRect = new Rect (Screen.width / 2 - challengeBlackBG.width / 2, Screen.height / 2 - challengeBlackBG.height / 2 + 35, challengeBlackBG.width, challengeBlackBG.height);
		
		lockTexureRect = new Rect (challengeBlackBGRect.center.x - lockTexure.width / 2, challengeBlackBGRect.center.y - lockTexure.height / 2, lockTexure.width, lockTexure.height);

		StartCoroutine (ChangeScreen ());
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
		//		switch (buttonName) {
		//		case "Help":
		//			Properties.PreviousScene = "UnLockChallenge";
		//			WorkAround.LoadLevelWorkaround("Help");
		//			break;
		//		case "Options":
		//			Properties.PreviousScene = "UnLockChallenge";
		//			WorkAround.LoadLevelWorkaround("OptionschallengeBlackBGRect");
		//			break;
		//		case "PlayAnotherChallenge":
		//			WorkAround.LoadLevelWorkaround ("ChallengesScene");
		//			break;
		//		case "UnLockChallenge":
		//			var challengeProperties = Properties.winLoseDictionary[Properties.ChallengeMode];
		//			int unLockAmount = challengeProperties.UnLockAmount; 
		//			if(unLockAmount	<= Properties.TotalTickets){
		//				
		//				PlayerPrefsX.SetBool(Properties.winLoseDictionary[Properties.ChallengeMode].Name,false);
		//				
		//				Properties.TotalTickets -= unLockAmount;
		//				
		//				PlayerPrefs.SetInt("Total Tickets", (int)Properties.TotalTickets);
		//				
		//				PlayerPrefs.Save();
		//				//show unlockedScreen
		//				texture = challengeUnLockedTexture;
		//				challengeUnlocked = true;
		//				StartCoroutine("ChangeScreen");
		////					WorkAround.LoadLevelWorkaround("UnLockedChallenge");
		//			}
		//			
		//			else{
		//				Properties.MoreTickets = unLockAmount - (int)Properties.TotalTickets;
		//				WorkAround.LoadLevelWorkaround("MoreTickets");
		//			}
		//			break;
		//		}
	}
	
	IEnumerator ChangeScreen(){
		yield return new WaitForSeconds (3f);
		WorkAround.LoadLevelWorkaround ("ChallengesScene");
	}
	
	bool challengeUnlocked = false;
	
	void OnGUI(){
		
		GUI.depth = 0;
		
		if(!Properties.isPopUpShown)
		{
			if (!challengeUnlocked) 
			{
				GUI.DrawTexture(lockTexureRect,lockTexure);
				
				GUI.DrawTexture(challengeBlackBGRect,challengeBlackBG);
			
				float requiredTickets = Properties.winLoseDictionary[Properties.SelectedChallenge].UnLockAmount - Properties.TotalTickets;

				labelString = "YOU NEED MORE "+ requiredTickets.ToString() + " TICKETS TO UNLOCK";
				sizeOfLabel1 = labelStyle.CalcSize (new GUIContent (labelString));
				
				GUI.BeginGroup(challengeBlackBGRect);
				GUI.Label(new Rect(0,0, challengeBlackBGRect.width, challengeBlackBGRect.height), labelString,labelStyle);
				GUI.EndGroup();
				
			} 
			else 
			{
				if(GUI.Button(yesTextureRect, "", yesStyle)){
					
				}
				
				if(GUI.Button(noTextureRect, "", noStyle)){
					
				}
			}
		}
	}
}
