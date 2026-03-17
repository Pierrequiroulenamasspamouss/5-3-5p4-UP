namespace Kampai.Game
{
	public class CancelTSMQuestTaskCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Quest Quest { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateTSMQuestTaskSignal UpdateTSMQuestTaskSignal { get; set; }

		public override void Execute()
		{
			UpdateTSMQuestTaskSignal.Dispatch(Quest, false);
		}
	}
}
