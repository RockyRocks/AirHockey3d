using UnityEngine;
using System.Collections;

public class WagerbChipsBG : MonoBehaviour {

	public Texture2D bChipsBG;
	public Rect bChipsBGRect;
	
	public GUIStyle bChipsStyle;
	public Rect bChipsRect;
    //public  bool audio_flag;

	// Use this for initialization
	void Start () {
		bChipsBGRect = new Rect (Screen.width / 2 - bChipsBG.width / 2, -5, bChipsBG.width, bChipsBG.height);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		GUI.DrawTexture (bChipsBGRect, bChipsBG);
		
		var sizeOfLabel = bChipsStyle.CalcSize (new GUIContent (Properties.GamePlayBalance.ToString()));
		
		bChipsRect = new Rect (550, 40,sizeOfLabel.x,sizeOfLabel.y);
		
		GUI.Label (bChipsRect, Properties.GamePlayBalance.ToString(), bChipsStyle);

	}
}
