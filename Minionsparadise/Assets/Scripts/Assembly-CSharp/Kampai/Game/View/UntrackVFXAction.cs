namespace Kampai.Game.View
{
	public class UntrackVFXAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.MinionObject minion;

		public UntrackVFXAction(global::Kampai.Game.View.MinionObject minion, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.minion = minion;
		}

		public override void Execute()
		{
			minion.UntrackVFX();
			base.Done = true;
		}

		public override void Abort()
		{
			minion.UntrackVFX();
		}
	}
}
