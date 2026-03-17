namespace Kampai.Game
{
	internal class PlayerSerializerV5 : global::Kampai.Game.PlayerSerializerV4
	{
		private const int FTUE_LEVEL_COMPLETED_V4_AND_UNDER = 7;

		private const int XP_TO_LEVEL_UP = 4;

		public override int Version
		{
			get
			{
				return 5;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 5)
			{
				if (player.HighestFtueLevel < 7)
				{
					long iD = player.ID;
					string country = player.country;
					logger.Warning("Old user has not completed ftue, let's reset their inventory.");
					json = definitionService.GetInitialPlayer();
					player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
					player.ID = iD;
					player.country = country;
				}
				else
				{
					global::System.Array values = global::System.Enum.GetValues(typeof(global::Kampai.Game.FtueLevel));
					player.HighestFtueLevel = (int)values.GetValue(values.Length - 1);
					player.AddUnlock(80000, 1);
					int quantity = (int)player.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
					int quantity2 = (int)player.GetQuantity(global::Kampai.Game.StaticItem.XP_ID);
					global::Kampai.Util.Tuple<int, int> tuple = partyService.V4toV5UpdatePartyPointsAndIndex(quantity, quantity2);
					if (tuple != null)
					{
						player.SetQuantityByStaticItem(global::Kampai.Game.StaticItem.XP_ID, (uint)tuple.Item1);
						player.SetQuantityByStaticItem(global::Kampai.Game.StaticItem.LEVEL_PARTY_INDEX_ID, (uint)tuple.Item2);
					}
					global::Kampai.Game.Instance firstInstanceByDefinitionId = player.GetFirstInstanceByDefinitionId<global::Kampai.Game.Instance>(4);
					if (firstInstanceByDefinitionId != null)
					{
						player.Remove(firstInstanceByDefinitionId);
					}
					else
					{
						logger.Warning("Old user does not have XP To Level Up ID in their inventory");
					}
				}
				global::Kampai.Game.PurchasedLandExpansion byInstanceId = player.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
				if (byInstanceId.HasPurchased(1579032) && !byInstanceId.HasPurchased(1579033))
				{
					byInstanceId.PurchasedExpansions.Add(1579033);
				}
				if (byInstanceId.HasPurchased(1973790) && !byInstanceId.HasPurchased(1973791))
				{
					byInstanceId.PurchasedExpansions.Add(1973791);
				}
				if (byInstanceId.HasPurchased(2960685) && !byInstanceId.HasPurchased(2960686))
				{
					byInstanceId.PurchasedExpansions.Add(2960686);
				}
				if (byInstanceId.HasPurchased(3355443) && !byInstanceId.HasPurchased(3355444))
				{
					byInstanceId.PurchasedExpansions.Add(3355444);
				}
				player.Version = 5;
			}
			return player;
		}
	}
}
