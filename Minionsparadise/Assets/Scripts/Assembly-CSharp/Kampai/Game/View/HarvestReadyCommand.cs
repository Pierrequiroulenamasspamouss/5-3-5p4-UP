namespace Kampai.Game.View
{
	public class HarvestReadyCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("HarvestReadyCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateResourceIconSignal createResourceIconSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveMinionFromLeisureSignal removeMinionFromLeisureSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateLeisureMenuViewSignal updateLeisureMenuViewSignal { get; set; }

		[Inject]
		public int buildingID { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingID);
			global::Kampai.Game.ResourceBuilding resourceBuilding = byInstanceId as global::Kampai.Game.ResourceBuilding;
			global::Kampai.Game.CraftingBuilding craftingBuilding = byInstanceId as global::Kampai.Game.CraftingBuilding;
			global::Kampai.Game.LeisureBuilding leisureBuilding = byInstanceId as global::Kampai.Game.LeisureBuilding;
			global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = byInstanceId as global::Kampai.Game.VillainLairResourcePlot;
			if (resourceBuilding != null)
			{
				HarvestTaskableBuilding(resourceBuilding);
			}
			else if (craftingBuilding != null)
			{
				HarvestCraftingBuilding(craftingBuilding);
			}
			else if (leisureBuilding != null)
			{
				HarvestLeisureBuilding(leisureBuilding);
			}
			else if (villainLairResourcePlot != null)
			{
				HarvestLairResourcePLot(villainLairResourcePlot);
			}
			else
			{
				logger.Fatal(global::Kampai.Util.FatalCode.TE_NULL_ARG, "A non-producing building just finished its task...");
			}
		}

		private void HarvestTaskableBuilding(global::Kampai.Game.ResourceBuilding resourceBuilding)
		{
			int transactionID = resourceBuilding.GetTransactionID(definitionService);
			int harvestItemDefinitionIdFromTransactionId = definitionService.GetHarvestItemDefinitionIdFromTransactionId(transactionID);
			global::Kampai.Game.IQuestService obj = questService;
			int item = harvestItemDefinitionIdFromTransactionId;
			obj.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Harvest, global::Kampai.Game.QuestTaskTransition.Harvestable, null, 0, item);
			int count = resourceBuilding.BonusMinionItems.Count;
			if (count > 0)
			{
				routineRunner.StartCoroutine(CreateResourceIcon(resourceBuilding.ID, 196, count, true));
			}
			int availableHarvest = resourceBuilding.GetAvailableHarvest();
			routineRunner.StartCoroutine(CreateResourceIcon(resourceBuilding.ID, harvestItemDefinitionIdFromTransactionId, availableHarvest));
		}

		private void HarvestLeisureBuilding(global::Kampai.Game.LeisureBuilding leisureBuilding)
		{
			removeMinionFromLeisureSignal.Dispatch(leisureBuilding.ID);
			buildingStateChangeSignal.Dispatch(leisureBuilding.ID, global::Kampai.Game.BuildingState.Harvestable);
			updateLeisureMenuViewSignal.Dispatch();
			createResourceIconSignal.Dispatch(new global::Kampai.UI.View.ResourceIconSettings(leisureBuilding.ID, 2, leisureBuilding.Definition.PartyPointsReward));
		}

		private void HarvestLairResourcePLot(global::Kampai.Game.VillainLairResourcePlot resourcePlot)
		{
			global::Kampai.Game.VillainLairEntranceBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(resourcePlot.parentLair.portalInstanceID);
			int count = resourcePlot.BonusMinionItems.Count;
			global::Kampai.Util.Tuple<int, int> newHarvestAvailableForPortal = byInstanceId.GetNewHarvestAvailableForPortal(playerService);
			if (count > 0)
			{
				routineRunner.StartCoroutine(CreateResourceIcon(resourcePlot.ID, 196, count, true));
				routineRunner.StartCoroutine(CreateResourceIcon(byInstanceId.ID, 196, newHarvestAvailableForPortal.Item2, true));
			}
			int resourceItemID = resourcePlot.parentLair.Definition.ResourceItemID;
			routineRunner.StartCoroutine(CreateResourceIcon(resourcePlot.ID, resourceItemID, resourcePlot.harvestCount));
			routineRunner.StartCoroutine(CreateResourceIcon(byInstanceId.ID, resourceItemID, newHarvestAvailableForPortal.Item1));
		}

		private void HarvestCraftingBuilding(global::Kampai.Game.CraftingBuilding craftingBuilding)
		{
			int num = craftingBuilding.CompletedCrafts[craftingBuilding.CompletedCrafts.Count - 1];
			global::Kampai.Game.IQuestService obj = questService;
			int item = num;
			obj.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Harvest, global::Kampai.Game.QuestTaskTransition.Harvestable, null, 0, item);
			for (int i = 0; i < craftingBuilding.CompletedCrafts.Count; i++)
			{
				SetupCraftingIcon(craftingBuilding, i);
			}
		}

		private void SetupCraftingIcon(global::Kampai.Game.CraftingBuilding craftingBuilding, int itemIndex)
		{
			int num = craftingBuilding.CompletedCrafts[itemIndex];
			int num2 = 0;
			foreach (int completedCraft in craftingBuilding.CompletedCrafts)
			{
				if (completedCraft == num)
				{
					num2++;
				}
			}
			routineRunner.StartCoroutine(CreateResourceIcon(buildingID, num, num2));
		}

		private global::System.Collections.IEnumerator CreateResourceIcon(int buildingId, int itemDefId, int count, bool isBonus = false)
		{
			if (count != 0)
			{
				yield return null;
				createResourceIconSignal.Dispatch(new global::Kampai.UI.View.ResourceIconSettings(buildingId, itemDefId, count, isBonus));
			}
		}
	}
}
