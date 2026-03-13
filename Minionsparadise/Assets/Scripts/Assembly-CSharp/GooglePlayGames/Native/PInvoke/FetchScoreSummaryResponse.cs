namespace GooglePlayGames.Native.PInvoke
{
	internal class FetchScoreSummaryResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal FetchScoreSummaryResponse(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScoreSummaryResponse_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus GetStatus()
		{
			return global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScoreSummaryResponse_GetStatus(SelfPtr());
		}

		internal global::GooglePlayGames.Native.PInvoke.NativeScoreSummary GetScoreSummary()
		{
			return global::GooglePlayGames.Native.PInvoke.NativeScoreSummary.FromPointer(global::GooglePlayGames.Native.Cwrapper.LeaderboardManager.LeaderboardManager_FetchScoreSummaryResponse_GetData(SelfPtr()));
		}

		internal static global::GooglePlayGames.Native.PInvoke.FetchScoreSummaryResponse FromPointer(global::System.IntPtr pointer)
		{
			if (pointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.FetchScoreSummaryResponse(pointer);
		}
	}
}
