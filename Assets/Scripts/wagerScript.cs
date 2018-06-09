using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wagerScript : MonoBehaviour {

	public AudioClip ReturnClick;
	public AudioClip CommonClick;
    public AudioClip CustomStake_audio;

	public Texture2D deno1;
	public Texture2D deno2;
	public Texture2D deno3;
	public Texture2D deno4;
	public Texture2D deno5;
	public Texture2D deno6;
	public Texture2D deno7;

	public Texture2D setYourStakes;
	public Texture2D customAmount;

	public Rect setYourStakesRect;
	public Rect customAmountRect;

	public GUIStyle buttonStyle;
	public GUIStyle customAmointStyle;

	public Rect[] denoRects;
	float selectedDenomination = 0;


	// Use this for initialization
	void Start () {

		Properties.PreviousScene = "WagerScene";

		setYourStakesRect = new Rect (Screen.width/2 - setYourStakes.width/2, 180, setYourStakes.width, setYourStakes.height);
		customAmountRect = new Rect (Screen.width/2 - customAmount.width/2, 370, customAmount.width, customAmount.height);

		denoRects =  new Rect[8];
		denoRects [0] = new Rect (190, Screen.height/2 - deno1.height/2, deno1.width,deno1.height );

		for(int i = 1 ; i< denoRects.Length; i++)
			denoRects [i] = new Rect (20 + denoRects[i-1].x + deno1.width , Screen.height/2 - deno1.height/2, deno1.width, deno1.height );
	}
	
	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}

	IEnumerator PlayAudio(AudioClip audioClip, string buttonName){
		audio.volume = Properties.sfxVolume;
		audio.PlayOneShot(audioClip);
		yield return new WaitForSeconds (audioClip.length );
		switch (buttonName) {
		case "Play":
			Properties.SelectedDenomination = selectedDenomination;         
			if (Properties.GamePlayBalance != 0)
			{
				Debug.Log(Properties.GamePlayBalance);
				Properties.S_GoingtoPlay = true;
				Properties.GamePlayBalance -= Properties.SelectedDenomination;
				WorkAround.LoadLevelWorkaround("Airhockey");
			}
			else{
				Properties.ParentScene = "ChallengesScene";
				WorkAround.LoadLevelWorkaround("Airhockey");
			}
			break;
		case "Custom":
			WorkAround.LoadLevelWorkaround("CustomizeBetScene");
			break;
		}
	}

	void OnGUI(){
		GUI.depth = 1;


		GUI.DrawTexture (setYourStakesRect, setYourStakes);

		if (GUI.Button (customAmountRect,"", customAmointStyle)) {
            StartCoroutine(PlayAudio(CustomStake_audio, "Custom"));
		}

		if (GUI.Button (denoRects [0], deno1, buttonStyle)) {
			selectedDenomination = 0.25f;
			StartCoroutine(PlayAudio(CommonClick, "Play"));
		}

		if (GUI.Button (denoRects [1], deno2, buttonStyle)) {
			selectedDenomination = 0.5f;
			StartCoroutine(PlayAudio(CommonClick, "Play"));
		}

		if (GUI.Button (denoRects [2], deno3, buttonStyle)) {
			selectedDenomination = 1f;
			StartCoroutine(PlayAudio(CommonClick, "Play"));
		}

		if (GUI.Button (denoRects [3], deno4, buttonStyle)) {
			selectedDenomination = 2f;
			StartCoroutine(PlayAudio(CommonClick, "Play"));
		}

		if (GUI.Button (denoRects [4], deno5, buttonStyle)) {
			selectedDenomination = 2f;
			StartCoroutine(PlayAudio(CommonClick, "Play"));
		}

		if (GUI.Button (denoRects [5], deno6, buttonStyle)) {
			selectedDenomination = 5f;
			StartCoroutine(PlayAudio(CommonClick, "Play"));
		}

		if (GUI.Button (denoRects [6], deno7, buttonStyle)) {
			selectedDenomination = 10f;
			StartCoroutine(PlayAudio(CommonClick, "Play"));

		}
	}
}

