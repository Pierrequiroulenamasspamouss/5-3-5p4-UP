public static class GoEaseExponential
{
	public static float EaseIn(float t, float b, float c, float d)
	{
		if (t != 0f)
		{
			return c * (float)global::System.Math.Pow(2.0, 10f * (t / d - 1f)) + b;
		}
		return b;
	}

	public static float EaseOut(float t, float b, float c, float d)
	{
		if (t != d)
		{
			return c * (float)(0.0 - global::System.Math.Pow(2.0, -10f * t / d) + 1.0) + b;
		}
		return b + c;
	}

	public static float EaseInOut(float t, float b, float c, float d)
	{
		if (t == 0f)
		{
			return b;
		}
		if (t == d)
		{
			return b + c;
		}
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * (float)global::System.Math.Pow(2.0, 10f * (t - 1f)) + b;
		}
		return c / 2f * (float)(0.0 - global::System.Math.Pow(2.0, -10f * (t -= 1f)) + 2.0) + b;
	}
}
