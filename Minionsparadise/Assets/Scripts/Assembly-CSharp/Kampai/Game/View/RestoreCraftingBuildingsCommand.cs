namespace Kampai.Game.View
{
	public class RestoreCraftingBuildingsCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.CraftingBuilding building { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.HarvestReadySignal harvestSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CraftingCompleteSignal craftingCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		public override void Execute()
		{
			RestoreCraftingBuilding(building);
		}

		private void RestoreCraftingBuilding(global::Kampai.Game.CraftingBuilding craftingBuilding)
		{
			if (craftingBuilding.State != global::Kampai.Game.BuildingState.Construction && craftingBuilding.State != global::Kampai.Game.BuildingState.Inactive)
			{
				timeEventService.RemoveEvent(craftingBuilding.ID);
			}
			global::System.Collections.Generic.IList<int> list = new global::System.Collections.Generic.List<int>();
			int craftingStartTime = craftingBuilding.CraftingStartTime;
			craftingStartTime -= craftingBuilding.PartyTimeReduction;
			craftingBuilding.PartyTimeReduction = 0;
			global::System.Collections.Generic.IList<int> recipesInQueue = craftingBuilding.RecipeInQueue;
			RemoveUnusedOneOffCraftable(craftingBuilding, list, ref recipesInQueue);
			foreach (int item in recipesInQueue)
			{
				global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(item);
				if (craftingStartTime + ingredientsItemDefinition.TimeToHarvest <= timeService.CurrentTime())
				{
					list.Add(ingredientsItemDefinition.ID);
					craftingBuilding.CompletedCrafts.Add(ingredientsItemDefinition.ID);
					craftingStartTime += global::System.Convert.ToInt32(ingredientsItemDefinition.TimeToHarvest);
					continue;
				}
				timeEventService.AddEvent(craftingBuilding.ID, craftingStartTime, (int)ingredientsItemDefinition.TimeToHarvest, craftingCompleteSignal, global::Kampai.Game.TimeEventType.ProductionBuff);
				break;
			}
			craftingBuilding.CraftingStartTime = craftingStartTime;
			foreach (int item2 in list)
			{
				craftingBuilding.RecipeInQueue.Remove(item2);
			}
			ValidateCompletedQueue(craftingBuilding);
			SetState(craftingBuilding);
		}

		private void ValidateCompletedQueue(global::Kampai.Game.CraftingBuilding craftingBuilding)
		{
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			foreach (int completedCraft in craftingBuilding.CompletedCrafts)
			{
				global::Kampai.Game.DynamicIngredientsDefinition definition;
				if (definitionService.TryGet<global::Kampai.Game.DynamicIngredientsDefinition>(completedCraft, out definition) && definition.Depreciated)
				{
					list.Add(completedCraft);
				}
			}
			foreach (int item in list)
			{
				craftingBuilding.CompletedCrafts.Remove(item);
			}
		}

		private void RemoveUnusedOneOffCraftable(global::Kampai.Game.CraftingBuilding craftingBuilding, global::System.Collections.Generic.IList<int> toBeRemoved, ref global::System.Collections.Generic.IList<int> recipesInQueue)
		{
			foreach (int item in recipesInQueue)
			{
				global::Kampai.Game.DynamicIngredientsDefinition definition = null;
				definitionService.TryGet<global::Kampai.Game.DynamicIngredientsDefinition>(item, out definition);
				if (definition != null && definition.Depreciated)
				{
					toBeRemoved.Add(item);
				}
			}
			foreach (int item2 in toBeRemoved)
			{
				craftingBuilding.RecipeInQueue.Remove(item2);
			}
			recipesInQueue = craftingBuilding.RecipeInQueue;
			toBeRemoved.Clear();
		}

		private void SetState(global::Kampai.Game.CraftingBuilding craftingBuilding)
		{
			global::Kampai.Game.BuildingState newState = global::Kampai.Game.BuildingState.Inactive;
			if (craftingBuilding.CompletedCrafts.Count > 0)
			{
				harvestSignal.Dispatch(craftingBuilding.ID);
				if (craftingBuilding.RecipeInQueue.Count > 0)
				{
					newState = global::Kampai.Game.BuildingState.HarvestableAndWorking;
				}
				else
				{
					newState = global::Kampai.Game.BuildingState.Harvestable;
				}
			}
			else if (craftingBuilding.RecipeInQueue.Count > 0)
			{
				newState = global::Kampai.Game.BuildingState.Working;
			}
			if (newState != global::Kampai.Game.BuildingState.Inactive)
			{
				routineRunner.StartCoroutine(WaitAFrame(delegate
				{
					buildingChangeStateSignal.Dispatch(craftingBuilding.ID, newState);
				}));
			}
		}

		private global::System.Collections.IEnumerator WaitAFrame(global::System.Action a)
		{
			yield return null;
			a();
		}
	}
}
