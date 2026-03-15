namespace Kampai.Wrappers
{
	public class MasterLuaState : global::Kampai.Wrappers.LuaState
	{
		private static class NativeMethods
		{
			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern global::System.IntPtr luaL_newstate();

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_close(global::System.IntPtr L);
		}

		public MasterLuaState()
			: base(true)
		{
			handle = global::Kampai.Wrappers.MasterLuaState.NativeMethods.luaL_newstate();
		}

		protected override bool ReleaseHandle()
		{
			global::Kampai.Wrappers.MasterLuaState.NativeMethods.lua_close(handle);
			handle = global::System.IntPtr.Zero;
			return true;
		}
	}
}
