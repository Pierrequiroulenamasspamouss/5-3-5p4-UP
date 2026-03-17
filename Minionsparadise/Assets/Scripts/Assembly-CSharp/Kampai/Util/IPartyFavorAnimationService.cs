namespace Kampai.Util
{
	public interface IPartyFavorAnimationService
	{
		void CreateRandomPartyFavor(int minionId = -1);

		global::System.Collections.Generic.HashSet<int> GetAllPartyFavorItems();

		global::System.Collections.Generic.List<int> GetAvailablePartyFavorItems();

		void AddAvailablePartyFavorItem(int ID);

		void ReleasePartyFavor(int partyFavorId);

		void AddMinionsToPartyFavor(int partyFavorId, global::Kampai.Game.View.MinionObject minion);

		bool PlayRandomIncidentalAnimation(int minionID);

		void RemoveAllPartyFavorAnimations();
	}
}
