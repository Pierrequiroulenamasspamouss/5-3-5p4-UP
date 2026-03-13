public static class RewardUtil
{
	public static global::Kampai.Game.Transaction.TransactionDefinition GetRewardTransaction(global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, int playerLevel = -1)
	{
		global::Kampai.Game.LevelUpDefinition levelUpDefinition = definitionService.Get<global::Kampai.Game.LevelUpDefinition>(88888);
		int num = ((playerLevel >= 0) ? playerLevel : ((int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID)));
		global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition;
		if (num < levelUpDefinition.transactionList.Count)
		{
			transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(levelUpDefinition.transactionList[num]);
		}
		else
		{
			transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
			transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
			transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(1, 2u));
			transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(21, 1u));
		}
		return transactionDefinition;
	}

	public static global::Kampai.Game.Transaction.TransactionDefinition GetPartyTransaction(global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, int playerLevel = -1)
	{
		int num = ((playerLevel >= 0) ? playerLevel : ((int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID)));
		global::Kampai.Game.LevelFunTable levelFunTable = definitionService.Get<global::Kampai.Game.LevelFunTable>();
		if (num >= levelFunTable.partiesNeededList.Count)
		{
			num = levelFunTable.partiesNeededList.Count - 1;
		}
		int num2 = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_PARTY_INDEX_ID);
		global::Kampai.Game.PartyUpDefinition partyUpDefinition = levelFunTable.partiesNeededList[num];
		if (num2 >= partyUpDefinition.PointsNeeded.Count)
		{
			num2 = partyUpDefinition.PointsNeeded.Count - 1;
		}
		global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = partyUpDefinition.PartyTransaction.CopyTransaction();
		for (int i = 0; i < transactionDefinition.Outputs.Count; i++)
		{
			float num3 = partyUpDefinition.Multiplier * (float)num2;
			global::Kampai.Util.QuantityItem quantityItem = transactionDefinition.Outputs[i];
			quantityItem.Quantity += (uint)(num3 * (float)quantityItem.Quantity);
		}
		return transactionDefinition;
	}

	public static global::System.Collections.Generic.List<global::Kampai.Game.View.RewardQuantity> GetRewardQuantityFromTransaction(global::Kampai.Game.Transaction.TransactionDefinition transaction, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService)
	{
		global::System.Collections.Generic.List<global::Kampai.Game.View.RewardQuantity> list = new global::System.Collections.Generic.List<global::Kampai.Game.View.RewardQuantity>();
		foreach (global::Kampai.Util.QuantityItem output in transaction.Outputs)
		{
			if (output.ID == 0 || output.ID == 1 || output.ID == 5 || output.ID == 21)
			{
				list.Add(new global::Kampai.Game.View.RewardQuantity(output.ID, (int)output.Quantity, false, true));
			}
			else
			{
				if (output.ID == 6 || output.ID == 9 || output.ID == 1000012555)
				{
					continue;
				}
				bool isNew = false;
				global::Kampai.Game.UnlockDefinition definition;
				if (definitionService.TryGet<global::Kampai.Game.UnlockDefinition>(output.ID, out definition))
				{
					int unlockedQuantityOfID = playerService.GetUnlockedQuantityOfID(definition.ReferencedDefinitionID);
					if (!definition.Delta && unlockedQuantityOfID >= definition.UnlockedQuantity * (int)output.Quantity)
					{
						continue;
					}
					if (unlockedQuantityOfID == 0)
					{
						isNew = true;
					}
				}
				list.Add(new global::Kampai.Game.View.RewardQuantity(output.ID, (int)output.Quantity, isNew, false));
			}
		}
		return list;
	}
}
