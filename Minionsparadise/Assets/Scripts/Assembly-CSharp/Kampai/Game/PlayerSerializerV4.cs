namespace Kampai.Game
{
	internal class PlayerSerializerV4 : global::Kampai.Game.PlayerSerializerV3
	{
		public override int Version
		{
			get
			{
				return 4;
			}
		}

		public override global::Kampai.Game.Player Deserialize(string json, global::Kampai.Game.IDefinitionService definitionService, ILocalPersistanceService localPersistanceService, global::Kampai.Game.IPartyService partyService, global::Kampai.Util.IKampaiLogger logger)
		{
			global::Kampai.Game.Player player = base.Deserialize(json, definitionService, localPersistanceService, partyService, logger);
			if (player.Version < 4)
			{
				foreach (global::Kampai.Game.KampaiPendingTransaction pendingTransaction in player.GetPendingTransactions())
				{
					if (pendingTransaction.Transaction != null)
					{
						pendingTransaction.TransactionInstance = pendingTransaction.Transaction.ToInstance();
					}
				}
				player.Version = 4;
			}
			return player;
		}
	}
}
