namespace Kampai.Game
{
	public class StartMinionPartyUnlockSequenceCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.UpdatePartyPointButtonsSignal updatePartyPointButtonsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.MinionPartyUnlockedSignal unlockSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSoundFXSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.CheckMinionPartyLevelSignal checkMinionPartyLevelSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
			transactionDefinition.ID = int.MaxValue;
			transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			global::Kampai.Util.QuantityItem quantityItem = new global::Kampai.Util.QuantityItem();
			quantityItem.ID = 387;
			quantityItem.Quantity = 1u;
			transactionDefinition.Outputs.Add(quantityItem);
			transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			playerService.RunEntireTransaction(transactionDefinition, global::Kampai.Game.TransactionTarget.NO_VISUAL, null);
			checkMinionPartyLevelSignal.Dispatch(false);
			updatePartyPointButtonsSignal.Dispatch();
			playerService.UpdateMinionPartyPointValues();
			unlockSignal.Dispatch();
			DisplayOnBoardingSkrim();
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.StartLeisurePartyPointsFinishedSignal>().AddOnce(DisplayFunMeter);
		}

		private void DisplayFunMeter()
		{
			playSoundFXSignal.Dispatch("Play_partyMeter_barSpawn_01");
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.CreateFunMeterSignal>().Dispatch();
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.CreatePartyMeterSignal>().Dispatch();
		}

		private void DisplayOnBoardingSkrim()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "PartyOnboarding");
			iGUICommand.skrimScreen = "PartyOnboardSkrim";
			iGUICommand.darkSkrim = true;
			guiService.Execute(iGUICommand);
		}
	}
}
