namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeEndpointDetails : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeEndpointDetails(global::System.IntPtr pointer)
			: base(pointer)
		{
		}

		internal string EndpointId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.EndpointDetails_GetEndpointId(SelfPtr(), out_arg, out_size));
		}

		internal string DeviceId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.EndpointDetails_GetDeviceId(SelfPtr(), out_arg, out_size));
		}

		internal string Name()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.EndpointDetails_GetName(SelfPtr(), out_arg, out_size));
		}

		internal string ServiceId()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.EndpointDetails_GetServiceId(SelfPtr(), out_arg, out_size));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.EndpointDetails_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.BasicApi.Nearby.EndpointDetails ToDetails()
		{
			return new global::GooglePlayGames.BasicApi.Nearby.EndpointDetails(EndpointId(), DeviceId(), Name(), ServiceId());
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeEndpointDetails FromPointer(global::System.IntPtr pointer)
		{
			if (pointer.Equals(global::System.IntPtr.Zero))
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeEndpointDetails(pointer);
		}
	}
}
