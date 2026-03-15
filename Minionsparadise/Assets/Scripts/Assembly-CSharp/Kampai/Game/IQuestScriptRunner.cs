namespace Kampai.Game
{
	public interface IQuestScriptRunner
	{
		global::System.Action<global::Kampai.Game.QuestScriptInstance> OnQuestScriptComplete { get; set; }

		global::Kampai.Game.QuestRunnerLanguage Lang { get; }

		global::Kampai.Game.ReturnValueContainer InvokationValues { get; }

		void Start(global::Kampai.Game.QuestScriptInstance questScriptInstance, string scriptText, string filename, string startMethodName);

		void Stop();

		void Pause();

		void Resume();
	}
}
