namespace Kampai.Game.View
{
	public class ZoomView : global::Kampai.Util.KampaiView, global::Kampai.Game.IDefinitionsHotSwapHandler, global::Kampai.Game.View.CameraView
	{
		protected global::UnityEngine.Vector3 velocity;

		protected float decayAmount;

		protected global::UnityEngine.Ray mouseRay;

		protected global::UnityEngine.Plane groundPlane;

		protected float hitDistance;

		protected global::UnityEngine.Vector3 hitPosition;

		internal float fraction;

		protected float totalPixels;

		protected float zoomDistance;

		private global::UnityEngine.Camera mainCamera;

		private float totalInches;

		private float fractionMax = 1f;

		private float fractionMin;

		private global::System.Func<float, float, float, float, float> positionEaseFunction;

		private global::System.Func<float, float, float, float, float> rotationEaseFunction;

		private global::System.Func<float, float, float, float, float> fovEaseFunction;

		internal global::strange.extensions.signal.impl.Signal<float> zoomSignal = new global::strange.extensions.signal.impl.Signal<float>();

		private float initialFraction = 0.4f;

		private float inverseMinTilt;

		private global::Kampai.Game.CameraDefinition cameraDefinition;

		private float zoomOutOffset;

		private float zoomInOffset;

		private float multiplier;

		public global::UnityEngine.Vector3 Velocity
		{
			get
			{
				return velocity;
			}
			set
			{
				velocity = value;
			}
		}

		public float DecayAmount
		{
			get
			{
				return decayAmount;
			}
			set
			{
				decayAmount = value;
			}
		}

		public float InitialFraction
		{
			get
			{
				return initialFraction;
			}
			set
			{
				initialFraction = value;
			}
		}

		protected override void Start()
		{
			mainCamera = base.gameObject.GetComponent<global::UnityEngine.Camera>();
			positionEaseFunction = GoTweenUtils.easeFunctionForType(GoEaseType.Linear);
			rotationEaseFunction = GoTweenUtils.easeFunctionForType(GoEaseType.Linear);
			fovEaseFunction = GoTweenUtils.easeFunctionForType(GoEaseType.Linear);
			float arg = initialFraction * fractionMax;
			mainCamera.transform.eulerAngles = new global::UnityEngine.Vector3(rotationEaseFunction(arg, 55f, -30f, 1f), mainCamera.transform.eulerAngles.y, mainCamera.transform.eulerAngles.z);
			mainCamera.transform.position = new global::UnityEngine.Vector3(mainCamera.transform.position.x, positionEaseFunction(arg, 30f, -17f, 1f), mainCamera.transform.position.z);
			mainCamera.fieldOfView = fovEaseFunction(arg, 40f, -31f, 1f);
			fraction = arg;
			decayAmount = 0.75f;
			global::UnityEngine.Vector3 inNormal = new global::UnityEngine.Vector3(0f, 1f, 0f);
			global::UnityEngine.Vector3 inPoint = new global::UnityEngine.Vector3(0f, 0f, 0f);
			groundPlane = new global::UnityEngine.Plane(inNormal, inPoint);
			zoomDistance = 17f;
			if (global::UnityEngine.Screen.dpi == 0f)
			{
				totalPixels = 1000f;
			}
			else
			{
				if (global::Kampai.Util.DeviceCapabilities.IsTablet())
				{
					totalInches = 2.5f;
				}
				else
				{
					totalInches = 1.5f;
				}
				totalPixels = global::UnityEngine.Screen.dpi * (totalInches / fractionMax);
			}
			inverseMinTilt = 25f / fractionMax;
			base.Start();
		}

		public void OnDefinitionsHotSwap(global::Kampai.Game.IDefinitionService definitionService)
		{
			Init(definitionService);
		}

		public void Init(global::Kampai.Game.IDefinitionService definitionService)
		{
			cameraDefinition = definitionService.Get<global::Kampai.Game.CameraDefinition>(1000008101);
			zoomOutOffset = (fractionMin - cameraDefinition.MaxZoomOutLevel) * (1f / cameraDefinition.ZoomOutBounceSpeed);
			zoomInOffset = (cameraDefinition.MaxZoomInLevel - fractionMax) * (1f / cameraDefinition.ZoomInBounceSpeed);
		}

		public virtual void CalculateBehaviour(global::UnityEngine.Vector3 position)
		{
		}

		public virtual void PerformBehaviour(global::Kampai.Game.View.CameraUtils cameraUtils)
		{
			if (IsInputStationary())
			{
				return;
			}
			if (IsInputDone())
			{
				if (fraction > fractionMax)
				{
					fraction -= zoomInOffset * global::UnityEngine.Time.deltaTime;
					velocity = global::UnityEngine.Vector3.zero;
				}
				else if (fraction < fractionMin)
				{
					fraction += zoomOutOffset * global::UnityEngine.Time.deltaTime;
					velocity = global::UnityEngine.Vector3.zero;
				}
			}
			float num = velocity.y / totalPixels;
			fraction += num;
			fraction = global::UnityEngine.Mathf.Clamp(fraction, cameraDefinition.MaxZoomOutLevel, cameraDefinition.MaxZoomInLevel);
			base.transform.eulerAngles = new global::UnityEngine.Vector3(rotationEaseFunction(fraction, 55f, -30f, 1f), base.transform.eulerAngles.y, base.transform.eulerAngles.z);
			base.transform.position = new global::UnityEngine.Vector3(base.transform.position.x, positionEaseFunction(fraction, 30f, -17f, 1f), base.transform.position.z);
			mainCamera.fieldOfView = fovEaseFunction(fraction, 40f, -31f, 1f);
			zoomSignal.Dispatch(fraction);
		}

		protected virtual bool IsInputStationary()
		{
			return false;
		}

		protected virtual bool IsInputDone()
		{
			return false;
		}

		public virtual void ResetBehaviour()
		{
		}

		public virtual void Decay()
		{
			velocity *= decayAmount;
		}

		public virtual void SetupAutoZoom(float zoomTo)
		{
			multiplier = zoomTo - fraction;
		}

		public virtual void PerformAutoZoom(float delta)
		{
			float num = delta * multiplier;
			fraction += num;
			base.transform.eulerAngles = new global::UnityEngine.Vector3(rotationEaseFunction(fraction, 55f, -30f, 1f), base.transform.eulerAngles.y, base.transform.eulerAngles.z);
			base.transform.position = new global::UnityEngine.Vector3(base.transform.position.x, positionEaseFunction(fraction, 30f, -17f, 1f), base.transform.position.z);
			mainCamera.fieldOfView = fovEaseFunction(fraction, 40f, -31f, 1f);
			zoomSignal.Dispatch(fraction);
		}

		internal float GetCurrentPercentage()
		{
			return 1f - (base.transform.eulerAngles.x - inverseMinTilt) / (55f - inverseMinTilt);
		}

		internal void UpdateFraction()
		{
			fraction = GetCurrentPercentage();
		}
	}
}
