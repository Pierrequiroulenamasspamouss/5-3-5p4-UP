namespace Kampai.Game
{
	public interface ITriggerService
	{
		global::Kampai.Game.Trigger.TriggerInstance ActiveTrigger { get; }

		global::Kampai.Game.Trigger.TriggerInstance AddActiveTrigger(global::Kampai.Game.Trigger.TriggerDefinition triggerDefinition);

		void RemoveOldTriggers();

		void ResetCurrentTrigger();

		void Initialize();
	}
}
