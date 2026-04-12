namespace Kampai.Game.View
{
	public abstract class ZoomMediator : global::strange.extensions.mediation.impl.Mediator, global::Kampai.Game.View.CameraMediator
	{
		protected bool blocked;

		public float previousFraction;

		protected bool isAutoZooming;

		private GoTween lastTween;

		[Inject]
		public global::Kampai.Game.ICameraControlsService cameraControlsService { get; set; }

		[Inject]
		public global::Kampai.Game.DisableCameraBehaviourSignal disableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableCameraBehaviourSignal enableCameraSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoZoomSignal autoZoomSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickModel { get; set; }

		[Inject]
		public global::Kampai.Game.CameraCinematicZoomSignal cinematicZoomSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoZoomCompleteSignal cameraAutoZoomCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CancelAutozoomSignal cancelAutopanSignal { get; set; }

		public float Fraction { get; set; }

		public override void OnRegister()
		{
			cameraControlsService.RegisterListener(OnGameInput);
			disableCameraSignal.AddListener(OnDisableBehaviour);
			enableCameraSignal.AddListener(OnEnableBehaviour);
			autoZoomSignal.AddListener(OnAutoZoom);
			cinematicZoomSignal.AddListener(OnCinematicZoom);
			cancelAutopanSignal.AddListener(CancelAutozoom);
			Init();
		}

		private void Init()
		{
			GetView().Init(definitionService);
		}

		public override void OnRemove()
		{
			cameraControlsService.UnregisterListener(OnGameInput);
			disableCameraSignal.RemoveListener(OnDisableBehaviour);
			enableCameraSignal.RemoveListener(OnEnableBehaviour);
			autoZoomSignal.RemoveListener(OnAutoZoom);
			cinematicZoomSignal.RemoveListener(OnCinematicZoom);
			cancelAutopanSignal.RemoveListener(CancelAutozoom);
		}

		private void CancelAutozoom()
		{
			if (lastTween != null)
			{
				lastTween.destroy();
				isAutoZooming = false;
				Fraction = 0f;
			}
		}

		public virtual void OnGameInput(global::UnityEngine.Vector3 position, int input)
		{
		}

		public virtual void OnDisableBehaviour(int behaviour)
		{
		}

		public virtual void OnEnableBehaviour(int behaviour)
		{
		}

		public virtual void ReenablePickService()
		{
			pickModel.ZoomingCameraBlocked = false;
		}

		public virtual void OnAutoZoom(float zoomTo)
		{
			OnCinematicZoom(global::Kampai.Util.Tuple.Create(zoomTo, 0.8f));
		}

		protected void OnComplete()
		{
			cameraAutoZoomCompleteSignal.Dispatch();
		}

		public virtual void OnCinematicZoom(global::Kampai.Util.Tuple<float, float> zoomInfo)
		{
			if (isAutoZooming)
			{
				return;
			}
			pickModel.ZoomingCameraBlocked = true;
			float zoomTo = zoomInfo.Item1;
			float item = zoomInfo.Item2;
			float num = global::UnityEngine.Mathf.Abs(GetView().GetCurrentPercentage() - zoomTo);
			if (num <= 0.001f)
			{
				OnComplete();
				ReenablePickService();
				return;
			}
			lastTween = Go.to(this, item, new GoTweenConfig().floatProp("Fraction", 1f).setEaseType(GoEaseType.Linear).setUpdateType(GoUpdateType.LateUpdate)
				.onBegin(delegate
				{
					isAutoZooming = true;
					previousFraction = Fraction;
					SetupAutoZoom(zoomTo);
				})
				.onUpdate(delegate
				{
					float delta = Fraction - previousFraction;
					PerformAutoZoom(delta);
					previousFraction = Fraction;
				})
				.onComplete(delegate
				{
					isAutoZooming = false;
					ReenablePickService();
					Fraction = 0f;
					OnComplete();
				}));
		}

		public abstract global::Kampai.Game.View.ZoomView GetView();

		public virtual void SetupAutoZoom(float zoomTo)
		{
		}

		public virtual void PerformAutoZoom(float delta)
		{
		}
	}
}
