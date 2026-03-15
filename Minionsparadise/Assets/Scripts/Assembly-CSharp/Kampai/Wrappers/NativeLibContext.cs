namespace Kampai.Wrappers
{
	public class NativeLibContext : global::System.IDisposable
	{
		private static class NativeMethods
		{
			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_kampai_set_log_func(global::System.IntPtr instance, global::Kampai.Wrappers.NativeLibContext.NativeCallbackDelegate callback);

			[global::System.Runtime.InteropServices.DllImport("lua52")]
			public static extern void lua_kampai_set_error_func(global::System.IntPtr instance, global::Kampai.Wrappers.NativeLibContext.NativeCallbackDelegate callback);
		}

		public delegate void NativeCallbackDelegate(global::System.IntPtr instance, global::System.IntPtr str, int strLength);

		private global::System.Runtime.InteropServices.GCHandle thisHandle;

		private global::System.Action<string> debugAction;

		private global::System.Action<string> errorAction;

		public NativeLibContext(global::System.Action<string> log_method, global::System.Action<string> error_method)
		{
			debugAction = log_method;
			errorAction = error_method;
			thisHandle = global::System.Runtime.InteropServices.GCHandle.Alloc(this);
			global::System.IntPtr instance = global::System.Runtime.InteropServices.GCHandle.ToIntPtr(thisHandle);
#if !UNITY_EDITOR
			global::Kampai.Wrappers.NativeLibContext.NativeMethods.lua_kampai_set_log_func(instance, HandleDebugLog);
			global::Kampai.Wrappers.NativeLibContext.NativeMethods.lua_kampai_set_error_func(instance, HandleErrorLog);
#else
			global::UnityEngine.Debug.LogWarning("NativeLibContext: Skipping native lua_kampai_set log/error functions in Editor.");
#endif
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::Kampai.Wrappers.NativeLibContext.NativeCallbackDelegate))]
		public static void HandleDebugLog(global::System.IntPtr instance, global::System.IntPtr str, int strLength)
		{
			string obj = global::System.Runtime.InteropServices.Marshal.PtrToStringAnsi(str, strLength);
			global::Kampai.Wrappers.NativeLibContext nativeLibContext = global::System.Runtime.InteropServices.GCHandle.FromIntPtr(instance).Target as global::Kampai.Wrappers.NativeLibContext;
			nativeLibContext.debugAction(obj);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::Kampai.Wrappers.NativeLibContext.NativeCallbackDelegate))]
		public static void HandleErrorLog(global::System.IntPtr instance, global::System.IntPtr str, int strLength)
		{
			string obj = global::System.Runtime.InteropServices.Marshal.PtrToStringAnsi(str, strLength);
			global::Kampai.Wrappers.NativeLibContext nativeLibContext = global::System.Runtime.InteropServices.GCHandle.FromIntPtr(instance).Target as global::Kampai.Wrappers.NativeLibContext;
			nativeLibContext.errorAction(obj);
		}

		protected virtual void Dispose(bool fromDispose)
		{
#if !UNITY_EDITOR
			global::Kampai.Wrappers.NativeLibContext.NativeMethods.lua_kampai_set_log_func(global::System.IntPtr.Zero, null);
			global::Kampai.Wrappers.NativeLibContext.NativeMethods.lua_kampai_set_error_func(global::System.IntPtr.Zero, null);
#endif
			thisHandle.Free();
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
