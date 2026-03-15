namespace Kampai.Game
{
	public static class AchievementType
	{
		public enum AchievementTypeIdentifier
		{
			UNKNOWN = 0,
			PLAYMIGNETTEXTIMES = 1
		}

		public static global::Kampai.Game.AchievementType.AchievementTypeIdentifier ParseIdentifier(string identifier)
		{
			if (!string.IsNullOrEmpty(identifier))
			{
				return (global::Kampai.Game.AchievementType.AchievementTypeIdentifier)(int)global::System.Enum.Parse(typeof(global::Kampai.Game.AchievementType.AchievementTypeIdentifier), identifier);
			}
			return global::Kampai.Game.AchievementType.AchievementTypeIdentifier.UNKNOWN;
		}
	}
}
