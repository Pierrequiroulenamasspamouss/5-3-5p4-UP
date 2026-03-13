namespace Kampai.Game
{
	public class BuildingRepairQuestStepController : global::Kampai.Game.QuestStepController
	{
		public override bool NeedActiveProgressBar
		{
			get
			{
				if (StepState == global::Kampai.Game.QuestStepState.Notstarted)
				{
					return false;
				}
				return true;
			}
		}

		public BuildingRepairQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			if ((StepType != global::Kampai.Game.QuestStepType.CabanaRepair || base.questStep.TrackedID == building.ID) && questTaskTransition == global::Kampai.Game.QuestTaskTransition.Complete)
			{
				GoToTaskState(global::Kampai.Game.QuestStepState.Complete);
			}
		}

		public override void SetupTracking()
		{
			int itemDefinitionID = base.questStepDefinition.ItemDefinitionID;
			int trackedID = 0;
			switch (StepType)
			{
			case global::Kampai.Game.QuestStepType.CabanaRepair:
			{
				global::Kampai.Game.CabanaBuilding firstInstanceByDefinitionId5 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.CabanaBuilding>(itemDefinitionID);
				if (firstInstanceByDefinitionId5 == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_MISSING_CABANA, "Cabana instance not found");
					return;
				}
				trackedID = firstInstanceByDefinitionId5.ID;
				break;
			}
			case global::Kampai.Game.QuestStepType.FountainRepair:
			{
				global::Kampai.Game.FountainBuilding firstInstanceByDefinitionId3 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.FountainBuilding>(itemDefinitionID);
				if (firstInstanceByDefinitionId3 == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_MISSING_FOUNTAIN, "Fountain instance not found");
					return;
				}
				trackedID = firstInstanceByDefinitionId3.ID;
				break;
			}
			case global::Kampai.Game.QuestStepType.StageRepair:
			{
				global::Kampai.Game.StageBuilding firstInstanceByDefinitionId6 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(itemDefinitionID);
				if (firstInstanceByDefinitionId6 == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_MISSING_STAGE, "Stage instance not found");
					return;
				}
				trackedID = firstInstanceByDefinitionId6.ID;
				break;
			}
			case global::Kampai.Game.QuestStepType.StorageRepair:
			{
				global::Kampai.Game.StorageBuilding firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StorageBuilding>(itemDefinitionID);
				if (firstInstanceByDefinitionId2 == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_MISSING_FOUNTAIN, "Storage instance not found");
					return;
				}
				trackedID = firstInstanceByDefinitionId2.ID;
				break;
			}
			case global::Kampai.Game.QuestStepType.WelcomeHutRepair:
			{
				global::Kampai.Game.WelcomeHutBuilding firstInstanceByDefinitionId4 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.WelcomeHutBuilding>(itemDefinitionID);
				if (firstInstanceByDefinitionId4 == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_MISSING_WELCOME_HUT, "Welcome hut instance not found");
					return;
				}
				trackedID = firstInstanceByDefinitionId4.ID;
				break;
			}
			case global::Kampai.Game.QuestStepType.LairPortalRepair:
			{
				global::Kampai.Game.VillainLairEntranceBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.VillainLairEntranceBuilding>(itemDefinitionID);
				if (firstInstanceByDefinitionId == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.PS_NEGATIVE_VALUE, "Villain Lair Portal instance not found");
					return;
				}
				trackedID = firstInstanceByDefinitionId.ID;
				break;
			}
			case global::Kampai.Game.QuestStepType.MinionUpgradeBuildingRepair:
				trackedID = 375;
				break;
			}
			base.questStep.TrackedID = trackedID;
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			if (StepType == global::Kampai.Game.QuestStepType.CabanaRepair || StepType == global::Kampai.Game.QuestStepType.StageRepair)
			{
				return localService.GetString("buildAction");
			}
			return localService.GetString("repairAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			string result = null;
			switch (StepType)
			{
			case global::Kampai.Game.QuestStepType.CabanaRepair:
				result = localService.GetString(string.Format("{0}{1}", "CabanaRepair", base.questStepDefinition.ItemAmount));
				break;
			case global::Kampai.Game.QuestStepType.FountainRepair:
				result = localService.GetString("repairFountainDesc");
				break;
			case global::Kampai.Game.QuestStepType.StageRepair:
				result = localService.GetString("repairStageDesc");
				break;
			case global::Kampai.Game.QuestStepType.StorageRepair:
				result = localService.GetString("repairStorageDesc");
				break;
			case global::Kampai.Game.QuestStepType.WelcomeHutRepair:
				result = localService.GetString("repairWelcomeDesc");
				break;
			case global::Kampai.Game.QuestStepType.LairPortalRepair:
				result = localService.GetString("repairLairPortalDesc");
				break;
			case global::Kampai.Game.QuestStepType.MinionUpgradeBuildingRepair:
				result = localService.GetString("repairMinionUpgradeBuildingDesc");
				break;
			}
			return result;
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = defService.Get<global::Kampai.Game.BuildingDefinition>(base.questStepDefinition.ItemDefinitionID);
			mainSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Mask);
		}
	}
}
