namespace Kampai.Game
{
	public class FrolicCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("FrolicCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int CharacterID { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IdleInTownHallSignal idleInTownHallSignal { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.FrolicCharacter byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.FrolicCharacter>(CharacterID);
			if (byInstanceId != null)
			{
				global::Kampai.Game.LocationIncidentalAnimationDefinition locationIncidentalAnimationDefinition = NextAnimation(byInstanceId);
				if (locationIncidentalAnimationDefinition != null)
				{
					idleInTownHallSignal.Dispatch(CharacterID, locationIncidentalAnimationDefinition);
				}
				else
				{
					logger.Error("Cannot find an animation for frolic");
				}
			}
			else
			{
				logger.Error("Cannot find frolic");
			}
		}

		private global::Kampai.Game.LocationIncidentalAnimationDefinition NextAnimation(global::Kampai.Game.FrolicCharacter character)
		{
			global::Kampai.Game.FloatLocation currentFrolicLocation = character.CurrentFrolicLocation;
			global::Kampai.Game.FrolicCharacterDefinition definition = character.Definition;
			global::Kampai.Game.Transaction.WeightedDefinition wanderWeightedDeck = definition.WanderWeightedDeck;
			if (playerService.GetMinionPartyInstance().IsPartyHappening)
			{
				foreach (global::Kampai.Game.LocationIncidentalAnimationDefinition wanderAnimation in definition.WanderAnimations)
				{
					if (wanderAnimation.ID == 1000009613)
					{
						return wanderAnimation;
					}
				}
			}
			global::Kampai.Game.Transaction.WeightedInstance weightedInstance = playerService.GetWeightedInstance(wanderWeightedDeck.ID, wanderWeightedDeck);
			int num = 10;
			global::Kampai.Game.LocationIncidentalAnimationDefinition locationIncidentalAnimationDefinition = null;
			do
			{
				global::Kampai.Util.QuantityItem quantityItem = weightedInstance.NextPick(randomService);
				foreach (global::Kampai.Game.LocationIncidentalAnimationDefinition wanderAnimation2 in definition.WanderAnimations)
				{
					if (wanderAnimation2.ID == quantityItem.ID)
					{
						locationIncidentalAnimationDefinition = wanderAnimation2;
						break;
					}
				}
			}
			while (num-- > 0 && locationIncidentalAnimationDefinition != null && locationIncidentalAnimationDefinition.Location.Equals(currentFrolicLocation));
			if (locationIncidentalAnimationDefinition == null)
			{
				logger.Error("Weighted deck {0} has illegal location animation defs", wanderWeightedDeck.ID);
			}
			return locationIncidentalAnimationDefinition;
		}
	}
}
