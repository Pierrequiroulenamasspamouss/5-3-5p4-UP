public static class GoEaseQuadratic
{
	public static float EaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t + b;
	}

	public static float EaseOut(float t, float b, float c, float d)
	{
		return (0f - c) * (t /= d) * (t - 2f) + b;
	}

	public static float EaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t + b;
		}
		return (0f - c) / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
	}
}
