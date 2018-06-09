using UnityEngine;
using System.Collections;

public class CustomizeBet : MonoBehaviour {

    public AudioClip[] PlayButton_audio;

	public GameObject customDeno;
	public Texture2D playButton;
	public Texture2D textBox;

	public Texture2D customStakes;

	public GameObject deno1;
	public GameObject deno2;
	public GameObject deno3;


	public Rect playButtonRect;
	public Rect textBoxRect;
	
	public Rect customStakesRect;
	
	public Rect deno1Rect;
	public Rect deno2Rect;
	public Rect deno3Rect;


	public GUIStyle labelStyle;
	public GUIStyle playStyle;

	// Use this for initialization
	void Start () {

		Properties.PreviousScene = "ChallengesScene";
		playButtonRect = new Rect (Screen.width/2 - playButton.width/2, 495,playButton.width,playButton.height);
		textBoxRect = new Rect (Screen.width/2 - textBox.width/2, 370,textBox.width,textBox.height);
		customStakesRect = new Rect (Screen.width/2 - customStakes.width/2, 170,customStakes.width,customStakes.height);

		deno1 = GameObject.Instantiate (customDeno, new Vector3 (275,215,0), Quaternion.identity) as GameObject; 
		deno1.GetComponent<CustomDeno> ().value = 1;

		deno2 = GameObject.Instantiate (customDeno, new Vector3 (450,215,0), Quaternion.identity) as GameObject; 
		deno2.GetComponent<CustomDeno> ().value = 25;

		deno3 = GameObject.Instantiate (customDeno, new Vector3 (625,215,0), Quaternion.identity) as GameObject; 
		deno3.GetComponent<CustomDeno> ().value = 50;

	}
	
    IEnumerator PlayAudio(AudioClip audioClip, string buttonName)
    {
        audio.volume = Properties.sfxVolume;
        audio.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        switch (buttonName)
        {
            case "Play":
                Properties.S_GoingtoPlay = true;
			    Properties.GamePlayBalance -= Properties.SelectedDenomination;
			    WorkAround.LoadLevelWorkaround("Airhockey");
                break;
        }
    }
	int count = 0;
	
	void OnGUI(){

		GUI.DrawTexture (customStakesRect, customStakes);
		GUI.DrawTexture (textBoxRect, textBox);

		GUI.BeginGroup (textBoxRect);
		{
			var sizeOfLabel = labelStyle.CalcSize(new GUIContent(Properties.SelectedDenomination.ToString()));

			GUI.Label(new Rect(textBoxRect.width/2 - sizeOfLabel.x/2, textBoxRect.height/2 - sizeOfLabel.y/2, sizeOfLabel.x, sizeOfLabel.y), Properties.SelectedDenomination.ToString(),labelStyle);

		}
		GUI.EndGroup ();

		if (GUI.Button (playButtonRect, "",playStyle)) {
            StartCoroutine(PlayAudio(PlayButton_audio[Random.Range(0, 2)], "Play"));
		}
	}

	void OnApplicationQuit() {
		PlayerPrefs.Save();
	}
}
