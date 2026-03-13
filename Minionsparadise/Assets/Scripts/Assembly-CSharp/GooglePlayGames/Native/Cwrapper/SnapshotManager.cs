namespace GooglePlayGames.Native.Cwrapper
{
	internal static class SnapshotManager
	{
		internal delegate void FetchAllCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void OpenCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void CommitCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void ReadCallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		internal delegate void SnapshotSelectUICallback(global::System.IntPtr arg0, global::System.IntPtr arg1);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_FetchAll(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, global::GooglePlayGames.Native.Cwrapper.SnapshotManager.FetchAllCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_ShowSelectUIOperation(global::System.Runtime.InteropServices.HandleRef self, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool allow_create, [global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)] bool allow_delete, uint max_snapshots, string title, global::GooglePlayGames.Native.Cwrapper.SnapshotManager.SnapshotSelectUICallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_Read(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr snapshot_metadata, global::GooglePlayGames.Native.Cwrapper.SnapshotManager.ReadCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_Commit(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr snapshot_metadata, global::System.IntPtr metadata_change, byte[] data, global::System.UIntPtr data_size, global::GooglePlayGames.Native.Cwrapper.SnapshotManager.CommitCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_Open(global::System.Runtime.InteropServices.HandleRef self, global::GooglePlayGames.Native.Cwrapper.Types.DataSource data_source, string file_name, global::GooglePlayGames.Native.Cwrapper.Types.SnapshotConflictPolicy conflict_policy, global::GooglePlayGames.Native.Cwrapper.SnapshotManager.OpenCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_ResolveConflict(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr snapshot_metadata, global::System.IntPtr metadata_change, string conflict_id, global::GooglePlayGames.Native.Cwrapper.SnapshotManager.CommitCallback callback, global::System.IntPtr callback_arg);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_Delete(global::System.Runtime.InteropServices.HandleRef self, global::System.IntPtr snapshot_metadata);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_FetchAllResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus SnapshotManager_FetchAllResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr SnapshotManager_FetchAllResponse_GetData_Length(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr SnapshotManager_FetchAllResponse_GetData_GetElement(global::System.Runtime.InteropServices.HandleRef self, global::System.UIntPtr index);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_OpenResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.SnapshotOpenStatus SnapshotManager_OpenResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr SnapshotManager_OpenResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr SnapshotManager_OpenResponse_GetConflictId(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr SnapshotManager_OpenResponse_GetConflictOriginal(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr SnapshotManager_OpenResponse_GetConflictUnmerged(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_CommitResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus SnapshotManager_CommitResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr SnapshotManager_CommitResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_ReadResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus SnapshotManager_ReadResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr SnapshotManager_ReadResponse_GetData(global::System.Runtime.InteropServices.HandleRef self, [global::System.Runtime.InteropServices.In][global::System.Runtime.InteropServices.Out] byte[] out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotManager_SnapshotSelectUIResponse_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus SnapshotManager_SnapshotSelectUIResponse_GetStatus(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr SnapshotManager_SnapshotSelectUIResponse_GetData(global::System.Runtime.InteropServices.HandleRef self);
	}
}
