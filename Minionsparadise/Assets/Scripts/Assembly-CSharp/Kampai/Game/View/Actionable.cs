namespace Kampai.Game.View
{
	public interface Actionable
	{
		global::Kampai.Game.View.KampaiAction currentAction { get; }

		void EnqueueAction(global::Kampai.Game.View.KampaiAction action, bool clear = false);

		void InjectAction(global::Kampai.Game.View.KampaiAction action);

		void ReplaceCurrentAction(global::Kampai.Game.View.KampaiAction action);

		void ReplaceActionsOfType(global::Kampai.Game.View.KampaiAction action);

		void ExecuteAction(global::Kampai.Game.View.KampaiAction action);

		int GetActionQueueCount();

		void ClearActionQueue();

		global::Kampai.Game.View.KampaiAction GetNextAction();

		T GetAction<T>() where T : global::Kampai.Game.View.KampaiAction;

		void ShelveActionQueue();

		void UnshelveActionQueue();

		void LogActions();
	}
}
