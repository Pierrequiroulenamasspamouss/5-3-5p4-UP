namespace Kampai.Game.View
{
	public class ResetRootPositionAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.MinionObject obj;

		public ResetRootPositionAction(global::Kampai.Game.View.MinionObject obj, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.obj = obj;
		}

		public override void Execute()
		{
			if (!base.Done)
			{
				obj.ResetRootPosition();
				base.Done = true;
			}
		}
	}
}
