namespace Kampai.Game
{
	public class StartMinionPartyCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.PostMinionPartyStartSignal postMinionPartyStartSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalMusicSignal musicSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDiscoGlobeSignal displayDiscoGlobeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetupEndMinionPartyTimerSignal setupEndTimerSignal { get; set; }

		public override void Execute()
		{
			setupEndTimerSignal.Dispatch();
			displayDiscoGlobeSignal.Dispatch(true);
			postMinionPartyStartSignal.Dispatch();
			playerService.UpdateMinionPartyPointValues();
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.SetXPSignal>().Dispatch();
			global::System.Collections.Generic.Dictionary<string, float> dictionary = new global::System.Collections.Generic.Dictionary<string, float>();
			dictionary.Add("endParty", 0f);
			global::System.Collections.Generic.Dictionary<string, float> type = dictionary;
			musicSignal.Dispatch("Play_partyMeterMusic_01", type);
		}
	}
}
