namespace Kampai.Util
{
	public static class Extensions
	{
		private static readonly global::Kampai.Util.FatalCode[] NETWORK = new global::Kampai.Util.FatalCode[17]
		{
			global::Kampai.Util.FatalCode.GS_ERROR_LOGIN,
			global::Kampai.Util.FatalCode.GS_ERROR_LOGIN_2,
			global::Kampai.Util.FatalCode.GS_ERROR_LOGIN_3,
			global::Kampai.Util.FatalCode.GS_ERROR_LOGIN_4,
			global::Kampai.Util.FatalCode.GS_ERROR_LOGIN_5,
			global::Kampai.Util.FatalCode.GS_ERROR_FETCHING_PLAYER_DATA,
			global::Kampai.Util.FatalCode.CMD_HTTP_CLIENT,
			global::Kampai.Util.FatalCode.CMD_SAVE_PLAYER,
			global::Kampai.Util.FatalCode.DS_UNABLE_TO_FETCH,
			global::Kampai.Util.FatalCode.CONFIG_NETWORK_FAIL,
			global::Kampai.Util.FatalCode.DLC_REQ_FAIL,
			global::Kampai.Util.FatalCode.GS_ERROR_REGISTER,
			global::Kampai.Util.FatalCode.GS_ERROR_DOWNLOAD_MANIFEST,
			global::Kampai.Util.FatalCode.GS_ERROR_FETCH_DEFINITIONS,
			global::Kampai.Util.FatalCode.GS_ERROR_LOAD_PLAYER,
			global::Kampai.Util.FatalCode.DS_UNABLE_TO_LOAD,
			global::Kampai.Util.FatalCode.CMD_LOAD_PLAYER
		};

		public static bool IsNetworkError(this global::Kampai.Util.FatalCode code)
		{
			global::Kampai.Util.FatalCode[] nETWORK = NETWORK;
			foreach (global::Kampai.Util.FatalCode fatalCode in nETWORK)
			{
				if (fatalCode == code)
				{
					return true;
				}
			}
			return false;
		}
	}
}
