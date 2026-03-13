namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeRealTimeRoom : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeRealTimeRoom(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal string Id()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr size) => global::GooglePlayGames.Native.Cwrapper.RealTimeRoom.RealTimeRoom_Id(SelfPtr(), out_string, size));
		}

		internal global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant> Participants()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerable(global::GooglePlayGames.Native.Cwrapper.RealTimeRoom.RealTimeRoom_Participants_Length(SelfPtr()), (global::System.UIntPtr index) => new global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant(global::GooglePlayGames.Native.Cwrapper.RealTimeRoom.RealTimeRoom_Participants_GetElement(SelfPtr(), index)));
		}

		internal uint ParticipantCount()
		{
			return global::GooglePlayGames.Native.Cwrapper.RealTimeRoom.RealTimeRoom_Participants_Length(SelfPtr()).ToUInt32();
		}

		internal global::GooglePlayGames.Native.Cwrapper.Types.RealTimeRoomStatus Status()
		{
			return global::GooglePlayGames.Native.Cwrapper.RealTimeRoom.RealTimeRoom_Status(SelfPtr());
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeRoom.RealTimeRoom_Dispose(selfPointer);
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom FromPointer(global::System.IntPtr selfPointer)
		{
			if (selfPointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom(selfPointer);
		}
	}
}
