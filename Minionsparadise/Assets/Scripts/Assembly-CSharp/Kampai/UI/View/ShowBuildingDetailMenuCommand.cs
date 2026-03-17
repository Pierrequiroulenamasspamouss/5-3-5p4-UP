namespace Kampai.UI.View
{
	public class ShowBuildingDetailMenuCommand : global::strange.extensions.command.impl.Command
	{
		private bool isMignetteCooldown;

		[Inject]
		public global::Kampai.Game.Building building { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartMignetteSignal startMignetteSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowBridgeUISignal bridgeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.OpenOrderBoardSignal openOrderBoardSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideCharactersSignal hideCharactersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.OpenVillainLairPortalBuildingSignal openVillainLairPortalSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(building.ID);
			global::Kampai.Game.VillainLairEntranceBuilding villainLairEntranceBuilding = building as global::Kampai.Game.VillainLairEntranceBuilding;
			if (villainLairEntranceBuilding != null)
			{
				openVillainLairPortalSignal.Dispatch(villainLairEntranceBuilding, buildingObject as global::Kampai.Game.View.VillainLairEntranceBuildingObject);
				return;
			}
			global::Kampai.UI.View.IGUICommand command = GetCommand();
			if (command != null)
			{
				global::UnityEngine.Vector2 uIAnchorRatioPosition = positionService.GetUIAnchorRatioPosition(buildingObject.ZoomCenter);
				global::Kampai.Game.ScreenPosition screenPosition = building.Definition.ScreenPosition;
				screenPosition = screenPosition ?? new global::Kampai.Game.ScreenPosition();
				global::UnityEngine.Vector2 endPosition = new global::UnityEngine.Vector2(screenPosition.x, screenPosition.z);
				if (isMignetteCooldown)
				{
					playSFXSignal.Dispatch("Play_menu_popUp_02");
				}
				else
				{
					playSFXSignal.Dispatch("Play_menu_popUp_01");
				}
				closeSignal.Dispatch(null);
				command.Args.Add(building);
				command.Args.Add(new global::Kampai.UI.BuildingPopupPositionData(uIAnchorRatioPosition, endPosition));
				guiService.Execute(command);
			}
		}

		private global::Kampai.UI.View.IGUICommand GetCommand()
		{
			string skrimName = null;
			bool darkSkrim = false;
			global::Kampai.UI.View.IGUICommand command = null;
			if (building is global::Kampai.Game.CraftingBuilding)
			{
				darkSkrim = false;
				skrimName = "CraftingSkrim";
				command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_CraftingMenu");
				command.Args.Add(building.ID);
			}
			else if (building is global::Kampai.Game.OrderBoard)
			{
				global::Kampai.Game.OrderBoard orderBoard = building as global::Kampai.Game.OrderBoard;
				if (orderBoard != null && orderBoard.menuEnabled)
				{
					openOrderBoardSignal.Dispatch(orderBoard);
					return null;
				}
			}
			else
			{
				if (building is global::Kampai.Game.BridgeBuilding)
				{
					bridgeSignal.Dispatch(building.ID);
					return null;
				}
				if (building is global::Kampai.Game.ResourceBuilding)
				{
					getResourceCommand(building as global::Kampai.Game.ResourceBuilding, out command, out darkSkrim, out skrimName);
					if (command.prefab.Equals("screen_BaseResource"))
					{
						hideCharactersSignal.Dispatch();
					}
				}
				else if (building is global::Kampai.Game.MignetteBuilding)
				{
					global::Kampai.Game.MignetteBuilding mignetteBuilding = (global::Kampai.Game.MignetteBuilding)building;
					if (MignetteIsRunning(mignetteBuilding))
					{
						return null;
					}
					darkSkrim = false;
					skrimName = "MignetteSkrim";
					bool flag = playerService.HasPurchasedMinigamePack();
					command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, mignetteBuilding.SelectMenuToLoad(flag));
					if (mignetteBuilding.State == global::Kampai.Game.BuildingState.Cooldown && !flag)
					{
						isMignetteCooldown = true;
					}
				}
				else if (building is global::Kampai.Game.DebrisBuilding)
				{
					if (building.State == global::Kampai.Game.BuildingState.Idle)
					{
						darkSkrim = false;
						skrimName = "DebrisSkrim";
						command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_ClearDebris");
						command.Args.Add(global::Kampai.UI.View.RushDialogView.RushDialogType.DEBRIS);
					}
				}
				else if (building.HasDetailMenuToShow())
				{
					darkSkrim = false;
					skrimName = "BuildingSkrim";
					command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, building.Definition.MenuPrefab);
				}
			}
			if (!string.IsNullOrEmpty(skrimName))
			{
				command.skrimScreen = skrimName;
				command.darkSkrim = darkSkrim;
			}
			return command;
		}

		private bool MignetteIsRunning(global::Kampai.Game.MignetteBuilding mignetteBuilding)
		{
			if (mignetteBuilding.AreAllMinionSlotsFilled() && !mignetteBuilding.Definition.ShowPlayConfirmMenu)
			{
				startMignetteSignal.Dispatch(mignetteBuilding.ID);
				return true;
			}
			return false;
		}

		private void getResourceCommand(global::Kampai.Game.ResourceBuilding resourceBuilding, out global::Kampai.UI.View.IGUICommand command, out bool darkSkrim, out string skrimName)
		{
			global::Kampai.Game.StorageBuilding firstInstanceByDefintion = playerService.GetFirstInstanceByDefintion<global::Kampai.Game.StorageBuilding, global::Kampai.Game.StorageBuildingDefinition>();
			if (playerService.isStorageFull() && resourceBuilding.AvailableHarvest > 0 && firstInstanceByDefintion != null)
			{
				command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_StorageBuilding");
				skrimName = "StorageSkrim";
				darkSkrim = true;
				command.Args.Add(firstInstanceByDefintion);
			}
			else
			{
				darkSkrim = false;
				skrimName = "BuildingSkrim";
				command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_BaseResource");
			}
		}
	}
}
