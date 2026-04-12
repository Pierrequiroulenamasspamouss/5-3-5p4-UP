namespace Kampai.Game
{
	public class DisplayTreasureTeaserCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Trigger.TriggerInstance instance { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.CaptainWaveAndCallCallbackSignal waveAndCallbackSignal { get; set; }

		public override void Execute()
		{
			waveAndCallbackSignal.Dispatch(DisplayTreasureTeaseView, instance.Definition.TreasureIntro);
		}

		private void DisplayTreasureTeaseView()
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_MysteryMinionTeaserSelectionModal");
			iGUICommand.skrimScreen = "TSMTeaseSkrim";
			iGUICommand.darkSkrim = true;
			iGUICommand.disableSkrimButton = true;
			iGUICommand.Args.Add(typeof(global::Kampai.Game.Trigger.TriggerInstance), instance);
			guiService.Execute(iGUICommand);
		}
	}
}
