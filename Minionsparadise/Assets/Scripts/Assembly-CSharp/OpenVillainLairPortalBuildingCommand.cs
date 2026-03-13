public class OpenVillainLairPortalBuildingCommand : global::strange.extensions.command.impl.Command
{
	[Inject]
	public global::Kampai.Game.VillainLairEntranceBuilding lairPortal { get; set; }

	[Inject]
	public global::Kampai.Game.View.VillainLairEntranceBuildingObject lairPortalObject { get; set; }

	[Inject]
	public global::Kampai.Game.IPlayerService playerService { get; set; }

	[Inject]
	public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

	[Inject]
	public global::Kampai.Game.CameraAutoMoveSignal AutoMoveSignal { get; set; }

	[Inject]
	public global::Kampai.Main.ILocalizationService localService { get; set; }

	[Inject]
	public global::Kampai.UI.View.IGUIService guiService { get; set; }

	[Inject]
	public global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal { get; set; }

	[Inject]
	public global::Kampai.Game.IQuestService questService { get; set; }

	[Inject]
	public global::Kampai.Game.UnlockVillainLairSignal unlockVillainLairSignal { get; set; }

	[Inject]
	public global::Kampai.UI.IPositionService positionService { get; set; }

	public override void Execute()
	{
		if (lairPortal.State == global::Kampai.Game.BuildingState.Inaccessible)
		{
			global::Kampai.Game.IQuestController questControllerByDefinitionID = questService.GetQuestControllerByDefinitionID(3849292);
			if (questControllerByDefinitionID == null || questControllerByDefinitionID.State == global::Kampai.Game.QuestState.Notstarted)
			{
				popupMessageSignal.Dispatch(localService.GetString(lairPortal.Definition.AspirationalMessage_NeedKevinsQuest), global::Kampai.UI.View.PopupMessageType.NORMAL);
			}
			else
			{
				OpenModal("screen_UnlockLair", true);
			}
			return;
		}
		if (!lairPortal.IsUnlocked)
		{
			unlockVillainLairSignal.Dispatch(lairPortal, 3137);
		}
		global::Kampai.Game.VillainLair byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLair>(lairPortal.VillainLairInstanceID);
		string prefabName = ((!byInstanceId.hasVisited) ? "screen_EnterLair" : "screen_Resource_LairPortal");
		OpenModal(prefabName, false);
	}

	private void OpenModal(string prefabName, bool isBuildModal)
	{
		Pan();
		sfxSignal.Dispatch("Play_menu_popUp_01");
		global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, prefabName);
		global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
		iGUICommand.skrimScreen = "VillainLairPortalSkrim";
		args.Add(lairPortal);
		if (isBuildModal)
		{
			iGUICommand.darkSkrim = true;
			args.Add(lairPortal.ID);
			args.Add(global::Kampai.UI.View.RushDialogView.RushDialogType.VILLAIN_LAIR_PORTAL_REPAIR);
		}
		else
		{
			args.Add(3137);
		}
		global::UnityEngine.Vector2 uIAnchorRatioPosition = positionService.GetUIAnchorRatioPosition(lairPortalObject.transform.position);
		global::Kampai.Game.ScreenPosition screenPosition = lairPortal.Definition.ScreenPosition;
		screenPosition = screenPosition ?? new global::Kampai.Game.ScreenPosition();
		global::UnityEngine.Vector2 endPosition = new global::UnityEngine.Vector2(screenPosition.x, screenPosition.z);
		args.Add(new global::Kampai.UI.BuildingPopupPositionData(uIAnchorRatioPosition, endPosition));
		guiService.Execute(iGUICommand);
	}

	private void Pan()
	{
		global::UnityEngine.Vector3 position = lairPortalObject.transform.position;
		global::Kampai.Game.ScreenPosition screenPosition = lairPortal.Definition.ScreenPosition;
		global::Kampai.Game.CameraMovementSettings cameraMovementSettings = new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.KeepUIOpen, lairPortal, null);
		cameraMovementSettings.cameraSpeed = 0.4f;
		AutoMoveSignal.Dispatch(position, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), cameraMovementSettings, false);
	}
}
