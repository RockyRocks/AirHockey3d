using UnityEngine;
using System.Collections;

public class FaderScript : MonoBehaviour
{
	public float fadeSpeed = 1.5f;
	private bool sceneStarting = true;
	private Color fadeColor = Color.black;
	public bool isFadeDone = false;
	private bool sceneEnding = false;

	void Start()
	{
		fadeColor.a = 1f;
	}

	void Update ()
	{
		if (sceneStarting)
		{
			fadeColor = Color.Lerp(fadeColor, Color.clear, fadeSpeed * Time.deltaTime);
			if(fadeColor.a <= 0.05f)
			{
				fadeColor = Color.clear;
				sceneStarting = false;
				isFadeDone = true;
			}
		}
		else if (sceneEnding)
		{
			fadeColor = Color.Lerp(fadeColor, Color.black, fadeSpeed * Time.deltaTime);
			if (fadeColor.a >= 0.95f) {
				// Scene ended
			}
		}
	}
	
	public void EndScene ()
	{
		sceneEnding = true;
	}

	void OnGUI()
	{
		if (fadeColor.a > 0.01f)
		{
			Color oldColor = GUI.color;
			GUI.color = fadeColor;
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
			GUI.color = oldColor;
		}
	}
}