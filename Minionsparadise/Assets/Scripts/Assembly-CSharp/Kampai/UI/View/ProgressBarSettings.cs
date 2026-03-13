namespace Kampai.UI.View
{
	public class ProgressBarSettings : global::Kampai.UI.View.WorldToGlassUISettings
	{
		public int StartTime { get; set; }

		public int Duration { get; set; }

		public global::strange.extensions.signal.impl.Signal<int> ConstructionCompleteSignal { get; set; }

		public ProgressBarSettings(int trackedId, global::strange.extensions.signal.impl.Signal<int> constructionCompleteSignal, int startTime, int duration)
			: base(trackedId)
		{
			StartTime = startTime;
			Duration = duration;
			ConstructionCompleteSignal = constructionCompleteSignal;
		}
	}
}
