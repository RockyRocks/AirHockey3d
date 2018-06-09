using UnityEngine;
using System.Collections;

public class CustomDeno : MonoBehaviour {

	public Texture2D bChip;
	public Texture2D decrement;
	public Texture2D increment;

	public GUIStyle incrementStyle;
	public GUIStyle decrementStyle;

	public Rect bChipRect;

	public Rect decrementRect;
	public Rect incrementRect;

	public GUIStyle labelStyle;

	public float value = 0;

	// Use this for initialization
	void Start () {

		bChipRect = new Rect (transform.position.x, transform.position.y, bChip.width, bChip.height);
		decrementRect = new Rect (2, 76, decrement.width,decrement.height);
		incrementRect = new Rect (76, 32, increment.width,increment.height);

		if(value != 1)
		{
			value = value/100;
		}

	}
	
	void OnGUI(){

		GUI.depth = 0;

		bChipRect = new Rect (transform.position.x, transform.position.y, bChip.width,bChip.height);

		GUI.DrawTexture (bChipRect, bChip);
		GUI.BeginGroup (bChipRect);
		{
			var sizeOfLabel = labelStyle.CalcSize(new GUIContent(value.ToString()));
			GUI.Label(new Rect(bChipRect.width/2 - sizeOfLabel.x/2, bChipRect.height/2 - sizeOfLabel.y/2, sizeOfLabel.x, sizeOfLabel.y), value.ToString(), labelStyle);

			if(GUI.Button(incrementRect, "", incrementStyle)){
				if(Properties.SelectedDenomination >= 0){
					Properties.SelectedDenomination += value;
					Debug.Log("Increment by  "+ value);
				}
			}

			if(GUI.Button(decrementRect, "", decrementStyle)){
				if(Properties.SelectedDenomination - value >= 0){
					Properties.SelectedDenomination -= value;
					Debug.Log("decrement by  "+ value);
				}

			}
		}
		GUI.EndGroup ();

	}
}
