namespace GooglePlayGames.Native.PInvoke
{
	internal class EventManager
	{
		internal class FetchResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus)0;
			}

			internal global::GooglePlayGames.Native.PInvoke.NativeEvent Data()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.NativeEvent(global::GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_GetData(SelfPtr()));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.EventManager.FetchResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.EventManager.FetchResponse(pointer);
			}
		}

		internal class FetchAllResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchAllResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return global::GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_GetStatus(SelfPtr());
			}

			internal global::System.Collections.Generic.List<global::GooglePlayGames.Native.PInvoke.NativeEvent> Data()
			{
				global::System.IntPtr[] source = global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToArray((global::System.IntPtr[] out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_GetData(SelfPtr(), out_arg, out_size));
				return global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Select(source, (global::System.IntPtr ptr) => new global::GooglePlayGames.Native.PInvoke.NativeEvent(ptr)));
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus)0;
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse(pointer);
			}
		}

		private readonly global::GooglePlayGames.Native.PInvoke.GameServices mServices;

		internal EventManager(global::GooglePlayGames.Native.PInvoke.GameServices services)
		{
			mServices = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(services);
		}

		internal void FetchAll(global::GooglePlayGames.Native.Cwrapper.Types.DataSource source, global::System.Action<global::GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAll(mServices.AsHandle(), source, InternalFetchAllCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.EventManager.FetchAllCallback))]
		internal static void InternalFetchAllCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("EventManager#FetchAllCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void Fetch(global::GooglePlayGames.Native.Cwrapper.Types.DataSource source, string eventId, global::System.Action<global::GooglePlayGames.Native.PInvoke.EventManager.FetchResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.EventManager.EventManager_Fetch(mServices.AsHandle(), source, eventId, InternalFetchCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.EventManager.FetchResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.EventManager.FetchCallback))]
		internal static void InternalFetchCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("EventManager#FetchCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void Increment(string eventId, uint steps)
		{
			global::GooglePlayGames.Native.Cwrapper.EventManager.EventManager_Increment(mServices.AsHandle(), eventId, steps);
		}
	}
}
