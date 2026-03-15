namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeAppIdentifier : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeAppIdentifier(global::System.IntPtr pointer)
			: base(pointer)
		{
		}

		[global::System.Runtime.InteropServices.DllImport("gpg")]
		internal static extern global::System.IntPtr NearbyUtils_ConstructAppIdentifier(string appId);

		internal string Id()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.AppIdentifier_GetIdentifier(SelfPtr(), out_arg, out_size));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.AppIdentifier_Dispose(selfPointer);
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeAppIdentifier FromString(string appId)
		{
			return new global::GooglePlayGames.Native.PInvoke.NativeAppIdentifier(NearbyUtils_ConstructAppIdentifier(appId));
		}
	}
}
