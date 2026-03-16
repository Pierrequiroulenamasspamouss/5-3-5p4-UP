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
			object target = luaState.lua_touserdata(global::Kampai.Wrappers.LuaState.lua_upvalueindex(1)).Target;
			global::Kampai.Wrappers.LuaDelegate luaDelegate = target as global::Kampai.Wrappers.LuaDelegate;
			if (luaDelegate == null)
			{
				global::UnityEngine.Debug.LogError("[Lua] cfunc_CallDelegate: Native handle target is null or not a LuaDelegate!");
				return 0;
			}
			return luaDelegate(luaState);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::Kampai.Wrappers.LuaCFunction))]
		public static int cfunc_CallDelegateFromStackTop(global::System.IntPtr Lptr)
		{
			global::Kampai.Wrappers.LuaState luaState = new global::Kampai.Wrappers.WeakLuaState(Lptr);
			object target = luaState.lua_touserdata(-1).Target;
			global::Kampai.Wrappers.LuaDelegate luaDelegate = target as global::Kampai.Wrappers.LuaDelegate;
			if (luaDelegate == null)
			{
				global::UnityEngine.Debug.LogError("[Lua] cfunc_CallDelegateFromStackTop: Stack top handle target is null or not a LuaDelegate!");
				return 0;
			}
			return luaDelegate(luaState);
		}
	}
}
