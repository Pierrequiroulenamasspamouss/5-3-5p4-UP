namespace Kampai.Game
{
	public class LuaKernel : global::System.IDisposable
	{
		private readonly global::Kampai.Wrappers.NativeLibContext context;

		public readonly global::Kampai.Wrappers.MasterLuaState L;

		private readonly global::Kampai.Util.IKampaiLogger _logger = global::Elevation.Logging.LogManager.GetClassLogger("LuaKernel") as global::Kampai.Util.IKampaiLogger;

		private readonly global::Kampai.Wrappers.SafeGCHandle luaSearcherHandle;

		private readonly global::Kampai.Wrappers.SafeGCHandle cSearcherHandle;

		public LuaKernel()
		{
			if (global::UnityEngine.Application.isEditor)
			{
				_logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "LuaKernel: Skipping native initialization in Editor.");
				return;
			}
			try
			{
				luaSearcherHandle = global::Kampai.Wrappers.LuaUtil.MakeHandle(LuaSearcher);
				cSearcherHandle = global::Kampai.Wrappers.LuaUtil.MakeHandle(CSearcher);
				context = new global::Kampai.Wrappers.NativeLibContext(LogMethod, ErrorMethod);
				L = new global::Kampai.Wrappers.MasterLuaState();
				L.luaL_openlibs();
				SetupState(L);
			}
			catch (global::System.Exception ex)
			{
				_logger.Error(ex.ToString());
				_logger.Error(ex.Message);
				throw;
			}
		}

		private void SetupState(global::Kampai.Wrappers.LuaState state)
		{
			state.lua_createtable(2, 0);
			state.lua_pushinteger(1);
			state.lua_pushlightuserdata(luaSearcherHandle);
			state.lua_pushcclosure(global::Kampai.Wrappers.LuaUtil.cfunc_CallDelegate, 1);
			state.lua_settable(-3);
			state.lua_pushinteger(2);
			state.lua_pushlightuserdata(cSearcherHandle);
			state.lua_pushcclosure(global::Kampai.Wrappers.LuaUtil.cfunc_CallDelegate, 1);
			state.lua_settable(-3);
			state.lua_getglobal("package");
			state.lua_pushvalue(-2);
			state.lua_setfield(-2, "searchers");
			state.lua_pop(2);
		}

		private int LuaSearcher(global::Kampai.Wrappers.LuaState state)
		{
			string arg = state.lua_tostring(1);
			string text = string.Format("LUA/{0}", arg);
			global::UnityEngine.TextAsset textAsset = global::UnityEngine.Resources.Load<global::UnityEngine.TextAsset>(text);
			if (textAsset == null)
			{
				state.lua_pushstring("Failed to load asset " + text);
				return 1;
			}
			state.luaL_loadbufferx(textAsset.text, textAsset.text.Length, text, null);
			return 1;
		}

		private int CSearcher(global::Kampai.Wrappers.LuaState state)
		{
			string text = state.lua_tostring(1);
			string[] value = text.Split('.');
			string function_name = "luaopen_" + string.Join("_", value);
			global::Kampai.Wrappers.KampaiNativeLib.kampai_push_cfunction_from_lib(state, text, function_name);
			return 1;
		}

		private void LogMethod(string message)
		{
			_logger.Log(global::Kampai.Util.KampaiLogLevel.Info, message);
		}

		private void ErrorMethod(string message)
		{
			_logger.Error(message);
		}

		protected virtual void Dispose(bool fromDispose)
		{
			if (fromDispose && !global::UnityEngine.Application.isEditor)
			{
				L.Dispose();
				context.Dispose();
				luaSearcherHandle.Dispose();
				cSearcherHandle.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			global::System.GC.SuppressFinalize(this);
		}

		~LuaKernel()
		{
			Dispose(false);
		}
	}
}
