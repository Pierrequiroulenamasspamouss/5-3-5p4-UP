namespace Kampai.Wrappers
{
    public static class LuaUtil
    {
        public static global::Kampai.Wrappers.SafeGCHandle MakeHandle(global::Kampai.Wrappers.LuaDelegate func)
        {
            return new global::Kampai.Wrappers.SafeGCHandle(func);
        }

        // 1. We create permanent static references to the delegates. 
        // The Garbage Collector cannot delete these, keeping the unmanaged pointers safe forever.
        public static readonly global::Kampai.Wrappers.LuaCFunction cfunc_CallDelegate = new global::Kampai.Wrappers.LuaCFunction(Internal_CallDelegate);
        public static readonly global::Kampai.Wrappers.LuaCFunction cfunc_CallDelegateFromStackTop = new global::Kampai.Wrappers.LuaCFunction(Internal_CallDelegateFromStackTop);

        // 2. The actual methods are renamed so the fields above can use the public names.
        [global::AOT.MonoPInvokeCallback(typeof(global::Kampai.Wrappers.LuaCFunction))]
        private static int Internal_CallDelegate(global::System.IntPtr Lptr)
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
        private static int Internal_CallDelegateFromStackTop(global::System.IntPtr Lptr)
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