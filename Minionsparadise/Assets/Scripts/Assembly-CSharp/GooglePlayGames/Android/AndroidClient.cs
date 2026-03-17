namespace GooglePlayGames.Android
{
	internal class AndroidClient : global::GooglePlayGames.IClientImpl
	{
		private class StatsResultCallback : global::Com.Google.Android.Gms.Common.Api.ResultCallbackProxy<global::Com.Google.Android.Gms.Games.Stats.Stats_LoadPlayerStatsResultObject>
		{
			private global::System.Action<int, global::Com.Google.Android.Gms.Games.Stats.PlayerStats> callback;

			public StatsResultCallback(global::System.Action<int, global::Com.Google.Android.Gms.Games.Stats.PlayerStats> callback)
			{
				this.callback = callback;
			}

			public override void OnResult(global::Com.Google.Android.Gms.Games.Stats.Stats_LoadPlayerStatsResultObject arg_Result_1)
			{
				callback(arg_Result_1.getStatus().getStatusCode(), arg_Result_1.getPlayerStats());
			}
		}

		internal const string BridgeActivityClass = "com.google.games.bridge.NativeBridgeActivity";

		private const string LaunchBridgeMethod = "launchBridgeIntent";

		private const string LaunchBridgeSignature = "(Landroid/app/Activity;Landroid/content/Intent;)V";

		private global::GooglePlayGames.TokenClient tokenClient;

		public global::GooglePlayGames.Native.PInvoke.PlatformConfiguration CreatePlatformConfiguration()
		{
			global::GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration androidPlatformConfiguration = global::GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration.Create();
			using (global::UnityEngine.AndroidJavaObject androidJavaObject = global::GooglePlayGames.Android.AndroidTokenClient.GetActivity())
			{
				androidPlatformConfiguration.SetActivity(androidJavaObject.GetRawObject());
				androidPlatformConfiguration.SetOptionalIntentHandlerForUI(delegate(global::System.IntPtr intent)
				{
					global::System.IntPtr intentRef = global::UnityEngine.AndroidJNI.NewGlobalRef(intent);
					global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
					{
						try
						{
							LaunchBridgeIntent(intentRef);
						}
						finally
						{
							global::UnityEngine.AndroidJNI.DeleteGlobalRef(intentRef);
						}
					});
				});
				return androidPlatformConfiguration;
			}
		}

		public global::GooglePlayGames.TokenClient CreateTokenClient(string playerId, bool reset)
		{
			if (tokenClient == null || reset)
			{
				tokenClient = new global::GooglePlayGames.Android.AndroidTokenClient(playerId);
			}
			return tokenClient;
		}

		private static void LaunchBridgeIntent(global::System.IntPtr bridgedIntent)
		{
			object[] args = new object[2];
			global::UnityEngine.jvalue[] array = global::UnityEngine.AndroidJNIHelper.CreateJNIArgArray(args);
			try
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.google.games.bridge.NativeBridgeActivity"))
				{
					using (global::UnityEngine.AndroidJavaObject androidJavaObject = global::GooglePlayGames.Android.AndroidTokenClient.GetActivity())
					{
						global::System.IntPtr staticMethodID = global::UnityEngine.AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "launchBridgeIntent", "(Landroid/app/Activity;Landroid/content/Intent;)V");
						array[0].l = androidJavaObject.GetRawObject();
						array[1].l = bridgedIntent;
						global::UnityEngine.AndroidJNI.CallStaticVoidMethod(androidJavaClass.GetRawClass(), staticMethodID, array);
					}
				}
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Exception launching bridge intent: " + ex.Message);
				global::GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
			finally
			{
				global::UnityEngine.AndroidJNIHelper.DeleteJNIArgArray(args, array);
			}
		}

		public void GetPlayerStats(global::System.IntPtr apiClient, global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, global::GooglePlayGames.BasicApi.PlayerStats> callback)
		{
			global::Com.Google.Android.Gms.Common.Api.GoogleApiClient arg_GoogleApiClient_ = new global::Com.Google.Android.Gms.Common.Api.GoogleApiClient(apiClient);
			global::GooglePlayGames.Android.AndroidClient.StatsResultCallback resultCallback;
			try
			{
				resultCallback = new global::GooglePlayGames.Android.AndroidClient.StatsResultCallback(delegate(int result, global::Com.Google.Android.Gms.Games.Stats.PlayerStats stats)
				{
					global::UnityEngine.Debug.Log("Result for getStats: " + result);
					global::GooglePlayGames.BasicApi.PlayerStats arg = null;
					if (stats != null)
					{
						arg = new global::GooglePlayGames.BasicApi.PlayerStats
						{
							AvgSessonLength = stats.getAverageSessionLength(),
							DaysSinceLastPlayed = stats.getDaysSinceLastPlayed(),
							NumberOfPurchases = stats.getNumberOfPurchases(),
							NumberOfSessions = stats.getNumberOfSessions(),
							SessPercentile = stats.getSessionPercentile(),
							SpendPercentile = stats.getSpendPercentile(),
							ChurnProbability = stats.getChurnProbability(),
							SpendProbability = stats.getSpendProbability()
						};
					}
					callback((global::GooglePlayGames.BasicApi.CommonStatusCodes)result, arg);
				});
			}
			catch (global::System.Exception exception)
			{
				global::UnityEngine.Debug.LogException(exception);
				callback(global::GooglePlayGames.BasicApi.CommonStatusCodes.DeveloperError, null);
				return;
			}
			global::Com.Google.Android.Gms.Common.Api.PendingResult<global::Com.Google.Android.Gms.Games.Stats.Stats_LoadPlayerStatsResultObject> pendingResult = global::Com.Google.Android.Gms.Games.Games.Stats.loadPlayerStats(arg_GoogleApiClient_, true);
			pendingResult.setResultCallback(resultCallback);
		}
	}
}
