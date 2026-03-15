namespace Kampai.Game.View
{
	public class LoadCurrencyWarningDialogCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.CurrencyWarningModel model { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowMTXStoreSignal showMTXStoreSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand;
			if (model.Type == global::Kampai.Game.StoreItemType.PremiumCurrency)
			{
				if (!model.GrindFromPremium)
				{
					showMTXStoreSignal.Dispatch(new global::Kampai.Util.Tuple<int, int>(800002, model.Cost));
					return;
				}
				iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "PremiumCurrencyWarning");
			}
			else
			{
				iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "GrindCurrencyWarning");
			}
			iGUICommand.skrimScreen = "CurrencySkrim";
			iGUICommand.darkSkrim = true;
			iGUICommand.singleSkrimClose = true;
			iGUICommand.Args.Add(model);
			guiService.Execute(iGUICommand);
		}
	}
}
