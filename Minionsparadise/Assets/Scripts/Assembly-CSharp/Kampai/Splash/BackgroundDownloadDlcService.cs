namespace Kampai.Splash
{
	public class BackgroundDownloadDlcService : global::Kampai.Splash.IBackgroundDownloadDlcService
	{
		public bool Stopped { get { return true; } }

		public void Start()
		{
			// Background downloading for bundles is disabled.
		}

		public void Stop()
		{
			// Background downloading for bundles is disabled.
		}
	}
}
