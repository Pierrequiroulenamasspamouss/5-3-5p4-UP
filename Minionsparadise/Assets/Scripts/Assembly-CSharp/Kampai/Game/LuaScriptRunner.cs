using System;
using System.Text;
using UnityEngine;
using MoonSharp.Interpreter;

namespace Kampai.Game
{
    public class LuaScriptRunner : global::System.IDisposable, global::Kampai.Game.IQuestScriptRunner
    {
        public readonly global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("LuaScriptRunner") as global::Kampai.Util.IKampaiLogger;

        private MoonSharp.Interpreter.Coroutine coroutine;
        private readonly global::Kampai.Game.LuaArgRetriever argRetriever = new global::Kampai.Game.LuaArgRetriever();
        private readonly global::Kampai.Game.LuaReturnValueContainer returnContainer;
        private global::Kampai.Game.QuestScriptInstance questInstance;
        private string fileName;
        private bool canContinue;
        private global::Kampai.Game.LuaReturnValueContainer _invokationValues;

        private string startMethodName;
        private bool hasRanMethod;
        private bool _isDisposed;

        [Inject]
        public global::Kampai.Game.QuestScriptController controller { get; set; }

        [Inject]
        public global::Kampai.Game.LuaKernel qsKernel { get; set; }

        public global::Kampai.Game.QuestRunnerLanguage Lang
        {
            get { return global::Kampai.Game.QuestRunnerLanguage.Lua; }
        }

        public global::System.Action<global::Kampai.Game.QuestScriptInstance> OnQuestScriptComplete { get; set; }

        public global::Kampai.Game.ReturnValueContainer InvokationValues
        {
            get { return _invokationValues; }
        }

        public static LuaScriptRunner CurrentRunner { get; private set; }

        public LuaScriptRunner(global::Kampai.Game.LuaKernel kernel)
        {
            qsKernel = kernel;
            returnContainer = new global::Kampai.Game.LuaReturnValueContainer(logger);
            _invokationValues = new global::Kampai.Game.LuaReturnValueContainer(logger);
        }

        public static DynValue InvokeApiGlobal(string tableName, string key, CallbackArguments args)
        {
            if (CurrentRunner == null) return DynValue.Nil;
            if (CurrentRunner.qsKernel.HasApiFunction(key))
            {
                return DynValue.NewCallback((ctx, funcArgs) => CurrentRunner.InvokeMethodFromLua(key, funcArgs));
            }
            return DynValue.Nil;
        }

        private DynValue InvokeMethodFromLua(string methodName, CallbackArguments args)
        {
            var apiFunction = qsKernel.GetApiFunction(methodName);
            if (apiFunction == null)
            {
                throw new ScriptRuntimeException("Unbound method: " + methodName);
            }
            
            // Log arguments for debugging nil errors
            for (int i = 0; i < args.Count; i++)
            {
                logger.Info("Lua: {0} calling {1} with arg[{2}] = {3} (Type: {4})", fileName, methodName, i, args[i], args[i].Type);
            }

            argRetriever.Setup(args, methodName);
            returnContainer.Reset();
            
            if (apiFunction(controller, argRetriever, returnContainer))
            {
                return returnContainer.ToDynValue();
            }
            
            // Yield
            return DynValue.NewYieldReq(new DynValue[] { });
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

            try
            {
                // Load string into shared script, create coroutine
                DynValue parsedScript = qsKernel.SharedScript.LoadString(scriptText, null, filename);
                coroutine = qsKernel.SharedScript.CreateCoroutine(parsedScript).Coroutine;
                
                canContinue = true;
                Continue();
            }
            catch (SyntaxErrorException ex)
            {
                LogLuaError(ex.DecoratedMessage);
            }
            catch (ScriptRuntimeException ex)
            {
                LogLuaError(ex.DecoratedMessage);
            }
        }

        private void Continue(params DynValue[] args)
        {
            if (!canContinue || coroutine == null || coroutine.State == CoroutineState.Dead || coroutine.State == CoroutineState.Running) return;

            var previousRunner = CurrentRunner;
            CurrentRunner = this;
            try
            {
                // logger.Info("Lua: {0} Continue - Pre Globals Check", fileName);
                // qsKernel.LogGlobals();

                coroutine.Resume(args);

                if (coroutine.State == CoroutineState.Dead)
                {
                    HandleContinueFinished();
                }
                else
                {
                    canContinue = true;
                }
            }
            catch (ScriptRuntimeException ex)
            {
                LogLuaError(ex.DecoratedMessage);
                Stop();
            }
            finally
            {
                if (CurrentRunner == this) 
                {
                    CurrentRunner = previousRunner;
                }
            }
        }

        private void HandleContinueFinished()
        {
            if (!hasRanMethod && startMethodName != null)
            {
                hasRanMethod = true;
                DynValue startMethod = qsKernel.SharedScript.Globals.Get(startMethodName);
                if (startMethod.Type == DataType.Function || startMethod.Type == DataType.ClrFunction)
                {
                    coroutine = qsKernel.SharedScript.CreateCoroutine(startMethod).Coroutine;
                    canContinue = true;
                    Continue(_invokationValues.ToDynValueArray());
                    return;
                }
            }
            EndQuest();
        }

        private void LogLuaError(string message)
        {
            global::UnityEngine.Debug.LogError("<color=red>[LUA ERROR]</color> " + fileName + " : " + message);
        }

        public void Stop() { DisposedCheck(); InternalStop(); }
        public void Pause() { /* Interface implementation */ }
        public void Resume() { DisposedCheck(); if (canContinue) Continue(); }
        
        private void InternalStop() 
        { 
            if (controller != null)
            {
                controller.ContinueSignal.RemoveListener(ContinueFromYield); 
                controller.Stop(); 
            }
            canContinue = false;
        }

        private void ContinueFromYield() 
        { 
            Continue(returnContainer.ToDynValue()); 
        }

        private void EndQuest()
        {
            Stop();
            if (OnQuestScriptComplete != null)
            {
                OnQuestScriptComplete(questInstance);
            }
        }

        protected virtual void Dispose(bool fromDispose)
        {
            if (fromDispose && !_isDisposed)
            {
                Stop();
            }
            _isDisposed = true;
        }

        private void DisposedCheck() { if (_isDisposed) throw new global::System.ObjectDisposedException(ToString()); }
        public void Dispose() { Dispose(true); global::System.GC.SuppressFinalize(this); }
        ~LuaScriptRunner() { Dispose(false); }
    }
}