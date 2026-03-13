namespace Kampai.Game
{
	public class TikiBarService : global::Kampai.Game.ITikiBarService
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("TikiBarService") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService PlayerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService PrestigeService { get; set; }

		public global::Kampai.Game.Prestige GetPrestigeForSeatableCharacter(global::Kampai.Game.Character character)
		{
			global::Kampai.Game.Prestige prestigeFromMinionInstance = PrestigeService.GetPrestigeFromMinionInstance(character);
			if (prestigeFromMinionInstance == null)
			{
				logger.Warning("Unable to find prestige for character with def id: {0} ", character.Definition.ID);
				return null;
			}
			if (prestigeFromMinionInstance.Definition.ID == 40014)
			{
				return null;
			}
			return prestigeFromMinionInstance;
		}

		public bool IsCharacterSitting(global::Kampai.Game.Prestige prestige)
		{
			global::Kampai.Game.TikiBarBuilding firstInstanceByDefinitionId = PlayerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.TikiBarBuilding>(3041);
			if (firstInstanceByDefinitionId == null)
			{
				logger.Warning("Unable to find tikibar building!");
				return false;
			}
			int minionSlotIndex = firstInstanceByDefinitionId.GetMinionSlotIndex(prestige.Definition.ID);
			return minionSlotIndex >= 0 && minionSlotIndex < 3;
		}

		public bool IsCharacterSitting(global::Kampai.Game.Character character)
		{
			global::Kampai.Game.Prestige prestigeForSeatableCharacter = GetPrestigeForSeatableCharacter(character);
			if (prestigeForSeatableCharacter == null)
			{
				return false;
			}
			return IsCharacterSitting(prestigeForSeatableCharacter);
		}
	}
}
