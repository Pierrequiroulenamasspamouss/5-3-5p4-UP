namespace Kampai.UI.View
{
	public interface IQuestWayFinderView : global::Kampai.UI.View.IWayFinderView, global::Kampai.UI.View.IWorldToGlassView
	{
		global::Kampai.Game.Quest CurrentActiveQuest { get; }

		void AddQuest(int questDefId);

		void RemoveQuest(int questDefId);

		bool IsNewQuestAvailable();

		bool IsQuestAvailable();

		bool IsTaskReady();

		bool IsQuestComplete();
	}
}
