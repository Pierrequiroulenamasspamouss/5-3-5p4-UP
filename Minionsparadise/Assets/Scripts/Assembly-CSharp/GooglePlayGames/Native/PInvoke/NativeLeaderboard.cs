namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeLeaderboard : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeLeaderboard(global::System.IntPtr selfPtr)
			: base(selfPtr)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.Leaderboard.Leaderboard_Dispose(selfPointer);
		}

		internal string Title()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Leaderboard.Leaderboard_Name(SelfPtr(), out_string, out_size));
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeLeaderboard FromPointer(global::System.IntPtr pointer)
		{
			if (pointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeLeaderboard(pointer);
		}
	}
}
