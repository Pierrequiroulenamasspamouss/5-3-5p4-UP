namespace Kampai.Wrappers
{
	public class WeakLuaState : global::Kampai.Wrappers.LuaState
	{
		public WeakLuaState(global::System.IntPtr handle)
			: base(false)
		{
			base.handle = handle;
		}

		protected override bool ReleaseHandle()
		{
			handle = global::System.IntPtr.Zero;
			return true;
		}
	}
}
