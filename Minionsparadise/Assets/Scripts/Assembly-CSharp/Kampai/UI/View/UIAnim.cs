namespace Kampai.UI.View
{
	public abstract class UIAnim
	{
		protected global::System.Action onAnimationComplete;

		protected float duration;

		protected global::UnityEngine.Transform transform;

		protected GoEaseType easeType;

		protected virtual void ClearLastTween(ref GoTween tween, GoTweenConfig config)
		{
			if (tween != null && tween.isValid())
			{
				tween.destroy();
			}
			config.clearProperties();
			config.clearEvents();
		}

		protected virtual void AddNewTween(ref GoTween tween, GoTweenConfig config)
		{
			config.onComplete(delegate(AbstractGoTween thisTween)
			{
				thisTween.destroy();
				if (onAnimationComplete != null)
				{
					onAnimationComplete();
				}
			});
			config.setIsTo();
			tween = new GoTween(transform, duration, config);
			Go.addTween(tween);
		}

		protected virtual void ConfigAnimation(ref GoTween tween, GoTweenConfig config)
		{
		}

		public virtual void PlayAnimation(ref GoTween tween, GoTweenConfig config)
		{
			ClearLastTween(ref tween, config);
			ConfigAnimation(ref tween, config);
			AddNewTween(ref tween, config);
		}
	}
}
