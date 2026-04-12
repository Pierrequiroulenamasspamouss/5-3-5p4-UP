namespace Kampai.UI.View
{
	public class BuffButtonMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::UnityEngine.Vector3 originalScale;

		[Inject]
		public global::Kampai.UI.View.BuffButtonView view { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowBuffInfoPopupSignal showBuffInfoPopupSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickModel { get; set; }

		public override void OnRegister()
		{
			view.pointerDownSignal.AddListener(ShowBuffPopup);
			originalScale = view.transform.localScale;
			if (view.pulse)
			{
				global::Kampai.Util.TweenUtil.Throb(view.transform, 0.85f, 0.5f, out originalScale);
			}
		}

		public override void OnRemove()
		{
			view.transform.localScale = originalScale;
			view.pointerDownSignal.RemoveListener(ShowBuffPopup);
			Go.killAllTweensWithTarget(view.transform);
		}

		private void ShowBuffPopup()
		{
			if (!pickModel.PanningCameraBlocked)
			{
				global::UnityEngine.Vector3[] array = new global::UnityEngine.Vector3[4];
				(view.gameObject.transform as global::UnityEngine.RectTransform).GetWorldCorners(array);
				global::UnityEngine.Vector3 position = default(global::UnityEngine.Vector3);
				global::UnityEngine.Vector3[] array2 = array;
				foreach (global::UnityEngine.Vector3 vector in array2)
				{
					position += vector;
				}
				position /= 4f;
				showBuffInfoPopupSignal.Dispatch(uiCamera.WorldToViewportPoint(position), view.popupOffset);
			}
		}
	}
}
