namespace GooglePlayGames.Native.PInvoke
{
	internal static class Callbacks
	{
		internal enum Type
		{
			Permanent = 0,
			Temporary = 1
		}

		internal delegate void ShowUICallbackInternal(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus status, global::System.IntPtr data);

		internal static readonly global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> NoopUICallback = delegate(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus status)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Received UI callback: " + status);
		};

		internal static global::System.IntPtr ToIntPtr<T>(global::System.Action<T> callback, global::System.Func<global::System.IntPtr, T> conversionFunction) where T : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			global::System.Action<global::System.IntPtr> callback2 = delegate(global::System.IntPtr result)
			{
				using (T obj = conversionFunction(result))
				{
					if (callback != null)
					{
						callback(obj);
					}
				}
			};
			return ToIntPtr(callback2);
		}

		internal static global::System.IntPtr ToIntPtr<T, P>(global::System.Action<T, P> callback, global::System.Func<global::System.IntPtr, P> conversionFunction) where P : global::GooglePlayGames.Native.PInvoke.BaseReferenceHolder
		{
			global::System.Action<T, global::System.IntPtr> callback2 = delegate(T param1, global::System.IntPtr param2)
			{
				using (P arg = conversionFunction(param2))
				{
					if (callback != null)
					{
						callback(param1, arg);
					}
				}
			};
			return ToIntPtr(callback2);
		}

		internal static global::System.IntPtr ToIntPtr(global::System.Delegate callback)
		{
			if ((object)callback == null)
			{
				return global::System.IntPtr.Zero;
			}
			global::System.Runtime.InteropServices.GCHandle value = global::System.Runtime.InteropServices.GCHandle.Alloc(callback);
			return global::System.Runtime.InteropServices.GCHandle.ToIntPtr(value);
		}

		internal static T IntPtrToTempCallback<T>(global::System.IntPtr handle) where T : class
		{
			return IntPtrToCallback<T>(handle, true);
		}

		private static T IntPtrToCallback<T>(global::System.IntPtr handle, bool unpinHandle) where T : class
		{
			if (global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.IsNull(handle))
			{
				return (T)null;
			}
			global::System.Runtime.InteropServices.GCHandle gCHandle = global::System.Runtime.InteropServices.GCHandle.FromIntPtr(handle);
			try
			{
				return (T)gCHandle.Target;
			}
			catch (global::System.InvalidCastException ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("GC Handle pointed to unexpected type: " + gCHandle.Target.ToString() + ". Expected " + typeof(T));
				throw ex;
			}
			finally
			{
				if (unpinHandle)
				{
					gCHandle.Free();
				}
			}
		}

		internal static T IntPtrToPermanentCallback<T>(global::System.IntPtr handle) where T : class
		{
			return IntPtrToCallback<T>(handle, false);
		}

		[global::AOT.MonoPInvokeCallback(typeof(global::GooglePlayGames.Native.PInvoke.Callbacks.ShowUICallbackInternal))]
		internal static void InternalShowUICallback(global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus status, global::System.IntPtr data)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Showing UI Internal callback: " + status);
			global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus> action = IntPtrToTempCallback<global::System.Action<global::GooglePlayGames.Native.Cwrapper.CommonErrorStatus.UIStatus>>(data);
			try
			{
				action(status);
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing InternalShowAllUICallback. Smothering to avoid passing exception into Native: " + ex);
			}
		}

		internal static void PerformInternalCallback(string callbackName, global::GooglePlayGames.Native.PInvoke.Callbacks.Type callbackType, global::System.IntPtr response, global::System.IntPtr userData)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Entering internal callback for " + callbackName);
			global::System.Action<global::System.IntPtr> action = ((callbackType != global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Permanent) ? IntPtrToTempCallback<global::System.Action<global::System.IntPtr>>(userData) : IntPtrToPermanentCallback<global::System.Action<global::System.IntPtr>>(userData));
			if (action == null)
			{
				return;
			}
			try
			{
				action(response);
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing " + callbackName + ". Smothering to avoid passing exception into Native: " + ex);
			}
		}

		internal static void PerformInternalCallback<T>(string callbackName, global::GooglePlayGames.Native.PInvoke.Callbacks.Type callbackType, T param1, global::System.IntPtr param2, global::System.IntPtr userData)
		{
			global::GooglePlayGames.OurUtils.Logger.d("Entering internal callback for " + callbackName);
			global::System.Action<T, global::System.IntPtr> action = null;
			try
			{
				action = ((callbackType != global::GooglePlayGames.Native.PInvoke.Callbacks.Type.Permanent) ? IntPtrToTempCallback<global::System.Action<T, global::System.IntPtr>>(userData) : IntPtrToPermanentCallback<global::System.Action<T, global::System.IntPtr>>(userData));
			}
			catch (global::System.Exception ex)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered converting " + callbackName + ". Smothering to avoid passing exception into Native: " + ex);
				return;
			}
			global::GooglePlayGames.OurUtils.Logger.d("Internal Callback converted to action");
			if (action == null)
			{
				return;
			}
			try
			{
				action(param1, param2);
			}
			catch (global::System.Exception ex2)
			{
				global::GooglePlayGames.OurUtils.Logger.e("Error encountered executing " + callbackName + ". Smothering to avoid passing exception into Native: " + ex2);
			}
		}

		internal static global::System.Action<T> AsOnGameThreadCallback<T>(global::System.Action<T> toInvokeOnGameThread)
		{
			return delegate(T result)
			{
				if (toInvokeOnGameThread != null)
				{
					global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
					{
						toInvokeOnGameThread(result);
					});
				}
			};
		}

		internal static global::System.Action<T1, T2> AsOnGameThreadCallback<T1, T2>(global::System.Action<T1, T2> toInvokeOnGameThread)
		{
			return delegate(T1 result1, T2 result2)
			{
				if (toInvokeOnGameThread != null)
				{
					global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunOnGameThread(delegate
					{
						toInvokeOnGameThread(result1, result2);
					});
				}
			};
		}

		internal static void AsCoroutine(global::System.Collections.IEnumerator routine)
		{
			global::GooglePlayGames.OurUtils.PlayGamesHelperObject.RunCoroutine(routine);
		}

		internal static byte[] IntPtrAndSizeToByteArray(global::System.IntPtr data, global::System.UIntPtr dataLength)
		{
			if (dataLength.ToUInt64() == 0L)
			{
				return null;
			}
			byte[] array = new byte[dataLength.ToUInt32()];
			global::System.Runtime.InteropServices.Marshal.Copy(data, array, 0, (int)dataLength.ToUInt32());
			return array;
		}
	}
}
