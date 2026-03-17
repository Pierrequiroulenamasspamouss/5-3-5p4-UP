namespace Kampai.Game.View
{
	public class SkipToTimeAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.SkipToTime skipToTime;

		private global::Kampai.Game.View.ActionableObject minion;

		public SkipToTimeAction(global::Kampai.Game.View.ActionableObject minion, global::Kampai.Game.View.SkipToTime skipToTime, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.minion = minion;
			this.skipToTime = skipToTime;
		}

		public override void LateUpdate()
		{
			if (minion.IsInAnimatorState(skipToTime.StateHash, skipToTime.Layer))
			{
				float time = skipToTime.GetTime();
				if (time > 0f)
				{
					minion.PlayAnimation(skipToTime.StateHash, skipToTime.Layer, time);
					base.Done = true;
				}
			}
		}
	}
}
