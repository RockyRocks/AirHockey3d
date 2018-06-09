using UnityEngine;
using System.Collections;

public class CountriesScript : MonoBehaviour
{

    public AudioClip buttonClip;

    public Texture2D back;
    public Rect backRect;
    public GUIStyle backStyle;

    public Texture2D settings;
    public Rect settingsRect;

    public Texture2D GreatBritian;
    public Texture2D Germany;
    public Texture2D USA;
    public Texture2D Czech;
    public Texture2D Finland;
    public Texture2D Russia;
    public Texture2D Canada;
    public Texture2D Sweden;

    public Rect GreatBritianRect;
    public Rect GermanyRect;
    public Rect USARect;
    public Rect CzechRect;
    public Rect FinlandRect;
    public Rect RussiaRect;
    public Rect CanadaRect;
    public Rect SwedenRect;

    public GUIStyle textStyle;

    int buttonWidth = 64;
    int buttonHeight = 64;
    Vector2 BackFrame;
    Vector2 offset;         
    // Use this for initialization
    void Start()
    {
        Properties.CurrentScene = "Countries";
        offset = new Vector2(120,100);
        BackFrame = new Vector2(427, 247);
		Properties.semis_1 = Random.Range (1,2);
		Properties.semis_2 = Random.Range (3, 4);
		Properties.semis_3 = Random.Range (4, 5);
		Properties.finals_1 = Random.Range (Properties.semis_2,Properties.semis_3);
        backRect = new Rect(15, 525, buttonWidth, buttonHeight);
        
        settingsRect = new Rect(880, 525, buttonWidth, buttonHeight);
        GreatBritianRect = new Rect((Screen.width / 2- BackFrame.x)+GreatBritian.width/2+20, (Screen.height/2-BackFrame.y)+offset.y, GreatBritian.width, GreatBritian.height);
        GermanyRect = new Rect((Screen.width / 2 - BackFrame.x) + Germany.width / 2 + 220, (Screen.height / 2 - BackFrame.y) + offset.y, Germany.width, Germany.height);
        USARect = new Rect((Screen.width / 2 - BackFrame.x) + USA.width / 2 + 420, (Screen.height / 2 - BackFrame.y) + offset.y, USA.width, USA.height);
        CzechRect = new Rect((Screen.width / 2 - BackFrame.x) + Czech.width / 2 + 620, (Screen.height / 2 - BackFrame.y) + offset.y, Czech.width, Czech.height);
        FinlandRect = new Rect((Screen.width / 2 - BackFrame.x) + Finland.width / 2+20 , (Screen.height / 2 - BackFrame.y +167) + offset.y, Finland.width, Finland.height);
        RussiaRect = new Rect((Screen.width / 2 - BackFrame.x) + Russia.width / 2 + 220, (Screen.height / 2 - BackFrame.y + 167) + offset.y, Russia.width, Russia.height);
        CanadaRect = new Rect((Screen.width / 2 - BackFrame.x) + Canada.width / 2 + 420, (Screen.height / 2 - BackFrame.y+167) + offset.y, Canada.width, Canada.height);
        SwedenRect = new Rect((Screen.width / 2 - BackFrame.x) + Sweden.width / 2 + 620, (Screen.height / 2 - BackFrame.y+167) + offset.y, Sweden.width, Sweden.height);
       
    }


    IEnumerator PlayAudio(AudioClip audioClip, string buttonName)
    {
        audio.volume = Properties.sfxVolume;
        audio.PlayOneShot(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        switch (buttonName)
        {
            case "Back":
                WorkAround.LoadLevelWorkaround(Properties.PreviousScene);
                break;
        }
    }

    void OnGUI()
    {
        GUI.depth = 0;
        if (GUI.Button(GreatBritianRect, GreatBritian, textStyle))
        {
            Properties.countries = Properties.Countries.GreatBritian;
            WorkAround.LoadLevelWorkaround("Airhockey");
        }
        if (GUI.Button(GermanyRect, Germany, textStyle))
        {
            Properties.countries = Properties.Countries.Germany;
            WorkAround.LoadLevelWorkaround("Airhockey");
        }
        if (GUI.Button(USARect, USA, textStyle))
        {
            Properties.countries = Properties.Countries.USA;
            WorkAround.LoadLevelWorkaround("Airhockey");
        }
        if (GUI.Button(CzechRect, Czech, textStyle))
        {
            Properties.countries = Properties.Countries.CzechRepublic;
            WorkAround.LoadLevelWorkaround("Airhockey");
        }
        if (GUI.Button(FinlandRect, Finland, textStyle))
        {
            Properties.countries = Properties.Countries.Finland;
            WorkAround.LoadLevelWorkaround("Airhockey");
        }
        if (GUI.Button(RussiaRect, Russia, textStyle))
        {
            Properties.countries = Properties.Countries.Russia;
            WorkAround.LoadLevelWorkaround("Airhockey");
        }
        if (GUI.Button(CanadaRect, Canada, textStyle))
        {
            Properties.countries = Properties.Countries.Canada;
            WorkAround.LoadLevelWorkaround("Airhockey");
        }
        if (GUI.Button(SwedenRect, Sweden, textStyle))
        {
            Properties.countries = Properties.Countries.Sweden;
            WorkAround.LoadLevelWorkaround("Airhockey");
        }        
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
