namespace Kampai.Util
{
	public static class GracefulErrors
	{
		private static readonly global::System.Collections.Generic.Dictionary<global::Kampai.Util.FatalCode, global::Kampai.Util.GracefulMessage> graceful = new global::System.Collections.Generic.Dictionary<global::Kampai.Util.FatalCode, global::Kampai.Util.GracefulMessage>
		{
			{
				global::Kampai.Util.FatalCode.SESSION_INVALID,
				new global::Kampai.Util.GracefulMessage("DuplicateLogin", "DuplicateLoginDetail")
			},
			{
				global::Kampai.Util.FatalCode.EX_INSUFFICIENT_STORAGE,
				new global::Kampai.Util.GracefulMessage("InsufficientStorage", "InsufficientStorageMessage")
			},
			{
				global::Kampai.Util.FatalCode.GS_ERROR_CORRUPT_SAVE_DETECTED,
				new global::Kampai.Util.GracefulMessage("CorruptionTitle", "CorruptionMessage")
			}
		};

		public static bool IsGracefulError(global::Kampai.Util.FatalCode code)
		{
			return graceful.ContainsKey(code);
		}

		public static global::Kampai.Util.GracefulMessage GetGracefulError(global::Kampai.Util.FatalCode code)
		{
			return (!graceful.ContainsKey(code)) ? null : graceful[code];
		}
	}
}
