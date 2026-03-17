namespace Kampai.Game
{
	public class CreateTSMQuestTaskCommand : global::strange.extensions.command.impl.Command
	{
		private const int TSM_SORTEDLIST_PICK_LIMIT = 3;

		private const int DEFAULT_MIGNETTE_DELAY = 20;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CreateTSMQuestTaskCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int questGiverId { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateQuestWorldIconsSignal UpdateQuestWorldIconsSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.CreateTSMQuestTaskSignal CreateTSMQuestTaskSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService TimeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService TimeService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateNamedCharacterViewSignal createNamedCharacterViewSignal { get; set; }

		public global::Kampai.Game.TSMCharacter tsmCharacter { get; set; }

		public override void Execute()
		{
			tsmCharacter = playerService.GetByInstanceId<global::Kampai.Game.TSMCharacter>(questGiverId);
			if (tsmCharacter == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Unable to find TSM character in player inventory!");
				return;
			}
			if (global::Kampai.Game.Mignette.View.MignetteManagerView.GetIsPlaying())
			{
				int cooldownMignetteDelayInSeconds = tsmCharacter.Definition.CooldownMignetteDelayInSeconds;
				TimeEventService.AddEvent(tsmCharacter.ID, TimeService.CurrentTime(), (cooldownMignetteDelayInSeconds <= 0) ? 20 : cooldownMignetteDelayInSeconds, CreateTSMQuestTaskSignal);
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "TSM Creation delayed due to mignette");
				return;
			}
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			global::Kampai.Game.TaskLevelBandDefinition taskLevelBandForLevel = definitionService.GetTaskLevelBandForLevel(quantity);
			global::Kampai.Game.DynamicQuestDefinition dynamicQuestDefinition = GenerateQuest(quantity, taskLevelBandForLevel);
			if (dynamicQuestDefinition != null)
			{
				global::Kampai.Game.Quest quest = new global::Kampai.Game.Quest(dynamicQuestDefinition);
				quest.Initialize();
				questService.AddQuest(quest);
				quest.Steps[0].TrackedID = quest.GetActiveDefinition().QuestSteps[0].ItemDefinitionID;
				quest.QuestIconTrackedInstanceId = questGiverId;
				logger.Info("New TSM quest id: {0} and trackedId: {1}", quest.ID, quest.Steps[0].TrackedID);
				routineRunner.StartCoroutine(WaitAFrame(quest));
			}
			else
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Unable to create new task.");
			}
		}

		private global::Kampai.Game.DynamicQuestDefinition GenerateQuest(int level, global::Kampai.Game.TaskLevelBandDefinition definition)
		{
			if (tsmCharacter != null)
			{
				global::Kampai.Game.DynamicQuestDefinition dynamicQuestDefinition = null;
				int num = 4;
				while (dynamicQuestDefinition == null && num-- > 0)
				{
					global::Kampai.Game.QuestStepType type = PickQuestType(definition);
					dynamicQuestDefinition = BuildTask(level, type, definition);
				}
				if (dynamicQuestDefinition != null)
				{
					global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
					transactionDefinition.ID = int.MaxValue;
					int grindReward = GetGrindReward(dynamicQuestDefinition);
					int xpReward = GetXpReward(definition);
					transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
					transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
					if (grindReward > 0)
					{
						transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(0, (uint)grindReward));
					}
					if (xpReward > 0)
					{
						transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(2, (uint)xpReward));
					}
					dynamicQuestDefinition.RewardTransactionInstance = transactionDefinition.ToInstance();
					dynamicQuestDefinition.type = global::Kampai.Game.QuestType.DynamicQuest;
					dynamicQuestDefinition.DropStep = definition.DropOdds;
					return dynamicQuestDefinition;
				}
				logger.Error("Giving up trying to generate a task.");
			}
			else
			{
				logger.Error("TSM character can not be found");
			}
			return null;
		}

		private global::Kampai.Game.DynamicQuestDefinition BuildTask(int level, global::Kampai.Game.QuestStepType type, global::Kampai.Game.TaskLevelBandDefinition def)
		{
			global::Kampai.Game.QuestStepDefinition questStepDefinition = null;
			switch (type)
			{
			case global::Kampai.Game.QuestStepType.Delivery:
				questStepDefinition = GenerateGiveTask(def);
				break;
			case global::Kampai.Game.QuestStepType.MinionTask:
				questStepDefinition = GenerateGiveTask(def);
				break;
			case global::Kampai.Game.QuestStepType.OrderBoard:
				questStepDefinition = GenerateOrderBoardTask(level, def);
				break;
			default:
				logger.Error("Illegal task type {0}", type);
				break;
			}
			if (questStepDefinition != null)
			{
				global::Kampai.Game.DynamicQuestDefinition dynamicQuestDefinition = new global::Kampai.Game.DynamicQuestDefinition();
				dynamicQuestDefinition.ID = 77777;
				dynamicQuestDefinition.SurfaceType = global::Kampai.Game.QuestSurfaceType.ProcedurallyGenerated;
				dynamicQuestDefinition.LocalizedKey = global::Kampai.Game.QuestDefinition.GetProceduralQuestDescription(type);
				dynamicQuestDefinition.QuestSteps = new global::System.Collections.Generic.List<global::Kampai.Game.QuestStepDefinition> { questStepDefinition };
				dynamicQuestDefinition.QuestBookIcon = global::Kampai.Game.QuestDefinition.GetProceduralQuestIcon(type);
				dynamicQuestDefinition.SurfaceID = questStepDefinition.ItemDefinitionID;
				return dynamicQuestDefinition;
			}
			return null;
		}

		private global::System.Collections.IEnumerator WaitAFrame(global::Kampai.Game.Quest quest)
		{
			yield return null;
			if (!tsmCharacter.Created)
			{
				createNamedCharacterViewSignal.Dispatch(tsmCharacter);
				UpdateQuestWorldIconsSignal.Dispatch(quest);
			}
		}

		private global::Kampai.Game.QuestStepDefinition GenerateGiveTask(global::Kampai.Game.TaskLevelBandDefinition def)
		{
			global::Kampai.Game.Item item = PickIngredient();
			if (item != null)
			{
				int quantity = (int)item.Quantity;
				float num = PickBetween(def.GiveTaskMinMultiplier, def.GiveTaskMaxMultiplier);
				int num2 = (int)global::System.Math.Ceiling((float)quantity * num);
				if (num2 < def.GiveTaskMinQuantity && num2 > 0)
				{
					num2 = def.GiveTaskMinQuantity;
				}
				global::Kampai.Game.QuestStepDefinition questStepDefinition = new global::Kampai.Game.QuestStepDefinition();
				questStepDefinition.Type = global::Kampai.Game.QuestStepType.Delivery;
				questStepDefinition.ItemAmount = num2;
				questStepDefinition.ItemDefinitionID = item.Definition.ID;
				return questStepDefinition;
			}
			logger.Log(global::Kampai.Util.KampaiLogLevel.Warning, "Player has no ingredients to sell.");
			return null;
		}

		private global::Kampai.Game.QuestStepDefinition GenerateOrderBoardTask(int level, global::Kampai.Game.TaskLevelBandDefinition def)
		{
			int num = (int)global::System.Math.Floor((float)level * PickBetween(def.FillOrderTaskMinMultiplier, def.FillOrderTaskMaxMultiplier));
			if (num > 0)
			{
				global::Kampai.Game.QuestStepDefinition questStepDefinition = new global::Kampai.Game.QuestStepDefinition();
				questStepDefinition.Type = global::Kampai.Game.QuestStepType.OrderBoard;
				questStepDefinition.ItemAmount = num;
				return questStepDefinition;
			}
			logger.Error("Generated NO orders for level band {0}", def.MinLevel);
			return null;
		}

		private int GetXpReward(global::Kampai.Game.TaskLevelBandDefinition def)
		{
			return (int)global::System.Math.Floor(PickBetween(def.MinXpMultiplier, def.MaxXpMultiplier) * (float)def.XpReward);
		}

		private int GetGrindReward(global::Kampai.Game.DynamicQuestDefinition task)
		{
			global::Kampai.Game.Quest quest = new global::Kampai.Game.Quest(task);
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(quest.GetActiveDefinition().QuestSteps[0].ItemDefinitionID);
			int itemAmount = quest.GetActiveDefinition().QuestSteps[0].ItemAmount;
			return (int)((float)itemDefinition.BaseGrindCost * (itemDefinition.TSMRewardMultipler / 100f)) * itemAmount;
		}

		private global::Kampai.Game.Item PickIngredient()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Item> list = new global::System.Collections.Generic.List<global::Kampai.Game.Item>();
			list.AddRange(playerService.GetItemsByDefinition<global::Kampai.Game.IngredientsItemDefinition>());
			global::System.Collections.Generic.List<global::Kampai.Game.Item> list2 = new global::System.Collections.Generic.List<global::Kampai.Game.Item>();
			foreach (global::Kampai.Game.Item item in list)
			{
				if (item.Definition is global::Kampai.Game.DynamicIngredientsDefinition)
				{
					list2.Add(item);
				}
			}
			foreach (global::Kampai.Game.Item item2 in list2)
			{
				list.Remove(item2);
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.Item> list3 = global::Kampai.Util.ItemUtil.SortItemsByQuantity(list, false);
			int num = ((list3.Count <= 3) ? list3.Count : 3);
			if (num > 0)
			{
				int index = (int)global::System.Math.Floor(randomService.NextFloat() * (float)num);
				return list3[index];
			}
			return null;
		}

		private float PickBetween(float min, float max)
		{
			float num = max - min;
			return min + num * randomService.NextFloat();
		}

		private global::Kampai.Game.QuestStepType PickQuestType(global::Kampai.Game.TaskLevelBandDefinition def)
		{
			global::Kampai.Game.Transaction.WeightedInstance weightedInstance = playerService.GetWeightedInstance(def.PickWeightsId);
			if (weightedInstance == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DS_BAD_TASK_WEIGHT);
				return global::Kampai.Game.QuestStepType.Construction;
			}
			global::Kampai.Util.QuantityItem quantityItem = weightedInstance.NextPick(randomService);
			return (global::Kampai.Game.QuestStepType)quantityItem.ID;
		}
	}
}
