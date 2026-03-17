namespace Kampai.Game
{
	internal class PlayerSerializerV10 : global::Kampai.Game.PlayerSerializerV9
	{
		public override int Version
		{
			get
			{
				return 10;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 10)
			{
				FixCorruptedQuestInstanceIDs(player);
				player.Version = 10;
			}
			return player;
		}

		private void FixCorruptedQuestInstanceIDs(global::Kampai.Game.Player player)
		{
			if (player.nextId < 3146)
			{
				player.nextId = 3146;
			}
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			list.Add(3140);
			list.Add(3141);
			list.Add(3142);
			list.Add(3143);
			list.Add(3144);
			list.Add(3145);
			global::System.Collections.Generic.List<int> list2 = list;
			foreach (global::Kampai.Game.Quest item in player.GetInstancesByType<global::Kampai.Game.Quest>())
			{
				if (list2.Contains(item.ID))
				{
					player.AssignNextInstanceId(item);
				}
			}
		}
	}
}
