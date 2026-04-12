namespace GooglePlayGames.Native.PInvoke
{
	internal class GameServices : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal class FetchServerAuthCodeResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchServerAuthCodeResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
			{
				return global::GooglePlayGames.Native.Cwrapper.GameServices.GameServices_FetchServerAuthCodeResponse_GetStatus(SelfPtr());
			}

			internal string Code()
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.GameServices.GameServices_FetchServerAuthCodeResponse_GetCode(SelfPtr(), out_string, out_size));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.GameServices.GameServices_FetchServerAuthCodeResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse(pointer);
			}
		}

		internal GameServices(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal bool IsAuthenticated()
		{
			return global::GooglePlayGames.Native.Cwrapper.GameServices.GameServices_IsAuthorized(SelfPtr());
		}

		internal void SignOut()
		{
			global::GooglePlayGames.Native.Cwrapper.GameServices.GameServices_SignOut(SelfPtr());
		}

		internal void StartAuthorizationUI()
		{
			global::GooglePlayGames.Native.Cwrapper.GameServices.GameServices_StartAuthorizationUI(SelfPtr());
		}

		public global::GooglePlayGames.Native.PInvoke.AchievementManager AchievementManager()
		{
			return new global::GooglePlayGames.Native.PInvoke.AchievementManager(this);
		}

		public global::GooglePlayGames.Native.PInvoke.LeaderboardManager LeaderboardManager()
		{
			return new global::GooglePlayGames.Native.PInvoke.LeaderboardManager(this);
		}

		public global::GooglePlayGames.Native.PInvoke.PlayerManager PlayerManager()
		{
			return new global::GooglePlayGames.Native.PInvoke.PlayerManager(this);
		}

		public global::GooglePlayGames.Native.PInvoke.StatsManager StatsManager()
		{
			return new global::GooglePlayGames.Native.PInvoke.StatsManager(this);
		}

		internal global::System.Runtime.InteropServices.HandleRef AsHandle()
		{
			return SelfPtr();
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.GameServices.GameServices_Dispose(selfPointer);
		}

		internal void FetchServerAuthCode(string server_client_id, global::System.Action<global::GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(server_client_id);
			global::GooglePlayGames.Native.Cwrapper.GameServices.GameServices_FetchServerAuthCode(AsHandle(), server_client_id, InternalFetchServerAuthCodeCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.GameServices.FetchServerAuthCodeResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.GameServices.FetchServerAuthCodeCallback))]
		private static void InternalFetchServerAuthCodeCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("GameServices#InternalFetchServerAuthCodeCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}
	}
}
