public class AppTrackerView : global::strange.extensions.mediation.impl.View
{
	private bool isInitialized;

	public global::Kampai.Common.AppPauseSignal pauseSignal { get; set; }

	public global::Kampai.Common.AppEarlyPauseSignal earlyPauseSignal { get; set; }

	public global::Kampai.Common.AppResumeSignal resumeSignal { get; set; }

	public global::Kampai.Common.AppQuitSignal quitSignal { get; set; }

	public global::Kampai.Common.AppFocusGainedSignal focusGainedSignal { get; set; }

	public global::Kampai.Util.IKampaiLogger logger { get; set; }

	public void SetIsInitialized(bool isInitialized)
	{
		this.isInitialized = isInitialized;
	}

	public void OnApplicationPause(bool isPausing)
	{
		if (isPausing)
		{
			if (isInitialized)
			{
				pauseSignal.Dispatch();
			}
			earlyPauseSignal.Dispatch();
		}
		else if (isInitialized)
		{
			resumeSignal.Dispatch();
		}
		global::Kampai.Util.TimeProfiler.Flush();
	}

	public void OnApplicationQuit()
	{
		if (isInitialized)
		{
			quitSignal.Dispatch();
		}
		global::Kampai.Util.TimeProfiler.Flush();
	}

	public void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus && isInitialized && focusGainedSignal != null)
		{
			focusGainedSignal.Dispatch();
		}
	}
}
