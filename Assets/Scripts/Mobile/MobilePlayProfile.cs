using UnityEngine;

public static class MobilePlayProfile
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void Apply()
	{
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Input.multiTouchEnabled = true;
		Time.fixedDeltaTime = 1f / 60f;
		Time.maximumDeltaTime = 0.05f;
		GraphicsTierSettings.LoadOrSuggest();
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static void ApplyTierToScene()
	{
		GraphicsTierSettings.Apply(GraphicsTierSettings.Current);
	}
}
