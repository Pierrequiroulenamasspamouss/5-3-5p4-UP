public static class GoEaseAnimationCurve
{
	public static global::System.Func<float, float, float, float, float> EaseCurve(GoTween tween)
	{
		if (tween == null)
		{
			global::UnityEngine.Debug.LogError("no tween to extract easeCurve from");
		}
		if (tween.easeCurve == null)
		{
			global::UnityEngine.Debug.LogError("no curve found for tween");
		}
		return (float t, float b, float c, float d) => tween.easeCurve.Evaluate(t / d) * c + b;
	}
}
