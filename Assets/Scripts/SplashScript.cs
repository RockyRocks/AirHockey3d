using UnityEngine;
using System.Collections;


public class SplashScript : MonoBehaviour {

	public Texture2D bSpotSplash;
	public Texture2D tikiSplash;
	public Texture2D AirhockeySplash;

	//public BasicHandShake basicHandshake;
	float bspotFader = 1f;
	float tikiFader = 0f;
	float shuffleBowlingFader = 0f;

	//string label = "labbel";
	//Texture2D splash;

	//bool isLable = false;
	
	// Use this for initialization
	void Start () {
        Debug.Log("we are inside the splash script");

		//audio.Play ();

	}


	float time;
	//float fader = 1;
	bool isFade = true;
	void FixedUpdate(){
        if (Properties.IsLogoTransitionsAllowed && !Properties.IsLoggedIn)
        {
            time += Time.deltaTime;
            if (time > 2f && isFade)
            {
                StartCoroutine(BSpotSplash());
                isFade = false;
            }
        }
        else if (!Properties.IsLogoTransitionsAllowed && Properties.IsLoggedIn)
        {
            Debug.Log("transitions are completed:");
			Properties.GameType = Properties.Modes.PlayforMoney;
            WorkAround.LoadLevelWorkaround("ChallengesScene");
            Debug.Log("going to enter the modesscene");
        }
	}
	

	IEnumerator TikiSplashFadeIn(){
		while (tikiFader <= 1f) {
			tikiFader += Time.deltaTime*0.5f;
			yield return null;
		}
		StartCoroutine (TikiSplashFadeOut ());

	}

	IEnumerator TikiSplashFadeOut(){
		while (tikiFader >= 0f) {
			tikiFader -= Time.deltaTime * 0.5f;
			yield return null;
		}

		StartCoroutine (BowlingSplashFadeIn ());
	}

	IEnumerator BowlingSplashFadeIn(){
		while (shuffleBowlingFader <= 1f) {
			shuffleBowlingFader += Time.deltaTime*0.5f;
			yield return null;
		}
		StartCoroutine (BowlingSplashFadeOut ());
		
	}


	IEnumerator BowlingSplashFadeOut(){
		while (shuffleBowlingFader >= 0f) {
			shuffleBowlingFader -= Time.deltaTime * 0.5f;
			yield return null;
		}
		// start new scene.
		WorkAround.LoadLevelWorkaround ("ModesScene");
		Debug.Log ("i did not hit this");
	}

	IEnumerator BSpotSplash(){
		while (bspotFader >= 0f) {
			bspotFader -= Time.deltaTime;
			yield return null;
		}
        WorkAround.LoadLevelWorkaround("ModesScene");
		//StartCoroutine (TikiSplashFadeIn());
	}

	void OnGUI(){

        if (Properties.IsLogoTransitionsAllowed && !Properties.IsLoggedIn)
        {
            GUI.color = new Color(1, 1, 1, shuffleBowlingFader);
            GUI.DrawTexture(new Rect(Screen.width / 2 - AirhockeySplash.width / 2, Screen.height / 2 - AirhockeySplash.height / 2, AirhockeySplash.width, AirhockeySplash.height), AirhockeySplash);

            GUI.color = new Color(1, 1, 1, tikiFader);
            GUI.DrawTexture(new Rect(Screen.width / 2 - tikiSplash.width / 2, Screen.height / 2 - tikiSplash.height / 2, tikiSplash.width, tikiSplash.height), tikiSplash);

            GUI.color = new Color(1, 1, 1, bspotFader);
            GUI.DrawTexture(new Rect(Screen.width / 2 - bSpotSplash.width / 2, Screen.height / 2 - bSpotSplash.height / 2, bSpotSplash.width, bSpotSplash.height), bSpotSplash);
        }
	}
}
