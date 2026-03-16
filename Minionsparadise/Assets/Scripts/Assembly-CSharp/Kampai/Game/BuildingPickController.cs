namespace Kampai.Game
{
	public class BuildingPickController : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int pickEvent { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 inputPosition { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.Game.SelectBuildingSignal selectBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DeselectBuildingSignal deselectBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RevealBuildingSignal revealBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Common.DeselectTaskedMinionsSignal deselectTaskedMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Common.DragAndDropPickSignal dragAndDropSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartMinionTaskSignal startMinionTaskSignal { get; set; }

		[Inject]
		public global::Kampai.Common.TryHarvestBuildingSignal tryHarvestSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFXSignal { get; set; }

		[Inject]
		public global::Kampai.Common.DeselectAllMinionsSignal deselectMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Common.SelectMinionSignal selectMinionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowNeedXMinionsSignal showNeedXMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Common.RepairBuildingSignal repairBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowQuestPanelSignal showQuestPanel { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowQuestRewardSignal showQuestRewardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.OpenBuildingMenuSignal openBuildingMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayInaccessibleMessageSignal displayInaccessibleMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ClickedVillainLairGhostedComponentBuildingSignal clickedGhostComponentSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowActivitySpinnerSignal showActivitySpinnerSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			if (minionPartyInstance.IsPartyHappening || uiModel.LevelUpUIOpen || uiModel.WelcomeBuddyOpen)
			{
				return;
			}
			switch (pickEvent)
			{
			case 2:
			{
				if (model.CurrentMode != global::Kampai.Common.PickControllerModel.Mode.Building || !(model.StartHitObject != null))
				{
					break;
				}
				global::Kampai.Game.View.BuildingObject component = model.StartHitObject.GetComponent<global::Kampai.Game.View.BuildingObject>();
				if (component == null)
				{
					break;
				}
				global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(component.ID);
				if (byInstanceId == null)
				{
					break;
				}
				if (!model.DetectedMovement)
				{
					if (!model.activitySpinnerExists && byInstanceId.Definition.Movable)
					{
						model.activitySpinnerExists = true;
						showActivitySpinnerSignal.Dispatch(true, component.transform.position);
					}
				}
				else if (model.activitySpinnerExists)
				{
					model.activitySpinnerExists = false;
					showActivitySpinnerSignal.Dispatch(false, global::UnityEngine.Vector3.zero);
				}
				global::Kampai.Game.BuildingState state = byInstanceId.State;
				if (state != global::Kampai.Game.BuildingState.Construction && state != global::Kampai.Game.BuildingState.Inactive && state != global::Kampai.Game.BuildingState.Complete && !model.IsInstanceIgnored(byInstanceId.ID))
				{
					TrySelectBuilding(component, component.ID);
				}
				break;
			}
			case 3:
				PickEnd();
				break;
			}
		}

		private void PickEnd()
		{
			if (model.activitySpinnerExists)
			{
				showActivitySpinnerSignal.Dispatch(false, global::UnityEngine.Vector3.zero);
				model.activitySpinnerExists = false;
			}
			if (!(model.EndHitObject != null) || !(model.StartHitObject == model.EndHitObject) || model.DetectedMovement)
			{
				return;
			}
			globalSFXSignal.Dispatch("Play_button_click_01");
			global::Kampai.Game.View.BuildingObject component = model.EndHitObject.GetComponent<global::Kampai.Game.View.BuildingObject>();
			if (component != null)
			{
				global::Kampai.Game.View.IScaffoldingPart scaffoldingPart = component as global::Kampai.Game.View.IScaffoldingPart;
				if (scaffoldingPart != null)
				{
					global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(component.ID);
					revealBuildingSignal.Dispatch(byInstanceId);
				}
				else
				{
					PickEndBuilding(component);
				}
			}
		}

		private void PickEndBuilding(global::Kampai.Game.View.BuildingObject endHitObject)
		{
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(endHitObject.ID);
			if (byInstanceId != null)
			{
				if (model.IsInstanceIgnored(byInstanceId.ID))
				{
					return;
				}
				global::Kampai.Game.CabanaBuilding cabanaBuilding = byInstanceId as global::Kampai.Game.CabanaBuilding;
				if (cabanaBuilding != null && cabanaBuilding.Quest != null)
				{
					OpenCabanaQuest(cabanaBuilding.Quest);
				}
				if (byInstanceId.State == global::Kampai.Game.BuildingState.Broken)
				{
					repairBuildingSignal.Dispatch(byInstanceId);
					return;
				}
				if (byInstanceId.State == global::Kampai.Game.BuildingState.Inaccessible)
				{
					displayInaccessibleMessageSignal.Dispatch(endHitObject, byInstanceId);
					return;
				}
				if (global::Kampai.Game.InputUtils.touchCount < 2)
				{
					openBuildingMenuSignal.Dispatch(endHitObject, byInstanceId);
				}
				TrySendMinions(endHitObject, byInstanceId);
				global::Kampai.Game.VillainLairEntranceBuilding villainLairEntranceBuilding = byInstanceId as global::Kampai.Game.VillainLairEntranceBuilding;
				if (villainLairEntranceBuilding == null)
				{
					tryHarvestSignal.Dispatch(endHitObject, delegate
					{
					}, false);
				}
				return;
			}
			global::Kampai.Game.View.MasterPlanComponentBuildingObject masterPlanComponentBuildingObject = endHitObject as global::Kampai.Game.View.MasterPlanComponentBuildingObject;
			if (masterPlanComponentBuildingObject != null)
			{
				clickedGhostComponentSignal.Dispatch(masterPlanComponentBuildingObject);
				return;
			}
			global::Kampai.Game.View.MignetteBuildingObject mignetteBuildingObject = endHitObject as global::Kampai.Game.View.MignetteBuildingObject;
			if (mignetteBuildingObject != null)
			{
				int id = mignetteBuildingObject.ID * -1;
				global::Kampai.Game.AspirationalBuildingDefinition aspirationalBuildingDefinition = definitionService.Get<global::Kampai.Game.AspirationalBuildingDefinition>(id);
				int buildingDefinitionID = aspirationalBuildingDefinition.BuildingDefinitionID;
				global::Kampai.Game.MignetteBuildingDefinition mignetteBuildingDefinition = definitionService.Get<global::Kampai.Game.MignetteBuildingDefinition>(buildingDefinitionID);
				int levelUnlocked = mignetteBuildingDefinition.LevelUnlocked;
				string aspirationalMessage = mignetteBuildingDefinition.AspirationalMessage;
				globalSFXSignal.Dispatch("Play_action_locked_01");
				popupMessageSignal.Dispatch(localeService.GetString(aspirationalMessage, levelUnlocked), global::Kampai.UI.View.PopupMessageType.NORMAL);
			}
		}

		private void OpenCabanaQuest(global::Kampai.Game.Quest q)
		{
			switch (q.state)
			{
			case global::Kampai.Game.QuestState.Notstarted:
			case global::Kampai.Game.QuestState.RunningStartScript:
			case global::Kampai.Game.QuestState.RunningTasks:
			case global::Kampai.Game.QuestState.RunningCompleteScript:
				showQuestPanel.Dispatch(q.ID);
				break;
			case global::Kampai.Game.QuestState.Harvestable:
				showQuestRewardSignal.Dispatch(q.ID);
				break;
			}
		}

		private void TrySelectBuilding(global::Kampai.Game.View.BuildingObject bo, int id)
		{
			if (model.SelectedBuilding.HasValue || model.DetectedMovement || !(model.HeldTimer >= 0.75f) || !(bo != null))
			{
				return;
			}
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(bo.ID);
			if (byInstanceId == null)
			{
				return;
			}
			global::Kampai.Game.BuildingDefinition definition = byInstanceId.Definition;
			if (!definition.Movable)
			{
				return;
			}
			global::Kampai.Game.StorageBuilding storageBuilding = byInstanceId as global::Kampai.Game.StorageBuilding;
			if (storageBuilding != null && !storageBuilding.IsBuildingRepaired())
			{
				return;
			}
#if UNITY_IOS || UNITY_ANDROID
			global::UnityEngine.Handheld.Vibrate();
#endif
			model.SelectedBuilding = id;
			model.CurrentMode = global::Kampai.Common.PickControllerModel.Mode.DragAndDrop;
			global::Kampai.Game.DragOffsetType type = global::Kampai.Game.DragOffsetType.NONE;
			if (definition.FootprintID == 300000)
			{
				type = global::Kampai.Game.DragOffsetType.ONE_X_ONE;
			}
			deselectMinionsSignal.Dispatch();
			dragAndDropSignal.Dispatch(1, inputPosition, type);
			global::Kampai.Game.BuildingDefinition definition2 = byInstanceId.Definition;
			if (byInstanceId != null && definition2.Movable)
			{
				selectBuildingSignal.Dispatch(id, definitionService.GetBuildingFootprint(definition2.FootprintID));
				deselectBuildingSignal.AddListener(DeselectBuilding);
				global::Kampai.Game.BuildingState state = byInstanceId.State;
				if (state == global::Kampai.Game.BuildingState.Working || state == global::Kampai.Game.BuildingState.Harvestable || state == global::Kampai.Game.BuildingState.HarvestableAndWorking || playerService.GetHighestFtueCompleted() < 6)
				{
					uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.DisableMoveToInventorySignal>().Dispatch();
				}
			}
		}

		private void DeselectBuilding(int id)
		{
			if (model.SelectedBuilding == id)
			{
				model.SelectedBuilding = null;
				deselectBuildingSignal.RemoveListener(DeselectBuilding);
			}
		}

		private void TrySendMinions(global::Kampai.Game.View.BuildingObject buildingObj, global::Kampai.Game.Building building)
		{
			global::Kampai.Game.TaskableBuilding taskableBuilding = building as global::Kampai.Game.TaskableBuilding;
			if (model.SelectedMinions.Count == 0 || model.SelectedBuilding.HasValue || model.HeldTimer >= 0.75f || taskableBuilding == null || taskableBuilding.State == global::Kampai.Game.BuildingState.Harvestable || taskableBuilding.State == global::Kampai.Game.BuildingState.Cooldown || taskableBuilding is global::Kampai.Game.TikiBarBuilding)
			{
				return;
			}
			global::Kampai.Game.DebrisBuilding debrisBuilding = taskableBuilding as global::Kampai.Game.DebrisBuilding;
			if (debrisBuilding != null && !debrisBuilding.PaidInputCostToClear)
			{
				return;
			}
			global::Kampai.Game.MignetteBuilding mignetteBuilding = taskableBuilding as global::Kampai.Game.MignetteBuilding;
			if (mignetteBuilding != null && !TrySelectToFillTaskableBuilding(buildingObj, taskableBuilding))
			{
				showNeedXMinionsSignal.Dispatch(taskableBuilding.GetMinionSlotsOwned());
				return;
			}
			if (taskableBuilding.Definition.WorkStations > taskableBuilding.GetMinionsInBuilding())
			{
				globalSFXSignal.Dispatch("Play_minion_counter_down_01");
			}
			bool flag = true;
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			foreach (int key in model.SelectedMinions.Keys)
			{
				if (flag)
				{
					globalSFXSignal.Dispatch("Play_minion_confirm_pathToBldg_01");
					flag = false;
				}
				global::Kampai.Game.View.MinionObject second = component.Get(key);
				startMinionTaskSignal.Dispatch(new global::Kampai.Util.Tuple<int, global::Kampai.Game.View.MinionObject, int>(taskableBuilding.ID, second, timeService.CurrentTime()));
			}
			deselectTaskedMinionsSignal.Dispatch();
		}

		private bool TrySelectToFillTaskableBuilding(global::Kampai.Game.View.BuildingObject buildingObj, global::Kampai.Game.TaskableBuilding building)
		{
			int num = building.GetMinionSlotsOwned() - building.GetMinionsInBuilding();
			int count = model.SelectedMinions.Count;
			int num2 = num - count;
			if (num2 <= 0)
			{
				return true;
			}
			int num3 = playerService.GetMinionCount() - count;
			if (num2 > num3)
			{
				return false;
			}
			global::UnityEngine.Vector3 center = buildingObj.Center;
			global::System.Collections.Generic.Queue<int> minionListSortedByDistanceAndState = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>().GetMinionListSortedByDistanceAndState(inputPosition);
			global::System.Collections.Generic.Queue<int> queue = new global::System.Collections.Generic.Queue<int>();
			while (minionListSortedByDistanceAndState.Count > 0 && num2 > 0)
			{
				int num4 = minionListSortedByDistanceAndState.Dequeue();
				global::Kampai.Game.Minion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Minion>(num4);
				if (!model.SelectedMinions.ContainsKey(num4) && (byInstanceId.State == global::Kampai.Game.MinionState.Idle || byInstanceId.State == global::Kampai.Game.MinionState.Selectable || byInstanceId.State == global::Kampai.Game.MinionState.Leisure || byInstanceId.State == global::Kampai.Game.MinionState.Uninitialized))
				{
					queue.Enqueue(num4);
					num2--;
				}
			}
			global::Kampai.Util.Boxed<global::UnityEngine.Vector3> param = new global::Kampai.Util.Boxed<global::UnityEngine.Vector3>(center);
			bool flag = num2 == 0;
			if (flag)
			{
				while (queue.Count > 0)
				{
					selectMinionSignal.Dispatch(queue.Dequeue(), param, true);
				}
			}
			return flag;
		}
	}
}
