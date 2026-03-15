namespace GooglePlayGames.Native
{
	internal class NativeNearbyConnectionsClient : global::GooglePlayGames.BasicApi.Nearby.INearbyConnectionClient
	{
		protected class OnGameThreadMessageListener : global::GooglePlayGames.BasicApi.Nearby.IMessageListener
		{
			private readonly global::GooglePlayGames.BasicApi.Nearby.IMessageListener mListener;

			public OnGameThreadMessageListener(global::GooglePlayGames.BasicApi.Nearby.IMessageListener listener)
			{
				mListener = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(listener);
			}

			public void OnMessageReceived(string remoteEndpointId, byte[] data, bool isReliableMessage)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnMessageReceived(remoteEndpointId, data, isReliableMessage);
				});
			}

			public void OnRemoteEndpointDisconnected(string remoteEndpointId)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnRemoteEndpointDisconnected(remoteEndpointId);
				});
			}
		}

		protected class OnGameThreadDiscoveryListener : global::GooglePlayGames.BasicApi.Nearby.IDiscoveryListener
		{
			private readonly global::GooglePlayGames.BasicApi.Nearby.IDiscoveryListener mListener;

			public OnGameThreadDiscoveryListener(global::GooglePlayGames.BasicApi.Nearby.IDiscoveryListener listener)
			{
				mListener = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(listener);
			}

			public void OnEndpointFound(global::GooglePlayGames.BasicApi.Nearby.EndpointDetails discoveredEndpoint)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnEndpointFound(discoveredEndpoint);
				});
			}

			public void OnEndpointLost(string lostEndpointId)
			{
				global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
				{
					mListener.OnEndpointLost(lostEndpointId);
				});
			}
		}

		private readonly global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManager mManager;

		internal NativeNearbyConnectionsClient(global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManager manager)
		{
			mManager = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(manager);
		}

		public int MaxUnreliableMessagePayloadLength()
		{
			return 1168;
		}

		public int MaxReliableMessagePayloadLength()
		{
			return 4096;
		}

		public void SendReliable(global::System.Collections.Generic.List<string> recipientEndpointIds, byte[] payload)
		{
			InternalSend(recipientEndpointIds, payload, true);
		}

		public void SendUnreliable(global::System.Collections.Generic.List<string> recipientEndpointIds, byte[] payload)
		{
			InternalSend(recipientEndpointIds, payload, false);
		}

		private void InternalSend(global::System.Collections.Generic.List<string> recipientEndpointIds, byte[] payload, bool isReliable)
		{
			if (recipientEndpointIds == null)
			{
				throw new global::System.ArgumentNullException("recipientEndpointIds");
			}
			if (payload == null)
			{
				throw new global::System.ArgumentNullException("payload");
			}
			if (recipientEndpointIds.Contains(null))
			{
				throw new global::System.InvalidOperationException("Cannot send a message to a null recipient");
			}
			if (recipientEndpointIds.Count == 0)
			{
				global::GooglePlayGames.OurUtils.Logger.w("Attempted to send a reliable message with no recipients");
				return;
			}
			if (isReliable)
			{
				if (payload.Length > MaxReliableMessagePayloadLength())
				{
					throw new global::System.InvalidOperationException("cannot send more than " + MaxReliableMessagePayloadLength() + " bytes");
				}
			}
			else if (payload.Length > MaxUnreliableMessagePayloadLength())
			{
				throw new global::System.InvalidOperationException("cannot send more than " + MaxUnreliableMessagePayloadLength() + " bytes");
			}
			foreach (string recipientEndpointId in recipientEndpointIds)
			{
				if (isReliable)
				{
					mManager.SendReliable(recipientEndpointId, payload);
				}
				else
				{
					mManager.SendUnreliable(recipientEndpointId, payload);
				}
			}
		}

		public void StartAdvertising(string name, global::System.Collections.Generic.List<string> appIdentifiers, global::System.TimeSpan? advertisingDuration, global::System.Action<global::GooglePlayGames.BasicApi.Nearby.AdvertisingResult> resultCallback, global::System.Action<global::GooglePlayGames.BasicApi.Nearby.ConnectionRequest> requestCallback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(appIdentifiers, "appIdentifiers");
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(resultCallback, "resultCallback");
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(requestCallback, "connectionRequestCallback");
			if (advertisingDuration.HasValue && advertisingDuration.Value.Ticks < 0)
			{
				throw new global::System.InvalidOperationException("advertisingDuration must be positive");
			}
			resultCallback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(resultCallback);
			requestCallback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(requestCallback);
			mManager.StartAdvertising(name, global::System.Linq.Enumerable.ToList<global::GooglePlayGames.Native.PInvoke.NativeAppIdentifier>(global::System.Linq.Enumerable.Select<string, global::GooglePlayGames.Native.PInvoke.NativeAppIdentifier>(appIdentifiers, (string id) => global::GooglePlayGames.Native.PInvoke.NativeAppIdentifier.FromString(id))), ToTimeoutMillis(advertisingDuration), delegate(long localClientId, global::GooglePlayGames.Native.PInvoke.NativeStartAdvertisingResult result)
			{
				resultCallback(result.AsResult());
			}, delegate(long localClientId, global::GooglePlayGames.Native.PInvoke.NativeConnectionRequest request)
			{
				requestCallback(request.AsRequest());
			});
		}

		private static long ToTimeoutMillis(global::System.TimeSpan? span)
		{
			return (!span.HasValue) ? 0 : global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.ToMilliseconds(span.Value);
		}

		public void StopAdvertising()
		{
			mManager.StopAdvertising();
		}

		public void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, global::System.Action<global::GooglePlayGames.BasicApi.Nearby.ConnectionResponse> responseCallback, global::GooglePlayGames.BasicApi.Nearby.IMessageListener listener)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(remoteEndpointId, "remoteEndpointId");
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(payload, "payload");
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(responseCallback, "responseCallback");
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(listener, "listener");
			responseCallback = global::GooglePlayGames.Native.PInvoke.Callbacks.AsOnGameThreadCallback(responseCallback);
			using (global::GooglePlayGames.Native.PInvoke.NativeMessageListenerHelper listener2 = ToMessageListener(listener))
			{
				mManager.SendConnectionRequest(name, remoteEndpointId, payload, delegate(long localClientId, global::GooglePlayGames.Native.PInvoke.NativeConnectionResponse response)
				{
					responseCallback(response.AsResponse(localClientId));
				}, listener2);
			}
		}

		private static global::GooglePlayGames.Native.PInvoke.NativeMessageListenerHelper ToMessageListener(global::GooglePlayGames.BasicApi.Nearby.IMessageListener listener)
		{
			listener = new global::GooglePlayGames.Native.NativeNearbyConnectionsClient.OnGameThreadMessageListener(listener);
			global::GooglePlayGames.Native.PInvoke.NativeMessageListenerHelper nativeMessageListenerHelper = new global::GooglePlayGames.Native.PInvoke.NativeMessageListenerHelper();
			nativeMessageListenerHelper.SetOnMessageReceivedCallback(delegate(long localClientId, string endpointId, byte[] data, bool isReliable)
			{
				listener.OnMessageReceived(endpointId, data, isReliable);
			});
			nativeMessageListenerHelper.SetOnDisconnectedCallback(delegate(long localClientId, string endpointId)
			{
				listener.OnRemoteEndpointDisconnected(endpointId);
			});
			return nativeMessageListenerHelper;
		}

		public void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, global::GooglePlayGames.BasicApi.Nearby.IMessageListener listener)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(remoteEndpointId, "remoteEndpointId");
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(payload, "payload");
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(listener, "listener");
			global::GooglePlayGames.OurUtils.Logger.d("Calling AcceptConncectionRequest");
			mManager.AcceptConnectionRequest(remoteEndpointId, payload, ToMessageListener(listener));
			global::GooglePlayGames.OurUtils.Logger.d("Called!");
		}

		public void StartDiscovery(string serviceId, global::System.TimeSpan? advertisingTimeout, global::GooglePlayGames.BasicApi.Nearby.IDiscoveryListener listener)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(serviceId, "serviceId");
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(listener, "listener");
			using (global::GooglePlayGames.Native.PInvoke.NativeEndpointDiscoveryListenerHelper listener2 = ToDiscoveryListener(listener))
			{
				mManager.StartDiscovery(serviceId, ToTimeoutMillis(advertisingTimeout), listener2);
			}
		}

		private static global::GooglePlayGames.Native.PInvoke.NativeEndpointDiscoveryListenerHelper ToDiscoveryListener(global::GooglePlayGames.BasicApi.Nearby.IDiscoveryListener listener)
		{
			listener = new global::GooglePlayGames.Native.NativeNearbyConnectionsClient.OnGameThreadDiscoveryListener(listener);
			global::GooglePlayGames.Native.PInvoke.NativeEndpointDiscoveryListenerHelper nativeEndpointDiscoveryListenerHelper = new global::GooglePlayGames.Native.PInvoke.NativeEndpointDiscoveryListenerHelper();
			nativeEndpointDiscoveryListenerHelper.SetOnEndpointFound(delegate(long localClientId, global::GooglePlayGames.Native.PInvoke.NativeEndpointDetails endpoint)
			{
				listener.OnEndpointFound(endpoint.ToDetails());
			});
			nativeEndpointDiscoveryListenerHelper.SetOnEndpointLostCallback(delegate(long localClientId, string lostEndpointId)
			{
				listener.OnEndpointLost(lostEndpointId);
			});
			return nativeEndpointDiscoveryListenerHelper;
		}

		public void StopDiscovery(string serviceId)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(serviceId, "serviceId");
			mManager.StopDiscovery(serviceId);
		}

		public void RejectConnectionRequest(string requestingEndpointId)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(requestingEndpointId, "requestingEndpointId");
			mManager.RejectConnectionRequest(requestingEndpointId);
		}

		public void DisconnectFromEndpoint(string remoteEndpointId)
		{
			mManager.DisconnectFromEndpoint(remoteEndpointId);
		}

		public void StopAllConnections()
		{
			mManager.StopAllConnections();
		}

		public string LocalEndpointId()
		{
			return mManager.LocalEndpointId();
		}

		public string LocalDeviceId()
		{
			return mManager.LocalDeviceId();
		}

		public string GetAppBundleId()
		{
			return mManager.AppBundleId;
		}

		public string GetServiceId()
		{
			return global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManager.ServiceId;
		}
	}
}
