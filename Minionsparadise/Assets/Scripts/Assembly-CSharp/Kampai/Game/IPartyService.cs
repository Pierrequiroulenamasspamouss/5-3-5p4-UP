namespace Kampai.Game
{
	public interface IPartyService
	{
		bool IsInspiredParty { get; set; }

		int GetTotalParties(int level);

		uint GetTotalPartyPoints(int level, int partyIndex);

		uint GetTotalPartyPoints(int level, int fromPartyIndex, int toPartyIndex);

		bool IsInspirationParty(int level, int currentIndex);

		void GetNewLevelIndexAndPointsAfterParty(int level, int currentIndex, int currentPoints, out int newLevel, out int newIndex, out int newPoints);

		int GetCumulativePointsEarnedThisLevel(int level, int currentIndex, int currentPartyPoints);

		int GetCumulativePointsRequiredThisLevel(int currentLevel);

		int GetCumulativePointsNeededForNextParty(int level, int currentIndex);

		global::Kampai.Util.Tuple<int, int> V4toV5UpdatePartyPointsAndIndex(int level, int xp);
	}
}
