namespace GooglePlayGames.Native.PInvoke
{
	internal class NearbyConnectionsManagerBuilder : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NearbyConnectionsManagerBuilder()
			: base(global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsBuilder.NearbyConnections_Builder_Construct())
		{
		}

		internal global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManagerBuilder SetOnInitializationFinished(global::System.Action<global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus> callback)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsBuilder.NearbyConnections_Builder_SetOnInitializationFinished(SelfPtr(), InternalOnInitializationFinishedCallback, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(callback));
			return this;
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsBuilder.OnInitializationFinishedCallback))]
		private static void InternalOnInitializationFinishedCallback(global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus status, global::System.IntPtr userData)
		{
			global::System.Action<global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus> action = global::GooglePlayGames.Native.PInvoke.Callbacks.IntPtrToPermanentCallback<global::System.Action<global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsStatus.InitializationStatus>>(userData);
			if (action == null)
			{
				global::GooglePlayGames.OurUtils.Logger.w("Callback for Initialization is null. Received status: " + status);
				return;
			}
			try
			{
				action(status);
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing NearbyConnectionsManagerBuilder#InternalOnInitializationFinishedCallback. Smothering exception: " + ex);
			}
		}

		internal global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManagerBuilder SetLocalClientId(long localClientId)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsBuilder.NearbyConnections_Builder_SetClientId(SelfPtr(), localClientId);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManagerBuilder SetDefaultLogLevel(global::GooglePlayGames.Native.Cwrapper.Types.LogLevel minLevel)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsBuilder.NearbyConnections_Builder_SetDefaultOnLog(SelfPtr(), minLevel);
			return this;
		}

		internal global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManager Build(global::GooglePlayGames.Native.PInvoke.PlatformConfiguration configuration)
		{
			return new global::GooglePlayGames.Native.PInvoke.NearbyConnectionsManager(global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsBuilder.NearbyConnections_Builder_Create(SelfPtr(), configuration.AsPointer()));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnectionsBuilder.NearbyConnections_Builder_Dispose(selfPointer);
		}
	}
}
