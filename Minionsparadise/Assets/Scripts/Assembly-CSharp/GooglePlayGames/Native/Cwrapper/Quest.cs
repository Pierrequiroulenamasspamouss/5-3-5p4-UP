namespace GooglePlayGames.Native.Cwrapper
{
	internal static class Quest
	{
		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr Quest_Description(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr Quest_BannerUrl(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern long Quest_ExpirationTime(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr Quest_IconUrl(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::GooglePlayGames.Native.Cwrapper.Types.QuestState Quest_State(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr Quest_Copy(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		[return: global::System.Runtime.InteropServices.MarshalAs(global::System.Runtime.InteropServices.UnmanagedType.I1)]
		internal static extern bool Quest_Valid(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern long Quest_StartTime(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern void Quest_Dispose(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr Quest_CurrentMilestone(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern long Quest_AcceptedTime(global::System.Runtime.InteropServices.HandleRef self);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr Quest_Id(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.UIntPtr Quest_Name(global::System.Runtime.InteropServices.HandleRef self, global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size);
	}
}
