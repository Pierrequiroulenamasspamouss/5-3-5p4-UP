namespace GooglePlayGames.Native.Cwrapper
{
	internal static class Builder
	{
		internal delegate void OnLogCallback(global::GooglePlayGames.Native.Cwrapper.Types.LogLevel arg0, string arg1, global::System.IntPtr arg2);

		internal delegate void OnAuthActionStartedCallback(global::GooglePlayGames.Native.Cwrapper.Types.AuthOperation arg0, global::System.IntPtr arg1);

		internal delegate void OnAuthActionFinishedCallback(global::GooglePlayGames.Native.Cwrapper.Types.AuthOperation arg0, global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.AuthStatus arg1, global::System.IntPtr arg2);

		internal delegate void OnMultiplayerInvitationEventCallback(global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent arg0, string arg1, global::System.IntPtr arg2, global::System.IntPtr arg3);

		internal delegate void OnTurnBasedMatchEventCallback(global::GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent arg0, string arg1, global::System.IntPtr arg2, global::System.IntPtr arg3);

		internal delegate void OnQuestCompletedCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnAuthActionStarted(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Builder.OnAuthActionStartedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_AddOauthScope(global::System.Runtime.InteropServices.HandleRef self, string scope);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_SetLogging(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Builder.OnLogCallback callback, global::System.IntPtr callback_arg, global::GooglePlayGames.Native.Cwrapper.Types.LogLevel min_level);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr GameServices_Builder_Construct();

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_EnableSnapshots(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_RequireGooglePlus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnLog(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Builder.OnLogCallback callback, global::System.IntPtr callback_arg, global::GooglePlayGames.Native.Cwrapper.Types.LogLevel min_level);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_SetDefaultOnLog(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.LogLevel min_level);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnAuthActionFinished(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Builder.OnAuthActionFinishedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnTurnBasedMatchEvent(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Builder.OnTurnBasedMatchEventCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnQuestCompleted(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Builder.OnQuestCompletedCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_SetOnMultiplayerInvitationEvent(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Builder.OnMultiplayerInvitationEventCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr GameServices_Builder_Create(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr platform);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void GameServices_Builder_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
