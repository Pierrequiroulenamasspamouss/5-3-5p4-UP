public static class GoTweenUtils
{
	public static global::System.Func<float, float, float, float, float> easeFunctionForType(GoEaseType easeType, GoTween tween = null)
	{
		switch (easeType)
		{
		case GoEaseType.Linear:
			return GoEaseLinear.EaseNone;
		case GoEaseType.BackIn:
			return GoEaseBack.EaseIn;
		case GoEaseType.BackOut:
			return GoEaseBack.EaseOut;
		case GoEaseType.BackInOut:
			return GoEaseBack.EaseInOut;
		case GoEaseType.BounceIn:
			return GoEaseBounce.EaseIn;
		case GoEaseType.BounceOut:
			return GoEaseBounce.EaseOut;
		case GoEaseType.BounceInOut:
			return GoEaseBounce.EaseInOut;
		case GoEaseType.CircIn:
			return GoEaseCircular.EaseIn;
		case GoEaseType.CircOut:
			return GoEaseCircular.EaseOut;
		case GoEaseType.CircInOut:
			return GoEaseCircular.EaseInOut;
		case GoEaseType.CubicIn:
			return GoEaseCubic.EaseIn;
		case GoEaseType.CubicOut:
			return GoEaseCubic.EaseOut;
		case GoEaseType.CubicInOut:
			return GoEaseCubic.EaseInOut;
		case GoEaseType.ElasticIn:
			return GoEaseElastic.EaseIn;
		case GoEaseType.ElasticOut:
			return GoEaseElastic.EaseOut;
		case GoEaseType.ElasticInOut:
			return GoEaseElastic.EaseInOut;
		case GoEaseType.Punch:
			return GoEaseElastic.Punch;
		case GoEaseType.ExpoIn:
			return GoEaseExponential.EaseIn;
		case GoEaseType.ExpoOut:
			return GoEaseExponential.EaseOut;
		case GoEaseType.ExpoInOut:
			return GoEaseExponential.EaseInOut;
		case GoEaseType.QuadIn:
			return GoEaseQuadratic.EaseIn;
		case GoEaseType.QuadOut:
			return GoEaseQuadratic.EaseOut;
		case GoEaseType.QuadInOut:
			return GoEaseQuadratic.EaseInOut;
		case GoEaseType.QuartIn:
			return GoEaseQuartic.EaseIn;
		case GoEaseType.QuartOut:
			return GoEaseQuartic.EaseOut;
		case GoEaseType.QuartInOut:
			return GoEaseQuartic.EaseInOut;
		case GoEaseType.QuintIn:
			return GoEaseQuintic.EaseIn;
		case GoEaseType.QuintOut:
			return GoEaseQuintic.EaseOut;
		case GoEaseType.QuintInOut:
			return GoEaseQuintic.EaseInOut;
		case GoEaseType.SineIn:
			return GoEaseSinusoidal.EaseIn;
		case GoEaseType.SineOut:
			return GoEaseSinusoidal.EaseOut;
		case GoEaseType.SineInOut:
			return GoEaseSinusoidal.EaseInOut;
		case GoEaseType.AnimationCurve:
			return GoEaseAnimationCurve.EaseCurve(tween);
		default:
			return GoEaseLinear.EaseNone;
		}
	}

	public static T setterForProperty<T>(object targetObject, string propertyName)
	{
		global::System.Reflection.PropertyInfo property = targetObject.GetType().GetProperty(propertyName);
		if (property == null)
		{
			global::UnityEngine.Debug.Log("could not find property with name: " + propertyName);
			return default(T);
		}
		return (T)(object)global::System.Delegate.CreateDelegate(typeof(T), targetObject, property.GetSetMethod());
	}

	public static T getterForProperty<T>(object targetObject, string propertyName)
	{
		global::System.Reflection.PropertyInfo property = targetObject.GetType().GetProperty(propertyName);
		if (property == null)
		{
			global::UnityEngine.Debug.Log("could not find property with name: " + propertyName);
			return default(T);
		}
		return (T)(object)global::System.Delegate.CreateDelegate(typeof(T), targetObject, property.GetGetMethod());
	}

	public static global::UnityEngine.Color unclampedColorLerp(global::UnityEngine.Color c1, global::UnityEngine.Color diff, float value)
	{
		return new global::UnityEngine.Color(c1.r + diff.r * value, c1.g + diff.g * value, c1.b + diff.b * value, c1.a + diff.a * value);
	}

	public static global::UnityEngine.Vector2 unclampedVector2Lerp(global::UnityEngine.Vector2 v1, global::UnityEngine.Vector2 diff, float value)
	{
		return new global::UnityEngine.Vector2(v1.x + diff.x * value, v1.y + diff.y * value);
	}

	public static global::UnityEngine.Vector3 unclampedVector3Lerp(global::UnityEngine.Vector3 v1, global::UnityEngine.Vector3 diff, float value)
	{
		return new global::UnityEngine.Vector3(v1.x + diff.x * value, v1.y + diff.y * value, v1.z + diff.z * value);
	}

	public static global::UnityEngine.Vector4 unclampedVector4Lerp(global::UnityEngine.Vector4 v1, global::UnityEngine.Vector4 diff, float value)
	{
		return new global::UnityEngine.Vector4(v1.x + diff.x * value, v1.y + diff.y * value, v1.z + diff.z * value, v1.w + diff.w * value);
	}
}
