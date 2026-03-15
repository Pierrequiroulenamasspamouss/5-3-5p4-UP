namespace Kampai.Game
{
	internal class PlayerSerializerV9 : global::Kampai.Game.PlayerSerializerV8
	{
		public override int Version
		{
			get
			{
				return 9;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 9)
			{
				if (localPersistanceService.HasKey("freezeTime"))
				{
					localPersistanceService.PutDataInt("freezeTime", 0);
					localPersistanceService.DeleteKey("freezeTime");
				}
				AwardKevin(player, definitionService, logger);
				AwardV9Buildings(player, definitionService, logger);
				global::Kampai.Game.CompositeBuilding byInstanceId = player.GetByInstanceId<global::Kampai.Game.CompositeBuilding>(371);
				byInstanceId.Location = new global::Kampai.Game.Location(152, 174);
				CleanupOrderBoard(player, logger);
				CleanupFlux(player, logger);
				CleanupOldSales(player, logger);
				RepairMinionPrestige(player, definitionService, logger);
				player.Version = 9;
			}
			return player;
		}

		private void AwardKevin(global::Kampai.Game.Player player, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Util.IKampaiLogger logger)
		{
			if (player.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) >= 10 && player.GetFirstInstanceByDefinitionId<global::Kampai.Game.Character>(70003) == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Awarding Kevin to the Player");
				global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(40004);
				global::Kampai.Game.KevinCharacterDefinition def = definitionService.Get<global::Kampai.Game.NamedCharacterDefinition>(prestigeDefinition.TrackedDefinitionID) as global::Kampai.Game.KevinCharacterDefinition;
				global::Kampai.Game.KevinCharacter kevinCharacter = new global::Kampai.Game.KevinCharacter(def);
				player.AssignNextInstanceId(kevinCharacter);
				player.Add(kevinCharacter);
				global::Kampai.Game.Prestige prestige = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(40004);
				if (prestige == null)
				{
					prestige = new global::Kampai.Game.Prestige(prestigeDefinition);
					player.AssignNextInstanceId(prestige);
					player.Add(prestige);
				}
				prestige.state = global::Kampai.Game.PrestigeState.Questing;
				prestige.trackedInstanceId = kevinCharacter.ID;
			}
			else
			{
				if (player.GetFirstInstanceByDefinitionId<global::Kampai.Game.Character>(70003) == null)
				{
					return;
				}
				int num = 0;
				global::Kampai.Game.TikiBarBuilding byInstanceId = player.GetByInstanceId<global::Kampai.Game.TikiBarBuilding>(313);
				for (int i = 1; i < byInstanceId.minionQueue.Count; i++)
				{
					if (byInstanceId.minionQueue[i] == 40004)
					{
						num = i;
						break;
					}
				}
				if (num > 0)
				{
					byInstanceId.minionQueue[num] = -1;
				}
				global::Kampai.Game.Prestige firstInstanceByDefinitionId = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(40004);
				if (firstInstanceByDefinitionId != null)
				{
					firstInstanceByDefinitionId.state = global::Kampai.Game.PrestigeState.Questing;
				}
			}
		}

		private void AwardV9Buildings(global::Kampai.Game.Player player, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.MinionUpgradeBuilding firstInstanceByDefinitionId = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.MinionUpgradeBuilding>(3133);
			if (firstInstanceByDefinitionId == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Awarding the Minion Upgrade Building to the Player");
				global::Kampai.Game.MinionUpgradeBuildingDefinition definition = null;
				if (definitionService.TryGet<global::Kampai.Game.MinionUpgradeBuildingDefinition>(3133, out definition))
				{
					firstInstanceByDefinitionId = definition.Build() as global::Kampai.Game.MinionUpgradeBuilding;
					firstInstanceByDefinitionId.State = global::Kampai.Game.BuildingState.Inaccessible;
					firstInstanceByDefinitionId.Location = new global::Kampai.Game.Location(108, 163);
					firstInstanceByDefinitionId.ID = 375;
					player.Add(firstInstanceByDefinitionId);
				}
			}
			global::Kampai.Game.VillainLairEntranceBuilding firstInstanceByDefinitionId2 = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.VillainLairEntranceBuilding>(3132);
			if (firstInstanceByDefinitionId2 == null)
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Awarding the Villain Lair Entrance Building to the Player");
				global::Kampai.Game.VillainLairEntranceBuildingDefinition definition2 = null;
				if (definitionService.TryGet<global::Kampai.Game.VillainLairEntranceBuildingDefinition>(3132, out definition2))
				{
					firstInstanceByDefinitionId2 = definition2.Build() as global::Kampai.Game.VillainLairEntranceBuilding;
					firstInstanceByDefinitionId2.State = global::Kampai.Game.BuildingState.Inaccessible;
					firstInstanceByDefinitionId2.Location = new global::Kampai.Game.Location(157, 148);
					firstInstanceByDefinitionId2.ID = 374;
					player.Add(firstInstanceByDefinitionId2);
				}
			}
		}

		private void CleanupOrderBoard(global::Kampai.Game.Player player, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.OrderBoard firstInstanceByDefinitionId = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.OrderBoard>(3022);
			if (firstInstanceByDefinitionId.tickets != null)
			{
				foreach (global::Kampai.Game.OrderBoardTicket ticket in firstInstanceByDefinitionId.tickets)
				{
					if (ticket.CharacterDefinitionId == 40004 || ticket.CharacterDefinitionId == 40001)
					{
						logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Changing a Ticket to a Nnon-Character one");
						ticket.CharacterDefinitionId = 0;
					}
				}
			}
			if (firstInstanceByDefinitionId.PriorityPrestigeDefinitionIDs != null)
			{
				if (firstInstanceByDefinitionId.PriorityPrestigeDefinitionIDs.Contains(40004))
				{
					firstInstanceByDefinitionId.PriorityPrestigeDefinitionIDs.Remove(40004);
				}
				if (firstInstanceByDefinitionId.PriorityPrestigeDefinitionIDs.Contains(40001))
				{
					firstInstanceByDefinitionId.PriorityPrestigeDefinitionIDs.Remove(40001);
				}
			}
		}

		private void CleanupFlux(global::Kampai.Game.Player player, global::Kampai.Util.IKampaiLogger logger)
		{
			foreach (global::Kampai.Game.Instance item in player.GetInstancesByDefinition<global::Kampai.Game.PrestigeDefinition>())
			{
				global::Kampai.Game.Prestige prestige = item as global::Kampai.Game.Prestige;
				if (prestige != null && prestige.Definition.Type == global::Kampai.Game.PrestigeType.Villain)
				{
					global::Kampai.Game.Villain byInstanceId = player.GetByInstanceId<global::Kampai.Game.Villain>(prestige.trackedInstanceId);
					if (byInstanceId != null && byInstanceId.Definition.ID == 70004)
					{
						prestige.state = global::Kampai.Game.PrestigeState.Locked;
						byInstanceId.CabanaBuildingId = 0;
						logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Removing Villain: " + byInstanceId.Name);
						player.Remove(byInstanceId);
					}
				}
			}
		}

		private void CleanupOldSales(global::Kampai.Game.Player player, global::Kampai.Util.IKampaiLogger logger)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Sale> instancesByType = player.GetInstancesByType<global::Kampai.Game.Sale>();
			global::System.Collections.Generic.List<global::Kampai.Game.Sale> list = new global::System.Collections.Generic.List<global::Kampai.Game.Sale>();
			foreach (global::Kampai.Game.Sale item in instancesByType)
			{
				if (item.Purchased)
				{
					player.AddPurchasedUpsells(item.Definition.ID);
				}
				if (item.Definition == null || (item.Definition.Type != global::Kampai.Game.SalePackType.Upsell && item.Definition.Type != global::Kampai.Game.SalePackType.Redeemable))
				{
					logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "Removing Sale: {0}", item.Definition.ID);
					list.Add(item);
				}
			}
			foreach (global::Kampai.Game.Sale item2 in list)
			{
				player.Remove(item2);
			}
		}

		private void RepairMinionPrestige(global::Kampai.Game.Player player, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Util.IKampaiLogger logger)
		{
			foreach (global::Kampai.Game.Instance item in player.GetInstancesByDefinition<global::Kampai.Game.PrestigeDefinition>())
			{
				global::Kampai.Game.Prestige prestige = item as global::Kampai.Game.Prestige;
				if (prestige != null)
				{
					global::Kampai.Game.Minion byInstanceId = player.GetByInstanceId<global::Kampai.Game.Minion>(prestige.trackedInstanceId);
					if (byInstanceId != null)
					{
						byInstanceId.PrestigeId = prestige.ID;
					}
				}
			}
			global::Kampai.Game.SpecialEventItem firstInstanceByDefinitionId = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.SpecialEventItem>(110000);
			if (firstInstanceByDefinitionId == null || firstInstanceByDefinitionId.Definition == null || !(firstInstanceByDefinitionId.Definition is global::Kampai.Game.SpecialEventItemDefinition))
			{
				return;
			}
			global::Kampai.Game.SpecialEventItemDefinition specialEventItemDefinition = firstInstanceByDefinitionId.Definition as global::Kampai.Game.SpecialEventItemDefinition;
			global::Kampai.Game.Minion minion = null;
			global::Kampai.Game.Prestige prestige2 = null;
			foreach (global::Kampai.Game.Minion item2 in player.GetInstancesByType<global::Kampai.Game.Minion>())
			{
				if (!item2.HasPrestige && minion == null)
				{
					minion = item2;
					continue;
				}
				prestige2 = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(specialEventItemDefinition.PrestigeDefinitionID);
				if (prestige2 != null && prestige2.Definition.CostumeDefinitionID == specialEventItemDefinition.AwardCostumeId)
				{
					logger.Error("Winter minion already exists {0}", item2.ID);
					minion = null;
					break;
				}
			}
			if (minion == null)
			{
				return;
			}
			logger.Info("Transmuting generic minion {0} to event costume ID {1}", minion.ID, specialEventItemDefinition.AwardCostumeId);
			prestige2 = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(specialEventItemDefinition.PrestigeDefinitionID);
			if (prestige2 == null)
			{
				global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(specialEventItemDefinition.PrestigeDefinitionID);
				if (prestigeDefinition != null)
				{
					prestige2 = new global::Kampai.Game.Prestige(prestigeDefinition);
					player.AssignNextInstanceId(prestige2);
					minion.PrestigeId = prestige2.ID;
					prestige2.trackedInstanceId = minion.ID;
					prestige2.state = global::Kampai.Game.PrestigeState.Taskable;
					player.Add(prestige2);
				}
			}
			else
			{
				global::Kampai.Game.Minion byInstanceId2 = player.GetByInstanceId<global::Kampai.Game.Minion>(prestige2.trackedInstanceId);
				if (byInstanceId2 == null)
				{
					minion.PrestigeId = prestige2.ID;
					prestige2.trackedInstanceId = minion.ID;
				}
				else
				{
					byInstanceId2.PrestigeId = prestige2.ID;
				}
				prestige2.state = global::Kampai.Game.PrestigeState.Taskable;
			}
		}
	}
}
