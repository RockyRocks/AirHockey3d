using UnityEngine;
using System.Collections;

public class GamePropertiesScript : MonoBehaviour {


	public string GameType{ 
		get; 
		set;
	}

	void Awake(){
		DontDestroyOnLoad (this);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
