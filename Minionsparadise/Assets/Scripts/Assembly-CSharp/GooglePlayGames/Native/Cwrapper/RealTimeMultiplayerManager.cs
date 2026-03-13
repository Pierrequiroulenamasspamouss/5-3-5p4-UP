namespace GooglePlayGames.Native.Cwrapper
{
	internal static class RealTimeMultiplayerManager
	{
		internal delegate void RealTimeRoomCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void LeaveRoomCallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus arg0, global::System.IntPtr arg1);

		internal delegate void SendReliableMessageCallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus arg0, global::System.IntPtr arg1);

		internal delegate void RoomInboxUICallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void PlayerSelectUICallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void WaitingRoomUICallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void FetchInvitationsCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_CreateRealTimeRoom(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr config, global::System.IntPtr helper, global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeRoomCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_LeaveRoom(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr room, global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.LeaveRoomCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_SendUnreliableMessage(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr room, global::System.IntPtr[] participants, global::System.UIntPtr participants_size, byte[] data, global::System.UIntPtr data_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_ShowWaitingRoomUI(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr room, uint min_participants_to_start, global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.WaitingRoomUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_ShowPlayerSelectUI(global::System.Runtime.InteropServices.HandleRef self, uint minimum_players, uint maximum_players, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool allow_automatch, global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.PlayerSelectUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_DismissInvitation(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr invitation);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_DeclineInvitation(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr invitation);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_SendReliableMessage(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr room, global::System.IntPtr participant, byte[] data, global::System.UIntPtr data_size, global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.SendReliableMessageCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_AcceptInvitation(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr invitation, global::System.IntPtr helper, global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeRoomCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_FetchInvitations(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.FetchInvitationsCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_SendUnreliableMessageToOthers(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr room, byte[] data, global::System.UIntPtr data_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_ShowRoomInboxUI(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RoomInboxUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_RealTimeRoomResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus RealTimeMultiplayerManager_RealTimeRoomResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr RealTimeMultiplayerManager_RealTimeRoomResponse_GetRoom(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_RoomInboxUIResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus RealTimeMultiplayerManager_RoomInboxUIResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr RealTimeMultiplayerManager_RoomInboxUIResponse_GetInvitation(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_WaitingRoomUIResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus RealTimeMultiplayerManager_WaitingRoomUIResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr RealTimeMultiplayerManager_WaitingRoomUIResponse_GetRoom(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void RealTimeMultiplayerManager_FetchInvitationsResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus RealTimeMultiplayerManager_FetchInvitationsResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);
	}
}
