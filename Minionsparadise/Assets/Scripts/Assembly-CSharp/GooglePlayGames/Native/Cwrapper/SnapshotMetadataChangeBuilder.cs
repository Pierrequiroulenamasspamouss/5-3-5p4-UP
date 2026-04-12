namespace GooglePlayGames.Native.Cwrapper
{
	internal static class SnapshotMetadataChangeBuilder
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotMetadataChange_Builder_SetDescription(global::System.Runtime.InteropServices.HandleRef self, string description);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr SnapshotMetadataChange_Builder_Construct();

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotMetadataChange_Builder_SetPlayedTime(global::System.Runtime.InteropServices.HandleRef self, ulong played_time);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotMetadataChange_Builder_SetCoverImageFromPngData(global::System.Runtime.InteropServices.HandleRef self, byte[] png_data, global::System.UIntPtr png_data_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr SnapshotMetadataChange_Builder_Create(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotMetadataChange_Builder_Dispose(global::System.Runtime.InteropServices.HandleRef self);
	}
}
