using UnityEngine;

/// <summary>
/// Sharpens the table for the current screen and paints the rink, puck, and
/// mallets with a glossy arcade surface. The camera grade stays cheap enough
/// for a phone and is stronger on the high tier.
/// </summary>
public class TablePresentation : MonoBehaviour
{
	public float saturation = 1.18f;
	public float contrast = 1.06f;
	public float vignette = 0.22f;

	Material gradeMaterial;
	static Material puckMaterial;
	static Material playerMaterial;
	static Material opponentMaterial;
	static Material rinkMaterial;
	static readonly int saturationId = Shader.PropertyToID("_Saturation");
	static readonly int contrastId = Shader.PropertyToID("_Contrast");
	static readonly int vignetteId = Shader.PropertyToID("_Vignette");
	static readonly int liftId = Shader.PropertyToID("_Lift");

	public static void Apply(GraphicsTier tier)
	{
		float shortSide = DisplayScale.ShortSide(Screen.width, Screen.height);
		int tierIndex = (int)tier;
		float renderScale = DisplayScale.RenderScale(tierIndex, shortSide);
		int msaa = DisplayScale.Msaa(tierIndex, shortSide);
		QualitySettings.antiAliasing = msaa;
		ScalableBufferManager.ResizeBuffers(renderScale, renderScale);

		Camera[] cameras = Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);
		for (int i = 0; i < cameras.Length; i++)
		{
			Camera camera = cameras[i];
			if (camera == null)
				continue;
			camera.allowHDR = tier != GraphicsTier.Low;
			camera.allowMSAA = msaa > 0;
			if (camera.clearFlags == CameraClearFlags.SolidColor || camera.clearFlags == CameraClearFlags.Skybox)
			{
				camera.clearFlags = CameraClearFlags.SolidColor;
				camera.backgroundColor = new Color(0.015f, 0.03f, 0.06f, 1f);
			}
			TablePresentation grade = camera.GetComponent<TablePresentation>();
			if (grade == null)
				grade = camera.gameObject.AddComponent<TablePresentation>();
			grade.SetGrade(tier);
		}

		RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
		RenderSettings.ambientLight = new Color(0.16f, 0.24f, 0.34f, 1f);
		DressSurfaces();
	}

	public void SetGrade(GraphicsTier tier)
	{
		if (tier == GraphicsTier.Low)
		{
			saturation = 1.08f;
			contrast = 1.04f;
			vignette = 0.12f;
		}
		else if (tier == GraphicsTier.Medium)
		{
			saturation = 1.18f;
			contrast = 1.08f;
			vignette = 0.22f;
		}
		else
		{
			saturation = 1.28f;
			contrast = 1.1f;
			vignette = 0.3f;
		}
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (gradeMaterial == null)
		{
			Shader shader = LoadShader("AirHockeyGrade", "Hidden/AirHockey/Grade");
			if (shader != null)
				gradeMaterial = new Material(shader);
		}
		if (gradeMaterial == null)
		{
			Graphics.Blit(source, destination);
			return;
		}
		gradeMaterial.SetFloat(saturationId, saturation);
		gradeMaterial.SetFloat(contrastId, contrast);
		gradeMaterial.SetFloat(vignetteId, vignette);
		gradeMaterial.SetColor(liftId, new Color(0.015f, 0.035f, 0.06f, 0f));
		Graphics.Blit(source, destination, gradeMaterial);
	}

	static void DressSurfaces()
	{
		Shader shader = LoadShader("AirHockeySurface", "AirHockey/Surface");
		if (shader == null)
			return;
		if (puckMaterial == null)
		{
			puckMaterial = Surface(shader, new Color(0.86f, 0.07f, 0.13f), new Color(1f, 0.48f, 0.32f), 0.92f, 0.12f, 0.4f, 0f);
			playerMaterial = Surface(shader, new Color(0.0f, 0.42f, 0.78f), new Color(0.35f, 0.95f, 1f), 0.84f, 0.18f, 0.6f, 0f);
			opponentMaterial = Surface(shader, new Color(0.62f, 0.05f, 0.38f), new Color(1f, 0.28f, 0.62f), 0.84f, 0.18f, 0.6f, 0f);
			rinkMaterial = Surface(shader, new Color(0.015f, 0.1f, 0.2f), new Color(0.15f, 0.92f, 1f), 0.9f, 0.04f, 0.55f, 1f);
		}
		Renderer[] renderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
		for (int i = 0; i < renderers.Length; i++)
		{
			Renderer renderer = renderers[i];
			if (renderer == null || renderer is ParticleSystemRenderer || renderer is SpriteRenderer)
				continue;
			Material material = MaterialFor(renderer.gameObject);
			if (material == null)
				continue;
			renderer.sharedMaterial = material;
		}
	}

	static Material MaterialFor(GameObject target)
	{
		string tag = target.tag;
		if (tag == "Puck")
			return puckMaterial;
		if (tag == "Player")
			return playerMaterial;
		if (tag == "AIPlayer")
			return opponentMaterial;
		string lower = target.name.ToLowerInvariant();
		if (lower.Contains("table") || lower.Contains("board") || lower.Contains("rink"))
			return rinkMaterial;
		return null;
	}

	static Shader LoadShader(string resourceName, string shaderName)
	{
		Shader shader = Resources.Load<Shader>(resourceName);
		if (shader == null)
			shader = Shader.Find(shaderName);
		return shader;
	}

	static Material Surface(Shader shader, Color baseColor, Color accent, float smoothness, float metallic, float emission, float stripe)
	{
		Material material = new Material(shader);
		material.SetColor("_BaseColor", baseColor);
		material.SetColor("_Accent", accent);
		material.SetFloat("_Smoothness", smoothness);
		material.SetFloat("_Metallic", metallic);
		material.SetFloat("_Emission", emission);
		material.SetFloat("_Stripe", stripe);
		return material;
	}
}
