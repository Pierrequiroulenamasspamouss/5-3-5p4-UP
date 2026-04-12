namespace GooglePlayGames.Native.Cwrapper
{
	internal static class SnapshotMetadataChange
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr SnapshotMetadataChange_Description(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr SnapshotMetadataChange_Image(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool SnapshotMetadataChange_PlayedTimeIsChanged(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool SnapshotMetadataChange_Valid(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern ulong SnapshotMetadataChange_PlayedTime(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void SnapshotMetadataChange_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool SnapshotMetadataChange_ImageIsChanged(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool SnapshotMetadataChange_DescriptionIsChanged(global::System.Runtime.InteropServices.HandleRef self);
	}
}
