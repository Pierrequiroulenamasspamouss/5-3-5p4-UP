namespace Kampai.Util
{
	public static class TweenUtil
	{
		public static GoTween Throb(global::UnityEngine.Transform target, float scalar, float duration, out global::UnityEngine.Vector3 originalScale, int interations = -1)
		{
			global::UnityEngine.Vector3 endValue = new global::UnityEngine.Vector3(target.localScale.x, target.localScale.y, target.localScale.z);
			endValue *= scalar;
			originalScale = target.localScale;
			ScaleTweenProperty tweenProp = new ScaleTweenProperty(endValue);
			GoTweenConfig goTweenConfig = new GoTweenConfig();
			goTweenConfig.loopType = GoLoopType.PingPong;
			goTweenConfig.addTweenProperty(tweenProp);
			goTweenConfig.iterations = interations;
			goTweenConfig.easeType = GoEaseType.SineInOut;
			GoTween goTween = new GoTween(target, duration, goTweenConfig);
			Go.addTween(goTween);
			goTween.play();
			return goTween;
		}

		public static GoTween Bounce(global::UnityEngine.Transform target, global::UnityEngine.Vector3 localOffset, float duration)
		{
			PositionTweenProperty tweenProp = new PositionTweenProperty(localOffset, true, true);
			GoTweenConfig goTweenConfig = new GoTweenConfig();
			goTweenConfig.loopType = GoLoopType.PingPong;
			goTweenConfig.addTweenProperty(tweenProp);
			goTweenConfig.iterations = -1;
			GoTween goTween = new GoTween(target, duration, goTweenConfig);
			Go.addTween(goTween);
			goTween.play();
			return goTween;
		}

		public static void Cleanup(ref GoTween tween)
		{
			if (tween != null)
			{
				tween.destroy();
			}
			tween = null;
		}

		public static void Cleanup(ref GoTweenChain chain)
		{
			if (chain != null)
			{
				chain.destroy();
			}
			chain = null;
		}
	}
}
