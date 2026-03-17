namespace GooglePlayGames.Native.PInvoke
{
	internal sealed class AndroidPlatformConfiguration : global::GooglePlayGames.Native.PInvoke.PlatformConfiguration
	{
		private delegate void IntentHandlerInternal(global::System.IntPtr intent, global::System.IntPtr userData);

		private AndroidPlatformConfiguration(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal void SetActivity(global::System.IntPtr activity)
		{
			global::GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetActivity(SelfPtr(), activity);
		}

		internal void SetOptionalIntentHandlerForUI(global::System.Action<global::System.IntPtr> intentHandler)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(intentHandler);
			global::GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(SelfPtr(), InternalIntentHandler, global::GooglePlayGames.Native.PInvoke.Callbacks.ToIntPtr(intentHandler));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_Dispose(selfPointer);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration.IntentHandlerInternal))]
		private static void InternalIntentHandler(global::System.IntPtr intent, global::System.IntPtr userData)
		{
			global::GooglePlayGames.Native.PInvoke.Callbacks.PerformInternalCallback("AndroidPlatformConfiguration#InternalIntentHandler", global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Permanent, intent, userData);
		}

		internal static global::GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration Create()
		{
			return new global::GooglePlayGames.Native.PInvoke.AndroidPlatformConfiguration(global::GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_Construct());
		}
	}
}
