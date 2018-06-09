using UnityEngine;
using System.Collections;

public class Scoring : MonoBehaviour 
{

    public Texture2D attemp1;
    public Texture2D attemp2;
    public Texture2D attemp3;
    public Texture2D attemp4;
    public Texture2D attemp5;

	private static Scoring Instance;
	public static Scoring Get()
	{
		return Instance;
	}
	
	void Awake()
	{
		Instance = this;
	}

	public enum HitType{
		HIT,
		SPARE,
		STRIKE
	};

	public int totalScore = 0;

	public TextMesh playerOneScore;

    public GameObject text;

    //public Texture2D strikeTexture2d;
    //public Texture2D spareTexture2d;

	//private int m_RoundCount;
	//private int	m_FrameCount; 

    //GameObject player1;
    //TextScript player1Script;

    //GameObject player2;
    //TextScript player2Script;

    //GameObject strike;
    //GameObject spare;
    //GameObject frame;

    //TextScript strikeScript;
    //TextScript spareScript;
    //TextScript frameScript;
    TextScript attemptScript;
	//public GameObject sliderQuad;
    GameObject attempts;

	void Start () 
	{

        attempts = Instantiate(text, Vector3.zero, Quaternion.identity) as GameObject;
        attemptScript = attempts.GetComponent<TextScript>();
        attemptScript.texture2D = attemp1;

        attemptScript.Hide();

        if (Properties.GameType == Properties.Modes.PlayforMoney)
        {
            Scoring.Get().UpdateAttemptTexture();
        }
	}

	void Update ()
	{

	}

    public void UpdateAttemptTexture()
    {

        switch (Properties.Attempts) {
            case 1:
                attemptScript.texture2D = attemp1;
                break;
            case 2:
                attemptScript.texture2D = attemp2;
                break;
            case 3:
                attemptScript.texture2D = attemp3;
                break;
            case 4:
                attemptScript.texture2D = attemp4;
                break;
            case 5:
                attemptScript.texture2D = attemp5;
                break;
        }

        StartCoroutine(attemptScript.ScaleInScaleOut());

    }
	public void ResetScore (){
		totalScore = 0;
		playerOneScore.text = totalScore.ToString();
	}
}
