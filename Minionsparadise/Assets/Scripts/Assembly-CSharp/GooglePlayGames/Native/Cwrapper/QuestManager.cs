namespace GooglePlayGames.Native.Cwrapper
{
	internal static class QuestManager
	{
		internal delegate void FetchCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void FetchListCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void AcceptCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void ClaimMilestoneCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void QuestUICallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_FetchList(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, int fetch_flags, global::GooglePlayGames.Native.Cwrapper.QuestManager.FetchListCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_Accept(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr quest, global::GooglePlayGames.Native.Cwrapper.QuestManager.AcceptCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_ShowAllUI(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_ShowUI(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr quest, global::GooglePlayGames.Native.Cwrapper.QuestManager.QuestUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_ClaimMilestone(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr milestone, global::GooglePlayGames.Native.Cwrapper.QuestManager.ClaimMilestoneCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_Fetch(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, string quest_id, global::GooglePlayGames.Native.Cwrapper.QuestManager.FetchCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_FetchResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus QuestManager_FetchResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr QuestManager_FetchResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_FetchListResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus QuestManager_FetchListResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr QuestManager_FetchListResponse_GetData_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr QuestManager_FetchListResponse_GetData_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_AcceptResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestAcceptStatus QuestManager_AcceptResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr QuestManager_AcceptResponse_GetAcceptedQuest(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_ClaimMilestoneResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.QuestClaimMilestoneStatus QuestManager_ClaimMilestoneResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr QuestManager_ClaimMilestoneResponse_GetClaimedMilestone(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr QuestManager_ClaimMilestoneResponse_GetQuest(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void QuestManager_QuestUIResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus QuestManager_QuestUIResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr QuestManager_QuestUIResponse_GetAcceptedQuest(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr QuestManager_QuestUIResponse_GetMilestoneToClaim(global::System.Runtime.InteropServices.HandleRef self);
	}
}
