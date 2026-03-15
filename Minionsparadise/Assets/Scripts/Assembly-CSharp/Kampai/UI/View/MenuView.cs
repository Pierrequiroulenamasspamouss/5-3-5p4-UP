namespace Kampai.UI.View
{
	public class MenuView : global::Kampai.Util.KampaiView
	{
		public global::strange.extensions.signal.impl.Signal<bool> MenuMoved = new global::strange.extensions.signal.impl.Signal<bool>();

		public global::strange.extensions.signal.impl.Signal<bool> MenuMoveComplete = new global::strange.extensions.signal.impl.Signal<bool>();

		internal global::Kampai.UI.View.UIAnimator animator;

		internal bool isOpen;

		public virtual float SlideSpeed
		{
			get
			{
				return 0.5f;
			}
		}

		internal virtual void Init()
		{
			animator = new global::Kampai.UI.View.UIAnimator();
			isOpen = false;
		}

		internal virtual void MoveMenu(bool show)
		{
			MoveMenu(show, global::UnityEngine.Vector2.zero, global::UnityEngine.Vector2.zero);
		}

		internal virtual void MoveAnchoredPosition(bool show, global::UnityEngine.Vector2 offset, GoEaseType easeType = GoEaseType.ElasticOut)
		{
			if (show)
			{
				isOpen = true;
				MenuMoved.Dispatch(true);
				SetActive(true);
				animator.Play(new global::Kampai.UI.View.UIAnchorPositionSlideAnim(base.transform, SlideSpeed, offset, easeType, OnShowAnimationComplete), true);
			}
			else
			{
				isOpen = false;
				MenuMoved.Dispatch(false);
				animator.Play(new global::Kampai.UI.View.UIAnchorPositionSlideAnim(base.transform, SlideSpeed, offset, easeType, OnHideAnimationComplete), true);
			}
		}

		private void OnShowAnimationComplete()
		{
			MenuMoveComplete.Dispatch(true);
		}

		private void OnHideAnimationComplete()
		{
			MenuMoveComplete.Dispatch(false);
			SetActive(false);
		}

		protected void MoveMenu(bool show, global::UnityEngine.Vector2 offset, GoEaseType easeType = GoEaseType.ElasticOut)
		{
			MoveMenu(show, offset, offset, easeType);
		}

		protected void MoveMenu(bool show, global::UnityEngine.Vector2 offsetMin, global::UnityEngine.Vector2 offsetMax, GoEaseType easeType = GoEaseType.ElasticOut)
		{
			if (show)
			{
				isOpen = true;
				MenuMoved.Dispatch(true);
				SetActive(true);
				animator.Play(new global::Kampai.UI.View.UIOffsetPositionSlideAnim(base.transform, SlideSpeed, offsetMin, offsetMax, easeType, OnShowAnimationComplete), true);
			}
			else if (!show)
			{
				isOpen = false;
				MenuMoved.Dispatch(false);
				animator.Play(new global::Kampai.UI.View.UIOffsetPositionSlideAnim(base.transform, SlideSpeed, offsetMin, offsetMax, easeType, OnHideAnimationComplete), true);
			}
		}

		internal virtual void SetActive(bool active)
		{
			base.gameObject.SetActive(active);
		}
	}
}
