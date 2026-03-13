namespace GooglePlayGames.Native
{
	internal static class ConversionUtils
	{
		internal static global::GooglePlayGames.BasicApi.ResponseStatus ConvertResponseStatus(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status)
		{
			switch (status)
			{
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID:
				return global::GooglePlayGames.BasicApi.ResponseStatus.Success;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
				return global::GooglePlayGames.BasicApi.ResponseStatus.SuccessWithStale;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_INTERNAL:
				return global::GooglePlayGames.BasicApi.ResponseStatus.InternalError;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
				return global::GooglePlayGames.BasicApi.ResponseStatus.LicenseCheckFailed;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_NOT_AUTHORIZED:
				return global::GooglePlayGames.BasicApi.ResponseStatus.NotAuthorized;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
				return global::GooglePlayGames.BasicApi.ResponseStatus.Timeout;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
				return global::GooglePlayGames.BasicApi.ResponseStatus.VersionUpdateRequired;
			default:
				throw new global::System.InvalidOperationException("Unknown status: " + status);
			}
		}

		internal static global::GooglePlayGames.BasicApi.CommonStatusCodes ConvertResponseStatusToCommonStatus(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus status)
		{
			switch (status)
			{
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID:
				return global::GooglePlayGames.BasicApi.CommonStatusCodes.Success;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE:
				return global::GooglePlayGames.BasicApi.CommonStatusCodes.SuccessCached;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_INTERNAL:
				return global::GooglePlayGames.BasicApi.CommonStatusCodes.InternalError;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED:
				return global::GooglePlayGames.BasicApi.CommonStatusCodes.LicenseCheckFailed;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_NOT_AUTHORIZED:
				return global::GooglePlayGames.BasicApi.CommonStatusCodes.AuthApiAccessForbidden;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_TIMEOUT:
				return global::GooglePlayGames.BasicApi.CommonStatusCodes.Timeout;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_VERSION_UPDATE_REQUIRED:
				return global::GooglePlayGames.BasicApi.CommonStatusCodes.ServiceVersionUpdateRequired;
			default:
				global::UnityEngine.Debug.LogWarning(string.Concat("Unknown ResponseStatus: ", status, ", defaulting to CommonStatusCodes.Error"));
				return global::GooglePlayGames.BasicApi.CommonStatusCodes.Error;
			}
		}

		internal static global::GooglePlayGames.BasicApi.UIStatus ConvertUIStatus(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus status)
		{
			switch (status)
			{
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID:
				return global::GooglePlayGames.BasicApi.UIStatus.Valid;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_INTERNAL:
				return global::GooglePlayGames.BasicApi.UIStatus.InternalError;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_NOT_AUTHORIZED:
				return global::GooglePlayGames.BasicApi.UIStatus.NotAuthorized;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_TIMEOUT:
				return global::GooglePlayGames.BasicApi.UIStatus.Timeout;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_VERSION_UPDATE_REQUIRED:
				return global::GooglePlayGames.BasicApi.UIStatus.VersionUpdateRequired;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_CANCELED:
				return global::GooglePlayGames.BasicApi.UIStatus.UserClosedUI;
			case global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.ERROR_UI_BUSY:
				return global::GooglePlayGames.BasicApi.UIStatus.UiBusy;
			default:
				throw new global::System.InvalidOperationException("Unknown status: " + status);
			}
		}

		internal static global::GooglePlayGames.Native.Cwrapper.Types.DataSource AsDataSource(global::GooglePlayGames.BasicApi.DataSource source)
		{
			switch (source)
			{
			case global::GooglePlayGames.BasicApi.DataSource.ReadCacheOrNetwork:
				return global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK;
			case global::GooglePlayGames.BasicApi.DataSource.ReadNetworkOnly:
				return global::GooglePlayGames.Native.Cwrapper.Types.DataSource.NETWORK_ONLY;
			default:
				throw new global::System.InvalidOperationException("Found unhandled DataSource: " + source);
			}
		}
	}
}
