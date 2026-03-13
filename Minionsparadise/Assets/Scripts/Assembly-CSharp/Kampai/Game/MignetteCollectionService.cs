namespace Kampai.Game
{
	public class MignetteCollectionService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MignetteCollectionService") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.MignetteGameModel mignetteGameModel { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public global::Kampai.Game.Transaction.TransactionDefinition pendingRewardTransaction { get; set; }

		public global::Kampai.Game.RewardCollection GetCollectionForActiveMignette()
		{
			return GetActiveCollectionForMignette(mignetteGameModel.BuildingId);
		}

		public global::Kampai.Game.RewardCollection GetActiveCollectionForMignette(int mignetteBuildingId, bool persistCreatedCollection = true)
		{
			global::Kampai.Game.MignetteBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(mignetteBuildingId);
			return GetActiveCollectionForMignette(byInstanceId, persistCreatedCollection);
		}

		public global::Kampai.Game.RewardCollection GetActiveCollectionForMignette(global::Kampai.Game.MignetteBuilding building, bool persistCreatedCollection = true)
		{
			global::Kampai.Game.RewardCollection orCreateActiveCollection = getOrCreateActiveCollection(building.StartedMainCollectionIDs, building.Definition.MainCollectionDefinitionIDs, persistCreatedCollection, true);
			if (orCreateActiveCollection != null)
			{
				return orCreateActiveCollection;
			}
			orCreateActiveCollection = getOrCreateActiveCollection(building.StartedRepeatableCollectionIDs, building.Definition.RepeatableCollectionDefinitionIDs, persistCreatedCollection, false);
			if (orCreateActiveCollection != null)
			{
				return orCreateActiveCollection;
			}
			global::Kampai.Game.RewardCollection rewardCollection = null;
			foreach (int startedRepeatableCollectionID in building.StartedRepeatableCollectionIDs)
			{
				orCreateActiveCollection = playerService.GetByInstanceId<global::Kampai.Game.RewardCollection>(startedRepeatableCollectionID);
				orCreateActiveCollection.CollectionScorePreReset = orCreateActiveCollection.CollectionScoreProgress;
				orCreateActiveCollection.ResetProgress();
				if (rewardCollection == null)
				{
					rewardCollection = orCreateActiveCollection;
				}
			}
			return rewardCollection;
		}

		private global::Kampai.Game.RewardCollection getOrCreateActiveCollection(global::System.Collections.Generic.IList<int> startedCollectionIDs, global::System.Collections.Generic.IList<int> collectionDefinitionIDs, bool persistCreatedCollection, bool isMain)
		{
			global::System.Collections.Generic.HashSet<int> hashSet = new global::System.Collections.Generic.HashSet<int>();
			foreach (int startedCollectionID in startedCollectionIDs)
			{
				global::Kampai.Game.RewardCollection byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.RewardCollection>(startedCollectionID);
				if (!byInstanceId.IsCompleted())
				{
					bool flag = false;
					if (isMain)
					{
						flag = checkAndCompleteFinalRewardEarned(byInstanceId);
					}
					if (!flag)
					{
						return byInstanceId;
					}
				}
				hashSet.Add(byInstanceId.Definition.ID);
			}
			if (startedCollectionIDs.Count < collectionDefinitionIDs.Count)
			{
				foreach (int collectionDefinitionID in collectionDefinitionIDs)
				{
					if (!hashSet.Contains(collectionDefinitionID))
					{
						global::Kampai.Game.RewardCollectionDefinition definition = definitionService.Get<global::Kampai.Game.RewardCollectionDefinition>(collectionDefinitionID);
						global::Kampai.Game.RewardCollection rewardCollection = new global::Kampai.Game.RewardCollection(definition);
						if (persistCreatedCollection)
						{
							playerService.Add(rewardCollection);
							startedCollectionIDs.Add(rewardCollection.ID);
						}
						return rewardCollection;
					}
				}
			}
			return null;
		}

		public bool checkAndCompleteFinalRewardEarned(global::Kampai.Game.RewardCollection collection)
		{
			bool result = false;
			int transactionID = collection.Definition.Rewards[collection.Definition.Rewards.Count - 1].TransactionID;
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(transactionID);
			if (transactionDefinition != null)
			{
				foreach (global::Kampai.Util.QuantityItem output in transactionDefinition.Outputs)
				{
					global::Kampai.Game.CompositeBuildingPiece firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.CompositeBuildingPiece>(output.ID);
					if (firstInstanceByDefinitionId != null)
					{
						result = true;
						logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "User already has earned last reward of reward collection, marking this collection as completed");
						collection.NumRewardsCollected = collection.Definition.Rewards.Count;
						break;
					}
				}
			}
			return result;
		}

		public void IncreaseScoreForMignetteCollection(int mignetteBuildingId, int scoreIncrease)
		{
			global::Kampai.Game.MignetteBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(mignetteBuildingId);
			global::Kampai.Game.RewardCollection activeCollectionForMignette = GetActiveCollectionForMignette(byInstanceId);
			byInstanceId.TotalScore += scoreIncrease;
			activeCollectionForMignette.IncreaseScore(scoreIncrease);
		}

		public global::Kampai.Game.Transaction.TransactionDefinition CreditRewardForActiveMignette()
		{
			global::Kampai.Game.RewardCollection collectionForActiveMignette = GetCollectionForActiveMignette();
			if (!collectionForActiveMignette.HasRewardReadyForCollection())
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "CreditRewardForActiveMignette called, but no reward is available! collectionID: " + collectionForActiveMignette.ID);
			}
			int transactionIDReadyForCollection = collectionForActiveMignette.GetTransactionIDReadyForCollection();
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(transactionIDReadyForCollection);
			playerService.RunEntireTransaction(transactionDefinition, global::Kampai.Game.TransactionTarget.NO_VISUAL, null);
			collectionForActiveMignette.NumRewardsCollected++;
			if (collectionForActiveMignette.IsCompleted())
			{
				int num = collectionForActiveMignette.CollectionScoreProgress - collectionForActiveMignette.GetMaxScore();
				if (num > 0)
				{
					GetCollectionForActiveMignette().CollectionScoreProgress += num;
				}
			}
			return transactionDefinition;
		}

		public void ResetMignetteProgress()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.MignetteBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MignetteBuilding>();
			foreach (global::Kampai.Game.MignetteBuilding item in instancesByType)
			{
				item.TotalScore = 0;
				global::Kampai.Game.RewardCollection activeCollectionForMignette = GetActiveCollectionForMignette(item.ID);
				activeCollectionForMignette.ResetProgress();
			}
		}
	}
}
