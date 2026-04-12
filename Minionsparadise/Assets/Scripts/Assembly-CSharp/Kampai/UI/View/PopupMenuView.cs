namespace Kampai.UI.View
{
	public class PopupMenuView : global::Kampai.Util.KampaiView
	{
		[global::UnityEngine.Header("PopupMenu Property")]
		public float OpenSpeed = 0.3f;

		public global::strange.extensions.signal.impl.Signal OnMenuClose = new global::strange.extensions.signal.impl.Signal();

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PopupMenuView") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.UI.IPopupController controller;

		protected global::UnityEngine.Animator animator
		{
			get
			{
				global::Kampai.UI.AnimatorPopupController animatorPopupController = controller as global::Kampai.UI.AnimatorPopupController;
				return (animatorPopupController == null) ? null : animatorPopupController.currentAnimator;
			}
		}

		protected bool isOpened
		{
			get
			{
				return controller.isOpened;
			}
		}

		protected override void Awake()
		{
			if (global::UnityEngine.Application.isPlaying)
			{
				base.Awake();
			}
		}

		public virtual void Init()
		{
			global::UnityEngine.Animator component = GetComponent<global::UnityEngine.Animator>();
			if (component == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "PopupMenuView has a NULL animator on Init!!!");
			}
			else
			{
				controller = new global::Kampai.UI.AnimatorPopupController(component, this, logger, FinishClosing);
			}
		}

		public void InitProgrammatic(global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData)
		{
			controller = new global::Kampai.UI.ProgrammaticPopupController(base.transform as global::UnityEngine.RectTransform, buildingPopupPositionData.StartPosition, buildingPopupPositionData.EndPosition, OpenSpeed, FinishClosing);
		}

		public void OpenInstantly(int defaultLayer, float lastFrame)
		{
			global::Kampai.UI.AnimatorPopupController animatorPopupController = controller as global::Kampai.UI.AnimatorPopupController;
			if (animatorPopupController != null)
			{
				animatorPopupController.OpenInstantly(defaultLayer, lastFrame);
			}
		}

		private void FinishClosing()
		{
			base.gameObject.SetActive(false);
			OnMenuClose.Dispatch();
			StopAllCoroutines();
		}

		public void DestroyMenu()
		{
			FinishClosing();
		}

		internal virtual void Open()
		{
			if (controller != null)
			{
				base.gameObject.SetActive(true);
				controller.Open();
			}
			else
			{
				logger.Error("Popup Controller is null, make sure you init it properly!");
			}
		}

		public virtual void Close(bool instant = false)
		{
			if (controller != null)
			{
				controller.Close(instant);
			}
			else
			{
				logger.Error("Popup Controller is null, make sure you init it properly!");
			}
		}

		public bool IsAnimationPlaying(string animationState)
		{
			global::UnityEngine.AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
			if (currentAnimatorStateInfo.IsName(animationState) && currentAnimatorStateInfo.normalizedTime < 1f)
			{
				return true;
			}
			if (currentAnimatorStateInfo.loop || currentAnimatorStateInfo.normalizedTime < 1f)
			{
				return true;
			}
			return false;
		}

		public virtual void FinishedOpening()
		{
		}
	}
}
