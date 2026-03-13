namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeScoreEntry : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		private const ulong MinusOne = ulong.MaxValue;

		internal NativeScoreEntry(global::System.IntPtr selfPtr)
			: base(selfPtr)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Entry_Dispose(selfPointer);
		}

		internal ulong GetLastModifiedTime()
		{
			return global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Entry_LastModifiedTime(SelfPtr());
		}

		internal string GetPlayerId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Entry_PlayerId(SelfPtr(), out_string, out_size));
		}

		internal global::GooglePlayGames.Native.PInvoke.NativeScore GetScore()
		{
			return new global::GooglePlayGames.Native.PInvoke.NativeScore(global::GooglePlayGames.Native.Cwrapper.ScorePage.ScorePage_Entry_Score(SelfPtr()));
		}

		internal global::GooglePlayGames.PlayGamesScore AsScore(string leaderboardId)
		{
			global::System.DateTime dateTime = new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0, global::System.DateTimeKind.Utc);
			ulong num = GetLastModifiedTime();
			if (num == ulong.MaxValue)
			{
				num = 0uL;
			}
			global::System.DateTime date = dateTime.AddMilliseconds(num);
			return new global::GooglePlayGames.PlayGamesScore(date, leaderboardId, GetScore().GetRank(), GetPlayerId(), GetScore().GetValue(), GetScore().GetMetadata());
		}
	}
}
