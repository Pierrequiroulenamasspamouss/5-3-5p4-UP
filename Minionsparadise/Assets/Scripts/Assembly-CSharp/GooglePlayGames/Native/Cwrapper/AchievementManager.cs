namespace GooglePlayGames.Native.Cwrapper
{
	internal static class AchievementManager
	{
		internal delegate void FetchAllCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void FetchCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void ShowAllUICallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AchievementManager_FetchAll(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, global::GooglePlayGames.Native.Cwrapper.AchievementManager.FetchAllCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AchievementManager_Reveal(global::System.Runtime.InteropServices.HandleRef self, string achievement_id);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AchievementManager_Unlock(global::System.Runtime.InteropServices.HandleRef self, string achievement_id);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AchievementManager_ShowAllUI(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.AchievementManager.ShowAllUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AchievementManager_SetStepsAtLeast(global::System.Runtime.InteropServices.HandleRef self, string achievement_id, uint steps);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AchievementManager_Increment(global::System.Runtime.InteropServices.HandleRef self, string achievement_id, uint steps);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AchievementManager_Fetch(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, string achievement_id, global::GooglePlayGames.Native.Cwrapper.AchievementManager.FetchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AchievementManager_FetchAllResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus AchievementManager_FetchAllResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr AchievementManager_FetchAllResponse_GetData_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr AchievementManager_FetchAllResponse_GetData_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void AchievementManager_FetchResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus AchievementManager_FetchResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr AchievementManager_FetchResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);
	}
}
