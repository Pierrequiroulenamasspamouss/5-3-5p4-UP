public static class GoEaseCircular
{
	public static float EaseIn(float t, float b, float c, float d)
	{
		return (0f - c) * ((float)global::System.Math.Sqrt(1f - (t /= d) * t) - 1f) + b;
	}

	public static float EaseOut(float t, float b, float c, float d)
	{
		return c * (float)global::System.Math.Sqrt(1f - (t = t / d - 1f) * t) + b;
	}

	public static float EaseInOut(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return (0f - c) / 2f * ((float)global::System.Math.Sqrt(1f - t * t) - 1f) + b;
		}
		return c / 2f * ((float)global::System.Math.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
	}
}
