namespace Kampai.Game.View
{
	public class PickNextStuartAnimationAction : global::Kampai.Game.View.KampaiAction
	{
		protected global::Kampai.Game.View.StuartView obj;

		protected global::Kampai.Game.StuartStageAnimationType targetState;

		public PickNextStuartAnimationAction(global::Kampai.Game.View.StuartView obj, global::Kampai.Game.StuartStageAnimationType targetState, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.obj = obj;
			this.targetState = targetState;
		}

		public override void Execute()
		{
			obj.StartingState(targetState);
			base.Done = true;
		}
	}
}
