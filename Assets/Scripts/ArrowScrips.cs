using UnityEngine;
using System.Collections;

public class ArrowScrips : MonoBehaviour {

	public Texture2D upArrow;
	public Texture2D downArrow;
	public Texture2D angledTicket;

	public Rect upArrowRect;
	public Rect downArrowRect;
	public Rect angledTicketRect;

	public GUIStyle upArrowStyle;
	public GUIStyle downArrowStyle;

	public GUIStyle labelStyle;

	// Use this for initialization
	void Start () {
		angledTicketRect = new Rect (620, 108, angledTicket.width, angledTicket.height);
		upArrowRect = new Rect(760, Screen.height / 2 - upArrow.height / 2, upArrow.width, upArrow.height);
		downArrowRect = new Rect(100, Screen.height / 2 - downArrow.height / 2, downArrow.width, downArrow.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.depth = 1;

		GUI.DrawTexture (angledTicketRect, angledTicket);

		//GUI.Label (angledTicketRect, Properties.winLoseDictionary[Properties.ChallengeMode],labelStyle);

		GUI.Label (angledTicketRect, "100",labelStyle);

		if (GUI.Button (upArrowRect, "", upArrowStyle)) 
		{					
			
		}
		
		if (GUI.Button (downArrowRect, "", downArrowStyle)) {
			
		}
	}
}
