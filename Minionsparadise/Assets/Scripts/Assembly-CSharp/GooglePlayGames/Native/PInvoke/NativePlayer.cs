namespace GooglePlayGames.Native.PInvoke
{
	internal class NativePlayer : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		internal NativePlayer(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal string Id()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Player.Player_Id(SelfPtr(), out_string, out_size));
		}

		internal string Name()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Player.Player_Name(SelfPtr(), out_string, out_size));
		}

		internal string AvatarURL()
		{
			return global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutParamsToString((global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size) => global::GooglePlayGames.Native.Cwrapper.Player.Player_AvatarUrl(SelfPtr(), global::GooglePlayGames.Native.Cwrapper.Types.ImageResolution.ICON, out_string, out_size));
		}

		protected override void CallDispose(global::System.Runtime.InteropServices.HandleRef selfPointer)
		{
			global::GooglePlayGames.Native.Cwrapper.Player.Player_Dispose(selfPointer);
		}

		internal global::GooglePlayGames.BasicApi.Multiplayer.Player AsPlayer()
		{
			return new global::GooglePlayGames.BasicApi.Multiplayer.Player(Name(), Id(), AvatarURL());
		}
	}
}
