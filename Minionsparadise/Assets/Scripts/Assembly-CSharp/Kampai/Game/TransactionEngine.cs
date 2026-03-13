namespace Kampai.Game
{
	public class TransactionEngine
	{
		private global::Kampai.Game.ExchangeRate exchangeRate;

		private global::Kampai.Util.IKampaiLogger logger { get; set; }

		private global::Kampai.Game.IDefinitionService defService { get; set; }

		private global::Kampai.Common.IRandomService randomService { get; set; }

		private global::Kampai.Game.IPlayerService playerService { get; set; }

		public TransactionEngine(global::Kampai.Util.IKampaiLogger myLogger, global::Kampai.Game.IDefinitionService myDefs, global::Kampai.Common.IRandomService myRand, global::Kampai.Game.IPlayerService myPlayer)
		{
			logger = myLogger;
			defService = myDefs;
			randomService = myRand;
			playerService = myPlayer;
			exchangeRate = new global::Kampai.Game.ExchangeRate(myDefs);
		}

		public bool Perform(global::Kampai.Game.Player player, global::Kampai.Game.Transaction.TransactionDefinition transaction, global::Kampai.Game.TransactionArg arg = null)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> newItems;
			return Perform(player, transaction, out newItems, arg);
		}

		public bool Perform(global::Kampai.Game.Player player, global::Kampai.Game.Transaction.TransactionDefinition transaction, out global::System.Collections.Generic.IList<global::Kampai.Game.Instance> newItems, global::Kampai.Game.TransactionArg arg = null)
		{
			newItems = null;
			if (player != null && transaction != null)
			{
				if (ValidateInputs(player, transaction) && ValidateOutputs(transaction, arg))
				{
					if (transaction.Inputs != null)
					{
						foreach (global::Kampai.Util.QuantityItem input in transaction.Inputs)
						{
							player.AlterQuantityByDefId(input.ID, (int)(0L - (long)input.Quantity));
						}
					}
					if (transaction.Outputs != null)
					{
						newItems = new global::System.Collections.Generic.List<global::Kampai.Game.Instance>();
						foreach (global::Kampai.Util.QuantityItem output in transaction.Outputs)
						{
							int iD = output.ID;
							global::Kampai.Game.Definition definition = defService.Get(iD);
							global::Kampai.Game.Instance instance = null;
							if (definition is global::Kampai.Game.ItemDefinition)
							{
								instance = ProcessItemOutput(player, output, definition);
							}
							else if (definition is global::Kampai.Game.BuildingDefinition)
							{
								instance = AddBuildingItem(player, definition, arg, output.Quantity);
							}
							else if (definition is global::Kampai.Game.MinionDefinition)
							{
								instance = new global::Kampai.Game.Minion(definition as global::Kampai.Game.MinionDefinition);
							}
							else if (definition is global::Kampai.Game.Transaction.WeightedDefinition)
							{
								global::Kampai.Util.QuantityItem quantityItem = player.GetWeightedInstance(iD).NextPick(randomService);
								instance = player.AlterQuantityByDefId(quantityItem.ID, (int)quantityItem.Quantity);
							}
							else if (definition is global::Kampai.Game.LandExpansionConfig)
							{
								global::Kampai.Game.LandExpansionConfig expansionConfig = definition as global::Kampai.Game.LandExpansionConfig;
								player.AddLandExpansion(expansionConfig);
							}
							else if (definition is global::Kampai.Game.CompositeBuildingPieceDefinition)
							{
								AddCompositePieceAndPossiblyBuilding(player, newItems, (global::Kampai.Game.CompositeBuildingPieceDefinition)definition);
							}
							else if (definition is global::Kampai.Game.StickerDefinition)
							{
								global::Kampai.Game.Sticker sticker = new global::Kampai.Game.Sticker(definition as global::Kampai.Game.StickerDefinition);
								sticker.UTCTimeEarned = arg.TransactionUTCTime;
								instance = sticker;
							}
							else if (definition is global::Kampai.Game.AchievementDefinition)
							{
								global::Kampai.Game.Achievement firstInstanceByDefinitionId = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.Achievement>(iD);
								int quantity = (int)output.Quantity;
								if (firstInstanceByDefinitionId == null)
								{
									firstInstanceByDefinitionId = new global::Kampai.Game.Achievement(definition as global::Kampai.Game.AchievementDefinition);
									firstInstanceByDefinitionId.Progress += quantity;
									instance = firstInstanceByDefinitionId;
								}
								else
								{
									firstInstanceByDefinitionId.Progress += quantity;
								}
							}
							else
							{
								LogError(global::Kampai.Util.FatalCode.TE_UNKNOWN_OUTPUT, transaction, "Unknown output type");
							}
							if (arg != null)
							{
								foreach (global::Kampai.Game.ItemAccumulator accumulator in arg.GetAccumulators())
								{
									accumulator.AwardOutput(output);
								}
							}
							if (instance != null)
							{
								AssignIDAndAddToReturnList(player, newItems, instance);
							}
						}
					}
					return true;
				}
				return false;
			}
			logger.FatalNullArgument(global::Kampai.Util.FatalCode.TE_NULL_ARG);
			return true;
		}

		private global::Kampai.Game.Instance ProcessItemOutput(global::Kampai.Game.Player player, global::Kampai.Util.QuantityItem qi, global::Kampai.Game.Definition d)
		{
			int iD = d.ID;
			if (d is global::Kampai.Game.UnlockDefinition)
			{
				player.AddUnlockedItems(qi);
			}
			else
			{
				switch (iD)
				{
				case 5:
				{
					global::Kampai.Game.QuantityInstance quantityInstance = new global::Kampai.Game.QuantityInstance();
					quantityInstance.ID = qi.ID;
					quantityInstance.Quantity = qi.Quantity;
					return quantityInstance;
				}
				case 2:
					playerService.AddXP((int)qi.Quantity);
					break;
				case 21:
				case 700:
				case 701:
				case 702:
					return null;
				case 6:
				case 9:
				{
					uint quantityByDefinitionId = player.GetQuantityByDefinitionId(iD);
					if (qi.Quantity > quantityByDefinitionId)
					{
						return player.AlterQuantityByDefId(iD, (int)(qi.Quantity - quantityByDefinitionId));
					}
					break;
				}
				default:
					return player.AlterQuantityByDefId(iD, (int)qi.Quantity);
				}
			}
			return null;
		}

		private void AddCompositePieceAndPossiblyBuilding(global::Kampai.Game.Player player, global::System.Collections.Generic.IList<global::Kampai.Game.Instance> newItems, global::Kampai.Game.CompositeBuildingPieceDefinition pieceDefinition)
		{
			global::Kampai.Game.CompositeBuildingPiece compositeBuildingPiece = new global::Kampai.Game.CompositeBuildingPiece(pieceDefinition);
			AssignIDAndAddToReturnList(player, newItems, compositeBuildingPiece);
			global::Kampai.Game.CompositeBuilding compositeBuilding = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.CompositeBuilding>(pieceDefinition.BuildingDefinitionID);
			if (compositeBuilding == null)
			{
				global::Kampai.Game.CompositeBuildingDefinition definition = defService.Get<global::Kampai.Game.CompositeBuildingDefinition>(pieceDefinition.BuildingDefinitionID);
				global::Kampai.Game.Instance instance = AddBuildingItem(player, definition, null, 1u);
				AssignIDAndAddToReturnList(player, newItems, instance);
				compositeBuilding = (global::Kampai.Game.CompositeBuilding)instance;
			}
			compositeBuilding.AttachedCompositePieceIDs.Add(compositeBuildingPiece.ID);
		}

		private void AssignIDAndAddToReturnList(global::Kampai.Game.Player player, global::System.Collections.Generic.IList<global::Kampai.Game.Instance> newItems, global::Kampai.Game.Instance newItem)
		{
			if (!(newItem is global::Kampai.Game.Item) && !(newItem is global::Kampai.Game.QuantityInstance))
			{
				player.AssignNextInstanceId(newItem);
				player.Add(newItem);
			}
			newItems.Add(newItem);
		}

		private global::Kampai.Game.Instance AddBuildingItem(global::Kampai.Game.Player player, global::Kampai.Game.Definition definition, global::Kampai.Game.TransactionArg arg, uint count = 1)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = definition as global::Kampai.Game.BuildingDefinition;
			global::Kampai.Game.Building building = buildingDefinition.BuildBuilding();
			if (arg != null)
			{
				global::Kampai.Game.Location location = arg.Get<global::Kampai.Game.Location>();
				if (location != null)
				{
					building.Location = location;
				}
				else
				{
					building.SetState(global::Kampai.Game.BuildingState.Inventory);
				}
			}
			else
			{
				building.SetState(global::Kampai.Game.BuildingState.Inventory);
			}
			for (int i = 1; i < count; i++)
			{
				global::Kampai.Game.Building building2 = buildingDefinition.BuildBuilding();
				building2.SetState(global::Kampai.Game.BuildingState.Inventory);
				player.AssignNextInstanceId(building2);
				player.Add(building2);
			}
			return building;
		}

		public bool SubtractInputs(global::Kampai.Game.Player player, global::Kampai.Game.Transaction.TransactionDefinition transaction)
		{
			if (player != null && transaction != null)
			{
				if (ValidateInputs(player, transaction))
				{
					if (transaction.Inputs != null)
					{
						foreach (global::Kampai.Util.QuantityItem input in transaction.Inputs)
						{
							player.AlterQuantityByDefId(input.ID, (int)(0L - (long)input.Quantity));
						}
					}
					return true;
				}
				return false;
			}
			logger.FatalNullArgument(global::Kampai.Util.FatalCode.TE_NULL_ARG);
			return false;
		}

		public global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> GetRequiredItems(global::Kampai.Game.Player player, global::Kampai.Game.Transaction.TransactionDefinition transaction)
		{
			if (player != null && transaction != null && transaction.Inputs != null)
			{
				global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = transaction.Inputs;
				if (inputs != null)
				{
					global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> list = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
					{
						foreach (global::Kampai.Util.QuantityItem item2 in inputs)
						{
							uint quantityByDefinitionId = player.GetQuantityByDefinitionId(item2.ID);
							uint quantity = item2.Quantity;
							if (quantityByDefinitionId < quantity)
							{
								global::Kampai.Util.QuantityItem item = new global::Kampai.Util.QuantityItem(item2.ID, quantity - quantityByDefinitionId);
								list.Add(item);
							}
						}
						return list;
					}
				}
			}
			logger.FatalNullArgument(global::Kampai.Util.FatalCode.TE_NULL_ARG);
			return null;
		}

		public bool AddOutputs(global::Kampai.Game.Player player, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items)
		{
			if (player != null && items != null)
			{
				foreach (global::Kampai.Util.QuantityItem item in items)
				{
					int iD = item.ID;
					global::Kampai.Game.Definition definition = defService.Get(iD);
					global::Kampai.Game.Instance instance = null;
					if (definition is global::Kampai.Game.ItemDefinition)
					{
						player.AlterQuantityByDefId(iD, (int)item.Quantity);
					}
					else if (definition is global::Kampai.Game.Transaction.WeightedDefinition)
					{
						global::Kampai.Util.QuantityItem quantityItem = player.GetWeightedInstance(iD).NextPick(randomService);
						player.AlterQuantityByDefId(quantityItem.ID, (int)quantityItem.Quantity);
					}
					else
					{
						if (!(definition is global::Kampai.Game.MinionDefinition))
						{
							LogError(global::Kampai.Util.FatalCode.TE_UNKNOWN_OUTPUT, definition, "Unknown output type");
							return false;
						}
						instance = new global::Kampai.Game.Minion(definition as global::Kampai.Game.MinionDefinition);
					}
					if (instance != null)
					{
						player.AssignNextInstanceId(instance);
						player.Add(instance);
					}
				}
				return true;
			}
			logger.FatalNullArgument(global::Kampai.Util.FatalCode.TE_NULL_ARG);
			return false;
		}

		public bool AddOutputs(global::Kampai.Game.Player player, global::Kampai.Game.Transaction.TransactionDefinition transaction, global::Kampai.Game.TransactionArg arg = null)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> newItems;
			return AddOutputs(player, transaction, out newItems, arg);
		}

		public bool AddOutputs(global::Kampai.Game.Player player, global::Kampai.Game.Transaction.TransactionDefinition transaction, out global::System.Collections.Generic.IList<global::Kampai.Game.Instance> newItems, global::Kampai.Game.TransactionArg arg = null)
		{
			newItems = null;
			if (player != null && transaction != null)
			{
				if (ValidateOutputs(transaction, arg))
				{
					if (transaction.Outputs != null)
					{
						newItems = new global::System.Collections.Generic.List<global::Kampai.Game.Instance>();
						foreach (global::Kampai.Util.QuantityItem output in transaction.Outputs)
						{
							int iD = output.ID;
							global::Kampai.Game.Definition definition = defService.Get(iD);
							global::Kampai.Game.Instance instance = null;
							if (definition is global::Kampai.Game.ItemDefinition)
							{
								if (definition.ID == 2)
								{
									playerService.AddXP((int)output.Quantity);
								}
								else
								{
									player.AlterQuantityByDefId(iD, (int)output.Quantity);
								}
							}
							else if (definition is global::Kampai.Game.BuildingDefinition)
							{
								instance = AddBuildingItem(player, definition, arg, output.Quantity);
							}
							else if (definition is global::Kampai.Game.Transaction.WeightedDefinition)
							{
								global::Kampai.Util.QuantityItem quantityItem = player.GetWeightedInstance(iD).NextPick(randomService);
								player.AlterQuantityByDefId(quantityItem.ID, (int)quantityItem.Quantity);
							}
							else if (definition is global::Kampai.Game.MinionDefinition)
							{
								instance = new global::Kampai.Game.Minion(definition as global::Kampai.Game.MinionDefinition);
							}
							else
							{
								LogError(global::Kampai.Util.FatalCode.TE_UNKNOWN_OUTPUT, transaction, "Unknown output type");
							}
							if (arg != null)
							{
								foreach (global::Kampai.Game.ItemAccumulator accumulator in arg.GetAccumulators())
								{
									accumulator.AwardOutput(output);
								}
							}
							if (instance != null)
							{
								AssignIDAndAddToReturnList(player, newItems, instance);
							}
						}
					}
					return true;
				}
				return false;
			}
			logger.FatalNullArgument(global::Kampai.Util.FatalCode.TE_NULL_ARG);
			return false;
		}

		public bool ValidateOutputs(global::Kampai.Game.Transaction.TransactionDefinition transaction, global::Kampai.Game.TransactionArg arg)
		{
			if (transaction != null)
			{
				if (transaction.Outputs != null)
				{
					foreach (global::Kampai.Util.QuantityItem output in transaction.Outputs)
					{
						if (!ValidateOutput(transaction, output, arg))
						{
							return false;
						}
					}
				}
				return true;
			}
			logger.FatalNullArgument(global::Kampai.Util.FatalCode.TE_NULL_ARG);
			return false;
		}

		private bool ValidateOutput(global::Kampai.Game.Transaction.TransactionDefinition transaction, global::Kampai.Util.Identifiable qi, global::Kampai.Game.TransactionArg arg = null)
		{
			int iD = qi.ID;
			global::Kampai.Game.Definition definition = defService.Get(iD);
			if (!(definition is global::Kampai.Game.ItemDefinition) && !(definition is global::Kampai.Game.MinionDefinition) && !(definition is global::Kampai.Game.LandExpansionConfig) && !(definition is global::Kampai.Game.CompositeBuildingPieceDefinition) && !(definition is global::Kampai.Game.StickerDefinition) && !(definition is global::Kampai.Game.BuildingDefinition) && !(definition is global::Kampai.Game.AchievementDefinition))
			{
				if (!(definition is global::Kampai.Game.Transaction.WeightedDefinition))
				{
					LogError(global::Kampai.Util.FatalCode.TE_UNKNOWN_OUTPUT, transaction, "Unknown output type");
					return false;
				}
				foreach (global::Kampai.Game.Transaction.WeightedQuantityItem entity in ((global::Kampai.Game.Transaction.WeightedDefinition)definition).Entities)
				{
					if (!ValidateOutput(transaction, entity, arg))
					{
						return false;
					}
				}
			}
			return true;
		}

		public bool ValidateInputs(global::Kampai.Game.Player player, global::Kampai.Game.Transaction.TransactionDefinition transaction)
		{
			if (player != null && transaction != null)
			{
				if (transaction.Inputs != null)
				{
					global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = transaction.Inputs;
					if (inputs != null)
					{
						global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>(inputs.Count);
						foreach (global::Kampai.Util.QuantityItem item in inputs)
						{
							uint quantityByDefinitionId = player.GetQuantityByDefinitionId(item.ID);
							uint quantity = item.Quantity;
							if (quantityByDefinitionId < quantity)
							{
								return false;
							}
							if (list.Contains(item.ID))
							{
								LogError(global::Kampai.Util.FatalCode.TE_INCONSISTENT_DEF_MODEL, transaction, "Transaction has duplicate inputs ID={0} T={1}", item.ID, transaction.ID);
								return false;
							}
							list.Add(item.ID);
						}
					}
				}
				return true;
			}
			logger.FatalNullArgument(global::Kampai.Util.FatalCode.TE_NULL_ARG);
			return false;
		}

		public int CalculateRushCost(global::Kampai.Game.Player player, global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> items)
		{
			int num = 0;
			if (player != null && items != null)
			{
				foreach (global::Kampai.Util.QuantityItem item in items)
				{
					global::Kampai.Game.ItemDefinition itemDefinition = defService.Get<global::Kampai.Game.ItemDefinition>(item.ID);
					int num2 = global::UnityEngine.Mathf.FloorToInt(itemDefinition.BasePremiumCost * (float)item.Quantity);
					num2 = ((num2 == 0 && item.Quantity != 0) ? 1 : num2);
					num += num2;
				}
				return num;
			}
			logger.FatalNullArgument(global::Kampai.Util.FatalCode.TE_NULL_ARG);
			return num;
		}

		private void LogError(global::Kampai.Util.FatalCode code, object t, string message, params object[] args)
		{
			logger.Fatal(code, "{0} : {1}", t.ToString(), string.Format(message, args));
		}

		public int RequiredPremiumForGrind(int desiredGrind)
		{
			return exchangeRate.GrindToPremium(desiredGrind);
		}

		public int PremiumToGrind(int premium)
		{
			return exchangeRate.PremiumToGrind(premium);
		}
	}
}
