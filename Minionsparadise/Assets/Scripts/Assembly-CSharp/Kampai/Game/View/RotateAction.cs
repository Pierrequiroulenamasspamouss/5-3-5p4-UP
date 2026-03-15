namespace Kampai.Game.View
{
	public class RotateAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.ActionableObject obj;

		private float angle;

		private float speed;

		private GoTween tween;

		public RotateAction(global::Kampai.Game.View.ActionableObject obj, float angle, float speed, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.obj = obj;
			this.angle = angle;
			this.speed = speed;
		}

		public override void Abort()
		{
			if (tween != null)
			{
				tween.destroy();
			}
			base.Done = true;
		}

		public override void Execute()
		{
			float num = global::UnityEngine.Mathf.DeltaAngle(obj.transform.eulerAngles.y, angle);
			float duration = global::UnityEngine.Mathf.Abs(num / speed);
			if (num != 0f)
			{
				tween = Go.to(obj.transform, duration, new GoTweenConfig().setEaseType(GoEaseType.SineIn).rotation(new global::UnityEngine.Vector3(0f, angle, 0f)).onComplete(delegate(AbstractGoTween thisTween)
				{
					thisTween.destroy();
					base.Done = true;
				}));
			}
			else
			{
				base.Done = true;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} obj:{1} angle:{2} speed:{3}", GetType().ToString(), obj.ID, angle, speed);
		}
	}
}
