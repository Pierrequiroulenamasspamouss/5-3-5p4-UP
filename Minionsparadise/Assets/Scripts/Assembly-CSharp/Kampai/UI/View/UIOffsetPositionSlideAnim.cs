namespace Kampai.UI.View
{
	public class UIOffsetPositionSlideAnim : global::Kampai.UI.View.UIAnim
	{
		private global::UnityEngine.Vector2 offsetMinDestination;

		private global::UnityEngine.Vector2 offsetMaxDestination;

		public UIOffsetPositionSlideAnim(global::UnityEngine.Transform transform, float slideSpeed, global::UnityEngine.Vector2 offsetMinDestination, global::UnityEngine.Vector2 offsetMaxDestination, GoEaseType easeType, global::System.Action onAnimationComplete = null)
		{
			base.transform = transform;
			duration = slideSpeed;
			this.offsetMinDestination = offsetMinDestination;
			this.offsetMaxDestination = offsetMaxDestination;
			base.onAnimationComplete = onAnimationComplete;
			base.easeType = easeType;
		}

		protected override void ConfigAnimation(ref GoTween tween, GoTweenConfig tweenConfig)
		{
			tweenConfig.easeType = easeType;
			tweenConfig.vector2Prop("offsetMin", offsetMinDestination);
			tweenConfig.vector2Prop("offsetMax", offsetMaxDestination);
		}
	}
}
