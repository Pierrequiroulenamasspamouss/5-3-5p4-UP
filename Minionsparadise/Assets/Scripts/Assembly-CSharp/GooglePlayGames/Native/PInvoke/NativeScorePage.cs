namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeScorePage : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeScorePage(global::System.IntPtr selfPtr)
			: base(selfPtr)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardCollection GetCollection()
		{
			return global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Collection(SelfPtr());
		}

		private global::System.UIntPtr Length()
		{
			return global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Entries_Length(SelfPtr());
		}

		private global::GooglePlayGames.Native.PInvoke.NativeScoreEntry GetElement(global::System.UIntPtr index)
		{
			if (index.ToUInt64() >= Length().ToUInt64())
			{
				throw new global::System.ArgumentOutOfRangeException();
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeScoreEntry(global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Entries_GetElement(SelfPtr(), index));
		}

		public global::System.Collections.Generic.IEnumerator<global::GooglePlayGames.Native.PInvoke.NativeScoreEntry> GetEnumerator()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerator<global::GooglePlayGames.Native.PInvoke.NativeScoreEntry>(global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Entries_Length(SelfPtr()), (global::System.UIntPtr index) => GetElement(index));
		}

		internal bool HasNextScorePage()
		{
			return global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_HasNextScorePage(SelfPtr());
		}

		internal bool HasPrevScorePage()
		{
			return global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_HasPreviousScorePage(SelfPtr());
		}

		internal global::GooglePlayGames.Native.PInvoke.NativeScorePageToken GetNextScorePageToken()
		{
			return new global::GooglePlayGames.Native.PInvoke.NativeScorePageToken(global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_NextScorePageToken(SelfPtr()));
		}

		internal global::GooglePlayGames.Native.PInvoke.NativeScorePageToken GetPreviousScorePageToken()
		{
			return new global::GooglePlayGames.Native.PInvoke.NativeScorePageToken(global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_PreviousScorePageToken(SelfPtr()));
		}

		internal bool Valid()
		{
			return global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Valid(SelfPtr());
		}

		internal global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardTimeSpan GetTimeSpan()
		{
			return global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_TimeSpan(SelfPtr());
		}

		internal global::GooglePlayGames.Native.Cwrapper.Types.LeaderboardStart GetStart()
		{
			return global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Start(SelfPtr());
		}

		internal string GetLeaderboardId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_LeaderboardId(SelfPtr(), out_string, out_size));
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeScorePage FromPointer(global::System.IntPtr pointer)
		{
			if (pointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeScorePage(pointer);
		}
	}
}
