namespace GooglePlayGames.Native.PInvoke
{
	internal class RealTimeEventListenerHelper : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal RealTimeEventListenerHelper(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnRoomStatusChangedCallback(global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnRoomStatusChangedCallback(SelfPtr(), InternalOnRoomStatusChangedCallback, ToCallbackPointer(callback));
			return this;
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnRoomStatusChangedCallback))]
		internal static void InternalOnRoomStatusChangedCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("RealTimeEventListenerHelper#InternalOnRoomStatusChangedCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Permanent, response, data);
		}

		internal global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnRoomConnectedSetChangedCallback(global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnRoomConnectedSetChangedCallback(SelfPtr(), InternalOnRoomConnectedSetChangedCallback, ToCallbackPointer(callback));
			return this;
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnRoomConnectedSetChangedCallback))]
		internal static void InternalOnRoomConnectedSetChangedCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("RealTimeEventListenerHelper#InternalOnRoomConnectedSetChangedCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Permanent, response, data);
		}

		internal global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnP2PConnectedCallback(global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnP2PConnectedCallback(SelfPtr(), InternalOnP2PConnectedCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
			return this;
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnP2PConnectedCallback))]
		internal static void InternalOnP2PConnectedCallback(global::System.IntPtr room, global::System.IntPtr participant, global::System.IntPtr data)
		{
			PerformRoomAndParticipantCallback("InternalOnP2PConnectedCallback", room, participant, data);
		}

		internal global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnP2PDisconnectedCallback(global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnP2PDisconnectedCallback(SelfPtr(), InternalOnP2PDisconnectedCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
			return this;
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnP2PDisconnectedCallback))]
		internal static void InternalOnP2PDisconnectedCallback(global::System.IntPtr room, global::System.IntPtr participant, global::System.IntPtr data)
		{
			PerformRoomAndParticipantCallback("InternalOnP2PDisconnectedCallback", room, participant, data);
		}

		internal global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnParticipantStatusChangedCallback(global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnParticipantStatusChangedCallback(SelfPtr(), InternalOnParticipantStatusChangedCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
			return this;
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnParticipantStatusChangedCallback))]
		internal static void InternalOnParticipantStatusChangedCallback(global::System.IntPtr room, global::System.IntPtr participant, global::System.IntPtr data)
		{
			PerformRoomAndParticipantCallback("InternalOnParticipantStatusChangedCallback", room, participant, data);
		}

		internal static void PerformRoomAndParticipantCallback(string callbackName, global::System.IntPtr room, global::System.IntPtr participant, global::System.IntPtr data)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Entering " + callbackName);
			try
			{
				global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom arg = global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom.FromPointer(room);
				using (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant arg2 = global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant.FromPointer(participant))
				{
					global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant> action = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToPermanentCallback<global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant>>(data);
					if (action != null)
					{
						action(arg, arg2);
					}
				}
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing " + callbackName + ". Smothering to avoid passing exception into Native: " + ex);
			}
		}

		internal global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper SetOnDataReceivedCallback(global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant, byte[], bool> callback)
		{
			global::System.IntPtr callback_arg = global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback);
			global::GooglePlayGames.OurUtils.Logger.d("OnData Callback has addr: " + callback_arg.ToInt64());
			global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_SetOnDataReceivedCallback(SelfPtr(), InternalOnDataReceived, callback_arg);
			return this;
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.OnDataReceivedCallback))]
		internal static void InternalOnDataReceived(global::System.IntPtr room, global::System.IntPtr participant, global::System.IntPtr data, global::System.UIntPtr dataLength, bool isReliable, global::System.IntPtr userData)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Entering InternalOnDataReceived: " + userData.ToInt64());
			global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant, byte[], bool> action = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToPermanentCallback<global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom, global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant, byte[], bool>>(userData);
			using (global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom arg = global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom.FromPointer(room))
			{
				using (global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant arg2 = global::GooglePlayGames.Native.PInvoke.MultiplayerParticipant.FromPointer(participant))
				{
					if (action == null)
					{
						return;
					}
					byte[] array = null;
					if (dataLength.ToUInt64() != 0L)
					{
						array = new byte[dataLength.ToUInt32()];
						global::System.Runtime.InteropServices.Marshal.Copy(data, array, 0, (int)dataLength.ToUInt32());
					}
					try
					{
						action(arg, arg2, array, isReliable);
					}
					catch (global::System.Exception ex)
					{
						global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing InternalOnDataReceived. Smothering to avoid passing exception into Native: " + ex);
					}
				}
			}
		}

		private static global::System.IntPtr ToCallbackPointer(global::System.Action<global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom> callback)
		{
			global::System.Action<global::System.IntPtr> callback2 = delegate(global::System.IntPtr result)
			{
				global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom nativeRealTimeRoom = global::GooglePlayGames.Native.PInvoke.NativeRealTimeRoom.FromPointer(result);
				if (callback != null)
				{
					callback(nativeRealTimeRoom);
				}
				else if (nativeRealTimeRoom != null)
				{
					nativeRealTimeRoom.Dispose();
				}
			};
			return global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback2);
		}

		internal static global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper Create()
		{
			return new global::GooglePlayGames.Native.PInvoke.RealTimeEventListenerHelper(global::GooglePlayGames.Native.Cwrapper.RealTimeEventListenerHelper.RealTimeEventListenerHelper_Construct());
		}
	}
}
