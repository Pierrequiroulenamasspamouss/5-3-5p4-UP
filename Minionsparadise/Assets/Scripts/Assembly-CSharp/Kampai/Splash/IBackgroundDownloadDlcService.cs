namespace Kampai.Splash
{
	public interface IBackgroundDownloadDlcService
	{
		bool Stopped { get; }

		void Start();

		void Stop();
	}
}
