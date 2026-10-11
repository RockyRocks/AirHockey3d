using UnityEngine;

public enum GraphicsTier
{
	Low = 0,
	Medium = 1,
	High = 2
}

public static class GraphicsTierSettings
{
	const string PrefKey = "settings.graphics";

	public static GraphicsTier Current { get; private set; }

	public static GraphicsTier Suggested()
	{
		int memory = SystemInfo.systemMemorySize;
		if (memory > 0 && memory <= 3072)
			return GraphicsTier.Low;
		if (memory > 0 && memory <= 6144)
			return GraphicsTier.Medium;
		return GraphicsTier.High;
	}

	public static void LoadOrSuggest()
	{
		if (PlayerPrefs.HasKey(PrefKey))
			Apply((GraphicsTier)PlayerPrefs.GetInt(PrefKey, (int)Suggested()));
		else
			Apply(Suggested());
	}

	public static void Apply(GraphicsTier tier)
	{
		Current = tier;
		PlayerPrefs.SetInt(PrefKey, (int)tier);
		PlayerPrefs.Save();
		Time.fixedDeltaTime = 1f / 60f;
		Time.maximumDeltaTime = 0.05f;

		if (tier == GraphicsTier.Low)
		{
			Application.targetFrameRate = 30;
			QualitySettings.globalTextureMipmapLimit = 1;
			QualitySettings.pixelLightCount = 0;
			QualitySettings.shadows = ShadowQuality.Disable;
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
			QualitySettings.antiAliasing = 0;
		}
		else if (tier == GraphicsTier.Medium)
		{
			Application.targetFrameRate = 60;
			QualitySettings.globalTextureMipmapLimit = 0;
			QualitySettings.pixelLightCount = 1;
			QualitySettings.shadows = ShadowQuality.Disable;
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
			QualitySettings.antiAliasing = 0;
		}
		else
		{
			Application.targetFrameRate = 60;
			QualitySettings.globalTextureMipmapLimit = 0;
			QualitySettings.pixelLightCount = 2;
			QualitySettings.shadows = ShadowQuality.HardOnly;
			QualitySettings.shadowDistance = 18f;
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
			QualitySettings.antiAliasing = 2;
		}

		SetHeavyEffects(tier == GraphicsTier.High && !Application.isMobilePlatform);
		TablePresentation.Apply(tier);
	}

	public static void SetHeavyEffects(bool enabled)
	{
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
				behaviours[i].enabled = enabled;
			}
		}
	}
}
