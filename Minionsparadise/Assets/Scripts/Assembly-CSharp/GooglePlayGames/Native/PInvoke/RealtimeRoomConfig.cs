namespace GooglePlayGames.Native.PInvoke
{
	internal class RealtimeRoomConfig : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal RealtimeRoomConfig(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeRoomConfig.RealTimeRoomConfig_Dispose(selfPointer);
		}

		internal static global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfig FromPointer(global::System.IntPtr selfPointer)
		{
			if (selfPointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfig(selfPointer);
		}
	}
}
