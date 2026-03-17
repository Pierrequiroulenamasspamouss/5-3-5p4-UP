namespace GooglePlayGames.Native.PInvoke
{
	internal class StatsManager
	{
		internal class FetchForPlayerResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchForPlayerResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
			{
				return global::GooglePlayGames.Native.Cwrapper.StatsManager.StatsManager_FetchForPlayerResponse_GetStatus(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.NativePlayerStats PlayerStats()
			{
				global::System.IntPtr selfPointer = global::GooglePlayGames.Native.Cwrapper.StatsManager.StatsManager_FetchForPlayerResponse_GetData(SelfPtr());
				return new global::GooglePlayGames.Native.PInvoke.NativePlayerStats(selfPointer);
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.StatsManager.StatsManager_FetchForPlayerResponse_Dispose(selfPointer);
			}

			internal static global::GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse FromPointer(global::System.IntPtr pointer)
			{
				if (pointer.Equals(global::System.IntPtr.Zero))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse(pointer);
			}
		}

		private readonly global::GooglePlayGames.Native.PInvoke.GameServices mServices;

		internal StatsManager(global::GooglePlayGames.Native.PInvoke.GameServices services)
		{
			mServices = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(services);
		}

		internal void FetchForPlayer(global::System.Action<global::GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			global::GooglePlayGames.Native.Cwrapper.StatsManager.StatsManager_FetchForPlayer(mServices.AsHandle(), global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK, InternalFetchForPlayerCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.StatsManager.FetchForPlayerResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.StatsManager.FetchForPlayerCallback))]
		private static void InternalFetchForPlayerCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("StatsManager#InternalFetchForPlayerCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}
	}
}
