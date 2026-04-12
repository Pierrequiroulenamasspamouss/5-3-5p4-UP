namespace Kampai.Game.View
{
	public class EnterVillainLairCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("EnterVillainLairCommand") as global::Kampai.Util.IKampaiLogger;

		private bool isFaded;

		private global::Kampai.Game.VillainLair currentLair;

		private global::Kampai.Game.MasterPlan masterPlan;

		[Inject]
		public int villainLairInstanceID { get; set; }

		[Inject]
		public bool shouldDisplayMasterPlanUI { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		[Inject]
		public global::Kampai.Game.CameraMoveToCustomPositionSignal customCameraPositionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.EnableVillainLairHudSignal enableVillainHudSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.FadeBlackSignal fadeBlackSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayVolcanoLairVillainWayfinderSignal displayVolcanoWayfinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayMasterPlanSignal displayMasterPlanSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideFluxWayfinder hideFluxWayfinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CreateNamedCharacterViewSignal createNamedCharacterViewSignal { get; set; }

		[Inject]
		public global::Kampai.Game.InitializeVillainSignal initVillainSignal { get; set; }

		[Inject]
		public global::Kampai.Game.LoadVillainLairInstancesSignal loadVillainLairInstancesSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayVillainSignal displayVillainSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanCooldownRewardDialogSignal displayCooldownRewardSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllMessageDialogs closeAllDialogsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.GetWayFinderSignal getWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.StopEntranceWayfinderPulseSignal stopPulseSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanCooldownAlertSignal displayAlertUISignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetBuildMenuEnabledSignal setBuildMenuEnabledSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayMasterPlanOnboardingSignal displayOnboardingSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IGhostComponentService ghostComponentService { get; set; }

		[Inject]
		public global::Kampai.Game.EnableOneVillainLairColliderSignal enableOneVillainLairColliderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.GenerateNewMasterPlanSignal generateNewMasterPlanSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanIntroDialogSignal displayMasterPlanIntroDialogSignal { get; set; }

		public override void Execute()
		{
			closeAllDialogsSignal.Dispatch();
			currentLair = playerService.GetByInstanceId<global::Kampai.Game.VillainLair>(villainLairInstanceID);
			if (currentLair == null)
			{
				logger.Error("Trying to enter a  villain lair that doesn't exist");
				return;
			}
			if (!villainLairModel.areLairAssetsLoaded)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_INCOMPLETE_VILLAIN_LAIR_ASSETS_SIMPLE_UI, "Assets for Villain Lair {0} are not loaded", currentLair.ID);
			}
			villainLairModel.goingToLair = true;
			showHUDSignal.Dispatch(false);
			masterPlan = masterPlanService.CurrentMasterPlan;
			if (masterPlan == null || masterPlan.completionCount > 0)
			{
				generateNewMasterPlanSignal.Dispatch(new global::Kampai.Util.Boxed<global::System.Action>(CheckInstancesAndFadeOut));
			}
			else
			{
				CheckInstancesAndFadeOut();
			}
		}

		private void CheckInstancesAndFadeOut()
		{
			masterPlan = masterPlanService.CurrentMasterPlan;
			if (!villainLairModel.villainLairInstances.ContainsKey(currentLair.ID))
			{
				loadVillainLairInstancesSignal.Dispatch(currentLair.ID, delegate
				{
				});
				LoadVillain();
			}
			SetPlatformStatesAndGhostComponents();
			global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadStatic, "FadeBlack");
			guiService.Execute(command);
			routineRunner.StartCoroutine(FadeBlack());
		}

		private void LoadVillain()
		{
			if (masterPlan.cooldownUTCStartTime != 0)
			{
				return;
			}
			int villainCharacterDefID = masterPlan.Definition.VillainCharacterDefID;
			global::Kampai.Game.VillainDefinition villainDefinition = definitionService.Get<global::Kampai.Game.VillainDefinition>(villainCharacterDefID);
			global::Kampai.Game.Villain villain = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Villain>(villainDefinition.ID);
			if (villain == null)
			{
				villain = (global::Kampai.Game.Villain)villainDefinition.Build();
				playerService.Add(villain);
			}
			global::System.Collections.Generic.List<global::Kampai.Game.PrestigeDefinition> all = definitionService.GetAll<global::Kampai.Game.PrestigeDefinition>();
			foreach (global::Kampai.Game.PrestigeDefinition item in all)
			{
				if (item.TrackedDefinitionID == villainCharacterDefID)
				{
					global::Kampai.Game.Prestige prestige = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(item.ID);
					if (prestige == null)
					{
						prestige = prestigeService.CreatePrestige(item.ID);
					}
					prestige.state = global::Kampai.Game.PrestigeState.Taskable;
					prestige.trackedInstanceId = villain.ID;
					prestige.UTCTimeUnlocked = timeService.CurrentTime();
				}
			}
			createNamedCharacterViewSignal.Dispatch(villain);
		}

		private void SetPlatformStatesAndGhostComponents()
		{
			if (masterPlan.cooldownUTCStartTime != 0)
			{
				return;
			}
			for (int i = 0; i < masterPlan.Definition.ComponentDefinitionIDs.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(masterPlan.Definition.ComponentDefinitionIDs[i]);
				if (firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.State > global::Kampai.Game.MasterPlanComponentState.NotStarted)
				{
					if (firstInstanceByDefinitionId.State < global::Kampai.Game.MasterPlanComponentState.Scaffolding)
					{
						ghostComponentService.DisplayComponentMarkedAsInProgress(firstInstanceByDefinitionId);
					}
					enableOneVillainLairColliderSignal.Dispatch(false, firstInstanceByDefinitionId.buildingDefID);
				}
			}
		}

		private global::System.Collections.IEnumerator FadeBlack()
		{
			yield return null;
			global::System.Collections.Generic.IList<global::System.Action> actions = new global::System.Collections.Generic.List<global::System.Action> { (global::System.Action)FadedOutCallback };
			fadeBlackSignal.Dispatch(true, actions);
		}

		private void FadedOutCallback()
		{
			hideAllWayFindersSignal.Dispatch();
			setBuildMenuEnabledSignal.Dispatch(false);
			isFaded = true;
			villainLairModel.goingToLair = false;
			villainLairModel.currentActiveLair = currentLair;
			customCameraPositionSignal.Dispatch(currentLair.Definition.CustomCameraPositionDefinitionId, new global::Kampai.Util.Boxed<global::System.Action>(CameraCallback));
			global::Kampai.Game.MasterPlanDefinition definition = masterPlan.Definition;
			if (masterPlan.cooldownUTCStartTime == 0)
			{
				displayVolcanoWayfinderSignal.Dispatch();
				hideFluxWayfinderSignal.Dispatch(false);
				EnableBuildingsWayFinders();
				displayVillainSignal.Dispatch(definition.VillainCharacterDefID, true);
				initVillainSignal.Dispatch(currentLair, definition.VillainCharacterDefID);
			}
			else
			{
				displayAlertUISignal.Dispatch(masterPlan);
			}
			if (villainLairModel.villainLairInstances.ContainsKey(currentLair.ID))
			{
				villainLairModel.villainLairInstances[currentLair.ID].SetActive(true);
			}
		}

		private void FadedBackInCallback()
		{
			isFaded = false;
			if (shouldDisplayMasterPlanUI)
			{
				displayMasterPlanSignal.Dispatch(0);
			}
			if (!currentLair.hasVisited)
			{
				displayOnboardingSignal.Dispatch(65006);
			}
			else
			{
				enableVillainHudSignal.Dispatch(true);
				if (!masterPlan.introHasBeenDisplayed)
				{
					displayMasterPlanIntroDialogSignal.Dispatch();
				}
			}
			if (masterPlan.displayCooldownReward)
			{
				displayCooldownRewardSignal.Dispatch();
			}
			stopPulseSignal.Dispatch();
		}

		private void CameraCallback()
		{
			if (isFaded)
			{
				global::System.Collections.Generic.IList<global::System.Action> list = new global::System.Collections.Generic.List<global::System.Action>();
				list.Add(FadedBackInCallback);
				fadeBlackSignal.Dispatch(false, list);
			}
		}

		private void EnableBuildingsWayFinders()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MasterPlanComponentBuilding>();
			foreach (global::Kampai.Game.MasterPlanComponentBuilding item in instancesByType)
			{
				getWayFinderSignal.Dispatch(item.ID, delegate(int wayFinderId, global::Kampai.UI.View.IWayFinderView wayFinderView)
				{
					if (wayFinderView != null)
					{
						wayFinderView.SetForceHide(false);
					}
				});
			}
		}
	}
}
