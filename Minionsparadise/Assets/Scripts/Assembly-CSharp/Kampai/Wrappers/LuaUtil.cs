namespace Kampai.Wrappers
{
	public static class LuaUtil
	{
		public static global::Kampai.Wrappers.SafeGCHandle MakeHandle(global::Kampai.Wrappers.LuaDelegate func)
		{
			return new global::Kampai.Wrappers.SafeGCHandle(func);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::Kampai.Wrappers.LuaCFunction))]
		public static int cfunc_CallDelegate(global::System.IntPtr Lptr)
		{
			global::Kampai.Wrappers.LuaState luaState = new global::Kampai.Wrappers.WeakLuaState(Lptr);
			global::Kampai.Wrappers.LuaDelegate luaDelegate = luaState.lua_touserdata(global::Kampai.Wrappers.LuaState.lua_upvalueindex(1)).Target as global::Kampai.Wrappers.LuaDelegate;
			return luaDelegate(luaState);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::Kampai.Wrappers.LuaCFunction))]
		public static int cfunc_CallDelegateFromStackTop(global::System.IntPtr Lptr)
		{
			global::Kampai.Wrappers.LuaState luaState = new global::Kampai.Wrappers.WeakLuaState(Lptr);
			global::Kampai.Wrappers.LuaDelegate luaDelegate = luaState.lua_touserdata(-1).Target as global::Kampai.Wrappers.LuaDelegate;
			return luaDelegate(luaState);
		}
	}
}
