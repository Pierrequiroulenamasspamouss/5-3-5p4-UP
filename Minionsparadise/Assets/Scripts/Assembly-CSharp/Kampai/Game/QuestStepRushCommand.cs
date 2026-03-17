namespace Kampai.Game
{
	public class QuestStepRushCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Util.Tuple<int, int> tuple { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetGrindCurrencySignal setGrindCurrencySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		public override void Execute()
		{
			int item = tuple.Item1;
			int item2 = tuple.Item2;
			setGrindCurrencySignal.Dispatch();
			setPremiumCurrencySignal.Dispatch();
			questService.RushQuestStep(item, item2);
		}
	}
}
