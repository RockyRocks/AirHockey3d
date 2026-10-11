using UnityEngine;

public static class PointerInput
{
	public static bool TryGet(out Vector2 screen, out bool held)
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			screen = touch.position;
			held = touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled;
			return true;
		}

		screen = Input.mousePosition;
		if (Application.isMobilePlatform)
		{
			held = Input.GetMouseButton(0);
			return held;
		}

		held = true;
		return true;
	}

	public static bool IsPressed()
	{
		if (Input.touchCount > 0)
		{
			TouchPhase phase = Input.GetTouch(0).phase;
			return phase != TouchPhase.Ended && phase != TouchPhase.Canceled;
		}
		return Input.GetMouseButton(0);
	}

	public static bool WasReleased()
	{
		if (Input.touchCount > 0)
		{
			TouchPhase phase = Input.GetTouch(0).phase;
			return phase == TouchPhase.Ended || phase == TouchPhase.Canceled;
		}
		return Input.GetMouseButtonUp(0);
	}
}
