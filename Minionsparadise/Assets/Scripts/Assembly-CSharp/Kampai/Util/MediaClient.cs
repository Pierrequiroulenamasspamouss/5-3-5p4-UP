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
			// De-integrated: media_client_start is not used to bypass missing libmedia_client.so
		}

		public static string ConvertUrl(string url)
		{
			url = url.Replace("https://", "http://");
			// De-integrated: media_client_convert_url is not used
			return url;
		}

		public static void Stop()
		{
			// De-integrated: media_client_stop is not used
		}
	}
}
