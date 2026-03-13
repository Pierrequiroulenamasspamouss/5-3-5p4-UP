namespace Kampai.Game
{
	public class DisplayInaccessibleMessageCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Building hitBuilding { get; set; }

		[Inject]
		public global::Kampai.Game.View.BuildingObject hitBuildingObject { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.OpenBuildingMenuSignal openBuildingMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenVillainLairResourcePlotBuildingSignal openResourcePlotBuildingSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.VillainLairEntranceBuilding villainLairEntranceBuilding = hitBuilding as global::Kampai.Game.VillainLairEntranceBuilding;
			if (villainLairEntranceBuilding != null)
			{
				if (playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) < villainLairEntranceBuilding.Definition.UnlockAtLevel)
				{
					DisplayMessage(localizationService.GetString(villainLairEntranceBuilding.Definition.AspirationalMessage_NeedLevel, villainLairEntranceBuilding.Definition.UnlockAtLevel));
				}
				else
				{
					openBuildingMenuSignal.Dispatch(hitBuildingObject, hitBuilding);
				}
			}
			global::Kampai.Game.MinionUpgradeBuilding minionUpgradeBuilding = hitBuilding as global::Kampai.Game.MinionUpgradeBuilding;
			if (minionUpgradeBuilding != null)
			{
				if (playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) < minionUpgradeBuilding.Definition.UnlockAtLevel)
				{
					DisplayMessage(localizationService.GetString(minionUpgradeBuilding.Definition.AspirationalMessage_NeedLevel, minionUpgradeBuilding.Definition.UnlockAtLevel));
				}
				else
				{
					DisplayMessage(localizationService.GetString(minionUpgradeBuilding.Definition.AspirationalMessage_NeedQuest));
				}
				return;
			}
			global::Kampai.Game.StageBuilding stageBuilding = hitBuilding as global::Kampai.Game.StageBuilding;
			if (stageBuilding != null)
			{
				DisplayMessage(localizationService.GetString(stageBuilding.Definition.AspirationalMessage));
				return;
			}
			global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = hitBuilding as global::Kampai.Game.VillainLairResourcePlot;
			if (villainLairResourcePlot != null)
			{
				openResourcePlotBuildingSignal.Dispatch(villainLairResourcePlot);
				return;
			}
			global::Kampai.Game.FountainBuilding fountainBuilding = hitBuilding as global::Kampai.Game.FountainBuilding;
			if (fountainBuilding != null)
			{
				DisplayMessage(localizationService.GetString(fountainBuilding.Definition.AspirationalMessage));
			}
		}

		private void DisplayMessage(string message)
		{
			globalSFXSignal.Dispatch("Play_action_locked_01");
			popupMessageSignal.Dispatch(message, global::Kampai.UI.View.PopupMessageType.NORMAL);
		}
	}
}
