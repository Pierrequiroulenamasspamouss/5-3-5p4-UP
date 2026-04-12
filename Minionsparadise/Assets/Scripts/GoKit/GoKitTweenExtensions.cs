public static class GoKitTweenExtensions
{
	public static GoTween rotationTo(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.to(self, duration, new GoTweenConfig().rotation(endValue, isRelative));
	}

	public static GoTween localRotationTo(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.to(self, duration, new GoTweenConfig().localRotation(endValue, isRelative));
	}

	public static GoTween rotationTo(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Quaternion endValue, bool isRelative = false)
	{
		return Go.to(self, duration, new GoTweenConfig().rotation(endValue, isRelative));
	}

	public static GoTween localRotationTo(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Quaternion endValue, bool isRelative = false)
	{
		return Go.to(self, duration, new GoTweenConfig().localRotation(endValue, isRelative));
	}

	public static GoTween eulerAnglesTo(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.to(self, duration, new GoTweenConfig().eulerAngles(endValue, isRelative));
	}

	public static GoTween localEulerAnglesTo(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.to(self, duration, new GoTweenConfig().localEulerAngles(endValue, isRelative));
	}

	public static GoTween positionTo(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.to(self, duration, new GoTweenConfig().position(endValue, isRelative));
	}

	public static GoTween localPositionTo(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.to(self, duration, new GoTweenConfig().localPosition(endValue, isRelative));
	}

	public static GoTween scaleTo(this global::UnityEngine.Transform self, float duration, float endValue, bool isRelative = false)
	{
		return self.scaleTo(duration, new global::UnityEngine.Vector3(endValue, endValue, endValue), isRelative);
	}

	public static GoTween scaleTo(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.to(self, duration, new GoTweenConfig().scale(endValue, isRelative));
	}

	public static GoTween shake(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 shakeMagnitude, GoShakeType shakeType = GoShakeType.Position, int frameMod = 1, bool useLocalProperties = false)
	{
		return Go.to(self, duration, new GoTweenConfig().shake(shakeMagnitude, shakeType, frameMod, useLocalProperties));
	}

	public static GoTween rotationFrom(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.from(self, duration, new GoTweenConfig().rotation(endValue, isRelative));
	}

	public static GoTween localRotationFrom(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.from(self, duration, new GoTweenConfig().localRotation(endValue, isRelative));
	}

	public static GoTween eulerAnglesFrom(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.from(self, duration, new GoTweenConfig().eulerAngles(endValue, isRelative));
	}

	public static GoTween localEulerAnglesFrom(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.from(self, duration, new GoTweenConfig().localEulerAngles(endValue, isRelative));
	}

	public static GoTween positionFrom(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.from(self, duration, new GoTweenConfig().position(endValue, isRelative));
	}

	public static GoTween localPositionFrom(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.from(self, duration, new GoTweenConfig().localPosition(endValue, isRelative));
	}

	public static GoTween scaleFrom(this global::UnityEngine.Transform self, float duration, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		return Go.from(self, duration, new GoTweenConfig().scale(endValue, isRelative));
	}

	public static GoTween colorTo(this global::UnityEngine.Material self, float duration, global::UnityEngine.Color endValue, string colorName = "_Color")
	{
		return Go.to(self, duration, new GoTweenConfig().materialColor(endValue, colorName));
	}

	public static GoTween vectorTo(this global::UnityEngine.Material self, float duration, global::UnityEngine.Vector4 endValue, string propertyName)
	{
		return Go.to(self, duration, new GoTweenConfig().materialVector(endValue, propertyName));
	}

	public static GoTween floatTo(this global::UnityEngine.Material self, float duration, float endValue, string propertyName)
	{
		return Go.to(self, duration, new GoTweenConfig().materialFloat(endValue, propertyName));
	}

	public static GoTween colorFrom(this global::UnityEngine.Material self, float duration, global::UnityEngine.Color endValue, string colorName = "_Color")
	{
		return Go.from(self, duration, new GoTweenConfig().materialColor(endValue, colorName));
	}

	public static GoTween vectorFrom(this global::UnityEngine.Material self, float duration, global::UnityEngine.Vector4 endValue, string propertyName)
	{
		return Go.from(self, duration, new GoTweenConfig().materialVector(endValue, propertyName));
	}

	public static GoTween colorFrom(this global::UnityEngine.Material self, float duration, float endValue, string propertyName)
	{
		return Go.from(self, duration, new GoTweenConfig().materialFloat(endValue, propertyName));
	}
}
