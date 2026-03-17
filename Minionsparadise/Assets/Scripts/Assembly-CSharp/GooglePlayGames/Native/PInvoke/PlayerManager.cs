namespace GooglePlayGames.Native.PInvoke
{
	internal class PlayerManager
	{
		internal class FetchListResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerable<global::GooglePlayGames.Native.PInvoke.NativePlayer>
		{
			internal FetchListResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_Dispose(SelfPtr());
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
			{
				return global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetStatus(SelfPtr());
			}

			public global::System.Collections.Generic.IEnumerator<global::GooglePlayGames.Native.PInvoke.NativePlayer> GetEnumerator()
			{
				return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToEnumerator<global::GooglePlayGames.Native.PInvoke.NativePlayer>(Length(), (global::System.UIntPtr index) => GetElement(index));
			}

			internal global::System.UIntPtr Length()
			{
				return global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetData_Length(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.NativePlayer GetElement(global::System.UIntPtr index)
			{
				if (index.ToUInt64() >= Length().ToUInt64())
				{
					throw new global::System.ArgumentOutOfRangeException();
				}
				return new global::GooglePlayGames.Native.PInvoke.NativePlayer(global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchListResponse_GetData_GetElement(SelfPtr(), index));
			}

			internal static global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse FromPointer(global::System.IntPtr selfPointer)
			{
				if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(selfPointer))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse(selfPointer);
			}
		}

		internal class FetchResponseCollector
		{
			internal int pendingCount;

			internal global::System.Collections.Generic.List<global::GooglePlayGames.Native.PInvoke.NativePlayer> results = new global::System.Collections.Generic.List<global::GooglePlayGames.Native.PInvoke.NativePlayer>();

			internal global::System.Action<global::GooglePlayGames.Native.PInvoke.NativePlayer[]> callback;
		}

		internal class FetchResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_Dispose(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.NativePlayer GetPlayer()
			{
				return new global::GooglePlayGames.Native.PInvoke.NativePlayer(global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_GetData(SelfPtr()));
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
			{
				return global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchResponse_GetStatus(SelfPtr());
			}

			internal static global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse FromPointer(global::System.IntPtr selfPointer)
			{
				if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(selfPointer))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse(selfPointer);
			}
		}

		internal class FetchSelfResponse : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			internal FetchSelfResponse(global::System.IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus Status()
			{
				return global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_GetStatus(SelfPtr());
			}

			internal global::GooglePlayGames.Native.PInvoke.NativePlayer Self()
			{
				return new global::GooglePlayGames.Native.PInvoke.NativePlayer(global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_GetData(SelfPtr()));
			}

			protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
			{
				global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelfResponse_Dispose(SelfPtr());
			}

			internal static global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse FromPointer(global::System.IntPtr selfPointer)
			{
				if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(selfPointer))
				{
					return null;
				}
				return new global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse(selfPointer);
			}
		}

		private readonly global::GooglePlayGames.Native.PInvoke.GameServices mGameServices;

		internal PlayerManager(global::GooglePlayGames.Native.PInvoke.GameServices services)
		{
			mGameServices = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(services);
		}

		internal void FetchSelf(global::System.Action<global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchSelf(mGameServices.AsHandle(), global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK, InternalFetchSelfCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback, global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.PlayerManager.FetchSelfCallback))]
		private static void InternalFetchSelfCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("PlayerManager#InternalFetchSelfCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void FetchList(string[] userIds, global::System.Action<global::GooglePlayGames.Native.PInvoke.NativePlayer[]> callback)
		{
			global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponseCollector coll = new global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponseCollector();
			coll.pendingCount = userIds.Length;
			coll.callback = callback;
			foreach (string player_id in userIds)
			{
				global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_Fetch(mGameServices.AsHandle(), global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK, player_id, InternalFetchCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(delegate(global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse rsp)
				{
					HandleFetchResponse(coll, rsp);
				}, global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse.FromPointer));
			}
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.PlayerManager.FetchCallback))]
		private static void InternalFetchCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("PlayerManager#InternalFetchCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void HandleFetchResponse(global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponseCollector collector, global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchResponse resp)
		{
			if (resp.Status() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID || resp.Status() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				global::GooglePlayGames.Native.PInvoke.NativePlayer player = resp.GetPlayer();
				collector.results.Add(player);
			}
			collector.pendingCount--;
			if (collector.pendingCount == 0)
			{
				collector.callback(collector.results.ToArray());
			}
		}

		internal void FetchFriends(global::System.Action<global::GooglePlayGames.BasicApi.ResponseStatus, global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Player>> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.PlayerManager.PlayerManager_FetchConnected(mGameServices.AsHandle(), global::GooglePlayGames.Native.Cwrapper.Types.DataSource.CACHE_OR_NETWORK, InternalFetchConnectedCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(delegate(global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse rsp)
			{
				HandleFetchCollected(rsp, callback);
			}, global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse.FromPointer));
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.PlayerManager.FetchListCallback))]
		private static void InternalFetchConnectedCallback(global::System.IntPtr response, global::System.IntPtr data)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("PlayerManager#InternalFetchConnectedCallback", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Temporary, response, data);
		}

		internal void HandleFetchCollected(global::GooglePlayGames.Native.PInvoke.PlayerManager.FetchListResponse rsp, global::System.Action<global::GooglePlayGames.BasicApi.ResponseStatus, global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Player>> callback)
		{
			global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Player> list = new global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Multiplayer.Player>();
			if (rsp.Status() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID || rsp.Status() == global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
			{
				global::GooglePlayGames.OurUtils.Logger.d("Got " + rsp.Length().ToUInt64() + " players");
				foreach (global::GooglePlayGames.Native.PInvoke.NativePlayer item in rsp)
				{
					list.Add(item.AsPlayer());
				}
			}
			callback((global::GooglePlayGames.BasicApi.ResponseStatus)rsp.Status(), list);
		}
	}
}
