namespace Kampai.Game
{
	public class LuaScriptRunner : global::System.IDisposable, global::Kampai.Game.IQuestScriptRunner
	{
		private enum PauseResumeState
		{
			RUNNING = 0,
			WANT_TO_PAUSE = 1,
			PAUSED = 2
		}

		public readonly global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LuaScriptRunner") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Wrappers.LuaState masterState;

		private global::Kampai.Wrappers.LuaState threadState;

		private readonly global::Kampai.Game.LuaArgRetriever argRetriever = new global::Kampai.Game.LuaArgRetriever();

		private readonly global::Kampai.Game.LuaReturnValueContainer returnContainer;

		private int envTableRef;

		private int qsTableRef;

		private int qsutilTableRef;

		private global::Kampai.Game.QuestScriptInstance questInstance;

		private string fileName;

		private bool canContinue;

		private global::Kampai.Game.LuaScriptRunner.PauseResumeState pauseResumeState;

		private global::Kampai.Game.LuaReturnValueContainer _invokationValues;

		private global::Kampai.Wrappers.SafeGCHandle EnvIndexHandle;

		private global::Kampai.Wrappers.SafeGCHandle EnvNewIndexHandle;

		private global::Kampai.Wrappers.SafeGCHandle QSIndexHandle;

		private global::Kampai.Wrappers.SafeGCHandle QSNewIndexHandle;

		private global::Kampai.Wrappers.SafeGCHandle InvokeMethodFromLuaHandle;

		private global::Kampai.Wrappers.SafeGCHandle ContinuationHandle;

		private string startMethodName;

		private global::System.Text.StringBuilder errorMessageBuilder = new global::System.Text.StringBuilder();

		private bool hasRanMethod;

		private bool _isDisposed;

		[Inject]
		public global::Kampai.Game.QuestScriptController controller { get; set; }

		[Inject]
		public global::Kampai.Game.QuestScriptKernel qsKernel { get; set; }

		public global::Kampai.Game.QuestRunnerLanguage Lang
		{
			get
			{
				return global::Kampai.Game.QuestRunnerLanguage.Lua;
			}
		}

		public global::System.Action<global::Kampai.Game.QuestScriptInstance> OnQuestScriptComplete { get; set; }

		public global::Kampai.Game.ReturnValueContainer InvokationValues
		{
			get
			{
				return _invokationValues;
			}
		}

		public LuaScriptRunner(global::Kampai.Game.LuaKernel kernel)
		{
			masterState = kernel.L;
			returnContainer = new global::Kampai.Game.LuaReturnValueContainer(logger);
			_invokationValues = new global::Kampai.Game.LuaReturnValueContainer(logger);
			EnvIndexHandle = global::Kampai.Wrappers.LuaUtil.MakeHandle(EnvIndex);
			EnvNewIndexHandle = global::Kampai.Wrappers.LuaUtil.MakeHandle(EnvNewIndex);
			QSIndexHandle = global::Kampai.Wrappers.LuaUtil.MakeHandle(QSIndex);
			QSNewIndexHandle = global::Kampai.Wrappers.LuaUtil.MakeHandle(QSNewIndex);
			InvokeMethodFromLuaHandle = global::Kampai.Wrappers.LuaUtil.MakeHandle(InvokeMethodFromLua);
			ContinuationHandle = global::Kampai.Wrappers.LuaUtil.MakeHandle(continuation);
		}

		private void CreateLuaThread()
		{
			global::Kampai.Wrappers.LuaThreadState luaThreadState = (global::Kampai.Wrappers.LuaThreadState)(threadState = new global::Kampai.Wrappers.LuaThreadState(masterState));
			luaThreadState.lua_createtable(0, 0);
			luaThreadState.lua_createtable(0, 4);
			luaThreadState.lua_pushvalue(-1);
			int n = luaThreadState.luaL_ref(-1001000);
			luaThreadState.lua_pushlightuserdata(EnvIndexHandle);
			luaThreadState.lua_pushcclosure(global::Kampai.Wrappers.LuaUtil.cfunc_CallDelegate, 1);
			luaThreadState.lua_setfield(-2, "__index");
			luaThreadState.lua_pushlightuserdata(EnvNewIndexHandle);
			luaThreadState.lua_pushcclosure(global::Kampai.Wrappers.LuaUtil.cfunc_CallDelegate, 1);
			luaThreadState.lua_setfield(-2, "__newindex");
			luaThreadState.lua_setmetatable(-2);
			envTableRef = luaThreadState.luaL_ref(-1001000);
			luaThreadState.lua_createtable(0, 0);
			luaThreadState.lua_createtable(0, 4);
			luaThreadState.lua_pushlightuserdata(QSIndexHandle);
			luaThreadState.lua_pushcclosure(global::Kampai.Wrappers.LuaUtil.cfunc_CallDelegate, 1);
			luaThreadState.lua_setfield(-2, "__index");
			luaThreadState.lua_pushlightuserdata(QSNewIndexHandle);
			luaThreadState.lua_pushcclosure(global::Kampai.Wrappers.LuaUtil.cfunc_CallDelegate, 1);
			luaThreadState.lua_setfield(-2, "__newindex");
			luaThreadState.lua_setmetatable(-2);
			qsTableRef = luaThreadState.luaL_ref(-1001000);
			luaThreadState.lua_createtable(0, 0);
			luaThreadState.lua_pushvalue(-1);
			luaThreadState.lua_rawgeti(-1001000, n);
			luaThreadState.lua_setmetatable(-2);
			string text = global::UnityEngine.Resources.Load<global::UnityEngine.TextAsset>("LUA/Utilities").text;
			if (luaThreadState.luaL_loadbufferx(text, text.Length, "LUA/Utilities.txt", null) > 0)
			{
				LogLuaRuntimeError();
				luaThreadState.lua_pushnil();
			}
			else
			{
				luaThreadState.lua_pushvalue(-2);
				luaThreadState.lua_setupvalue(-2, 1);
				if (luaThreadState.lua_pcall(0, 0, 0) > 0)
				{
					LogLuaRuntimeError();
				}
			}
			qsutilTableRef = luaThreadState.luaL_ref(-1001000);
		}

		private int EnvIndex(global::Kampai.Wrappers.LuaState L)
		{
			string text = L.lua_tostring(2);
			int num = 0;
			switch (text)
			{
			case "qsutil":
				num = qsutilTableRef;
				break;
			default:
				L.lua_pushvalue(2);
				L.lua_rawget(1);
				if (L.lua_type(-1) == global::Kampai.Wrappers.LuaType.LUA_TNIL)
				{
					L.lua_pop(1);
					L.lua_getglobal(text);
				}
				return 1;
			case "qs":
				num = qsTableRef;
				break;
			}
			L.lua_rawgeti(-1001000, num);
			return 1;
		}

		private static int EnvNewIndex(global::Kampai.Wrappers.LuaState L)
		{
			string text = L.lua_tostring(2);
			if (text == "qs")
			{
				L.lua_pushstring("Please don't attempt to set the qs field. Thanks!");
				return L.lua_error();
			}
			L.lua_pushvalue(2);
			L.lua_pushvalue(3);
			L.lua_rawset(1);
			return 0;
		}

		private int QSIndex(global::Kampai.Wrappers.LuaState L)
		{
			string text = L.lua_tostring(2);
			if (!qsKernel.HasApiFunction(text))
			{
				L.lua_pushvalue(2);
				L.lua_rawget(1);
				return 1;
			}
			L.lua_pushlightuserdata(InvokeMethodFromLuaHandle);
			L.lua_pushstring(text);
			L.lua_pushcclosure(global::Kampai.Wrappers.LuaUtil.cfunc_CallDelegate, 2);
			return 1;
		}

		private int QSNewIndex(global::Kampai.Wrappers.LuaState L)
		{
			string text = L.lua_tostring(2);
			if (!qsKernel.HasApiFunction(text))
			{
				L.lua_pushvalue(2);
				L.lua_pushvalue(3);
				L.lua_rawset(1);
				return 0;
			}
			L.lua_pushstring(string.Format("Hey! You can't overwrite the C# bound method {0}! Knock it off!", text));
			return L.lua_error();
		}

		private int InvokeMethodFromLua(global::Kampai.Wrappers.LuaState L)
		{
			string text = L.lua_tostring(global::Kampai.Wrappers.LuaState.lua_upvalueindex(2));
			global::System.Func<global::Kampai.Game.QuestScriptController, global::Kampai.Game.IArgRetriever, global::Kampai.Game.ReturnValueContainer, bool> apiFunction = qsKernel.GetApiFunction(text);
			if (apiFunction == null)
			{
				L.lua_pushstring(string.Format("Woah! The method {0} was unbound from the C# side!", text));
				return L.lua_error();
			}
			argRetriever.Setup(L);
			returnContainer.Reset();
			if (apiFunction(controller, argRetriever, returnContainer))
			{
				return returnContainer.PushToStack(L);
			}
			L.lua_pushlightuserdata(ContinuationHandle);
			return L.lua_yieldk(0, 0, global::Kampai.Wrappers.LuaUtil.cfunc_CallDelegateFromStackTop);
		}

		private int continuation(global::Kampai.Wrappers.LuaState L)
		{
			return returnContainer.PushToStack(L);
		}

		public void Start(global::Kampai.Game.QuestScriptInstance questScriptInstance, string scriptText, string filename, string startMethodName)
		{
			DisposedCheck();
			questInstance = questScriptInstance;
			fileName = filename;
			this.startMethodName = startMethodName;
			hasRanMethod = false;
			controller.Setup(questInstance);
			controller.ContinueSignal.AddListener(ContinueFromYield);
			CreateLuaThread();
			if (threadState.luaL_loadbufferx(scriptText, scriptText.Length, filename, null) > 0)
			{
				string message = threadState.lua_tostring(-1);
				threadState.lua_pop(1);
				LogLuaError(message);
			}
			else
			{
				threadState.lua_rawgeti(-1001000, envTableRef);
				threadState.lua_setupvalue(-2, 1);
				canContinue = true;
				Continue(0);
			}
		}

		public void Stop()
		{
			DisposedCheck();
			InternalStop();
		}

		public void Pause()
		{
			DisposedCheck();
			if (pauseResumeState == global::Kampai.Game.LuaScriptRunner.PauseResumeState.RUNNING)
			{
				pauseResumeState = global::Kampai.Game.LuaScriptRunner.PauseResumeState.WANT_TO_PAUSE;
			}
		}

		public void Resume()
		{
			DisposedCheck();
			global::Kampai.Game.LuaScriptRunner.PauseResumeState pauseResumeState = this.pauseResumeState;
			if (pauseResumeState != global::Kampai.Game.LuaScriptRunner.PauseResumeState.RUNNING)
			{
				this.pauseResumeState = global::Kampai.Game.LuaScriptRunner.PauseResumeState.RUNNING;
				if (pauseResumeState == global::Kampai.Game.LuaScriptRunner.PauseResumeState.PAUSED)
				{
					Continue(0);
				}
			}
		}

		private void InternalStop()
		{
			controller.ContinueSignal.RemoveListener(ContinueFromYield);
			controller.Stop();
		}

		private void Continue(int nargs)
		{
			if (!canContinue)
			{
				logger.Error("LuaQuestStepRunner: Attempting to continue without a yielding thread.");
				return;
			}
			canContinue = false;
			global::Kampai.Wrappers.ThreadStatus threadStatus = threadState.lua_resume(masterState, nargs);
			if (threadStatus == global::Kampai.Wrappers.ThreadStatus.LUA_OK)
			{
				HandleContinueFinished();
			}
			else if (threadStatus > global::Kampai.Wrappers.ThreadStatus.LUA_YIELD)
			{
				LogLuaRuntimeError();
				CreateLuaThread();
				Stop();
			}
			else
			{
				canContinue = true;
			}
		}

		private void ContinueFromYield()
		{
			if (pauseResumeState == global::Kampai.Game.LuaScriptRunner.PauseResumeState.WANT_TO_PAUSE)
			{
				pauseResumeState = global::Kampai.Game.LuaScriptRunner.PauseResumeState.PAUSED;
			}
			else
			{
				Continue(0);
			}
		}

		private void LogLuaError(string message)
		{
			logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Lua Error in {0}: {1}", fileName, message);
		}

		private void LogLuaRuntimeError()
		{
			string value = threadState.lua_tostring(-1);
			threadState.lua_pop(1);
			errorMessageBuilder.AppendLine(value);
			global::System.IntPtr intPtr = global::Kampai.Wrappers.KampaiNativeLib.kampai_create_debug();
			for (int i = 0; threadState.lua_getstack(i, intPtr) > 0; i++)
			{
				global::Kampai.Wrappers.KampaiNativeLib.DebugData debugData = global::Kampai.Wrappers.KampaiNativeLib.kampai_get_debug(threadState, "nSl", intPtr);
				errorMessageBuilder.Append(debugData.name);
				errorMessageBuilder.Append(" : ");
				errorMessageBuilder.Append(debugData.line_number);
			}
			global::Kampai.Wrappers.KampaiNativeLib.kampai_free_debug(intPtr);
			LogLuaError(errorMessageBuilder.ToString());
			errorMessageBuilder.Length = 0;
		}

		private void HandleContinueFinished()
		{
			if (!hasRanMethod && startMethodName != null)
			{
				hasRanMethod = true;
				threadState.lua_rawgeti(-1001000, envTableRef);
				threadState.lua_getfield(-1, startMethodName);
				canContinue = true;
				int nargs = _invokationValues.PushArrayValuesToStack(threadState);
				Continue(nargs);
			}
			else
			{
				Stop();
				if (OnQuestScriptComplete != null)
				{
					OnQuestScriptComplete(questInstance);
				}
			}
		}

		protected virtual void Dispose(bool fromDispose)
		{
			if (fromDispose)
			{
				DisposedCheck();
				Stop();
				threadState.Dispose();
				EnvIndexHandle.Dispose();
				EnvNewIndexHandle.Dispose();
				QSIndexHandle.Dispose();
				QSNewIndexHandle.Dispose();
				InvokeMethodFromLuaHandle.Dispose();
				ContinuationHandle.Dispose();
			}
			_isDisposed = true;
		}

		private void DisposedCheck()
		{
			if (_isDisposed)
			{
				throw new global::System.ObjectDisposedException(ToString());
			}
		}

		public void Dispose()
		{
			Dispose(true);
			global::System.GC.SuppressFinalize(this);
		}

		~LuaScriptRunner()
		{
			Dispose(false);
		}
	}
}
