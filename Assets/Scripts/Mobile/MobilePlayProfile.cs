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
		QualitySettings.pixelLightCount = 1;
		QualitySettings.shadows = ShadowQuality.Disable;
		QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
		if (SystemInfo.systemMemorySize > 0 && SystemInfo.systemMemorySize <= 2048)
			QualitySettings.globalTextureMipmapLimit = 1;
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	static void StripHeavyEffects()
	{
		if (!Application.isMobilePlatform)
			return;
		MonoBehaviour[] behaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
		for (int i = 0; i < behaviours.Length; i++)
		{
			if (behaviours[i] == null)
				continue;
			string name = behaviours[i].GetType().Name;
			if (name.IndexOf("MotionBlur", System.StringComparison.Ordinal) >= 0
				|| name.IndexOf("Bloom", System.StringComparison.Ordinal) >= 0
				|| name.IndexOf("SSAO", System.StringComparison.Ordinal) >= 0
				|| name.IndexOf("DepthOfField", System.StringComparison.Ordinal) >= 0
				|| name.IndexOf("SunShafts", System.StringComparison.Ordinal) >= 0
				|| name.IndexOf("Vignetting", System.StringComparison.Ordinal) >= 0
				|| name.IndexOf("Antialiasing", System.StringComparison.Ordinal) >= 0)
			{
				behaviours[i].enabled = false;
			}
		}
	}
}
