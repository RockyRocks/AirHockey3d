using UnityEngine;
using System.Collections;

public class LeaderBoardItem : MonoBehaviour {

	public Rect rankRect;
	public Rect nameRect;
	public Rect scoreRect;
	public Vector2 position;
	public GUIStyle labelStyle;
	public string playerName;
	public string rank;
	public string score;

	// Use this for initialization
	void Start () {
		rankRect = new Rect (position.x + 20,position.y,100,100);
		nameRect = new Rect (110 + position.x,position.y,100,100);
		scoreRect = new Rect(220 + position.x,position.y,100,100);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.depth = 0;
		GUI.Label (rankRect, rank, labelStyle);
		GUI.Label (nameRect, playerName, labelStyle);
		GUI.Label (scoreRect, score, labelStyle);

	}
}
