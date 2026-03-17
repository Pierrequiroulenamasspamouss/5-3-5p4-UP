namespace Kampai.Util
{
	public class TimerUnit
	{
		public float TimeLeft { get; set; }

		public global::System.Action OnComplete { get; set; }

		public TimerUnit(float timeLeft, global::System.Action onComplete)
		{
			TimeLeft = timeLeft;
			OnComplete = onComplete;
		}
	}
}
