namespace Kampai.Game
{
	public class HarvestQuestStepController : global::Kampai.Game.QuestStepController
	{
		private global::Kampai.Game.IDefinitionService definitionService;

		public override bool NeedActiveDeliverButton
		{
			get
			{
				if (StepState == global::Kampai.Game.QuestStepState.Inprogress)
				{
					return true;
				}
				return false;
			}
		}

		public override int AmountNeeded
		{
			get
			{
				uint quantityByDefinitionId = playerService.GetQuantityByDefinitionId(base.questStepDefinition.ItemDefinitionID);
				base.questStep.AmountCompleted = (int)quantityByDefinitionId;
				return base.questStepDefinition.ItemAmount - (int)quantityByDefinitionId;
			}
		}

		public HarvestQuestStepController(global::Kampai.Game.Quest quest, int stepIndex, global::Kampai.Game.IQuestScriptService questScriptService, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService definitionService, global::strange.extensions.context.api.ICrossContextCapable gameContext, global::Kampai.Util.IKampaiLogger logger)
			: base(quest, stepIndex, questScriptService, playerService, gameContext, logger)
		{
			this.definitionService = definitionService;
		}

		public override void UpdateTask(global::Kampai.Game.QuestTaskTransition questTaskTransition, global::Kampai.Game.Building building, int buildingDefId, int itemDefId)
		{
			global::Kampai.Game.QuestStepDefinition questStepDefinition = base.questStepDefinition;
			global::Kampai.Game.QuestStep questStep = base.questStep;
			if (itemDefId != 0 && questStepDefinition.ItemDefinitionID != itemDefId)
			{
				return;
			}
			questStep.AmountCompleted = (int)playerService.GetQuantityByDefinitionId(questStepDefinition.ItemDefinitionID);
			questStep.AmountReady = GetHarvestableCount(questStepDefinition.ItemDefinitionID);
			if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Start && questStep.state == global::Kampai.Game.QuestStepState.Notstarted)
			{
				GoToNextState();
				if (questStep.AmountCompleted >= questStepDefinition.ItemAmount)
				{
					GoToTaskState(global::Kampai.Game.QuestStepState.WaitComplete);
				}
			}
			else if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Harvestable && questStep.state == global::Kampai.Game.QuestStepState.Inprogress)
			{
				if (questStep.AmountReady + questStep.AmountCompleted >= questStepDefinition.ItemAmount)
				{
					GoToNextState();
				}
			}
			else if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Complete && (questStep.state == global::Kampai.Game.QuestStepState.Inprogress || questStep.state == global::Kampai.Game.QuestStepState.Ready))
			{
				if (questStep.AmountCompleted >= questStepDefinition.ItemAmount)
				{
					GoToNextState(true);
				}
				else if (questStep.AmountReady + questStep.AmountCompleted >= questStepDefinition.ItemAmount && questStep.state == global::Kampai.Game.QuestStepState.Inprogress)
				{
					GoToNextState();
				}
			}
			else if (questTaskTransition == global::Kampai.Game.QuestTaskTransition.Start && questStep.AmountCompleted >= questStepDefinition.ItemAmount && questStep.state != global::Kampai.Game.QuestStepState.WaitComplete)
			{
				GoToNextState(true);
			}
		}

		private int GetHarvestableCount(int itemDefinitionId)
		{
			int num = 0;
			global::System.Collections.Generic.List<global::Kampai.Game.ResourceBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.ResourceBuilding>();
			global::System.Collections.Generic.List<global::Kampai.Game.CraftingBuilding> instancesByType2 = playerService.GetInstancesByType<global::Kampai.Game.CraftingBuilding>();
			int i = 0;
			for (int count = instancesByType.Count; i < count; i++)
			{
				global::Kampai.Game.ResourceBuilding resourceBuilding = instancesByType[i];
				if (resourceBuilding.Definition.ItemId == itemDefinitionId)
				{
					num += resourceBuilding.AvailableHarvest;
				}
			}
			int j = 0;
			for (int count2 = instancesByType2.Count; j < count2; j++)
			{
				global::Kampai.Game.CraftingBuilding craftingBuilding = instancesByType2[j];
				global::System.Collections.Generic.IList<int> completedCrafts = craftingBuilding.CompletedCrafts;
				int k = 0;
				for (int count3 = completedCrafts.Count; k < count3; k++)
				{
					if (completedCrafts[k] == itemDefinitionId)
					{
						num++;
					}
				}
			}
			return num;
		}

		public override void SetupTracking()
		{
			base.questStep.TrackedID = definitionService.GetBuildingDefintionIDFromItemDefintionID(base.questStepDefinition.ItemDefinitionID);
			if (base.questStep.TrackedID == 0)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.QS_NO_SUCH_TRACKED_HARVEST_ID, "Item definition id not found for Delivery Type quests");
			}
		}

		public override string GetStepAction(global::Kampai.Main.ILocalizationService localService)
		{
			return localService.GetString("haveAction");
		}

		public override string GetStepDescription(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService defService)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = defService.Get<global::Kampai.Game.ItemDefinition>(base.questStepDefinition.ItemDefinitionID);
			string text = localService.GetString(itemDefinition.LocalizedKey + "*", base.questStepDefinition.ItemAmount);
			return localService.GetString("harvestTaskDesc", text);
		}

		public override void GetStepDescIcon(global::Kampai.Game.IDefinitionService defService, out global::UnityEngine.Sprite mainSprite, out global::UnityEngine.Sprite maskSprite)
		{
			int buildingDefintionIDFromItemDefintionID = defService.GetBuildingDefintionIDFromItemDefintionID(base.questStepDefinition.ItemDefinitionID);
			global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get<global::Kampai.Game.BuildingDefinition>(buildingDefintionIDFromItemDefintionID);
			mainSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Image);
			maskSprite = UIUtils.LoadSpriteFromPath(buildingDefinition.Mask);
		}
	}
}
