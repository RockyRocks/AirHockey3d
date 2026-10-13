using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class MobilePlatformTargets : IPreprocessBuildWithReport
{
	public int callbackOrder
	{
		get { return 0; }
	}

	public void OnPreprocessBuild(BuildReport report)
	{
		Apply();
	}

	[InitializeOnLoadMethod]
	static void ApplyOnLoad()
	{
		Apply();
	}

	public static void Apply()
	{
		PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel31;
		PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
		PlayerSettings.iOS.targetOSVersionString = "13.0";
		PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
		PlayerSettings.allowedAutorotateToPortrait = false;
		PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
		PlayerSettings.allowedAutorotateToLandscapeLeft = true;
		PlayerSettings.allowedAutorotateToLandscapeRight = true;
	}
}
