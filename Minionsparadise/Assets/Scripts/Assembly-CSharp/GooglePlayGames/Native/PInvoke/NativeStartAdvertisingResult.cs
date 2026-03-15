namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeStartAdvertisingResult : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativeStartAdvertisingResult(global::System.IntPtr pointer)
			: base(pointer)
		{
		}

		internal int GetStatus()
		{
			return global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.StartAdvertisingResult_GetStatus(SelfPtr());
		}

		internal string LocalEndpointName()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_arg, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.StartAdvertisingResult_GetLocalEndpointName(SelfPtr(), out_arg, out_size));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.NearbyConnectionTypes.StartAdvertisingResult_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.BasicApi.Nearby.AdvertisingResult AsResult()
		{
			return new global::GooglePlayGames.BasicApi.Nearby.AdvertisingResult((global::GooglePlayGames.BasicApi.ResponseStatus)(int)global::System.Enum.ToObject(typeof(global::GooglePlayGames.BasicApi.ResponseStatus), GetStatus()), LocalEndpointName());
		}

		internal static global::GooglePlayGames.Native.PInvoke.NativeStartAdvertisingResult FromPointer(global::System.IntPtr pointer)
		{
			if (pointer == global::System.IntPtr.Zero)
			{
				return null;
			}
			return new global::GooglePlayGames.Native.PInvoke.NativeStartAdvertisingResult(pointer);
		}
	}
}
