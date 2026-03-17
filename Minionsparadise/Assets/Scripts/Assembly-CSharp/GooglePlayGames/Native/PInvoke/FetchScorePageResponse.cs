namespace GooglePlayGames.Native.PInvoke
{
	internal class FetchScorePageResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal FetchScorePageResponse(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScorePageResponse_Dispose(SelfPtr());
		}

		internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus GetStatus()
		{
			return global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScorePageResponse_GetStatus(SelfPtr());
		}

		internal global::GooglePlayGames.Native.PInvoke.NativeScorePage GetScorePage()
		{
			return global::GooglePlayGames.Native.PInvoke.NativeScorePage.FromPointer(global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScorePageResponse_GetData(SelfPtr()));
		}

		internal static global::GooglePlayGames.Native.PInvoke.FetchScorePageResponse FromPointer(global::System.IntPtr pointer)
		{
			if (pointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.FetchScorePageResponse(pointer);
		}
	}
}
