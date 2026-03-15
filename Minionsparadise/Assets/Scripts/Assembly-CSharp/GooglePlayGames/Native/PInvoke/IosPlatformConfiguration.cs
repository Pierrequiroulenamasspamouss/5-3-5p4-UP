namespace GooglePlayGames.Native.PInvoke
{
	internal sealed class IosPlatformConfiguration : global::GooglePlayGames.Native.PInvoke.PlatformConfiguration
	{
		private IosPlatformConfiguration(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal void SetClientId(string clientId)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(clientId);
			global::GooglePlayGames.Native.Cwrapper.IosPlatformConfiguration.IosPlatformConfiguration_SetClientID(SelfPtr(), clientId);
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.IosPlatformConfiguration.IosPlatformConfiguration_Dispose(selfPointer);
		}

		internal static global::GooglePlayGames.Native.PInvoke.IosPlatformConfiguration Create()
		{
			return new global::GooglePlayGames.Native.PInvoke.IosPlatformConfiguration(global::GooglePlayGames.Native.Cwrapper.IosPlatformConfiguration.IosPlatformConfiguration_Construct());
		}
	}
}
