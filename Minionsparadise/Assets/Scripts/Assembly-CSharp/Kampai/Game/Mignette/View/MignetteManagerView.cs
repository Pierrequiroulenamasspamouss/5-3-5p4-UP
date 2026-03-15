namespace Kampai.Game.Mignette.View
{
	public class MignetteManagerView : global::strange.extensions.mediation.impl.View
	{
		private sealed class CameraParams
		{
			public global::UnityEngine.Vector3 position;

			public global::UnityEngine.Quaternion rotation;

			public float fov;

			public float nearClip;
		}

		protected global::Kampai.UI.View.MignetteHUDView mignetteHUD;

		public global::Kampai.Game.View.MignetteBuildingObject MignetteBuildingObject;

		public global::Kampai.Game.Mignette.View.MignetteBuildingViewObject MignetteReferenceBuildingViewObject;

		private float CameraMoveTimer;

		private float CameraMoveTime;

		private bool DestroyMignetteAfterCameraMovement;

		private bool ShowScore;

		private bool cameraIsMoving;

		private global::Kampai.Game.Mignette.View.MignetteManagerView.CameraParams TargetParams = new global::Kampai.Game.Mignette.View.MignetteManagerView.CameraParams();

		private global::Kampai.Game.Mignette.View.MignetteManagerView.CameraParams ResetParams = new global::Kampai.Game.Mignette.View.MignetteManagerView.CameraParams();

		private global::Kampai.Game.Mignette.View.MignetteManagerView.CameraParams StartParams = new global::Kampai.Game.Mignette.View.MignetteManagerView.CameraParams();

		public float TimeElapsed;

		public float TotalEventTime;

		public float PercentCompleted;

		protected bool useCountDown = true;

		protected float preCountdownDelay = 3f;

		protected float countdownTimer = 3f;

		private bool countDownSignalDispatched;

		protected bool shutdownInProgress;

		private static bool IsPlaying;

		private global::UnityEngine.GameObject dooberInstance;

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal ShowAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllResourceIconsSignal ShowAllResourceIconsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllResourceIconsSignal HideAllResourceIconsSignal { get; set; }

		[Inject(global::Kampai.Main.MainElement.CAMERA)]
		public global::UnityEngine.Camera mignetteCamera { get; set; }

		[Inject]
		public global::Kampai.Game.MignetteEndedSignal mignetteEndedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.StartMignetteHUDCountdownSignal startMignetteHUDCountdownSignal { get; set; }

		[Inject(global::Kampai.Main.MainElement.UI_GLASSCANVAS)]
		public global::UnityEngine.GameObject glassCanvas { get; set; }

		public bool IsPaused { get; protected set; }

		protected override void Start()
		{
			SetIsPlaying(true);
			HideAllResourceIconsSignal.Dispatch();
			base.Start();
			ResetParams.position = mignetteCamera.transform.position;
			ResetParams.rotation = mignetteCamera.transform.rotation;
			ResetParams.fov = mignetteCamera.fieldOfView;
			ResetParams.nearClip = mignetteCamera.nearClipPlane;
			CameraMoveTimer = CameraMoveTime;
			MignetteReferenceBuildingViewObject = MignetteBuildingObject.GetComponent<global::Kampai.Game.Mignette.View.MignetteBuildingViewObject>();
			if (MignetteReferenceBuildingViewObject != null)
			{
				preCountdownDelay = MignetteReferenceBuildingViewObject.PreCountdownDelay;
				useCountDown = MignetteReferenceBuildingViewObject.UseCountDownTimer;
				countdownTimer = 3f;
				countDownSignalDispatched = false;
			}
			else
			{
				preCountdownDelay = 0f;
				countdownTimer = 0f;
				countDownSignalDispatched = true;
			}
			for (int i = 0; i < MignetteBuildingObject.GetMignetteMinionCount(); i++)
			{
				global::Kampai.Game.View.TaskingMinionObject childMinion = MignetteBuildingObject.GetChildMinion(i);
				childMinion.Minion.EnableRenderers(true);
			}
			mignetteHUD = glassCanvas.transform.GetComponentInChildren<global::Kampai.UI.View.MignetteHUDView>();
			dooberInstance = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("NumberedDoober"));
		}

		protected override void OnDestroy()
		{
			ShowAllWayFindersSignal.Dispatch();
			ShowAllResourceIconsSignal.Dispatch();
			global::UnityEngine.Object.Destroy(dooberInstance);
			base.OnDestroy();
			SetIsPlaying(false);
		}

		protected void SetCameraMoveTime(float t)
		{
			CameraMoveTime = t;
		}

		protected void SaveCurrentCameraPosition()
		{
			StartParams.position = mignetteCamera.transform.position;
			StartParams.rotation = mignetteCamera.transform.rotation;
			StartParams.fov = mignetteCamera.fieldOfView;
			StartParams.nearClip = mignetteCamera.nearClipPlane;
		}

		protected void RelocateCameraForMignette(global::UnityEngine.Transform newTransform, float fieldOfView, float nearClip, float duration)
		{
			SaveCurrentCameraPosition();
			if (newTransform != null)
			{
				TargetParams.position = newTransform.position;
				TargetParams.rotation = newTransform.rotation;
				TargetParams.fov = fieldOfView;
				TargetParams.nearClip = nearClip;
				CameraMoveTimer = 0f;
				CameraMoveTime = duration;
				cameraIsMoving = true;
			}
		}

		public virtual void Update()
		{
			if (IsPaused)
			{
				return;
			}
			if (preCountdownDelay > 0f)
			{
				preCountdownDelay -= global::UnityEngine.Time.deltaTime;
			}
			else if (useCountDown && countdownTimer > 0f)
			{
				if (!countDownSignalDispatched)
				{
					countDownSignalDispatched = true;
					startMignetteHUDCountdownSignal.Dispatch();
				}
				countdownTimer -= global::UnityEngine.Time.deltaTime;
			}
		}

		public virtual void LateUpdate()
		{
			if (!IsPaused && cameraIsMoving)
			{
				UpdateMignetteCamera();
			}
		}

		private void UpdateMignetteCamera()
		{
			if (!(CameraMoveTimer < CameraMoveTime))
			{
				return;
			}
			CameraMoveTimer += global::UnityEngine.Time.deltaTime;
			if (CameraMoveTimer >= CameraMoveTime)
			{
				CameraMoveTimer = CameraMoveTime;
				CameraTransitionComplete();
				if (DestroyMignetteAfterCameraMovement)
				{
					mignetteEndedSignal.Dispatch(ShowScore);
					shutdownInProgress = true;
				}
				cameraIsMoving = false;
			}
			float t = CameraMoveTimer / CameraMoveTime;
			mignetteCamera.nearClipPlane = global::UnityEngine.Mathf.Lerp(StartParams.nearClip, TargetParams.nearClip, t);
			mignetteCamera.transform.position = global::UnityEngine.Vector3.Lerp(StartParams.position, TargetParams.position, t);
			mignetteCamera.transform.rotation = global::UnityEngine.Quaternion.Slerp(StartParams.rotation, TargetParams.rotation, t);
			mignetteCamera.fieldOfView = global::UnityEngine.Mathf.Lerp(StartParams.fov, TargetParams.fov, t);
		}

		protected virtual void CameraTransitionComplete()
		{
		}

		public void ResetCameraAndStopMignette(bool showScore)
		{
			StartParams.position = mignetteCamera.transform.position;
			StartParams.rotation = mignetteCamera.transform.rotation;
			StartParams.fov = mignetteCamera.fieldOfView;
			StartParams.nearClip = mignetteCamera.nearClipPlane;
			ShowScore = showScore;
			DestroyMignetteAfterCameraMovement = true;
			TargetParams.position = ResetParams.position;
			TargetParams.rotation = ResetParams.rotation;
			TargetParams.fov = ResetParams.fov;
			TargetParams.nearClip = ResetParams.nearClip;
			CameraMoveTimer = 0f;
			cameraIsMoving = true;
		}

		public virtual void OnMignettePause(bool isPaused)
		{
			IsPaused = isPaused;
		}

		public static bool GetIsPlaying()
		{
			return IsPlaying;
		}

		private static void SetIsPlaying(bool isPlaying)
		{
			IsPlaying = isPlaying;
		}
	}
}
