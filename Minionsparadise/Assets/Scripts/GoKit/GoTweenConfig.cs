public class GoTweenConfig
{
	private global::System.Collections.Generic.List<AbstractTweenProperty> _tweenProperties = new global::System.Collections.Generic.List<AbstractTweenProperty>(2);

	public int id;

	public float delay;

	public int iterations = 1;

	public float timeScale = 1f;

	public GoLoopType loopType = Go.defaultLoopType;

	public GoEaseType easeType = Go.defaultEaseType;

	public global::UnityEngine.AnimationCurve easeCurve;

	public bool isPaused;

	public GoUpdateType propertyUpdateType = Go.defaultUpdateType;

	public bool isFrom;

	public global::System.Action<AbstractGoTween> onInitHandler;

	public global::System.Action<AbstractGoTween> onBeginHandler;

	public global::System.Action<AbstractGoTween> onIterationStartHandler;

	public global::System.Action<AbstractGoTween> onUpdateHandler;

	public global::System.Action<AbstractGoTween> onIterationEndHandler;

	public global::System.Action<AbstractGoTween> onCompleteHandler;

	public global::System.Collections.Generic.List<AbstractTweenProperty> tweenProperties
	{
		get
		{
			return _tweenProperties;
		}
	}

	public GoTweenConfig position(global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		PositionTweenProperty item = new PositionTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig localPosition(global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		PositionTweenProperty item = new PositionTweenProperty(endValue, isRelative, true);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig positionPath(GoSpline path, bool isRelative = false, GoLookAtType lookAtType = GoLookAtType.None, global::UnityEngine.Transform lookTarget = null)
	{
		PositionPathTweenProperty item = new PositionPathTweenProperty(path, isRelative, false, lookAtType, lookTarget);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig scale(float endValue, bool isRelative = false)
	{
		return scale(new global::UnityEngine.Vector3(endValue, endValue, endValue), isRelative);
	}

	public GoTweenConfig scale(global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		ScaleTweenProperty item = new ScaleTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig scalePath(GoSpline path, bool isRelative = false)
	{
		ScalePathTweenProperty item = new ScalePathTweenProperty(path, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig eulerAngles(global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		EulerAnglesTweenProperty item = new EulerAnglesTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig localEulerAngles(global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		EulerAnglesTweenProperty item = new EulerAnglesTweenProperty(endValue, isRelative, true);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig rotation(global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		RotationTweenProperty item = new RotationTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig localRotation(global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		RotationTweenProperty item = new RotationTweenProperty(endValue, isRelative, true);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig rotation(global::UnityEngine.Quaternion endValue, bool isRelative = false)
	{
		RotationQuaternionTweenProperty item = new RotationQuaternionTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig localRotation(global::UnityEngine.Quaternion endValue, bool isRelative = false)
	{
		RotationQuaternionTweenProperty item = new RotationQuaternionTweenProperty(endValue, isRelative, true);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig anchoredPosition(global::UnityEngine.Vector2 endValue, bool isRelative = false)
	{
		AnchoredPositionTweenProperty item = new AnchoredPositionTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig anchoredPosition3D(global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		AnchoredPosition3DTweenProperty item = new AnchoredPosition3DTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig anchorMax(global::UnityEngine.Vector2 endValue, bool isRelative = false)
	{
		AnchorMaxTweenProperty item = new AnchorMaxTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig anchorMin(global::UnityEngine.Vector2 endValue, bool isRelative = false)
	{
		AnchorMinTweenProperty item = new AnchorMinTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig offsetMax(global::UnityEngine.Vector2 endValue, bool isRelative = false)
	{
		OffsetTweenProperty item = new OffsetTweenProperty(endValue, isRelative, true);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig offsetMin(global::UnityEngine.Vector2 endValue, bool isRelative = false)
	{
		OffsetTweenProperty item = new OffsetTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig pivot(global::UnityEngine.Vector2 endValue, bool isRelative = false)
	{
		PivotTweenProperty item = new PivotTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig sizeDelta(global::UnityEngine.Vector2 endValue, bool isRelative = false)
	{
		SizeDeltaTweenProperty item = new SizeDeltaTweenProperty(endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig materialColor(global::UnityEngine.Color endValue, string colorName = "_Color", bool isRelative = false)
	{
		MaterialColorTweenProperty item = new MaterialColorTweenProperty(endValue, colorName, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig materialVector(global::UnityEngine.Vector4 endValue, string propertyName, bool isRelative = false)
	{
		MaterialVectorTweenProperty item = new MaterialVectorTweenProperty(endValue, propertyName, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig materialFloat(float endValue, string propertyName, bool isRelative = false)
	{
		MaterialFloatTweenProperty item = new MaterialFloatTweenProperty(endValue, propertyName, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig shake(global::UnityEngine.Vector3 shakeMagnitude, GoShakeType shakeType = GoShakeType.Position, int frameMod = 1, bool useLocalProperties = false)
	{
		ShakeTweenProperty item = new ShakeTweenProperty(shakeMagnitude, shakeType, frameMod, useLocalProperties);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig vector2Prop(string propertyName, global::UnityEngine.Vector2 endValue, bool isRelative = false)
	{
		Vector2TweenProperty item = new Vector2TweenProperty(propertyName, endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig vector3Prop(string propertyName, global::UnityEngine.Vector3 endValue, bool isRelative = false)
	{
		Vector3TweenProperty item = new Vector3TweenProperty(propertyName, endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig vector4Prop(string propertyName, global::UnityEngine.Vector4 endValue, bool isRelative = false)
	{
		Vector4TweenProperty item = new Vector4TweenProperty(propertyName, endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig vector3PathProp(string propertyName, GoSpline path, bool isRelative = false)
	{
		Vector3PathTweenProperty item = new Vector3PathTweenProperty(propertyName, path, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig vector3XProp(string propertyName, float endValue, bool isRelative = false)
	{
		Vector3XTweenProperty item = new Vector3XTweenProperty(propertyName, endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig vector3YProp(string propertyName, float endValue, bool isRelative = false)
	{
		Vector3YTweenProperty item = new Vector3YTweenProperty(propertyName, endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig vector3ZProp(string propertyName, float endValue, bool isRelative = false)
	{
		Vector3ZTweenProperty item = new Vector3ZTweenProperty(propertyName, endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig colorProp(string propertyName, global::UnityEngine.Color endValue, bool isRelative = false)
	{
		ColorTweenProperty item = new ColorTweenProperty(propertyName, endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig intProp(string propertyName, int endValue, bool isRelative = false)
	{
		IntTweenProperty item = new IntTweenProperty(propertyName, endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig floatProp(string propertyName, float endValue, bool isRelative = false)
	{
		FloatTweenProperty item = new FloatTweenProperty(propertyName, endValue, isRelative);
		_tweenProperties.Add(item);
		return this;
	}

	public GoTweenConfig addTweenProperty(AbstractTweenProperty tweenProp)
	{
		_tweenProperties.Add(tweenProp);
		return this;
	}

	public GoTweenConfig clearProperties()
	{
		_tweenProperties.Clear();
		return this;
	}

	public GoTweenConfig clearEvents()
	{
		onInitHandler = null;
		onBeginHandler = null;
		onIterationStartHandler = null;
		onUpdateHandler = null;
		onIterationEndHandler = null;
		onCompleteHandler = null;
		return this;
	}

	public GoTweenConfig setDelay(float seconds)
	{
		delay = seconds;
		return this;
	}

	public GoTweenConfig setIterations(int iterations)
	{
		this.iterations = iterations;
		return this;
	}

	public GoTweenConfig setIterations(int iterations, GoLoopType loopType)
	{
		this.iterations = iterations;
		this.loopType = loopType;
		return this;
	}

	public GoTweenConfig setTimeScale(float timeScale)
	{
		this.timeScale = timeScale;
		return this;
	}

	public GoTweenConfig setEaseType(GoEaseType easeType)
	{
		this.easeType = easeType;
		return this;
	}

	public GoTweenConfig setEaseCurve(global::UnityEngine.AnimationCurve easeCurve)
	{
		this.easeCurve = easeCurve;
		easeType = GoEaseType.AnimationCurve;
		return this;
	}

	public GoTweenConfig startPaused()
	{
		isPaused = true;
		return this;
	}

	public GoTweenConfig setUpdateType(GoUpdateType setUpdateType)
	{
		propertyUpdateType = setUpdateType;
		return this;
	}

	public GoTweenConfig setIsFrom()
	{
		isFrom = true;
		return this;
	}

	public GoTweenConfig setIsTo()
	{
		isFrom = false;
		return this;
	}

	public GoTweenConfig onInit(global::System.Action<AbstractGoTween> onInit)
	{
		onInitHandler = onInit;
		return this;
	}

	public GoTweenConfig onBegin(global::System.Action<AbstractGoTween> onBegin)
	{
		onBeginHandler = onBegin;
		return this;
	}

	public GoTweenConfig onIterationStart(global::System.Action<AbstractGoTween> onIterationStart)
	{
		onIterationStartHandler = onIterationStart;
		return this;
	}

	public GoTweenConfig onUpdate(global::System.Action<AbstractGoTween> onUpdate)
	{
		onUpdateHandler = onUpdate;
		return this;
	}

	public GoTweenConfig onIterationEnd(global::System.Action<AbstractGoTween> onIterationEnd)
	{
		onIterationEndHandler = onIterationEnd;
		return this;
	}

	public GoTweenConfig onComplete(global::System.Action<AbstractGoTween> onComplete)
	{
		onCompleteHandler = onComplete;
		return this;
	}

	public GoTweenConfig setId(int id)
	{
		this.id = id;
		return this;
	}

	public GoTweenConfig clone()
	{
		GoTweenConfig goTweenConfig = MemberwiseClone() as GoTweenConfig;
		goTweenConfig._tweenProperties = new global::System.Collections.Generic.List<AbstractTweenProperty>(2);
		for (int i = 0; i < _tweenProperties.Count; i++)
		{
			AbstractTweenProperty item = _tweenProperties[i];
			goTweenConfig._tweenProperties.Add(item);
		}
		return goTweenConfig;
	}
}
