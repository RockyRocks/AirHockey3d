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

	void OnGUI()
	{
		float scale = Mathf.Clamp(Screen.dpi <= 0f ? 1.5f : Screen.dpi / 160f, 1.15f, 2.4f);
		float width = Mathf.Min(Screen.width - 24f, 420f * scale);
		float x = (Screen.width - width) * 0.5f;

		if (panel == Panel.Tutorial && !menuOpen)
		{
			DrawBanner(x, 16f, width, 72f * scale, CoachText(), scale);
			if (GUI.Button(new Rect(x, 16f + 76f * scale, 120f * scale, 40f * scale), "Menu"))
				menuOpen = true;
			return;
		}

		if (!menuOpen)
		{
			if (GUI.Button(new Rect(16f, 16f, 120f * scale, 44f * scale), "Menu"))
				menuOpen = true;
			return;
		}

		float y = 16f;
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

	static float DrawBanner(float x, float y, float width, float height, string text, float scale)
	{
		GUI.Box(new Rect(x, y, width, height), "");
		GUI.Label(new Rect(x + 12f, y + 8f, width - 24f, height - 12f), text);
		return y + height;
	}

	static float Button(float x, float y, float width, float height, string text, float scale, System.Action action)
	{
		if (GUI.Button(new Rect(x, y, width, height), text) && action != null)
			action();
		return y + height;
	}
}
