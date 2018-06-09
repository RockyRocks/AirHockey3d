using UnityEngine;
using System.Collections;

public class StartGameScript : MonoBehaviour {

	//public GameObject gameProperties;
	public GamePropertiesScript gamePropertiesScript;

	// Use this for initialization
	void Start () {
		
	}

	void Awake(){
		//gamePropertiesScript = gameProperties.GetComponent<GamePropertiesScript> ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (GUI.Button (new Rect (20,40,200,20), "Single Player")) {
			gamePropertiesScript.GameType = "SINGLEPLAYER";
			WorkAround.LoadLevelWorkaround ("Airhockey");
		}
		if (GUI.Button (new Rect (20,80,200,20), "Multi Player")) {
			gamePropertiesScript.GameType = "MULTIPLAYER";
			WorkAround.LoadLevelWorkaround ("Airhockey");
		}

	}
}
