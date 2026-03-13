namespace Kampai.Game
{
	public class BRBCelebrationAnimationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int minionID { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.StartIncidentalAnimationSignal incidentalSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Util.QuantityItem quantityItem = playerService.GetWeightedInstance(4012).NextPick(randomService);
			incidentalSignal.Dispatch(minionID, quantityItem.ID);
		}
	}
}
