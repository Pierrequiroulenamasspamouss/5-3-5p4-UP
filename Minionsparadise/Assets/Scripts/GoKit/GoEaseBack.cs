public static class GoEaseBack
{
	public static float EaseIn(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * (2.70158f * t - 1.70158f) + b;
	}

	public static float EaseOut(float t, float b, float c, float d)
	{
		return c * ((t = t / d - 1f) * t * (2.70158f * t + 1.70158f) + 1f) + b;
	}

	public static float EaseInOut(float t, float b, float c, float d)
	{
		float num = 1.70158f;
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
		}
		return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
	}
}
