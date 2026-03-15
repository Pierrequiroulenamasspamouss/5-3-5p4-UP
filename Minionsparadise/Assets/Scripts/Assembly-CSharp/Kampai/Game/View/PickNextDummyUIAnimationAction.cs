namespace Kampai.Game.View
{
	public class PickNextDummyUIAnimationAction : global::Kampai.Game.View.KampaiAction
	{
		protected global::Kampai.Game.View.DummyCharacterObject obj;

		protected global::Kampai.UI.DummyCharacterAnimationState targetState;

		public PickNextDummyUIAnimationAction(global::Kampai.Game.View.DummyCharacterObject obj, global::Kampai.UI.DummyCharacterAnimationState targetState, global::Kampai.Util.IKampaiLogger logger)
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
