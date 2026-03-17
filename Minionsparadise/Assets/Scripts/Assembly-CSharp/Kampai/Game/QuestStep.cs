namespace Kampai.Game
{
	public class QuestStep
	{
		public global::Kampai.Game.QuestStepState state { get; set; }

		public int AmountCompleted { get; set; }

		public int AmountReady { get; set; }

		public int TrackedID { get; set; }
	}
}
