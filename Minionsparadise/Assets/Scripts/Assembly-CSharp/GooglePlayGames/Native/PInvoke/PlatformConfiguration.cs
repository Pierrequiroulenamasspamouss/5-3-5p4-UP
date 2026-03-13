namespace GooglePlayGames.Native.PInvoke
{
	internal abstract class PlatformConfiguration : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
	{
		protected PlatformConfiguration(global::System.IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal global::System.Runtime.InteropServices.HandleRef AsHandle()
		{
			return SelfPtr();
		}
	}
}
