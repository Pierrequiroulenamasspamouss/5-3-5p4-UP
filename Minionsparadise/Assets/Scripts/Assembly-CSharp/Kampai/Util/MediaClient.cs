namespace Kampai.Util
{
	public static class MediaClient
	{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR && !UNITY_STANDALONE_WIN
		[global::System.Runtime.InteropServices.DllImport("media_client")]
		public static extern int media_client_start(global::System.IntPtr log_handler, int log_severity);

		[global::System.Runtime.InteropServices.DllImport("media_client")]
		public static extern string media_client_convert_url(string url);

		[global::System.Runtime.InteropServices.DllImport("media_client")]
		public static extern string media_client_stop();
#endif

		public static void Start()
		{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR && !UNITY_STANDALONE_WIN
			media_client_start(global::System.IntPtr.Zero, 0);
#endif
		}

		public static string ConvertUrl(string url)
		{
			url = url.Replace("https://", "http://");
#if !UNITY_WEBPLAYER && !UNITY_EDITOR && !UNITY_STANDALONE_WIN
			return media_client_convert_url(url);
#else
			return url;
#endif
		}

		public static void Stop()
		{
#if !UNITY_WEBPLAYER && !UNITY_EDITOR && !UNITY_STANDALONE_WIN
			global::Kampai.Util.Native.LogInfo(string.Format("MediaClient.Stop: {0}", media_client_stop()));
#endif
		}
	}
}
