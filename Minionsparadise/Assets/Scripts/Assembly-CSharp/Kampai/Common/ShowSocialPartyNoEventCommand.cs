namespace Kampai.Common
{
	public class ShowSocialPartyNoEventCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFX { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
			if (firstInstanceByDefinitionId != null)
			{
				global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
				global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(firstInstanceByDefinitionId.ID);
				global::Kampai.Game.View.StageBuildingObject stageBuildingObject = buildingObject as global::Kampai.Game.View.StageBuildingObject;
				if (stageBuildingObject != null)
				{
					stageBuildingObject.UpdateStageState(global::Kampai.Game.BuildingState.Idle);
				}
				globalSFX.Dispatch("Play_menu_popUp_01");
				global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_SocialParty_NoEvent");
				iGUICommand.darkSkrim = false;
				iGUICommand.skrimScreen = "SocialSkrim";
				guiService.Execute(iGUICommand);
			}
		}
	}
}
