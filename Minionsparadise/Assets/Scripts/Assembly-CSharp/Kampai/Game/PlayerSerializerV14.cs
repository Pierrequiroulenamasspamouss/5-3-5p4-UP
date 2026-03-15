namespace Kampai.Game
{
	internal class PlayerSerializerV14 : global::Kampai.Game.PlayerSerializerV13
	{
		public override int Version
		{
			get
			{
				return 14;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 14)
			{
				FixPartyPauseItems(player);
				FixOverflowPrestigePoints(player);
				UpdateStagePosition(player);
				player.Version = 14;
			}
			return player;
		}

		private void FixPartyPauseItems(global::Kampai.Game.Player player)
		{
			TryToRemoveItem(player, 366);
			TryToRemoveItem(player, 367);
			TryToRemoveItem(player, 368);
			TryToRemoveItem(player, 708);
		}

		private void TryToRemoveItem(global::Kampai.Game.Player player, int definitionID)
		{
			int quantityByDefinitionId = (int)player.GetQuantityByDefinitionId(definitionID);
			if (quantityByDefinitionId > 0)
			{
				player.AlterQuantity(definitionID, -quantityByDefinitionId);
			}
		}

		private void FixOverflowPrestigePoints(global::Kampai.Game.Player player)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Prestige> instancesByType = player.GetInstancesByType<global::Kampai.Game.Prestige>();
			foreach (global::Kampai.Game.Prestige item in instancesByType)
			{
				int neededPrestigePoints = item.NeededPrestigePoints;
				if (neededPrestigePoints != 0 && item.CurrentPrestigePoints >= neededPrestigePoints)
				{
					item.CurrentPrestigePoints = item.NeededPrestigePoints - 1;
				}
			}
		}

		private void UpdateStagePosition(global::Kampai.Game.Player player)
		{
			global::Kampai.Game.StageBuilding byInstanceId = player.GetByInstanceId<global::Kampai.Game.StageBuilding>(370);
			if (byInstanceId != null)
			{
				byInstanceId.Location.x = 107;
				byInstanceId.Location.y = 171;
			}
		}
	}
}
