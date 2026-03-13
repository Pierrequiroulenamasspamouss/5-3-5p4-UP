namespace Kampai.Game
{
	public interface ITikiBarService
	{
		global::Kampai.Game.Prestige GetPrestigeForSeatableCharacter(global::Kampai.Game.Character character);

		bool IsCharacterSitting(global::Kampai.Game.Prestige prestige);

		bool IsCharacterSitting(global::Kampai.Game.Character character);
	}
}
