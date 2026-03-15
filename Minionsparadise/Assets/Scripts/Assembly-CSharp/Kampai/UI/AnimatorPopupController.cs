namespace Kampai.UI
{
	public class AnimatorPopupController : global::Kampai.UI.IPopupController
	{
		private global::Kampai.Util.IKampaiLogger logger;

		private global::UnityEngine.MonoBehaviour behavior;

		private global::UnityEngine.Animator animator;

		private bool opened;

		private global::System.Action onFinishClosing;

		private global::System.Collections.IEnumerator disableOnFinishEnumerator;

		public global::UnityEngine.Animator currentAnimator
		{
			get
			{
				return animator;
			}
		}

		public bool isOpened
		{
			get
			{
				return opened;
			}
		}

		public AnimatorPopupController(global::UnityEngine.Animator animator, global::UnityEngine.MonoBehaviour behavior, global::Kampai.Util.IKampaiLogger logger, global::System.Action onFinishClosing)
		{
			this.animator = animator;
			this.behavior = behavior;
			this.logger = logger;
			this.onFinishClosing = onFinishClosing;
		}

		public void Open()
		{
			if (!opened && animator != null)
			{
				if (disableOnFinishEnumerator != null)
				{
					behavior.StopCoroutine(disableOnFinishEnumerator);
					disableOnFinishEnumerator = null;
				}
				animator.Play("Open");
				opened = true;
			}
		}

		public void OpenInstantly(int defaultLayer, float lastFrame)
		{
			animator.Play("Open", defaultLayer, lastFrame);
			opened = true;
		}

		public void Close(bool instant)
		{
			if (opened)
			{
				opened = false;
				if (disableOnFinishEnumerator != null)
				{
					behavior.StopCoroutine(disableOnFinishEnumerator);
					disableOnFinishEnumerator = null;
				}
				if (instant)
				{
					DestroyMenu();
				}
				else if (animator != null)
				{
					animator.Play("Close");
					disableOnFinishEnumerator = DisableOnFinish();
					behavior.StartCoroutine(disableOnFinishEnumerator);
				}
				else
				{
					logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "PopupMenuView has a NULL animator on Close!!!");
					DestroyMenu();
				}
			}
		}

		private global::System.Collections.IEnumerator DisableOnFinish()
		{
			yield return null;
			float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
			yield return new global::UnityEngine.WaitForSeconds(animationLength);
			yield return null;
			disableOnFinishEnumerator = null;
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
