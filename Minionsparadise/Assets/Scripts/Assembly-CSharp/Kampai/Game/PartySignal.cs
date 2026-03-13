namespace Kampai.Game
{
	public class PartySignal : global::Kampai.Util.RateLimitedSignal
	{
		public override float MinimumGap
		{
			get
			{
				return 1f;
			}
		}

		public override float CurrentTime
		{
			get
			{
				return global::UnityEngine.Time.realtimeSinceStartup;
			}
		}
	}
}
