namespace Kampai.Game
{
	public class CreateBuildingInInventoryCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int transactionId { get; set; }

		[Inject]
		public global::Kampai.Game.SendBuildingToInventorySignal sendBuildingToInventorySignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Location value = new global::Kampai.Game.Location();
			global::Kampai.Game.TransactionArg transactionArg = new global::Kampai.Game.TransactionArg();
			transactionArg.Add(value);
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> outputs = null;
			if (!playerService.FinishTransaction(transactionId, global::Kampai.Game.TransactionTarget.REWARD_BUILDING, out outputs, transactionArg))
			{
				return;
			}
			foreach (global::Kampai.Game.Instance item in outputs)
			{
				if (item is global::Kampai.Game.Building)
				{
					buildingChangeStateSignal.Dispatch(item.ID, global::Kampai.Game.BuildingState.Inventory);
					sendBuildingToInventorySignal.Dispatch(item.ID);
					break;
				}
			}
		}
	}
}
