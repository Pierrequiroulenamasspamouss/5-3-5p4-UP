namespace Kampai.Game
{
	internal class PlayerSerializerV12 : global::Kampai.Game.PlayerSerializerV11
	{
		public override int Version
		{
			get
			{
				return 12;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 12)
			{
				FixOldMasterPlan(player);
				player.Version = 12;
			}
			return player;
		}

		private void FixOldMasterPlan(global::Kampai.Game.Player player)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlan> instancesByType = player.GetInstancesByType<global::Kampai.Game.MasterPlan>();
			if (instancesByType == null || instancesByType.Count != 1)
			{
				return;
			}
			int iD = instancesByType[0].ID;
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> instancesByType2 = player.GetInstancesByType<global::Kampai.Game.MasterPlanComponent>();
			foreach (global::Kampai.Game.MasterPlanComponent item in instancesByType2)
			{
				if (iD != item.planTrackingInstance)
				{
					player.Remove(item);
				}
			}
		}
	}
}
