using System;

namespace Kampai.Wrappers
{
    public static class KampaiNativeLib
    {
        public struct DebugData
        {
            public string name;
            public int line_number;
        }

        // ON SUPPRIME LA CLASSE NativeMethods QUI FAISAIT LES DllImport 
        // VERS LES FONCTIONS SPÉCIFIQUES D'EA ("kampai_create_debug", etc.)

        public const string dllString = "lua52";

        // --- REMPLACEMENTS C# PURS ---

        // EA utilisait cette fonction pour allouer de la mémoire pour la structure lua_Debug en C++.
        // Vu qu'on ne l'a plus, on va simuler ça en retournant simplement un pointeur vide (IntPtr.Zero).
        public static global::System.IntPtr kampai_create_debug()
        {
            // On retourne toujours IntPtr.Zero, même sur mobile.
            return global::System.IntPtr.Zero;
        }

        // Idem, on ne libère rien puisqu'on n'a rien alloué.
        public static void kampai_free_debug(global::System.IntPtr debug)
        {
            return;
        }

        // EA utilisait ça pour lire la structure lua_Debug en C++ et la renvoyer au C#.
        // Comme on n'a plus cette fonction C++, on renvoie une DebugData vide.
        // Le seul impact : Si un script Lua plante, l'erreur dans la console sera moins détaillée 
        // (tu n'auras pas le numéro de ligne exact), mais le jeu NE CRASHERA PAS.
        public static global::Kampai.Wrappers.KampaiNativeLib.DebugData kampai_get_debug(global::Kampai.Wrappers.LuaState L, string what, global::System.IntPtr ar)
        {
            return default(global::Kampai.Wrappers.KampaiNativeLib.DebugData);
        }

        // Cette fonction servait à charger des modules C natifs (fichiers .so ou .dll) depuis Lua.
        // C'était utilisé par la fonction CSearcher dans LuaKernel.
        // La plupart du temps, Minions Paradise n'en a pas besoin car tout est en C# ou en .lua.
        // On retourne -1 (erreur) pour dire à Lua "module non trouvé", 
        // ce qui forcera Lua à utiliser le LuaSearcher normal.
        public static int kampai_push_cfunction_from_lib(global::Kampai.Wrappers.LuaState L, string name, string function_name)
        {
            if (L == null || L.IsInvalid) return -1;

            // Remplacement de secours : On simule l'échec de la fonction native
            global::UnityEngine.Debug.LogWarning("[Lua] Tentative de chargement de module C natif (" + name + ") bloquée pour compatibilité.");
            return -1;
        }
    }
}