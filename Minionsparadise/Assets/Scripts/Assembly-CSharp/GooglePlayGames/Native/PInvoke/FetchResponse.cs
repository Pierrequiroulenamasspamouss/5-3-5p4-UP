namespace GooglePlayGames.Native.PInvoke
{
	internal class FetchResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal FetchResponse(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchResponse_Dispose(SelfPtr());
		}

		internal global::GooglePlayGames.Native.PInvoke.NativeLeaderboard Leaderboard()
		{
			return global::GooglePlayGames.Native.PInvoke.NativeLeaderboard.FromPointer(global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchResponse_GetData(SelfPtr()));
		}

		internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus GetStatus()
		{
			return global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchResponse_GetStatus(SelfPtr());
		}

		internal static global::GooglePlayGames.Native.PInvoke.FetchResponse FromPointer(global::System.IntPtr pointer)
		{
			if (pointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.FetchResponse(pointer);
		}
	}
}
