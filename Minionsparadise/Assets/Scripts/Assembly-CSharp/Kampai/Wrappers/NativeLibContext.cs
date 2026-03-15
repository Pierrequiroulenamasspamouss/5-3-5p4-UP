using System;
using System.Runtime.InteropServices;

namespace Kampai.Wrappers
{
    public class NativeLibContext : global::System.IDisposable
    {
        // On supprime totalement les appels DllImport vers les fonctions "kampai"

        private global::System.Action<string> debugAction;
        private global::System.Action<string> errorAction;

        public NativeLibContext(global::System.Action<string> log_method, global::System.Action<string> error_method)
        {
            debugAction = log_method;
            errorAction = error_method;

            // On ne fait plus rien de natif ici.
            // L'initialisation se fera directement via le LuaState plus tard.
            global::UnityEngine.Debug.Log("[Lua] NativeLibContext bypassé pour compatibilité avec DLL Lua standard.");
        }

        // On garde les méthodes au cas où d'autres scripts essaient de les appeler directement
        public void HandleDebugLog(string message)
        {
            if (debugAction != null) debugAction(message);
        }

        public void HandleErrorLog(string message)
        {
            if (errorAction != null) errorAction(message);
        }

        protected virtual void Dispose(bool fromDispose)
        {
            // Rien à libérer côté natif
        }

        public void Dispose()
        {
            Dispose(true);
            global::System.GC.SuppressFinalize(this);
        }

        ~NativeLibContext()
        {
            Dispose(false);
        }
    }
}