namespace Kampai.Splash
{
	public class DLCModel
	{
		public ulong TotalSize { get; set; }

		public global::System.Collections.Generic.IList<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> PendingRequests { get; set; }

		public global::System.Collections.Generic.List<global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> RunningRequests { get; set; }

		public float LastNetworkFailureTime { get; set; }

		public bool ShouldLaunchDownloadAgain { get; set; }

		public bool ShouldLoadAudio { get; set; }

		public bool NextDownloadShouldLoadAudio { get; set; }

		public bool UdpEnabled { get; set; }

		public global::System.DateTime DownloadStartTime { get; set; }

		public DLCModel()
		{
		}
	}
}
