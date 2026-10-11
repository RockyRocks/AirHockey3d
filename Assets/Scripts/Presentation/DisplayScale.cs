using System;

/// <summary>
/// Picks UI size and render scale from the real screen. A large high-density
/// display gets bigger type and a native image. A low tier keeps the same
/// framing but draws fewer pixels.
/// </summary>
public static class DisplayScale
{
	public static float ShortSide(float width, float height)
	{
		return width < height ? width : height;
	}

	public static float UiScale(float dpi, float width, float height)
	{
		float shortSide = ShortSide(width, height);
		float fromDpi = dpi <= 1f ? shortSide / 400f : dpi / 160f;
		float fromSize = shortSide / 720f;
		float scale = fromDpi > fromSize ? fromDpi : fromSize;
		if (scale < 1.25f)
			scale = 1.25f;
		if (scale > 3.2f)
			scale = 3.2f;
		return scale;
	}

	public static int FontSize(float uiScale, float body)
	{
		int size = (int)Math.Round(body * uiScale);
		if (size < 18)
			size = 18;
		if (size > 72)
			size = 72;
		return size;
	}

	public static float RenderScale(int tier, float shortSide)
	{
		if (tier <= 0)
			return shortSide >= 1440f ? 0.5f : 0.7f;
		if (tier == 1)
			return shortSide >= 1600f ? 0.8f : 1f;
		return 1f;
	}

	public static int Msaa(int tier, float shortSide)
	{
		if (tier <= 0)
			return 0;
		if (tier == 1)
			return shortSide >= 1080f ? 2 : 0;
		return shortSide >= 1080f ? 4 : 2;
	}
}
