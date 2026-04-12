namespace GooglePlayGames.Native.PInvoke
{
	internal class RealtimeManager
	{
		internal class RealTimeRoomResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal RealTimeRoomResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus)0;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom Room()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_GetRoom(SelfPtr()));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse(pointer);
			}
		}

		internal class RoomInboxUIResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal RoomInboxUIResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_GetStatus(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation Invitation()
			{
				if (ResponseStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_GetInvitation(SelfPtr()));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.RealtimeManager.RoomInboxUIResponse FromPointer(global::System.IntPtr pointer)
			{
				if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.RealtimeManager.RoomInboxUIResponse(pointer);
			}
		}

		internal class WaitingRoomUIResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal WaitingRoomUIResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_GetStatus(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom Room()
			{
				if (ResponseStatus() != global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus.VALID)
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_GetRoom(SelfPtr()));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.RealtimeManager.WaitingRoomUIResponse FromPointer(global::System.IntPtr pointer)
			{
				if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.RealtimeManager.WaitingRoomUIResponse(pointer);
			}
		}

		internal class FetchInvitationsResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchInvitationsResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus)0;
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetStatus(SelfPtr());
			}

			internal global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation> Invitations()
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerable(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_Length(SelfPtr()), (global::System.UIntPtr index) => new global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_GetElement(SelfPtr(), index)));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.RealtimeManager.FetchInvitationsResponse FromPointer(global::System.IntPtr pointer)
			{
				if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.RealtimeManager.FetchInvitationsResponse(pointer);
			}
		}

		private readonly global::GooglePlayGames.Native.PInvoke.GameServices mGameServices;

		internal RealtimeManager(global::GooglePlayGames.Native.PInvoke.GameServices gameServices)
		{
			mGameServices = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(gameServices);
		}

		internal void CreateRoom(global::GooglePlayGames.Native.PInvoke.RealtimeRoomConfig config, global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper helper, global::System.Action<global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_CreateRealTimeRoom(mGameServices.AsHandle(), config.AsPointer(), helper.AsPointer(), InternalRealTimeRoomCallback, ToCallbackPointer(callback));
		}

		internal void ShowPlayerSelectUI(uint minimumPlayers, uint maxiumPlayers, bool allowAutomatching, global::System.Action<global::GooglePlayGames.Native.PInvoke.PlayerSelectUIResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowPlayerSelectUI(mGameServices.AsHandle(), minimumPlayers, maxiumPlayers, allowAutomatching, InternalPlayerSelectUIcallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.PlayerSelectUIResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.PlayerSelectUICallback))]
		internal static void InternalPlayerSelectUIcallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("RealtimeManager#PlayerSelectUICallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeRoomCallback))]
		internal static void InternalRealTimeRoomCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("RealtimeManager#InternalRealTimeRoomCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RoomInboxUICallback))]
		internal static void InternalRoomInboxUICallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("RealtimeManager#InternalRoomInboxUICallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void ShowRoomInboxUI(global::System.Action<global::GooglePlayGames.Native.PInvoke.RealtimeManager.RoomInboxUIResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowRoomInboxUI(mGameServices.AsHandle(), InternalRoomInboxUICallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.RealtimeManager.RoomInboxUIResponse.FromPointer));
		}

		internal void ShowWaitingRoomUI(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, uint minimumParticipantsBeforeStarting, global::System.Action<global::GooglePlayGames.Native.PInvoke.RealtimeManager.WaitingRoomUIResponse> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(room);
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowWaitingRoomUI(mGameServices.AsHandle(), room.AsPointer(), minimumParticipantsBeforeStarting, InternalWaitingRoomUICallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.RealtimeManager.WaitingRoomUIResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.WaitingRoomUICallback))]
		internal static void InternalWaitingRoomUICallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("RealtimeManager#InternalWaitingRoomUICallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.FetchInvitationsCallback))]
		internal static void InternalFetchInvitationsCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("RealtimeManager#InternalFetchInvitationsCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void FetchInvitations(global::System.Action<global::GooglePlayGames.Native.PInvoke.RealtimeManager.FetchInvitationsResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitations(mGameServices.AsHandle(), InternalFetchInvitationsCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.RealtimeManager.FetchInvitationsResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.LeaveRoomCallback))]
		internal static void InternalLeaveRoomCallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus response, global::System.IntPtr data)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Entering internal callback for InternalLeaveRoomCallback");
			global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus> action = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToTempCallback<global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus>>(data);
			if (action == null)
			{
				return;
			}
			try
			{
				action(response);
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing InternalLeaveRoomCallback. Smothering to avoid passing exception into Native: " + ex);
			}
		}

		internal void LeaveRoom(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_LeaveRoom(mGameServices.AsHandle(), room.AsPointer(), InternalLeaveRoomCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		internal void AcceptInvitation(global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation, global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper listener, global::System.Action<global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_AcceptInvitation(mGameServices.AsHandle(), invitation.AsPointer(), listener.AsPointer(), InternalRealTimeRoomCallback, ToCallbackPointer(callback));
		}

		internal void DeclineInvitation(global::GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_DeclineInvitation(mGameServices.AsHandle(), invitation.AsPointer());
		}

		internal void SendReliableMessage(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant, byte[] data, global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendReliableMessage(mGameServices.AsHandle(), room.AsPointer(), participant.AsPointer(), data, global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ArrayToSizeT(data), InternalSendReliableMessageCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.SendReliableMessageCallback))]
		internal static void InternalSendReliableMessageCallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus response, global::System.IntPtr data)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Entering internal callback for InternalSendReliableMessageCallback " + response);
			global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus> action = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToTempCallback<global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.MultiplayerStatus>>(data);
			if (action == null)
			{
				return;
			}
			try
			{
				action(response);
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing InternalSendReliableMessageCallback. Smothering to avoid passing exception into Native: " + ex);
			}
		}

		internal void SendUnreliableMessageToAll(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, byte[] data)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendUnreliableMessageToOthers(mGameServices.AsHandle(), room.AsPointer(), data, global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ArrayToSizeT(data));
		}

		internal void SendUnreliableMessageToSpecificParticipants(global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom room, global::System.Collections.Generic.List<global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant> recipients, byte[] data)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendUnreliableMessage(mGameServices.AsHandle(), room.AsPointer(), global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.Select(recipients, (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant r) => r.AsPointer())), new global::System.UIntPtr((ulong)global::System.Linq.Enumerable.LongCount(recipients)), data, global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ArrayToSizeT(data));
		}

		private static global::System.IntPtr ToCallbackPointer(global::System.Action<global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse> callback)
		{
			return global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.RealtimeManager.RealTimeRoomResponse.FromPointer);
		}
	}
}
