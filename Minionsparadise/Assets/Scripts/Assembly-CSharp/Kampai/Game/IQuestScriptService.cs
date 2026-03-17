namespace Kampai.Game
{
	public interface IQuestScriptService
	{
		bool HasScript(int questInstanceID, bool pre, int stepID = -1, bool isQuestStep = false);

		bool HasScript(global::Kampai.Game.Quest quest, bool pre, int stepID = -1, bool isQuestStep = false);

		void StartQuestScript(int questInstanceID, bool pre, bool isReward = false, int stepID = -1, bool isStepQuest = false);

		void StartQuestScript(global::Kampai.Game.Quest quest, bool pre, bool isReward = false, int stepID = -1, bool isStepQuest = false);

		void stop();

		void PauseQuestScripts();

		void ResumeQuestScripts();
	}
}
