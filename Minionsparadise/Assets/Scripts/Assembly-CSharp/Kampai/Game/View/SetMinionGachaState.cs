namespace Kampai.Game.View
{
	public class SetMinionGachaState : global::Kampai.Game.View.KampaiAction
	{
		private readonly global::Kampai.Game.View.MinionObject minionObject;

		private readonly global::Kampai.Game.View.MinionObject.MinionGachaState gachaState;

		public SetMinionGachaState(global::Kampai.Game.View.MinionObject minionObject, global::Kampai.Game.View.MinionObject.MinionGachaState gachaState, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.minionObject = minionObject;
			this.gachaState = gachaState;
		}

		public override void Execute()
		{
			minionObject.GachaState = gachaState;
			base.Done = true;
		}

		public override void Abort()
		{
			Execute();
		}
	}
}
