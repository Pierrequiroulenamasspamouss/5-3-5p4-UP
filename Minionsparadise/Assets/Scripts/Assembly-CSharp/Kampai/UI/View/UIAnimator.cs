namespace Kampai.UI.View
{
	public class UIAnimator
	{
		protected GoTween sharedTween;

		protected GoTweenConfig sharedTweenConfig = new GoTweenConfig();

		internal void Play(global::Kampai.UI.View.UIAnim anim, bool interruptible = false)
		{
			if (interruptible)
			{
				anim.PlayAnimation(ref sharedTween, sharedTweenConfig);
				return;
			}
			GoTween tween = null;
			GoTweenConfig config = new GoTweenConfig();
			anim.PlayAnimation(ref tween, config);
		}
	}
}
