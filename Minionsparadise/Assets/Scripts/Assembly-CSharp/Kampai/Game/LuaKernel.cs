using MoonSharp.Interpreter;

namespace Kampai.Game
{
    public class LuaKernel : global::System.IDisposable
    {
        private readonly global::Kampai.Util.IKampaiLogger _logger = global::Elevation.Logging.LogManager.GetClassLogger("LuaKernel") as global::Kampai.Util.IKampaiLogger;

        [Inject]
        public QuestScriptKernel ApiKernel { get; set; }

        public Script SharedScript { get; private set; }
        public Table QSTable { get; private set; }
        public Table QSUtilTable { get; private set; }

        [PostConstruct]
        public void PostConstruct()
        {
            _logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "LuaKernel: Initializing Shared MoonSharp engine...");
            
            SharedScript = new Script();
            QSTable = new Table(SharedScript);
            QSUtilTable = new Table(SharedScript);

            // Set up __index proxies that call back into the active runner
            DynValue qsIndex = DynValue.NewCallback((ctx, args) => {
                string key = args.AsType(1, "__index", DataType.String).String;
                return LuaScriptRunner.InvokeApiGlobal("qs", key, args);
            });
            Table qsMeta = new Table(SharedScript);
            qsMeta.Set("__index", qsIndex);
            QSTable.MetaTable = qsMeta;

            DynValue qsutilIndex = DynValue.NewCallback((ctx, args) => {
                string key = args.AsType(1, "__index", DataType.String).String;
                // Try API first
                DynValue val = LuaScriptRunner.InvokeApiGlobal("qsutil", key, args);
                if (val.IsNil()) {
                    // Fallback to globals (standard lua funcs)
                    return SharedScript.Globals.Get(key);
                }
                return val;
            });
            Table qsutilMeta = new Table(SharedScript);
            qsutilMeta.Set("__index", qsutilIndex);
            QSUtilTable.MetaTable = qsutilMeta;

            SharedScript.Globals["qs"] = QSTable;
            SharedScript.Globals["qsutil"] = QSUtilTable;

            // Load Utilities.txt into qsutil once
            global::UnityEngine.TextAsset utilAsset = global::UnityEngine.Resources.Load<global::UnityEngine.TextAsset>("LUA/Utilities");
            if (utilAsset != null)
            {
                try
                {
                    SharedScript.DoString(utilAsset.text, QSUtilTable, "LUA/Utilities.txt");
                }
                catch (ScriptRuntimeException ex)
                {
                    _logger.Error("LuaKernel: Utilities.txt Load Error: " + ex.DecoratedMessage);
                }
            }

            _logger.Log(global::Kampai.Util.KampaiLogLevel.Info, "LuaKernel: MoonSharp Engine Ready.");
        }

        public bool HasApiFunction(string name)
        {
            return ApiKernel != null && ApiKernel.HasApiFunction(name);
        }

        public global::System.Func<global::Kampai.Game.QuestScriptController, global::Kampai.Game.IArgRetriever, global::Kampai.Game.ReturnValueContainer, bool> GetApiFunction(string name)
        {
            return ApiKernel != null ? ApiKernel.GetApiFunction(name) : null;
        }

        protected virtual void Dispose(bool fromDispose)
        {
            SharedScript = null;
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
