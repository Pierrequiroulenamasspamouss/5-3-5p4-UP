namespace Kampai.UI.View
{
	public class UIAnchorPositionSlideAnim : global::Kampai.UI.View.UIAnim
	{
		private global::UnityEngine.Vector2 destination;

		public UIAnchorPositionSlideAnim(global::UnityEngine.Transform transform, float slideSpeed, global::UnityEngine.Vector2 destination, GoEaseType easeType, global::System.Action onAnimationComplete = null)
		{
			base.transform = transform;
			duration = slideSpeed;
			this.destination = destination;
			base.onAnimationComplete = onAnimationComplete;
			base.easeType = easeType;
		}

		protected override void ConfigAnimation(ref GoTween tween, GoTweenConfig tweenConfig)
		{
			tweenConfig.easeType = easeType;
			tweenConfig.vector2Prop("anchoredPosition", destination);
		}
	}
}
