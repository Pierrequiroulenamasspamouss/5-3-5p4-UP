namespace Kampai.UI.View
{
	internal sealed class ShowMockStoreDialogCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.KampaiPendingTransaction product { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "PurchaseAuthorizationWarning");
			global::Kampai.Util.Tuple<string, string> value = new global::Kampai.Util.Tuple<string, string>(product.ExternalIdentifier, currencyService.GetPriceWithCurrencyAndFormat(product.ExternalIdentifier));
			iGUICommand.Args.Add(value);
			guiService.Execute(iGUICommand);
			playSFXSignal.Dispatch("Play_not_enough_items_01");
		}
	}
}
