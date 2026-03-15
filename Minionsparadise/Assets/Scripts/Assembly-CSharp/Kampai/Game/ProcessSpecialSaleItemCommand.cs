namespace Kampai.Game
{
	public class ProcessSpecialSaleItemCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionService landExpansionService { get; set; }

		[Inject]
		public global::Kampai.Game.ILandExpansionConfigService landExpansionConfigService { get; set; }

		[Inject]
		public global::Kampai.Game.CleanupDebrisSignal cleanupDebrisSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal setStorageSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.Transaction.TransactionUpdateData update { get; set; }

		public override void Execute()
		{
			if (update == null || update.Outputs == null)
			{
				return;
			}
			foreach (global::Kampai.Util.QuantityItem output in update.Outputs)
			{
				switch (output.ID)
				{
				case 700:
					UnlockAllLandExpansions();
					break;
				case 701:
					ClearAllDebris();
					break;
				case 702:
					UpgradeStorageLevel((int)output.Quantity);
					break;
				}
			}
		}

		private void UnlockAllLandExpansions()
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
			transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			transactionDefinition.ID = int.MaxValue;
			global::Kampai.Game.PurchasedLandExpansion byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> outputs = transactionDefinition.Outputs;
			foreach (global::Kampai.Game.LandExpansionBuilding allExpansionBuilding in landExpansionService.GetAllExpansionBuildings())
			{
				global::Kampai.Game.LandExpansionConfig expansionConfig = landExpansionConfigService.GetExpansionConfig(allExpansionBuilding.ExpansionID);
				global::Kampai.Util.QuantityItem item = new global::Kampai.Util.QuantityItem(expansionConfig.ID, 1u);
				if (!outputs.Contains(item) && !byInstanceId.HasPurchased(expansionConfig.expansionId))
				{
					outputs.Add(item);
				}
			}
			global::Kampai.Game.Trigger.QuantityItemTriggerRewardDefinition quantityItemTriggerRewardDefinition = new global::Kampai.Game.Trigger.QuantityItemTriggerRewardDefinition();
			quantityItemTriggerRewardDefinition.transaction = transactionDefinition.ToInstance();
			quantityItemTriggerRewardDefinition.RewardPlayer(gameContext);
		}

		private void ClearAllDebris()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.DebrisBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.DebrisBuilding>();
			foreach (global::Kampai.Game.DebrisBuilding item in instancesByType)
			{
				cleanupDebrisSignal.Dispatch(item.ID, false);
			}
		}

		private void UpgradeStorageLevel(int times)
		{
			global::Kampai.Game.StorageBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.StorageBuilding>(314);
			byInstanceId.CurrentStorageBuildingLevel += times;
			int count = byInstanceId.Definition.StorageUpgrades.Count;
			byInstanceId.CurrentStorageBuildingLevel = ((byInstanceId.CurrentStorageBuildingLevel >= count) ? (count - 1) : byInstanceId.CurrentStorageBuildingLevel);
			setStorageSignal.Dispatch();
		}
	}
}
