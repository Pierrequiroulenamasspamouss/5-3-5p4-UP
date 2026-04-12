namespace Kampai.UI
{
	public class GhostComponentService : global::Kampai.UI.IGhostComponentService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("GhostComponentService") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.Dictionary<string, global::UnityEngine.RuntimeAnimatorController> animationControllers;

		private global::System.Collections.Generic.Dictionary<int, GhostComponentFadeHelperObject> displayedFadableObjects = new global::System.Collections.Generic.Dictionary<int, GhostComponentFadeHelperObject>();

		private global::Kampai.UI.GhostBuildingDisplayType displayType_ZoomedInToComponentInProgress = global::Kampai.UI.GhostBuildingDisplayType.Glowing;

		private global::Kampai.UI.GhostBuildingDisplayType displayType_ZoomedInToRegularComponent = global::Kampai.UI.GhostBuildingDisplayType.Ghosted;

		private global::Kampai.UI.GhostBuildingDisplayType displayType_DisplayingAllNonSelectedComponents = global::Kampai.UI.GhostBuildingDisplayType.Glowing;

		private global::Kampai.UI.GhostBuildingDisplayType displayType_DisplayBuildingWithAutoClose;

		private bool popOutOverwrittenGhostComponents;

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel lairModel { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayLocalAudioSignal audioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.StartLoopingAudioSignal startLoopingAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.StopLocalAudioSignal stopAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayMinionStateAudioSignal minionStateAudioSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllMessageDialogs closeAllMessageDialogsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playGlobalSoundFXSignal { get; set; }

		public global::Kampai.Game.View.BuildingObject DisplayGhostBuilding(int componentDefID, global::Kampai.UI.GhostBuildingDisplayType displayType)
		{
			if (lairModel.currentActiveLair == null)
			{
				logger.Error("lairModel.currentActiveLair is null, returning a null building");
				return null;
			}
			int buildingDefID = masterPlanService.CurrentMasterPlan.Definition.BuildingDefID;
			bool isRegularBuilding = componentDefID != buildingDefID;
			return GetGhostBuildingFromComponentDefID(componentDefID, displayType, isRegularBuilding);
		}

		public void DisplayComponentMarkedAsInProgress(global::Kampai.Game.MasterPlanComponent component)
		{
			if (component != null)
			{
				GetGhostBuildingFromComponentDefID(component.Definition.ID, displayType_ZoomedInToComponentInProgress);
			}
		}

		public void DisplayZoomedInComponent(int componentID, bool isRegularBuilding = true)
		{
			global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(componentID);
			if (firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.State < global::Kampai.Game.MasterPlanComponentState.Scaffolding)
			{
				global::Kampai.UI.GhostBuildingDisplayType displayType = displayType_ZoomedInToRegularComponent;
				if (firstInstanceByDefinitionId.State > global::Kampai.Game.MasterPlanComponentState.NotStarted)
				{
					displayType = displayType_ZoomedInToComponentInProgress;
				}
				GetGhostBuildingFromComponentDefID(componentID, displayType, isRegularBuilding);
			}
		}

		public void DisplayAllSelectablePlanComponents()
		{
			if (lairModel.currentActiveLair == null)
			{
				return;
			}
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			global::Kampai.Game.MasterPlanDefinition definition = currentMasterPlan.Definition;
			for (int i = 0; i < definition.ComponentDefinitionIDs.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(definition.ComponentDefinitionIDs[i]);
				if (firstInstanceByDefinitionId == null || firstInstanceByDefinitionId.State == global::Kampai.Game.MasterPlanComponentState.NotStarted)
				{
					global::Kampai.Game.MasterPlanComponentBuildingDefinition componentBuildingDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(definition.CompBuildingDefinitionIDs[i]);
					GetGhostBuildingFromComponentBldgDef(componentBuildingDefinition, displayType_DisplayingAllNonSelectedComponents);
				}
			}
		}

		private void ShowAllBuildingsForFTUE()
		{
			if (lairModel.currentActiveLair != null)
			{
				global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
				global::Kampai.Game.MasterPlanDefinition definition = currentMasterPlan.Definition;
				for (int i = 0; i < definition.ComponentDefinitionIDs.Count; i++)
				{
					global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(definition.CompBuildingDefinitionIDs[i]);
					global::Kampai.UI.GhostBuildingDisplayType displayType = global::Kampai.UI.GhostBuildingDisplayType.Normal;
					int iD = masterPlanComponentBuildingDefinition.ID;
					string prefab = masterPlanComponentBuildingDefinition.GetPrefab();
					global::UnityEngine.Vector3 componentBuildingPosition = masterPlanService.GetComponentBuildingPosition(iD);
					global::Kampai.Game.View.MasterPlanComponentBuildingObject masterPlanComponentBuildingObject = CreateBuilding(prefab, displayType, masterPlanComponentBuildingDefinition, componentBuildingPosition);
					GhostComponentFadeHelperObject ghostComponentFadeHelperObject = masterPlanComponentBuildingObject.gameObject.AddComponent<GhostComponentFadeHelperObject>();
					ghostComponentFadeHelperObject.SetupAndDisplay(this, masterPlanComponentBuildingObject, displayType, false);
					displayedFadableObjects[iD] = ghostComponentFadeHelperObject;
					ghostComponentFadeHelperObject.TriggerFTUEDropAnimation(masterPlanComponentBuildingDefinition.dropAnimationController, playGlobalSoundFXSignal);
				}
			}
		}

		public bool DisplayAutoCloseGhostComponent(int componentBuildingDefID, float fadeTime, float openDuration)
		{
			global::Kampai.Game.View.BuildingObject ghostBuildingFromComponentDefID = GetGhostBuildingFromComponentDefID(componentBuildingDefID, displayType_DisplayBuildingWithAutoClose, false, false);
			if (ghostBuildingFromComponentDefID == null)
			{
				return false;
			}
			GhostComponentFadeHelperObject ghostComponentFadeHelperObject = ghostBuildingFromComponentDefID.gameObject.AddComponent<GhostComponentFadeHelperObject>();
			ghostComponentFadeHelperObject.SetupAndAutoFadeWithMessage(fadeTime, openDuration, popupMessageSignal, closeAllMessageDialogsSignal, this, displayType_DisplayBuildingWithAutoClose, ghostBuildingFromComponentDefID);
			displayedFadableObjects[ghostBuildingFromComponentDefID.DefinitionID] = ghostComponentFadeHelperObject;
			return true;
		}

		public void RunBeginGhostComponentFunctionFromDefinition(global::Kampai.UI.GhostComponentFunctionType functionType, int defID = 0)
		{
			switch (functionType)
			{
			case global::Kampai.UI.GhostComponentFunctionType.ShowAllSelectableBuildings:
				DisplayAllSelectablePlanComponents();
				break;
			case global::Kampai.UI.GhostComponentFunctionType.ShowAllBuildingsForFTUE:
				ShowAllBuildingsForFTUE();
				break;
			case global::Kampai.UI.GhostComponentFunctionType.DisplayGhostBuilding:
				if (defID == 0)
				{
					logger.Error("Trying to zoom in on a building without the correct buildingDefinitionID");
				}
				DisplayGhostBuilding(defID, global::Kampai.UI.GhostBuildingDisplayType.Ghosted);
				break;
			case global::Kampai.UI.GhostComponentFunctionType.DisplayGlowBuilding:
				if (defID == 0)
				{
					logger.Error("Trying to zoom in on a building without the correct buildingDefinitionID");
				}
				DisplayGhostBuilding(defID, global::Kampai.UI.GhostBuildingDisplayType.Glowing);
				break;
			case global::Kampai.UI.GhostComponentFunctionType.DisplayNormalBuilding:
				if (defID == 0)
				{
					logger.Error("Trying to zoom in on a building without the correct buildingDefinitionID");
				}
				DisplayGhostBuilding(defID, global::Kampai.UI.GhostBuildingDisplayType.Normal);
				break;
			}
		}

		public void RunEndGhostComponentFunctionFromDefinition(global::Kampai.UI.GhostFunctionCloseType closeType)
		{
			if (closeType == global::Kampai.UI.GhostFunctionCloseType.ClearAllBuildings)
			{
				ClearGhostComponentBuildings();
			}
		}

		public global::Kampai.Game.View.MasterPlanComponentBuildingObject CreateBuilding(string prefabName, global::Kampai.UI.GhostBuildingDisplayType displayType, global::Kampai.Game.MasterPlanComponentBuildingDefinition buildingDefinition, global::UnityEngine.Vector3 position, bool isAudible = true)
		{
			if (animationControllers == null)
			{
				animationControllers = new global::System.Collections.Generic.Dictionary<string, global::UnityEngine.RuntimeAnimatorController>();
			}
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>(prefabName);
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			global::Kampai.Game.Building building = buildingDefinition.BuildBuilding();
			if (gameObject == null)
			{
				logger.Error("Could not create dummy building object from building definition id: {0}", buildingDefinition.ID);
				return null;
			}
			gameObject.name = string.Format("{0}_{1}", displayType, prefabName);
			global::UnityEngine.Transform transform = gameObject.transform;
			global::UnityEngine.Vector3 localEulerAngles = (transform.localPosition = global::UnityEngine.Vector3.zero);
			transform.localEulerAngles = localEulerAngles;
			localEulerAngles = (transform.eulerAngles = global::UnityEngine.Vector3.one);
			transform.localScale = localEulerAngles;
			transform.position = position;
			gameObject.SetLayerRecursively(9);
			global::Kampai.Game.View.MasterPlanComponentBuildingObject masterPlanComponentBuildingObject = building.AddBuildingObject(gameObject.gameObject) as global::Kampai.Game.View.MasterPlanComponentBuildingObject;
			LoadBuildingAnimationControllers(buildingDefinition);
			LoadBuildingAnimationEventHandler(gameObject.transform);
			if (!isAudible)
			{
				masterPlanComponentBuildingObject.ExecuteAction(new global::Kampai.Game.View.MuteAction(masterPlanComponentBuildingObject, true, logger));
			}
			masterPlanComponentBuildingObject.Init(building, logger, animationControllers, definitionService);
			return masterPlanComponentBuildingObject;
		}

		private void LoadBuildingAnimationControllers(global::Kampai.Game.BuildingDefinition buildingDefinition)
		{
			global::Kampai.Game.AnimatingBuildingDefinition animatingBuildingDefinition = buildingDefinition as global::Kampai.Game.AnimatingBuildingDefinition;
			if (animatingBuildingDefinition == null)
			{
				return;
			}
			foreach (string item in animatingBuildingDefinition.AnimationControllerKeys())
			{
				if (!animationControllers.ContainsKey(item))
				{
					global::UnityEngine.RuntimeAnimatorController value = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(item);
					animationControllers.Add(item, value);
				}
			}
		}

		private void LoadBuildingAnimationEventHandler(global::UnityEngine.Transform transform)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				global::UnityEngine.Transform child = transform.GetChild(i);
				global::UnityEngine.GameObject gameObject = child.gameObject;
				global::UnityEngine.Animator component = gameObject.GetComponent<global::UnityEngine.Animator>();
				global::Kampai.Game.View.AnimEventHandler component2 = gameObject.GetComponent<global::Kampai.Game.View.AnimEventHandler>();
				if (component != null && component2 == null)
				{
					global::Kampai.Game.View.AnimEventHandler animEventHandler = gameObject.AddComponent<global::Kampai.Game.View.AnimEventHandler>();
					animEventHandler.Init(gameObject, audioSignal, stopAudioSignal, minionStateAudioSignal, startLoopingAudioSignal);
				}
			}
		}

		private global::Kampai.Game.View.BuildingObject GetGhostBuildingFromComponentDefID(int componentDefID, global::Kampai.UI.GhostBuildingDisplayType displayType, bool isRegularBuilding = true, bool doNotAutoClose = true)
		{
			int id = componentDefID;
			if (isRegularBuilding)
			{
				global::Kampai.Game.MasterPlanDefinition definition = masterPlanService.CurrentMasterPlan.Definition;
				for (int i = 0; i < definition.ComponentDefinitionIDs.Count; i++)
				{
					if (definition.ComponentDefinitionIDs[i] == componentDefID)
					{
						id = definition.CompBuildingDefinitionIDs[i];
						break;
					}
				}
			}
			global::Kampai.Game.MasterPlanComponentBuildingDefinition componentBuildingDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(id);
			return GetGhostBuildingFromComponentBldgDef(componentBuildingDefinition, displayType, doNotAutoClose);
		}

		private global::Kampai.Game.View.BuildingObject GetGhostBuildingFromComponentBldgDef(global::Kampai.Game.MasterPlanComponentBuildingDefinition componentBuildingDefinition, global::Kampai.UI.GhostBuildingDisplayType displayType, bool doNotAutoClose = true)
		{
			global::Kampai.Game.MasterPlanComponentBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponentBuilding>(componentBuildingDefinition.ID);
			if (firstInstanceByDefinitionId != null)
			{
				return null;
			}
			int iD = componentBuildingDefinition.ID;
			if (displayedFadableObjects.ContainsKey(iD) && displayedFadableObjects[iD] != null)
			{
				if (displayedFadableObjects[iD].ghostDisplayType == displayType)
				{
					logger.Info("Did not create a new ghost component: one of the same type already exists.");
					if (doNotAutoClose)
					{
						return displayedFadableObjects[iD].buildingObject;
					}
					return null;
				}
				ReleaseSingleBuildingObject(displayedFadableObjects[iD], popOutOverwrittenGhostComponents);
			}
			string ghostBuildingPrefabByType = GetGhostBuildingPrefabByType(componentBuildingDefinition.GetPrefab(), displayType);
			global::UnityEngine.Vector3 componentBuildingPosition = masterPlanService.GetComponentBuildingPosition(iD);
			global::Kampai.Game.View.MasterPlanComponentBuildingObject masterPlanComponentBuildingObject = CreateBuilding(ghostBuildingPrefabByType, displayType, componentBuildingDefinition, componentBuildingPosition);
			if (doNotAutoClose)
			{
				GhostComponentFadeHelperObject ghostComponentFadeHelperObject = masterPlanComponentBuildingObject.gameObject.AddComponent<GhostComponentFadeHelperObject>();
				ghostComponentFadeHelperObject.SetupAndDisplay(this, masterPlanComponentBuildingObject, displayType);
				displayedFadableObjects[iD] = ghostComponentFadeHelperObject;
			}
			return masterPlanComponentBuildingObject;
		}

		private void ReleaseSingleBuildingObject(GhostComponentFadeHelperObject helper, bool immediate = false)
		{
			if (helper != null && helper.gameObject != null && displayedFadableObjects.ContainsKey(helper.buildingObject.DefinitionID))
			{
				displayedFadableObjects[helper.buildingObject.DefinitionID].StartFadeOut(immediate);
				displayedFadableObjects.Remove(helper.buildingObject.DefinitionID);
			}
		}

		public void GhostBuildingAutoRemoved(int id, GhostComponentFadeHelperObject helper)
		{
			if (displayedFadableObjects.ContainsKey(id) && displayedFadableObjects[id] == helper)
			{
				displayedFadableObjects.Remove(id);
			}
		}

		public void ClearGhostComponentBuildings(bool alsoClearComponentsInProgress = false, bool immediate = false)
		{
			if (alsoClearComponentsInProgress)
			{
				WipeEverything(immediate);
				return;
			}
			global::Kampai.Game.MasterPlanComponent componentCurrentlyInProgress = GetComponentCurrentlyInProgress();
			if (componentCurrentlyInProgress == null)
			{
				WipeEverything(immediate);
				return;
			}
			int buildingDefID = componentCurrentlyInProgress.buildingDefID;
			global::System.Collections.Generic.List<int> list = global::System.Linq.Enumerable.ToList(displayedFadableObjects.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				int num = list[i];
				if (buildingDefID != num)
				{
					ReleaseSingleBuildingObject(displayedFadableObjects[num], immediate);
				}
			}
		}

		public void ClearAllGhostBuildingsExceptCurrent(int excludedComponentDefID, bool keepSelectedComponents = false, bool immediate = false)
		{
			int num = 0;
			if (!keepSelectedComponents)
			{
				WipeEverything(immediate);
				return;
			}
			if (keepSelectedComponents)
			{
				global::Kampai.Game.MasterPlanComponent componentCurrentlyInProgress = GetComponentCurrentlyInProgress();
				if (componentCurrentlyInProgress != null)
				{
					num = componentCurrentlyInProgress.Definition.ID;
				}
				return;
			}
			global::System.Collections.Generic.List<int> list = global::System.Linq.Enumerable.ToList(displayedFadableObjects.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				int num2 = list[i];
				if (num2 != excludedComponentDefID && num2 != num)
				{
					ReleaseSingleBuildingObject(displayedFadableObjects[num2], immediate);
				}
			}
		}

		private void WipeEverything(bool immediate = false)
		{
			global::System.Collections.Generic.List<GhostComponentFadeHelperObject> list = global::System.Linq.Enumerable.ToList(displayedFadableObjects.Values);
			for (int i = 0; i < list.Count; i++)
			{
				ReleaseSingleBuildingObject(list[i], immediate);
			}
		}

		private string GetGhostBuildingPrefabByType(string prefab, global::Kampai.UI.GhostBuildingDisplayType displayType)
		{
			int num = prefab.IndexOf("_Prefab");
			string text = prefab;
			if (num == -1 || displayType == global::Kampai.UI.GhostBuildingDisplayType.Normal)
			{
				return text;
			}
			switch (displayType)
			{
			case global::Kampai.UI.GhostBuildingDisplayType.Ghosted:
				return text.Insert(num, "_Ghost");
			case global::Kampai.UI.GhostBuildingDisplayType.Glowing:
				return text.Insert(num, "_Glow");
			default:
				return prefab;
			}
		}

		private global::Kampai.Game.MasterPlanComponent GetComponentCurrentlyInProgress()
		{
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			if (currentMasterPlan == null)
			{
				return null;
			}
			global::Kampai.Game.MasterPlanComponent result = null;
			if (lairModel.currentActiveLair != null)
			{
				result = masterPlanService.GetActiveComponentFromPlanDefinition(currentMasterPlan.Definition.ID);
			}
			return result;
		}
	}
}
