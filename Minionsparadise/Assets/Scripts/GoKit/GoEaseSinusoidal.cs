public static class GoEaseSinusoidal
{
	public static float EaseIn(float t, float b, float c, float d)
	{
		return (0f - c) * (float)global::System.Math.Cos((double)(t / d) * (global::System.Math.PI / 2.0)) + c + b;
	}

	public static float EaseOut(float t, float b, float c, float d)
	{
		return c * (float)global::System.Math.Sin((double)(t / d) * (global::System.Math.PI / 2.0)) + b;
	}

	public static float EaseInOut(float t, float b, float c, float d)
	{
		return (0f - c) / 2f * ((float)global::System.Math.Cos(global::System.Math.PI * (double)t / (double)d) - 1f) + b;
	}
}
