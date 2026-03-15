namespace Kampai.Util
{
	public static class HttpRequestConfig
	{
		private static int httpRequestTimeout = 30000;

		private static int httpRequestReadWriteTimeout = 30000;

		public static int HttpRequestTimeout
		{
			get
			{
				return httpRequestTimeout;
			}
		}

		public static int HttpRequestReadWriteTimeout
		{
			get
			{
				return httpRequestReadWriteTimeout;
			}
		}

		public static void SetConfig(global::Kampai.Game.ConfigurationDefinition clientConfig)
		{
			if (clientConfig.httpRequestTimeout > 0)
			{
				httpRequestTimeout = clientConfig.httpRequestTimeout;
			}
			if (clientConfig.httpRequestReadWriteTimeout > 0)
			{
				httpRequestReadWriteTimeout = clientConfig.httpRequestReadWriteTimeout;
			}
		}
	}
}
