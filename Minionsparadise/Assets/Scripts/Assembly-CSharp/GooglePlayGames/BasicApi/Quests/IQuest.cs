namespace GooglePlayGames.BasicApi.Quests
{
	public interface IQuest
	{
		string Id { get; }

		string Name { get; }

		string Description { get; }

		string BannerUrl { get; }

		string IconUrl { get; }

		global::System.DateTime StartTime { get; }

		global::System.DateTime ExpirationTime { get; }

		global::System.DateTime? AcceptedTime { get; }

		global::GooglePlayGames.BasicApi.Quests.IQuestMilestone Milestone { get; }

		global::GooglePlayGames.BasicApi.Quests.QuestState State { get; }
	}
}
