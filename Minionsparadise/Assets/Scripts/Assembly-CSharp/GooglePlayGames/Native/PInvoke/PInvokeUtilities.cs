namespace GooglePlayGames.Native.PInvoke
{
	internal static class PInvokeUtilities
	{
		internal delegate global::System.UIntPtr OutStringMethod(global::System.Text.StringBuilder out_string, global::System.UIntPtr out_size);

		internal delegate global::System.UIntPtr OutMethod<T>([global::System.Runtime.InteropServices.In][global::System.Runtime.InteropServices.Out] T[] out_bytes, global::System.UIntPtr out_size);

		private static readonly global::System.DateTime UnixEpoch = global::System.DateTime.SpecifyKind(new global::System.DateTime(1970, 1, 1), global::System.DateTimeKind.Utc);

		internal static global::System.Runtime.InteropServices.HandleRef CheckNonNull(global::System.Runtime.InteropServices.HandleRef reference)
		{
			if (IsNull(reference))
			{
				throw new global::System.InvalidOperationException();
			}
			return reference;
		}

		internal static bool IsNull(global::System.Runtime.InteropServices.HandleRef reference)
		{
			return IsNull(global::System.Runtime.InteropServices.HandleRef.ToIntPtr(reference));
		}

		internal static bool IsNull(global::System.IntPtr pointer)
		{
			return pointer.Equals(global::System.IntPtr.Zero);
		}

		internal static global::System.DateTime FromMillisSinceUnixEpoch(long millisSinceEpoch)
		{
			return UnixEpoch.Add(global::System.TimeSpan.FromMilliseconds(millisSinceEpoch));
		}

		internal static string OutParamsToString(global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutStringMethod outStringMethod)
		{
			global::System.UIntPtr out_size = outStringMethod(null, global::System.UIntPtr.Zero);
			if (out_size.Equals(global::System.UIntPtr.Zero))
			{
				return null;
			}
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder((int)out_size.ToUInt32());
			outStringMethod(stringBuilder, out_size);
			return stringBuilder.ToString();
		}

		internal static T[] OutParamsToArray<T>(global::GooglePlayGames.Native.PInvoke.PInvokeUtilities.OutMethod<T> outMethod)
		{
			global::System.UIntPtr out_size = outMethod(null, global::System.UIntPtr.Zero);
			if (out_size.Equals(global::System.UIntPtr.Zero))
			{
				return new T[0];
			}
			T[] array = new T[out_size.ToUInt64()];
			outMethod(array, out_size);
			return array;
		}

		internal static global::System.Collections.Generic.IEnumerable<T> ToEnumerable<T>(global::System.UIntPtr size, global::System.Func<global::System.UIntPtr, T> getElement)
		{
			for (ulong i = 0uL; i < size.ToUInt64(); i++)
			{
				yield return getElement(new global::System.UIntPtr(i));
			}
		}

		internal static global::System.Collections.Generic.IEnumerator<T> ToEnumerator<T>(global::System.UIntPtr size, global::System.Func<global::System.UIntPtr, T> getElement)
		{
			return ToEnumerable<T>(size, getElement).GetEnumerator();
		}

		internal static global::System.UIntPtr ArrayToSizeT<T>(T[] array)
		{
			if (array == null)
			{
				return global::System.UIntPtr.Zero;
			}
			return new global::System.UIntPtr((ulong)array.Length);
		}

		internal static long ToMilliseconds(global::System.TimeSpan span)
		{
			double totalMilliseconds = span.TotalMilliseconds;
			if (totalMilliseconds > 9.223372036854776E+18)
			{
				return long.MaxValue;
			}
			if (totalMilliseconds < -9.223372036854776E+18)
			{
				return long.MinValue;
			}
			return global::System.Convert.ToInt64(totalMilliseconds);
		}
	}
}
