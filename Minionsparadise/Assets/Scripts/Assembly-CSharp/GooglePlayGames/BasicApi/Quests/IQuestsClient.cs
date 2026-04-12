namespace GooglePlayGames.BasicApi.Quests
{
	public interface IQuestsClient
	{
		void Fetch(global::GooglePlayGames.BasicApi.DataSource source, string questId, global::System.Action<global::GooglePlayGames.BasicApi.ResponseStatus, global::GooglePlayGames.BasicApi.Quests.IQuest> callback);

		void FetchMatchingState(global::GooglePlayGames.BasicApi.DataSource source, global::GooglePlayGames.BasicApi.Quests.QuestFetchFlags flags, global::System.Action<global::GooglePlayGames.BasicApi.ResponseStatus, global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Quests.IQuest>> callback);

		void ShowAllQuestsUI(global::System.Action<global::GooglePlayGames.BasicApi.Quests.QuestUiResult, global::GooglePlayGames.BasicApi.Quests.IQuest, global::GooglePlayGames.BasicApi.Quests.IQuestMilestone> callback);

		void ShowSpecificQuestUI(global::GooglePlayGames.BasicApi.Quests.IQuest quest, global::System.Action<global::GooglePlayGames.BasicApi.Quests.QuestUiResult, global::GooglePlayGames.BasicApi.Quests.IQuest, global::GooglePlayGames.BasicApi.Quests.IQuestMilestone> callback);

		void Accept(global::GooglePlayGames.BasicApi.Quests.IQuest quest, global::System.Action<global::GooglePlayGames.BasicApi.Quests.QuestAcceptStatus, global::GooglePlayGames.BasicApi.Quests.IQuest> callback);

		void ClaimMilestone(global::GooglePlayGames.BasicApi.Quests.IQuestMilestone milestone, global::System.Action<global::GooglePlayGames.BasicApi.Quests.QuestClaimMilestoneStatus, global::GooglePlayGames.BasicApi.Quests.IQuest, global::GooglePlayGames.BasicApi.Quests.IQuestMilestone> callback);
	}
}
