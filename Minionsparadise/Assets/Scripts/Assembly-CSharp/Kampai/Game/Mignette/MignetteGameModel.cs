namespace Kampai.Game.Mignette
{
	public class MignetteGameModel
	{
		public int CurrentGameScore { get; set; }

		public int CounterValue { get; set; }

		public bool UsesTimerHUD { get; set; }

		public bool UsesCounterHUD { get; set; }

		public bool UsesProgressHUD { get; set; }

		public int BuildingId { get; set; }

		public bool TriggerCooldownOnComplete { get; set; }

		public bool IsMignetteActive { get; set; }

		public float ElapsedTime { get; set; }

		public float TimeRemaining { get; set; }

		public float TotalEventTime { get; set; }

		public float PercentCompleted { get; set; }

		public global::UnityEngine.Sprite CollectableImage { get; set; }

		public global::UnityEngine.Sprite CollectableImageMask { get; set; }
	}
}
