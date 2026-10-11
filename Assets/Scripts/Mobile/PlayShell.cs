using UnityEngine;

/// <summary>
/// First-run tutorial, practice against the server AI, and the graphics choice.
/// The panel uses the real screen size so the same buttons stay readable on a phone.
/// </summary>
public class PlayShell : MonoBehaviour
{
	const string FtueKey = "ftue.done";

	enum Panel
	{
		Welcome,
		Tutorial,
		Practice
	}

	Panel panel;
	int coachStep;
	int goalsAtStart;
	int hitsAtStart;
	Vector3 malletAtStart;
	bool menuOpen = true;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static void Install()
	{
		if (Object.FindAnyObjectByType<PlayShell>() != null)
			return;
		if (GameObject.FindGameObjectWithTag("Puck") == null)
			return;
		GameObject host = new GameObject("PlayShell");
		host.AddComponent<PlayShell>();
	}

	void Awake()
	{
		panel = PlayerPrefs.GetInt(FtueKey, 0) == 1 ? Panel.Practice : Panel.Welcome;
		if (panel == Panel.Welcome)
			ServerAiDriver.Difficulty = AiDifficulty.Easy;
		menuOpen = true;
	}

	void Update()
	{
		if (panel != Panel.Tutorial)
			return;
		if (coachStep == 0 && MalletMoved())
			coachStep = 1;
		else if (coachStep == 1 && PuckHit())
			coachStep = 2;
		else if (coachStep == 2 && Scored())
		{
			PlayerPrefs.SetInt(FtueKey, 1);
			PlayerPrefs.Save();
			panel = Panel.Practice;
			menuOpen = true;
			coachStep = 3;
		}
	}

	GUIStyle titleStyle;
	GUIStyle bodyStyle;
	GUIStyle buttonStyle;
	float styledScale = -1f;

	void OnGUI()
	{
		Rect safe = Screen.safeArea;
		float scale = DisplayScale.UiScale(Screen.dpi, Screen.width, Screen.height);
		EnsureStyles(scale);
		float left = safe.xMin + 12f * scale;
		float top = Screen.height - safe.yMax + 12f * scale;
		float width = Mathf.Min(safe.width - 24f * scale, 460f * scale);
		float x = safe.xMin + (safe.width - width) * 0.5f;

		if (panel == Panel.Tutorial && !menuOpen)
		{
			DrawBanner(x, top, width, 88f * scale, CoachText(), scale);
			if (GUI.Button(new Rect(x, top + 96f * scale, 140f * scale, 48f * scale), "Menu", buttonStyle))
				menuOpen = true;
			return;
		}

		if (!menuOpen)
		{
			if (GUI.Button(new Rect(left, top, 140f * scale, 52f * scale), "Menu", buttonStyle))
				menuOpen = true;
			return;
		}

		float y = top;
		y = DrawBanner(x, y, width, 64f * scale, Title(), scale) + 8f;
		if (panel == Panel.Welcome)
		{
			y = Button(x, y, width, 52f * scale, "Start tutorial", scale, BeginTutorial) + 8f;
			y = Button(x, y, width, 52f * scale, "Skip to practice", scale, SkipTutorial) + 8f;
		}
		else if (panel == Panel.Tutorial)
		{
			y = DrawBanner(x, y, width, 64f * scale, CoachText(), scale) + 8f;
			y = Button(x, y, width, 48f * scale, "Back to table", scale, CloseMenu) + 8f;
		}
		else
		{
			y = DrawBanner(x, y, width, 36f * scale, "Practice against the AI", scale) + 8f;
			y = Button(x, y, width, 48f * scale, "Easy", scale, delegate { BeginPractice(AiDifficulty.Easy); }) + 6f;
			y = Button(x, y, width, 48f * scale, "Medium", scale, delegate { BeginPractice(AiDifficulty.Medium); }) + 6f;
			y = Button(x, y, width, 48f * scale, "Hard", scale, delegate { BeginPractice(AiDifficulty.Hard); }) + 10f;
		}

		y = DrawBanner(x, y, width, 32f * scale, "Graphics", scale) + 6f;
		Button(x, y, width / 3f - 4f, 44f * scale, TierLabel(GraphicsTier.Low), scale, delegate { GraphicsTierSettings.Apply(GraphicsTier.Low); });
		Button(x + width / 3f, y, width / 3f - 4f, 44f * scale, TierLabel(GraphicsTier.Medium), scale, delegate { GraphicsTierSettings.Apply(GraphicsTier.Medium); });
		Button(x + (width / 3f) * 2f, y, width / 3f - 4f, 44f * scale, TierLabel(GraphicsTier.High), scale, delegate { GraphicsTierSettings.Apply(GraphicsTier.High); });
	}

	string Title()
	{
		if (panel == Panel.Welcome)
			return "Air hockey";
		if (panel == Panel.Tutorial)
			return "Tutorial";
		if (coachStep == 3)
			return "Tutorial complete";
		return "Practice";
	}

	string CoachText()
	{
		if (coachStep <= 0)
			return "Drag in your half. The mallet follows your finger.";
		if (coachStep == 1)
			return "Hit the puck. It slides and bounces off the rails.";
		if (coachStep == 2)
			return "Score in the far goal.";
		return "Nice goal. Pick a practice opponent.";
	}

	void BeginTutorial()
	{
		ServerAiDriver.Difficulty = AiDifficulty.Easy;
		AuthoritativeMatch.AiControlsTop = true;
		goalsAtStart = Properties.HumanplayerGoals;
		hitsAtStart = CurrentHits();
		malletAtStart = MalletPosition();
		coachStep = 0;
		panel = Panel.Tutorial;
		menuOpen = false;
		ResetPuck();
	}

	void SkipTutorial()
	{
		PlayerPrefs.SetInt(FtueKey, 1);
		PlayerPrefs.Save();
		panel = Panel.Practice;
	}

	void BeginPractice(AiDifficulty difficulty)
	{
		ServerAiDriver.Difficulty = difficulty;
		AuthoritativeMatch.AiControlsTop = true;
		panel = Panel.Practice;
		menuOpen = false;
		ResetPuck();
	}

	void CloseMenu()
	{
		menuOpen = false;
	}

	static void ResetPuck()
	{
		GameObject puck = GameObject.FindGameObjectWithTag("Puck");
		if (puck == null)
			return;
		PuckBehaviour behaviour = puck.GetComponent<PuckBehaviour>();
		if (behaviour != null)
			behaviour.Reset();
	}

	bool MalletMoved()
	{
		Vector3 now = MalletPosition();
		return Vector3.Distance(now, malletAtStart) > 0.35f;
	}

	bool PuckHit()
	{
		return CurrentHits() > hitsAtStart;
	}

	bool Scored()
	{
		return Properties.HumanplayerGoals > goalsAtStart;
	}

	static int CurrentHits()
	{
		GameObject puck = GameObject.FindGameObjectWithTag("Puck");
		if (puck == null)
			return 0;
		PuckBehaviour behaviour = puck.GetComponent<PuckBehaviour>();
		return behaviour != null ? behaviour.PlayerHits : 0;
	}

	static Vector3 MalletPosition()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		return player != null ? player.transform.position : Vector3.zero;
	}

	static string TierLabel(GraphicsTier tier)
	{
		string name = tier == GraphicsTier.Low ? "Low" : tier == GraphicsTier.Medium ? "Med" : "High";
		return GraphicsTierSettings.Current == tier ? "[" + name + "]" : name;
	}

	float DrawBanner(float x, float y, float width, float height, string text, float scale)
	{
		Color previous = GUI.backgroundColor;
		GUI.backgroundColor = new Color(0.04f, 0.16f, 0.28f, 0.94f);
		GUI.Box(new Rect(x, y, width, height), "", titleStyle);
		GUI.backgroundColor = previous;
		GUI.Label(new Rect(x + 16f * scale, y + 8f, width - 32f * scale, height - 12f), text, bodyStyle);
		return y + height;
	}

	float Button(float x, float y, float width, float height, string text, float scale, System.Action action)
	{
		Color previous = GUI.backgroundColor;
		GUI.backgroundColor = new Color(0.0f, 0.55f, 0.78f, 1f);
		bool pressed = GUI.Button(new Rect(x, y, width, height), text, buttonStyle);
		GUI.backgroundColor = previous;
		if (pressed && action != null)
			action();
		return y + height;
	}

	void EnsureStyles(float scale)
	{
		if (titleStyle != null && Mathf.Abs(styledScale - scale) < 0.01f)
			return;
		styledScale = scale;
		int font = DisplayScale.FontSize(scale, 18f);
		titleStyle = new GUIStyle(GUI.skin.box);
		titleStyle.fontSize = font;
		titleStyle.alignment = TextAnchor.MiddleLeft;
		bodyStyle = new GUIStyle(GUI.skin.label);
		bodyStyle.fontSize = font;
		bodyStyle.wordWrap = true;
		bodyStyle.alignment = TextAnchor.MiddleLeft;
		bodyStyle.normal.textColor = new Color(0.93f, 0.98f, 1f);
		buttonStyle = new GUIStyle(GUI.skin.button);
		buttonStyle.fontSize = font;
		buttonStyle.alignment = TextAnchor.MiddleCenter;
		buttonStyle.normal.textColor = Color.white;
		buttonStyle.hover.textColor = Color.white;
	}
}
