namespace Kampai.Game
{
	public interface ITimedSocialEventService
	{
		global::Kampai.Game.TimedSocialEventDefinition GetCurrentSocialEvent();

		global::Kampai.Game.TimedSocialEventDefinition GetSocialEvent(int id);

		void ClearCache();

		void GetSocialEventState(int eventID, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> resultSignal);

		global::Kampai.Game.SocialTeamResponse GetSocialEventStateCached(int eventID);

		void CreateSocialTeam(int eventID, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> resultSignal);

		void JoinSocialTeam(int eventID, long teamID, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> resultSignal);

		void LeaveSocialTeam(int eventID, long teamID, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> resultSignal);

		void InviteFriends(int eventID, long teamID, global::Kampai.Game.IdentityType identityType, global::System.Collections.Generic.IList<string> externalIDs, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> resultSignal);

		void RejectInvitation(int eventID, long teamID, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> resultSignal);

		void FillOrder(int eventID, long teamID, int orderID, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> resultSignal);

		void ClaimReward(int eventID, long teamID, global::strange.extensions.signal.impl.Signal<global::Kampai.Game.SocialTeamResponse, global::Kampai.Game.ErrorResponse> resultSignal);

		void GetSocialTeams(int eventID, global::System.Collections.Generic.IList<long> teamIds, global::strange.extensions.signal.impl.Signal<global::System.Collections.Generic.Dictionary<long, global::Kampai.Game.SocialTeam>> resultSignal);

		void setRewardCutscene(bool cutscene);

		bool isRewardCutscene();

		global::System.Collections.Generic.IList<int> GetPastEventsWithUnclaimedReward();

		global::Kampai.Game.TimedSocialEventDefinition GetNextSocialEvent();
	}
}
