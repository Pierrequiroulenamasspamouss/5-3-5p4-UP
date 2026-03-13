namespace Kampai.Game
{
	public class MIBBuildingSelectedCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MIBBuildingSelectedCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Main.IHindsightService hindsightService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal PopupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService LocalizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject(global::Kampai.Main.MainElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable mainContext { get; set; }

		[Inject]
		public global::Kampai.Game.IMIBService mibService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playGlobalSoundFXSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Main.HindsightCampaign cachedContent = hindsightService.GetCachedContent(global::Kampai.Main.HindsightCampaign.Scope.message_in_a_bottle);
			if (cachedContent != null)
			{
				global::Kampai.Game.MIBBuilding firstInstanceByDefinitionId = PlayerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MIBBuilding>(3129);
				if (firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.MIBState == global::Kampai.Game.MIBBuildingState.READY && !mibService.IsUserReturning())
				{
					playGlobalSoundFXSignal.Dispatch("Play_menu_popUp_01");
					mainContext.injectionBinder.GetInstance<global::Kampai.Main.DisplayHindsightContentSignal>().Dispatch(global::Kampai.Main.HindsightCampaign.Scope.message_in_a_bottle);
				}
			}
			else
			{
				PopupMessageSignal.Dispatch(LocalizationService.GetString("NoUpsightContentAvailable"), global::Kampai.UI.View.PopupMessageType.NORMAL);
			}
		}
	}
}
