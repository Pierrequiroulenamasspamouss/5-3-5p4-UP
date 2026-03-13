namespace Kampai.Game
{
	public class StopMignetteCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.MignetteGameModel mignetteGameModel { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.DestroyMignetteContextSignal destroyMignetteContextSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void Execute()
		{
			showHUDSignal.Dispatch(true);
			showStoreSignal.Dispatch(true);
			pickControllerModel.ForceDisabled = false;
			int buildingId = mignetteGameModel.BuildingId;
			global::Kampai.Game.MignetteBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(buildingId);
			global::Kampai.Game.MignetteBuildingDefinition mignetteBuildingDefinition = byInstanceId.MignetteBuildingDefinition;
			if (mignetteBuildingDefinition.ShowMignetteHUD)
			{
				guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "MignetteHUD");
			}
			destroyMignetteContextSignal.Dispatch();
			mignetteGameModel.IsMignetteActive = false;
			string localizedKey = mignetteBuildingDefinition.LocalizedKey;
			int currentGameScore = mignetteGameModel.CurrentGameScore;
			float elapsedTime = mignetteGameModel.ElapsedTime;
			telemetryService.Send_Telemetry_EVT_MINI_GAME_PLAYED(localizedKey, currentGameScore, elapsedTime, (int)mignetteBuildingDefinition.XPRewardFactor);
		}
	}
}
