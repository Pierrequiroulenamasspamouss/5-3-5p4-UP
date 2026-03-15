namespace Kampai.Game
{
	internal class PlayerSerializerV6 : global::Kampai.Game.PlayerSerializerV5
	{
		public override int Version
		{
			get
			{
				return 6;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 6)
			{
				global::Kampai.Game.PurchasedLandExpansion byInstanceId = player.GetByInstanceId<global::Kampai.Game.PurchasedLandExpansion>(354);
				if (player.GetCountByDefinitionId(3509) > 0 && !byInstanceId.HasPurchased(1579033))
				{
					byInstanceId.PurchasedExpansions.Add(1579033);
				}
				if (player.GetCountByDefinitionId(3505) > 0 && !byInstanceId.HasPurchased(1973791))
				{
					byInstanceId.PurchasedExpansions.Add(1973791);
				}
				if (player.GetCountByDefinitionId(3508) > 0 && !byInstanceId.HasPurchased(2960686))
				{
					byInstanceId.PurchasedExpansions.Add(2960686);
				}
				if (player.GetCountByDefinitionId(3507) > 0 && !byInstanceId.HasPurchased(3355444))
				{
					byInstanceId.PurchasedExpansions.Add(3355444);
				}
				if (player.GetCountByDefinitionId(3104) > 0 && !byInstanceId.HasPurchased(9671571))
				{
					byInstanceId.PurchasedExpansions.Add(9671571);
				}
				int craftingStartTime = global::System.Convert.ToInt32((global::System.DateTime.UtcNow - global::Kampai.Util.GameConstants.Timers.epochStart).TotalSeconds);
				foreach (global::Kampai.Game.CraftingBuilding item in player.GetInstancesByType<global::Kampai.Game.CraftingBuilding>())
				{
					if (item.PartyTimeReduction < 0)
					{
						item.PartyTimeReduction = 0;
						item.CraftingStartTime = craftingStartTime;
					}
				}
				player.Version = 6;
			}
			return player;
		}
	}
}
