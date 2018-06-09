using UnityEngine;
using System.Collections;

public class TextScript : MonoBehaviour {

	public Texture2D texture2D;
	public string textString;
	public Vector2 startPos;
	public Vector2 endPos;
	
	public int width;
	public int height;
	
	float alpha = 1f;

	public float scale = 1f;
	
	public float scaleValue = 1f;
	
	public float speed = 100f;

	public bool animationFinish = false;
	public GUIStyle textStyle;
	public Rect textRect;

	public enum TransitionMode{
		SlideLeft,
		SlideRight,
		SlideUp,
		SlideDown,
		FadeIn,
		FadeOut,
		ScaleIn,
		ScaleOut,
		Null
	}
	// Start the animation with the component's text
	public void Begin(){ 

	}
	
	public void Awake(){
		startPos = new Vector2(Screen.width/2,Screen.height/2);
		endPos = new Vector2(Screen.width/2,Screen.height/2);
		textRect = new Rect (Screen.width/2, Screen.height/2, 100,100);

	}
	
	void FixedUpdate(){
	}
	
	//bool val = false;
	
	//public TransitionMode mode = TransitionMode.ScaleIn;
	
	public void Animate(TransitionMode mode){
		switch (mode) {
		case TransitionMode.SlideLeft:{
			StartCoroutine ("SlideLeft");
			break;
		}
		case TransitionMode.SlideRight:{
			StartCoroutine ("SlideRight");	
			break;
		}
		case TransitionMode.SlideUp:{
			StartCoroutine ("SlideUp");	
			break;
		}
		case TransitionMode.SlideDown:{
			StartCoroutine ("SlideDown");	
			break;
		}
		case TransitionMode.FadeIn:{
			StartCoroutine ("FadeIn");
			break;
		}
		case TransitionMode.FadeOut:{
			StartCoroutine ("FadeOut");
			break;
		}
		case TransitionMode.ScaleIn:{
			StartCoroutine ("ScaleIn");
			break;
		}
		case TransitionMode.ScaleOut:{
			StartCoroutine ("ScaleOut");	
			break;
		}
		case TransitionMode.Null:{
			break;
		}
	}
}
	
	/// <summary>
	/// Moves the Text from left to right
	/// </summary>
	public IEnumerator  SlideRight(){
		while ((startPos.x + speed * Time.deltaTime) < endPos.x) {
			startPos.x += speed * Time.deltaTime;
			yield return null;
		}
	}
	
	/// <summary>
	/// Moves the Text from right to left
	/// </summary>
	public IEnumerator SlideLeft(){
		while ((startPos.x - speed * Time.deltaTime) > endPos.x) {
			startPos.x -= speed * Time.deltaTime;
			yield return null;
		}
	}
	
	/// <summary>
	/// Moves the Text from top to bottom
	/// </summary>
	public IEnumerator SlideBottom(){
		while ((startPos.y + speed * Time.deltaTime) < endPos.y) {
			startPos.y += speed * Time.deltaTime;
			yield return null;
		}
	}
	
	/// <summary>
	/// Moves the Text from bottom to top
	/// </summary>
	public IEnumerator SlideTop(){
		
		while ((startPos.y - speed * Time.deltaTime) > endPos.y) {
			startPos.y -= speed * Time.deltaTime;
			yield return null;
		}
	}
	
	/// <summary>
	/// Fades In the texture by reducing the alpha value of texture
	/// </summary>
	public IEnumerator FadeIn(){
		
		while (alpha < 1f) {
			alpha +=  Time.deltaTime;
			yield return null;
		}
	}
	
	/// <summary>
	/// Fades Out the texture by incresing the alpha value of texture
	/// </summary>
	public IEnumerator FadeOut(){
		while (alpha > 0f) {
			alpha -=  Time.deltaTime;
			yield return null;
		}
	}
	
	/// <summary>
	/// Scales the texure from small to large.
	/// </summary>
	public IEnumerator ScaleIn(){
		scale = 0.5f;
		scaleValue = 0.1f;
		speed = 1f;
		while (scaleValue  <= scale) {
			scaleValue +=  Time.deltaTime;
			yield return null;
		}
	}
	
	/// <summary>
	/// Scales the texure from small to large.
	/// </summary>
	public IEnumerator ScaleOut(){
		scale = 0f;
		scaleValue = 0.5f;
		speed = 1f;
		while (scaleValue  > scale) {
			scaleValue -= Time.deltaTime;
			yield return null;
		}
	}

	public IEnumerator ScaleInScaleOut(){

		Show ();
		yield return StartCoroutine (ScaleIn ());
		yield return StartCoroutine (ScaleOut ());
		Hide ();
		yield return null;
	}

//	private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight){
//		Texture2D result = new Texture2D (targetWidth, targetHeight, source.format, true);
//		Color[] rpixels = result.GetPixels (0);
//		float incX = 1f /(float) targetWidth;
//		float incY = 1f /(float) targetHeight;
//		for(int px =0;px<rpixels.Length;px++){
//			rpixels[px] = source.GetPixelBilinear(incX*((float)px%targetWidth), incY*((float)Mathf.Floor(px/targetWidth)));
//		}
//		result.SetPixels (rpixels, 0);
//		result.Apply ();
//		return result;
//	}

	public void Show(){
		alpha = 1f;
	}

	public void Hide(){
		alpha = 0f;
	}
	//int textHeight = 200;
	//int textWidth = 200;
	//int fontSize = 10;

	void OnGUI()
	{   var c = GUI.color;
		c.a = alpha;
		GUI.color = c;

		//Vector2 pivot = new Vector2 (startPos.x + texture.width/2, startPos.x + texture.height/2);
		//GUIUtility.ScaleAroundPivot (new Vector2(scaleValue, scaleValue), pivot);
		//GUI.DrawTexture (new Rect(startPos.x,startPos.y, width, height), texture);
		if (texture2D != null) {
			GUI.DrawTexture (new Rect (startPos.x - (texture2D.width * scaleValue / 2),
						               startPos.y - (texture2D.height * scaleValue / 2),
                        	 		   texture2D.width * scaleValue,
			                           texture2D.height * scaleValue),texture2D);
		} else {
			Vector2 sizeOfLabel = textStyle.CalcSize(new GUIContent(textString));
			GUI.Label (new Rect(Screen.width/2 - sizeOfLabel.x/2,Screen.height/2 - sizeOfLabel.y/2,sizeOfLabel.x,sizeOfLabel.y),textString, textStyle);

		}

	}
}
