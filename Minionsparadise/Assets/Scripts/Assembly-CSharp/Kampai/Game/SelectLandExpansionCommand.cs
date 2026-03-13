namespace Kampai.Game
{
	public class SelectLandExpansionCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SelectLandExpansionCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.PurchasedLandExpansion purchasedLandExpansion;

		private global::Kampai.Game.LandExpansionDefinition definition;

		private global::Kampai.Game.BuildingDefinition buildingDef;

		[Inject]
		public int buildingID { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionService landExpansionService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveSignal AutoMoveSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.HighlightLandExpansionSignal highlightSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.PromptReceivedSignal promptReceivedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDialogSignal showDialog { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.BobCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.BobCharacter>(70002);
			int expansionByItemID = landExpansionService.GetExpansionByItemID(buildingID);
			purchasedLandExpansion = playerService.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
			definition = FindLandExpansion(expansionByItemID);
			if (ExpansionIsPurchaseable(expansionByItemID))
			{
				int minimumLevel = definition.MinimumLevel;
				if (minimumLevel > playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID))
				{
					Error(localService.GetString("LandLockedByLevel", minimumLevel));
					return;
				}
				global::UnityEngine.GameObject forSaleSign = landExpansionService.GetForSaleSign(expansionByItemID);
				global::UnityEngine.Vector3 position = forSaleSign.transform.position;
				highlightSignal.Dispatch(expansionByItemID, true);
				buildingDef = definitionService.Get(definition.BuildingDefinitionID) as global::Kampai.Game.BuildingDefinition;
				AutoMoveSignal.Dispatch(position, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(buildingDef.ScreenPosition), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.None, null, null), false);
				if (expansionByItemID == playerService.GetTargetExpansion() && firstInstanceByDefinitionId != null && !firstInstanceByDefinitionId.HasShownExpansionNarrative)
				{
					global::Kampai.Game.QuestDialogSetting type = new global::Kampai.Game.QuestDialogSetting();
					global::Kampai.Util.Tuple<int, int> type2 = new global::Kampai.Util.Tuple<int, int>(0, 0);
					promptReceivedSignal.AddOnce(DialogDismissed);
					showDialog.Dispatch("BobExpansionNarrative", type, type2);
					firstInstanceByDefinitionId.HasShownExpansionNarrative = true;
				}
				else
				{
					ShowMenu(expansionByItemID);
				}
			}
			else if (!HasPurchased(expansionByItemID))
			{
				Error(localService.GetString("LandLockedByOtherLand"));
			}
		}

		private void DialogDismissed(int param1, int param2)
		{
			if (param1 == 0 && param2 == 0)
			{
				int expansionByItemID = landExpansionService.GetExpansionByItemID(buildingID);
				ShowMenu(expansionByItemID);
			}
		}

		private void Error(string message)
		{
			sfxSignal.Dispatch("Play_action_locked_01");
			popupMessageSignal.Dispatch(message, global::Kampai.UI.View.PopupMessageType.NORMAL);
		}

		private bool HasPurchased(int expansionID)
		{
			return purchasedLandExpansion.HasPurchased(expansionID);
		}

		private bool ExpansionIsPurchaseable(int expansionID)
		{
			return purchasedLandExpansion.IsUnpurchasedAdjacentExpansion(expansionID);
		}

		private void ShowMenu(int expansionID)
		{
			playSFXSignal.Dispatch("Play_menu_popUp_01");
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.LandExpansionBuildingObject landExpansionBuildingObject = component.GetBuildingObject(buildingID) as global::Kampai.Game.View.LandExpansionBuildingObject;
			global::UnityEngine.Vector2 uIAnchorRatioPosition = positionService.GetUIAnchorRatioPosition(landExpansionBuildingObject.ZoomCenter);
			global::Kampai.Game.ScreenPosition screenPosition = ((buildingDef == null) ? new global::Kampai.Game.ScreenPosition() : buildingDef.ScreenPosition);
			screenPosition = screenPosition ?? new global::Kampai.Game.ScreenPosition();
			global::UnityEngine.Vector2 endPosition = new global::UnityEngine.Vector2(screenPosition.x, screenPosition.z);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "popup_Confirmation_Expansion");
			iGUICommand.skrimScreen = "LandExpansionSkrim";
			iGUICommand.darkSkrim = false;
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(expansionID);
			args.Add(global::Kampai.UI.View.RushDialogView.RushDialogType.LAND_EXPANSION);
			args.Add(new global::Kampai.UI.BuildingPopupPositionData(uIAnchorRatioPosition, endPosition));
			guiService.Execute(iGUICommand);
		}

		private global::Kampai.Game.LandExpansionDefinition FindLandExpansion(int expansionId)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.LandExpansionDefinition> all = definitionService.GetAll<global::Kampai.Game.LandExpansionDefinition>();
			for (int i = 0; i < all.Count; i++)
			{
				if (all[i].ExpansionID == expansionId)
				{
					return all[i];
				}
			}
			logger.Fatal(global::Kampai.Util.FatalCode.DS_NO_SUCH_LAND_EXPANSION, expansionId);
			return null;
		}
	}
}
