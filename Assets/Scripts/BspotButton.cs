using UnityEngine;
using System.Collections;

public class BspotButton : MonoBehaviour {

	// Use this for initialization
    public Texture2D BspotLogo;
    public Rect BspotRect;
	void Start () {
        BspotRect = new Rect(20, 400, BspotLogo.width, BspotLogo.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if(Properties.IsLoggedIn)
        GUI.DrawTexture(BspotRect,BspotLogo);
    }
}
