namespace Kampai.Wrappers
{
	public abstract class LuaState : global::System.Runtime.InteropServices.SafeHandle
	{
		private static class NativeMethods
		{
			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_gettop(global::System.IntPtr L);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_pcallk(global::System.IntPtr L, int nargs, int nresults, int errfunc, int ctx, global::Kampai.Wrappers.LuaCFunction k);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_settop(global::System.IntPtr L, int n);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_createtable(global::System.IntPtr L, int narr, int nrec);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int luaL_loadstring(global::System.IntPtr L, string s);

            //[global::System.Runtime.InteropServices.DllImport("lua52")]
            //public static extern int luaL_loadbufferx(global::System.IntPtr L, string buff, int sz, string name, string mode);
            [global::System.Runtime.InteropServices.DllImport("lua52", CallingConvention = global::System.Runtime.InteropServices.CallingConvention.Cdecl)]
			public static extern int luaL_loadbufferx(global::System.IntPtr L, string buff, global::System.UIntPtr sz, string name, string mode);

            [global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void luaL_openlibs(global::System.IntPtr L);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int luaL_ref(global::System.IntPtr L, int t);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void luaL_unref(global::System.IntPtr L, int t, int reference);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_pushvalue(global::System.IntPtr L, int idx);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern global::Kampai.Wrappers.LuaType lua_type(global::System.IntPtr L, int idx);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern double lua_tonumberx(global::System.IntPtr L, int idx, global::System.IntPtr isnum);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_tointegerx(global::System.IntPtr L, int idx, global::System.IntPtr isnum);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern global::System.IntPtr lua_tolstring(global::System.IntPtr L, int idx, global::System.IntPtr len);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern global::System.IntPtr lua_touserdata(global::System.IntPtr L, int idx);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern bool lua_toboolean(global::System.IntPtr L, int idx);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_pushnil(global::System.IntPtr L);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_pushnumber(global::System.IntPtr L, double n);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_pushinteger(global::System.IntPtr L, int n);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_pushstring(global::System.IntPtr L, string s);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_pushcclosure(global::System.IntPtr L, global::Kampai.Wrappers.LuaCFunction fn, int n);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_pushboolean(global::System.IntPtr L, bool b);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_pushlightuserdata(global::System.IntPtr L, global::System.IntPtr p);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_getglobal(global::System.IntPtr L, string var);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_getfield(global::System.IntPtr L, int idx, string k);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_rawget(global::System.IntPtr L, int idx);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_rawgeti(global::System.IntPtr L, int idx, int n);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_setglobal(global::System.IntPtr L, string var);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_setfield(global::System.IntPtr L, int idx, string k);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_rawset(global::System.IntPtr L, int idx);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_rawseti(global::System.IntPtr L, int idx, int n);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_setmetatable(global::System.IntPtr L, int objindex);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_settable(global::System.IntPtr L, int index);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_yieldk(global::System.IntPtr L, int nresults, int ctx, global::Kampai.Wrappers.LuaCFunction k);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern global::Kampai.Wrappers.ThreadStatus lua_resume(global::System.IntPtr L, global::System.IntPtr from, int narg);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_getstack(global::System.IntPtr L, int level, global::System.IntPtr ar);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_getinfo(global::System.IntPtr L, string what, global::System.IntPtr ar);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_setupvalue(global::System.IntPtr L, int funcindex, int n);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern int lua_error(global::System.IntPtr L);
		}

		protected const string dllString = "lua52";

		protected const int LUA_MULTRET = -1;

		protected const int LUAI_MAXSTACK = 1000000;

		protected const int LUAI_FIRSTPSEUDOIDX = -1001000;

		public const int LUA_REGISTRYINDEX = -1001000;

		protected const int LUA_REFNIL = -1;

		public override bool IsInvalid
		{
			get
			{
				return handle == global::System.IntPtr.Zero;
			}
		}

		public LuaState(bool ownsHandle)
			: base(global::System.IntPtr.Zero, ownsHandle)
		{
		}

		public int lua_pcall(int nargs, int nresults, int errfunc)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_pcallk(handle, nargs, nresults, errfunc, 0, null);
		}

		public void lua_pop(int n)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_settop(handle, -n - 1);
		}

		public int luaL_dostring(string s)
		{
			if (IsInvalid) return -1;
			int num = global::Kampai.Wrappers.LuaState.NativeMethods.luaL_loadstring(handle, s);
			if (num > 0)
			{
				return num;
			}
			return lua_pcall(0, -1, 0);
		}

		public string lua_tostring(int idx)
		{
			if (IsInvalid) return string.Empty;
			return global::System.Runtime.InteropServices.Marshal.PtrToStringAnsi(global::Kampai.Wrappers.LuaState.NativeMethods.lua_tolstring(handle, idx, global::System.IntPtr.Zero));
		}

		public static int lua_upvalueindex(int i)
		{
			return -1001000 - i;
		}

		public int lua_gettop()
		{
			if (IsInvalid) return 0;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_gettop(handle);
		}

		public int lua_pcallk(int nargs, int nresults, int errfunc, int ctx, global::Kampai.Wrappers.LuaCFunction k)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_pcallk(handle, nargs, nresults, errfunc, ctx, k);
		}

		public void lua_settop(int n)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_settop(handle, n);
		}

		public void lua_createtable(int narr, int nrec)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_createtable(handle, narr, nrec);
		}

		public int luaL_loadstring(string s)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.luaL_loadstring(handle, s);
		}

		public int luaL_loadbufferx(string buff, global::System.UIntPtr sz, string name, string mode)
		{
			if (IsInvalid) return -1;
			// On passe le UIntPtr à la méthode statique de NativeMethods
			return global::Kampai.Wrappers.LuaState.NativeMethods.luaL_loadbufferx(handle, buff, sz, name, mode);
		}

		public void luaL_openlibs()
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.luaL_openlibs(handle);
		}

		public int luaL_ref(int t)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.luaL_ref(handle, t);
		}

		public void luaL_unref(int t, int reference)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.luaL_unref(handle, t, reference);
		}

		public int lua_pushvalue(int idx)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_pushvalue(handle, idx);
		}

		public global::Kampai.Wrappers.LuaType lua_type(int idx)
		{
			if (IsInvalid) return global::Kampai.Wrappers.LuaType.LUA_TNONE;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_type(handle, idx);
		}

		public double lua_tonumberx(int idx, global::System.IntPtr isnum)
		{
			if (IsInvalid) return 0.0;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_tonumberx(handle, idx, isnum);
		}

		public int lua_tointegerx(int idx, global::System.IntPtr isnum)
		{
			if (IsInvalid) return 0;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_tointegerx(handle, idx, isnum);
		}

		public global::System.IntPtr lua_tolstring(int idx, global::System.IntPtr len)
		{
			if (IsInvalid) return global::System.IntPtr.Zero;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_tolstring(handle, idx, len);
		}

		public global::Kampai.Wrappers.WeakGCHandle lua_touserdata(int idx)
		{
			if (IsInvalid) return default(global::Kampai.Wrappers.WeakGCHandle);
			return new global::Kampai.Wrappers.WeakGCHandle(global::Kampai.Wrappers.LuaState.NativeMethods.lua_touserdata(handle, idx));
		}

		public bool lua_toboolean(int idx)
		{
			if (IsInvalid) return false;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_toboolean(handle, idx);
		}

		public void lua_pushnil()
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_pushnil(handle);
		}

		public void lua_pushnumber(double n)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_pushnumber(handle, n);
		}

		public void lua_pushinteger(int n)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_pushinteger(handle, n);
		}

		public int lua_pushstring(string s)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_pushstring(handle, s);
		}

		public void lua_pushcclosure(global::Kampai.Wrappers.LuaCFunction fn, int n)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_pushcclosure(handle, fn, n);
		}

		public int lua_pushboolean(bool b)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_pushboolean(handle, b);
		}

		public void lua_pushlightuserdata(global::Kampai.Wrappers.SafeGCHandle p)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_pushlightuserdata(handle, p.DangerousGetHandle());
		}

		public int lua_getglobal(string var)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_getglobal(handle, var);
		}

		public int lua_getfield(int idx, string k)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_getfield(handle, idx, k);
		}

		public void lua_rawget(int idx)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_rawget(handle, idx);
		}

		public void lua_rawgeti(int idx, int n)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_rawgeti(handle, idx, n);
		}

		public void lua_setglobal(string var)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_setglobal(handle, var);
		}

		public void lua_setfield(int idx, string k)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_setfield(handle, idx, k);
		}

		public void lua_rawset(int idx)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_rawset(handle, idx);
		}

		public void lua_rawseti(int idx, int n)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_rawseti(handle, idx, n);
		}

		public void lua_setmetatable(int objindex)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_setmetatable(handle, objindex);
		}

		public void lua_settable(int index)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_settable(handle, index);
		}

		public int lua_yieldk(int nresults, int ctx, global::Kampai.Wrappers.LuaCFunction k)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_yieldk(handle, nresults, ctx, k);
		}

		public global::Kampai.Wrappers.ThreadStatus lua_resume(global::Kampai.Wrappers.LuaState from, int narg)
		{
			if (IsInvalid) return global::Kampai.Wrappers.ThreadStatus.LUA_ERRRUN;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_resume(handle, from.DangerousGetHandle(), narg);
		}

		public int lua_getstack(int level, global::System.IntPtr ar)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_getstack(handle, level, ar);
		}

		public int lua_getinfo(string what, global::System.IntPtr ar)
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_getinfo(handle, what, ar);
		}

		public void lua_setupvalue(int funcindex, int n)
		{
			if (IsInvalid) return;
			global::Kampai.Wrappers.LuaState.NativeMethods.lua_setupvalue(handle, funcindex, n);
		}

		public int lua_error()
		{
			if (IsInvalid) return -1;
			return global::Kampai.Wrappers.LuaState.NativeMethods.lua_error(handle);
		}
	}
}
