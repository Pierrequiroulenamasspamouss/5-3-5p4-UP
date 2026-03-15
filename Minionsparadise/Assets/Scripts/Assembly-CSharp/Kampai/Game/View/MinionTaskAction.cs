namespace Kampai.Game.View
{
	internal sealed class MinionTaskAction : global::Kampai.Game.View.KampaiAction
	{
		private global::Kampai.Game.View.MinionObject minion;

		private global::Kampai.Game.Building building;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.MinionObject, global::Kampai.Game.Building> signal;

		public MinionTaskAction(global::Kampai.Game.View.MinionObject minion, global::Kampai.Game.Building building, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.MinionObject, global::Kampai.Game.Building> signal, global::Kampai.Util.IKampaiLogger logger)
			: base(logger)
		{
			this.minion = minion;
			this.building = building;
			this.signal = signal;
		}

		public override void Abort()
		{
			base.Done = true;
		}

		public override void Execute()
		{
			signal.Dispatch(minion, building);
			base.Done = true;
		}
	}
}
