namespace Kampai.Wrappers
{
	public class LuaThreadState : global::Kampai.Wrappers.LuaState
	{
		private static class NativeMethods
		{
			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern global::System.IntPtr lua_newthread(global::System.IntPtr L);
		}

		private int threadReference = -1;

		private readonly global::Kampai.Wrappers.LuaState strongState;

		public override bool IsInvalid
		{
			get
			{
				return strongState.IsInvalid || handle == global::System.IntPtr.Zero;
			}
		}

		public LuaThreadState(global::Kampai.Wrappers.LuaState L)
			: base(true)
		{
			strongState = L;
			if (L == null || L.IsInvalid)
			{
				handle = global::System.IntPtr.Zero;
				threadReference = -1;
			}
			else
			{
				handle = global::Kampai.Wrappers.LuaThreadState.NativeMethods.lua_newthread(L.DangerousGetHandle());
				threadReference = L.luaL_ref(-1001000);
			}
		}

		protected override bool ReleaseHandle()
		{
			if (strongState.IsInvalid)
			{
				return true;
			}
			strongState.luaL_unref(-1001000, threadReference);
			threadReference = -1;
			handle = global::System.IntPtr.Zero;
			return true;
		}
	}
}
