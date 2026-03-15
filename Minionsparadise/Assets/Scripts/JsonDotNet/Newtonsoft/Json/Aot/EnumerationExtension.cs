namespace Newtonsoft.Json.Aot
{
	public static class EnumerationExtension
	{
		public static void ForEach<T>(this global::System.Collections.Generic.IEnumerable<T> enumerable, global::System.Action<T> action)
		{
			if (enumerable == null)
			{
				return;
			}
			global::System.Type typeFromHandle = typeof(global::System.Collections.IEnumerable);
			if (!global::System.Linq.Enumerable.Contains(enumerable.GetType().GetInterfaces(), typeFromHandle))
			{
				throw new global::System.ArgumentException("Object does not implement IEnumerable", "enumerable");
			}
			global::System.Reflection.MethodInfo method = typeFromHandle.GetMethod("GetEnumerator");
			if (method == null)
			{
				throw new global::System.InvalidOperationException("Failed to get 'GetEnumerator()' method from IEnumerable type");
			}
			global::System.Collections.IEnumerator enumerator = null;
			try
			{
				enumerator = (global::System.Collections.IEnumerator)method.Invoke(enumerable, null);
				if (enumerator != null)
				{
					while (enumerator.MoveNext())
					{
						action((T)enumerator.Current);
					}
				}
				else
				{
					global::UnityEngine.Debug.Log(string.Format("{0}.GetEnumerator() returned 'null' instead of IEnumerator.", enumerable.ToString()));
				}
			}
			finally
			{
				global::System.IDisposable disposable = enumerator as global::System.IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}
