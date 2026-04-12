namespace Kampai.Game
{
	public class CreateDoobersFromGiftBoxCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CreateDoobersFromGiftBoxCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPartyService partyService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal spawnDooberSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject NamedCharacterManager { get; set; }

		[Inject]
		public global::Kampai.Game.StartPartyFavorAnimationSignal startPartyFavorSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			int currentPartyIndex = minionPartyInstance.CurrentPartyIndex;
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			global::Kampai.Game.Transaction.TransactionDefinition transaction;
			if (partyService.IsInspirationParty(quantity, currentPartyIndex))
			{
				transaction = RewardUtil.GetRewardTransaction(definitionService, playerService, quantity);
			}
			else
			{
				transaction = RewardUtil.GetPartyTransaction(definitionService, playerService, quantity);
				startPartyFavorSignal.Dispatch();
			}
			global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = NamedCharacterManager.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>().Get(78);
			global::UnityEngine.Vector3 position = namedCharacterObject.gameObject.transform.position;
			position.y += 1f;
			global::System.Collections.Generic.List<global::Kampai.Game.View.RewardQuantity> rewardQuantityFromTransaction = RewardUtil.GetRewardQuantityFromTransaction(transaction, definitionService, playerService);
			foreach (global::Kampai.Game.View.RewardQuantity item in rewardQuantityFromTransaction)
			{
				if (item.IsReward)
				{
					switch (item.ID)
					{
					case 0:
						spawnDooberSignal.Dispatch(position, global::Kampai.UI.View.DestinationType.GRIND, 0, true);
						break;
					case 1:
						spawnDooberSignal.Dispatch(position, global::Kampai.UI.View.DestinationType.PREMIUM, 1, true);
						break;
					}
				}
			}
			if (guestService.PartyShouldProduceBuff())
			{
				global::Kampai.Game.BuffDefinition recentBuffDefinition = guestService.GetRecentBuffDefinition(true);
				if (recentBuffDefinition != null)
				{
					spawnDooberSignal.Dispatch(position, global::Kampai.UI.View.DestinationType.BUFF, recentBuffDefinition.ID, true);
				}
				else
				{
					logger.Fatal(global::Kampai.Util.FatalCode.BS_NULL_BUFF_DEFINITION);
				}
			}
		}
	}
}
