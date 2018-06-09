using UnityEngine;
using System.Collections;

public class Scroller : MonoBehaviour {

	public Texture2D bar;
	public Texture2D barScroll;
	public Texture2D barLoading;
	public Texture2D nameTexture;
	public Rect barRect;
	public Rect barScrollRect;
	public Rect barLoadingRect;
	public Rect position;
	public Rect labelRect;

	public GUISkin sliderSkin;

	public float volume;

	// Use this for initialization
	void Start () {

		barScrollRect = new Rect (6,40,barScroll.width,barScroll.height);
		barLoadingRect = new Rect (11,10,barLoading.width,barLoading.height);
		labelRect = new Rect (bar.width / 2 - nameTexture.width / 2, bar.height / 2 - nameTexture.height / 2 , nameTexture.width, nameTexture.height);
		barRect = new Rect (0,2*nameTexture.height,bar.width,bar.height);
	}

	// Update is called once per frame
	void Update () {
	
	}

	float value;
	void OnGUI(){
		GUI.depth = 0;
		GUI.BeginGroup (position);
		GUI.depth = 0;
		volume = GUI.HorizontalSlider(barRect, volume, 0.0F, 1.0F,sliderSkin.horizontalSlider, sliderSkin.horizontalSliderThumb);
		GUI.DrawTexture (labelRect, nameTexture );
		GUI.EndGroup ();
	}
}
