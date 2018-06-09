using UnityEngine;
using System.Collections;

public class WagerDenomination : MonoBehaviour {

	public Texture2D wagerBG;
	public Texture2D wagerBG_OverLay;

	public Rect wagerRect;

	public GUIStyle labelStyle;

	public Rect position;
	public Texture wagerTexture;
	public Rect labelRect;
	public string denomination;
	public bool selected = false;

	// Use this for initialization
	void Start () {
		wagerRect = new Rect (0,0, wagerBG.width,wagerBG.height);
		labelRect = new Rect (7 , 20, 100, 100);
		wagerTexture = wagerBG;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeTexture(){
		wagerTexture = wagerBG;
	}

	public void ChangeDenominationBG(){
		foreach (Transform tr in transform.parent) {
			if(tr.name != this.transform.name){
				//tr.GetComponent<WagerDenomination>().ChangeTexture();
			}
		}
	}
	void OnGUI(){
		if (!Properties.isPopUpShown) {
			GUI.depth = 0;
			GUI.BeginGroup (position);
			if (GUI.Button (wagerRect, wagerTexture, labelStyle)) {
					selected = true;
					wagerTexture = wagerBG_OverLay;
					var value = denomination;
					Properties.SelectedDenomination = (float)System.Convert.ToDouble (value);
					Debug.Log ("Selected Denomincation: " + Properties.SelectedDenomination);
					ChangeDenominationBG ();
			}
			Vector2 sizeOfLabel = labelStyle.CalcSize (new GUIContent (denomination));
			GUI.Label (new Rect (wagerRect.width / 2 - sizeOfLabel.x / 2, wagerRect.height / 2 - sizeOfLabel.y / 2, sizeOfLabel.x, sizeOfLabel.y), denomination, labelStyle);
			GUI.EndGroup ();
		}
	}
}
