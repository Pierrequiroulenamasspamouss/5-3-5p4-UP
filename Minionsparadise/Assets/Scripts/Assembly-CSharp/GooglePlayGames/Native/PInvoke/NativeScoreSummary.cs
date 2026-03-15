namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeScoreSummary : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeScoreSummary(global::System.IntPtr selfPtr)
			: base(selfPtr)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.ScoreSummary.ScoreSummary_Dispose(selfPointer);
		}

		internal ulong ApproximateResults()
		{
			return global::GooglePlayGames.Native.Cwrapper.ScoreSummary.ScoreSummary_ApproximateNumberOfScores(SelfPtr());
		}

		internal global::GooglePlayGames.Native.PInvoke.NativeScore LocalUserScore()
		{
			return global::GooglePlayGames.Native.PInvoke.NativeScore.FromPointer(global::GooglePlayGames.Native.Cwrapper.ScoreSummary.ScoreSummary_CurrentPlayerScore(SelfPtr()));
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeScoreSummary FromPointer(global::System.IntPtr pointer)
		{
			if (pointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeScoreSummary(pointer);
		}
	}
}
