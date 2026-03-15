namespace Kampai.Game
{
	public class OrderBoardSetNewTicketCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int inverseTicketIndex { get; set; }

		[Inject]
		public bool makeItSelected { get; set; }

		[Inject]
		public global::Kampai.Game.IOrderBoardService orderBoardService { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardFillOrderCompleteSignal fillOrderCompleteSignal { get; set; }

		public override void Execute()
		{
			orderBoardService.GetNewTicket(-inverseTicketIndex);
			if (makeItSelected)
			{
				fillOrderCompleteSignal.Dispatch(-inverseTicketIndex);
			}
		}
	}
}
