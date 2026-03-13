namespace GooglePlayGames.Android
{
	internal class AndroidTokenClient : global::GooglePlayGames.TokenClient
	{
		private const string TokenFragmentClass = "com.google.games.bridge.TokenFragment";

		private const string FetchTokenSignature = "(Landroid/app/Activity;Ljava/lang/String;Ljava/lang/String;ZZZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;";

		private const string FetchTokenMethod = "fetchToken";

		private string playerId;

		private bool fetchingEmail;

		private bool fetchingAccessToken;

		private bool fetchingIdToken;

		private string accountName;

		private string accessToken;

		private string idToken;

		private string idTokenScope;

		private global::System.Action<string> idTokenCb;

		private string rationale;

		private bool apiAccessDenied;

		private int apiWarningFreq = 100000;

		private int apiWarningCount;

		private int webClientWarningFreq = 100000;

		private int webClientWarningCount;

		public AndroidTokenClient(string playerId)
		{
			this.playerId = playerId;
		}

		public static global::UnityEngine.AndroidJavaObject GetActivity()
		{
			using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				return androidJavaClass.GetStatic<global::UnityEngine.AndroidJavaObject>("currentActivity");
			}
		}

		public void SetRationale(string rationale)
		{
			this.rationale = rationale;
		}

		internal void Fetch(string scope, bool fetchEmail, bool fetchAccessToken, bool fetchIdToken, global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes> doneCallback)
		{
			if (apiAccessDenied)
			{
				if (apiWarningCount++ % apiWarningFreq == 0)
				{
					global::GooglePlayGames.OurUtils.Logger.w("Access to API denied");
					apiWarningCount = apiWarningCount / apiWarningFreq + 1;
				}
				doneCallback(global::GooglePlayGames.BasicApi.CommonStatusCodes.AuthApiAccessForbidden);
				return;
			}
			global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
			{
				FetchToken(scope, playerId, rationale, fetchEmail, fetchAccessToken, fetchIdToken, delegate(int rc, string access, string id, string email)
				{
					if (rc != 0)
					{
						apiAccessDenied = rc == 3001 || rc == 16;
						global::GooglePlayGames.OurUtils.Logger.w("Non-success returned from fetch: " + rc);
						doneCallback(global::GooglePlayGames.BasicApi.CommonStatusCodes.AuthApiAccessForbidden);
					}
					else
					{
						if (fetchAccessToken)
						{
							global::GooglePlayGames.OurUtils.Logger.d("a = " + access);
						}
						if (fetchEmail)
						{
							global::GooglePlayGames.OurUtils.Logger.d("email = " + email);
						}
						if (fetchIdToken)
						{
							global::GooglePlayGames.OurUtils.Logger.d("idt = " + id);
						}
						if (fetchAccessToken && !string.IsNullOrEmpty(access))
						{
							accessToken = access;
						}
						if (fetchIdToken && !string.IsNullOrEmpty(id))
						{
							idToken = id;
							idTokenCb(idToken);
						}
						if (fetchEmail && !string.IsNullOrEmpty(email))
						{
							accountName = email;
						}
						doneCallback(global::GooglePlayGames.BasicApi.CommonStatusCodes.Success);
					}
				});
			});
		}

		internal static void FetchToken(string scope, string playerId, string rationale, bool fetchEmail, bool fetchAccessToken, bool fetchIdToken, global::System.Action<int, string, string, string> callback)
		{
			object[] args = new object[7];
			global::UnityEngine.jvalue[] array = global::UnityEngine.AndroidJNIHelper.CreateJNIArgArray(args);
			try
			{
				using (global::UnityEngine.AndroidJavaClass androidJavaClass = new global::UnityEngine.AndroidJavaClass("com.google.games.bridge.TokenFragment"))
				{
					using (global::UnityEngine.AndroidJavaObject androidJavaObject = GetActivity())
					{
						global::System.IntPtr staticMethodID = global::UnityEngine.AndroidJNI.GetStaticMethodID(androidJavaClass.GetRawClass(), "fetchToken", "(Landroid/app/Activity;Ljava/lang/String;Ljava/lang/String;ZZZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;");
						array[0].l = androidJavaObject.GetRawObject();
						array[1].l = global::UnityEngine.AndroidJNI.NewStringUTF(playerId);
						array[2].l = global::UnityEngine.AndroidJNI.NewStringUTF(rationale);
						array[3].z = fetchEmail;
						array[4].z = fetchAccessToken;
						array[5].z = fetchIdToken;
						array[6].l = global::UnityEngine.AndroidJNI.NewStringUTF(scope);
						global::System.IntPtr ptr = global::UnityEngine.AndroidJNI.CallStaticObjectMethod(androidJavaClass.GetRawClass(), staticMethodID, array);
						global::Com.Google.Android.Gms.Common.Api.PendingResult<global::GooglePlayGames.Android.TokenResult> pendingResult = new global::Com.Google.Android.Gms.Common.Api.PendingResult<global::GooglePlayGames.Android.TokenResult>(ptr);
						pendingResult.setResultCallback(new global::GooglePlayGames.Android.TokenResultCallback(callback));
					}
				}
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Exception launching token request: " + ex.Message);
				global::GooglePlayGames.OurUtils.Logger.e(ex.ToString());
			}
			finally
			{
				global::UnityEngine.AndroidJNIHelper.DeleteJNIArgArray(args, array);
			}
		}

		private string GetAccountName(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback)
		{
			if (string.IsNullOrEmpty(accountName))
			{
				if (!fetchingEmail)
				{
					fetchingEmail = true;
					Fetch(idTokenScope, true, false, false, delegate(global::GooglePlayGames.BasicApi.CommonStatusCodes status)
					{
						fetchingEmail = false;
						if (callback != null)
						{
							callback(status, accountName);
						}
					});
				}
			}
			else if (callback != null)
			{
				callback(global::GooglePlayGames.BasicApi.CommonStatusCodes.Success, accountName);
			}
			return accountName;
		}

		public string GetEmail()
		{
			return GetAccountName(null);
		}

		public void GetEmail(global::System.Action<global::GooglePlayGames.BasicApi.CommonStatusCodes, string> callback)
		{
			GetAccountName(callback);
		}

		[global::System.Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public string GetAccessToken()
		{
			if (string.IsNullOrEmpty(accessToken) && !fetchingAccessToken)
			{
				fetchingAccessToken = true;
				Fetch(idTokenScope, false, true, false, delegate
				{
					fetchingAccessToken = false;
				});
			}
			return accessToken;
		}

		[global::System.Obsolete("Use PlayGamesPlatform.GetServerAuthCode()")]
		public void GetIdToken(string serverClientId, global::System.Action<string> idTokenCallback)
		{
			if (string.IsNullOrEmpty(serverClientId))
			{
				if (webClientWarningCount++ % webClientWarningFreq == 0)
				{
					global::GooglePlayGames.OurUtils.Logger.w("serverClientId is empty, cannot get Id Token");
					webClientWarningCount = webClientWarningCount / webClientWarningFreq + 1;
				}
				idTokenCallback(null);
				return;
			}
			string text = "audience:server:client_id:" + serverClientId;
			if (string.IsNullOrEmpty(idToken) || text != idTokenScope)
			{
				if (fetchingIdToken)
				{
					return;
				}
				fetchingIdToken = true;
				idTokenScope = text;
				idTokenCb = idTokenCallback;
				Fetch(idTokenScope, false, false, true, delegate(global::GooglePlayGames.BasicApi.CommonStatusCodes status)
				{
					fetchingIdToken = false;
					if (status == global::GooglePlayGames.BasicApi.CommonStatusCodes.Success)
					{
						idTokenCb(null);
					}
					else
					{
						idTokenCb(idToken);
					}
				});
			}
			else
			{
				idTokenCallback(idToken);
			}
		}
	}
}
