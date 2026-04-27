using UnityEngine;
using System.Collections;

public class DebugStreamer : MonoBehaviour {

public static string message;
public bool showLineMovement;
public TextAnchor anchorAt = TextAnchor.UpperRight;
public int numberOfLines = 5;
public int pixelOffset = 5;
	
private string	_message;
private ArrayList messageHistory = new ArrayList ();
private int messageHistoryLength;
private string	displayText;
private int	patternIndex = 0;
private string[] pattern = new string[] {"-", "\\", "|", "/"};

	void Awake ()
	{
		_message = message;
	}
	
	void Update () {
		if(_message!=message)
		{
			_message = message;
			if(showLineMovement)
			{
				messageHistory.Insert(0,message + "\t" + pattern[patternIndex]);
				messageHistoryLength = messageHistory.Count;
			}
			else
			{
				messageHistory.Insert(0, message);
				messageHistoryLength = messageHistory.Count;
			}
			
			patternIndex = (patternIndex + 1) % 4;
			while(messageHistoryLength>numberOfLines)
			{
				messageHistory.RemoveAt(messageHistory.Count - 1);
				messageHistoryLength = messageHistory.Count;
			}
		
			displayText = "";
			for(int i = 0; i<messageHistory.Count; i++)
			{
				if(i==0)
					displayText = messageHistory[i] as string;
				else
					displayText = (messageHistory[i] as string) + "\n" + displayText;
			}
		}
	}
	
	void OnGUI()
	{
		if (!string.IsNullOrEmpty(displayText))
		{
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.alignment = anchorAt;
			style.normal.textColor = Color.black;
			Rect rect = new Rect(pixelOffset, pixelOffset, Screen.width - pixelOffset * 2, Screen.height - pixelOffset * 2);
			GUI.Label(rect, displayText, style);
		}
	}
}
