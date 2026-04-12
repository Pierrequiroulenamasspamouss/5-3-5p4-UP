namespace Kampai.UI
{
	public class ProgrammaticPopupController : global::Kampai.UI.IPopupController
	{
		private global::UnityEngine.Vector2 targetAnchorMin;

		private global::UnityEngine.Vector2 targetAnchorMax;

		private global::UnityEngine.Vector3 targetScale;

		private global::UnityEngine.Vector2 endAnchorPoint;

		private global::UnityEngine.RectTransform myRectTransform;

		private global::System.Action onFinishClosing;

		private GoTween tween;

		private float openSpeed;

		private bool opened;

		public bool isOpened
		{
			get
			{
				return opened;
			}
		}

		public ProgrammaticPopupController(global::UnityEngine.RectTransform rectTransform, global::UnityEngine.Vector2 startAnchorPoint, global::UnityEngine.Vector2 endAnchorPoint, float openSpeed, global::System.Action onFinishClosing)
		{
			myRectTransform = rectTransform;
			targetAnchorMin = myRectTransform.anchorMin;
			targetAnchorMax = myRectTransform.anchorMax;
			targetScale = myRectTransform.localScale;
			this.endAnchorPoint = endAnchorPoint;
			this.onFinishClosing = onFinishClosing;
			this.openSpeed = openSpeed;
			myRectTransform.anchorMin = startAnchorPoint;
			myRectTransform.anchorMax = startAnchorPoint;
			myRectTransform.localScale = global::UnityEngine.Vector3.zero;
			myRectTransform.offsetMin = global::UnityEngine.Vector2.zero;
			myRectTransform.offsetMax = global::UnityEngine.Vector2.zero;
		}

		public void Open()
		{
			if (!opened)
			{
				if (tween != null)
				{
					tween.destroy();
				}
				tween = new GoTween(myRectTransform, openSpeed, new GoTweenConfig().setEaseType(GoEaseType.Linear).scale(targetScale).vector2Prop("anchorMin", targetAnchorMin)
					.vector2Prop("anchorMax", targetAnchorMax));
				Go.addTween(tween);
				tween.play();
				opened = true;
			}
		}

		public void Close(bool instant)
		{
			if (opened)
			{
				opened = false;
				if (tween != null)
				{
					tween.destroy();
				}
				tween = null;
				if (instant)
				{
					DestroyMenu();
					return;
				}
				tween = new GoTween(myRectTransform, openSpeed, new GoTweenConfig().setEaseType(GoEaseType.Linear).scale(global::UnityEngine.Vector3.zero).vector2Prop("anchorMin", endAnchorPoint)
					.vector2Prop("anchorMax", endAnchorPoint)
					.onComplete(CloseComplete));
				Go.addTween(tween);
				tween.play();
			}
		}

		private void CloseComplete(AbstractGoTween tween)
		{
			if (tween != null)
			{
				tween.destroy();
			}
			tween = null;
			DestroyMenu();
		}

		private void DestroyMenu()
		{
			if (onFinishClosing != null)
			{
				onFinishClosing();
			}
		}
	}
}
