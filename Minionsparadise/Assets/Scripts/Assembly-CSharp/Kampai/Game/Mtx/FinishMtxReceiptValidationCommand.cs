namespace Kampai.Game.Mtx
{
	public class FinishMtxReceiptValidationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Mtx.ReceiptValidationResult validationResult { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		public override void Execute()
		{
			currencyService.ReceiptValidationCallback(validationResult);
		}
	}
}
