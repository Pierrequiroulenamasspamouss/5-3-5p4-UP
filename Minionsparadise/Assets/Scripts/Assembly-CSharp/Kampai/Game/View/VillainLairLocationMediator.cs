namespace Kampai.Game.View
{
	public class VillainLairLocationMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("VillainLairLocationMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.View.VillainLairLocationView view { get; set; }

		[Inject]
		public global::Kampai.Common.LairEnvironmentElementClickedSignal lairEnvironmentElementClickedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageWithComponentBuildingSignal popupMessageWithComponentBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableAllVillainLairCollidersSignal enableAllVillainLairCollidersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableOneVillainLairColliderSignal enableOneVillainLairColliderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateAllVillainLairCollidersSignal updateAllVillainLairCollidersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanCooldownAlertSignal displayAlertUISignal { get; set; }

		[Inject]
		public global::Kampai.Game.ClickedVillainLairGhostedComponentBuildingSignal clickedGhostComponentSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayMasterPlanSignal displayMasterPlanSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanIntroDialogSignal displayMasterPlanIntroDialogSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanSelectComponentSignal selectComponentSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel lairModel { get; set; }

		public override void OnRegister()
		{
			enableOneVillainLairColliderSignal.AddListener(PlatformHitboxAltered);
			enableAllVillainLairCollidersSignal.AddListener(view.EnableAllColliders);
			lairEnvironmentElementClickedSignal.AddListener(PlatformClicked);
			updateAllVillainLairCollidersSignal.AddListener(UpdateCollidersAndComponents);
			clickedGhostComponentSignal.AddListener(GhostComponentClicked);
		}

		public override void OnRemove()
		{
			enableOneVillainLairColliderSignal.RemoveListener(PlatformHitboxAltered);
			enableAllVillainLairCollidersSignal.RemoveListener(view.EnableAllColliders);
			lairEnvironmentElementClickedSignal.RemoveListener(PlatformClicked);
			updateAllVillainLairCollidersSignal.RemoveListener(UpdateCollidersAndComponents);
			clickedGhostComponentSignal.RemoveListener(GhostComponentClicked);
		}

		private void PlatformClicked(int instanceID)
		{
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			if (currentMasterPlan == null || masterPlanService.AllComponentsAreComplete(currentMasterPlan.Definition.ID) || lairModel.leavingLair || !lairModel.currentActiveLair.hasVisited)
			{
				return;
			}
			if (!currentMasterPlan.introHasBeenDisplayed)
			{
				displayMasterPlanIntroDialogSignal.Dispatch();
			}
			else if (currentMasterPlan.cooldownUTCStartTime > 0)
			{
				if (lairModel.seenCooldownAlert)
				{
					displayAlertUISignal.Dispatch(currentMasterPlan);
				}
			}
			else if (view != null && view.colliderInstanceKeysToComponentIDs.ContainsKey(instanceID))
			{
				if (instanceID == view.masterPlanPlatformCollider.GetInstanceID())
				{
					global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(currentMasterPlan.Definition.BuildingDefID);
					string type = localizationService.GetString(masterPlanComponentBuildingDefinition.LocalizedKey);
					popupMessageWithComponentBuildingSignal.Dispatch(type, false, masterPlanComponentBuildingDefinition.ID);
				}
				else
				{
					int type2 = view.colliderInstanceKeysToComponentIDs[instanceID];
					displayMasterPlanSignal.Dispatch(type2);
				}
			}
		}

		private void GhostComponentClicked(global::Kampai.Game.View.MasterPlanComponentBuildingObject obj)
		{
			if (!lairModel.currentActiveLair.hasVisited)
			{
				return;
			}
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			if (currentMasterPlan == null)
			{
				return;
			}
			global::System.Collections.Generic.List<int> componentDefinitionIDs = currentMasterPlan.Definition.ComponentDefinitionIDs;
			global::System.Collections.Generic.List<int> compBuildingDefinitionIDs = currentMasterPlan.Definition.CompBuildingDefinitionIDs;
			int definitionID = obj.DefinitionID;
			for (int i = 0; i < componentDefinitionIDs.Count; i++)
			{
				if (compBuildingDefinitionIDs[i] == definitionID)
				{
					global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(componentDefinitionIDs[i]);
					if (firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.State > global::Kampai.Game.MasterPlanComponentState.NotStarted && firstInstanceByDefinitionId.State < global::Kampai.Game.MasterPlanComponentState.Scaffolding)
					{
						selectComponentSignal.Dispatch(currentMasterPlan.Definition, i, false);
						break;
					}
					displayMasterPlanSignal.Dispatch(componentDefinitionIDs[i]);
				}
			}
		}

		private void PlatformHitboxAltered(bool enable, int componentBuildingDefID)
		{
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			if (currentMasterPlan == null)
			{
				return;
			}
			for (int i = 0; i < currentMasterPlan.Definition.CompBuildingDefinitionIDs.Count; i++)
			{
				if (currentMasterPlan.Definition.CompBuildingDefinitionIDs[i] == componentBuildingDefID)
				{
					view.EnableCollider(currentMasterPlan.Definition.ComponentDefinitionIDs[i], enable);
					break;
				}
			}
		}

		private void UpdateCollidersAndComponents()
		{
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			view.SetUpInstanceIDs(currentMasterPlan.Definition, playerService, logger);
		}
	}
}
