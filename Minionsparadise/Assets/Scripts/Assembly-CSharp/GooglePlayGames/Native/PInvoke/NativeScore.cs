namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeScore : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		private const ulong MinusOne = ulong.MaxValue;

		internal NativeScore(global::System.IntPtr selfPtr)
			: base(selfPtr)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.Score.Score_Dispose(SelfPtr());
		}

		internal ulong GetDate()
		{
			return ulong.MaxValue;
		}

		internal string GetMetadata()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Score.Score_Metadata(SelfPtr(), out_string, out_size));
		}

		internal ulong GetRank()
		{
			return global::GooglePlayGames.Native.Cwrapper.Score.Score_Rank(SelfPtr());
		}

		internal ulong GetValue()
		{
			return global::GooglePlayGames.Native.Cwrapper.Score.Score_Value(SelfPtr());
		}

		internal global::GooglePlayGames.PlayGamesScore AsScore(string leaderboardId, string selfPlayerId)
		{
			global::System.DateTime dateTime = new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0, global::System.DateTimeKind.Utc);
			ulong num = GetDate();
			if (num == ulong.MaxValue)
			{
				num = 0uL;
			}
			global::System.DateTime date = dateTime.AddMilliseconds(num);
			return new global::GooglePlayGames.PlayGamesScore(date, leaderboardId, GetRank(), selfPlayerId, GetValue(), GetMetadata());
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeScore FromPointer(global::System.IntPtr pointer)
		{
			if (pointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeScore(pointer);
		}
	}
}
