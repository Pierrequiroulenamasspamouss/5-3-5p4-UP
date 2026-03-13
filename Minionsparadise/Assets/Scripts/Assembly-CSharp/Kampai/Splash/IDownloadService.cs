namespace Kampai.Splash
{
	public interface IDownloadService
	{
		void Perform(global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request, bool forceRequest = false);

		void AddGlobalResponseListener(global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> response);

		void ProcessQueue();

		void Shutdown();

		void Abort();

		void Restart();
	}
}
