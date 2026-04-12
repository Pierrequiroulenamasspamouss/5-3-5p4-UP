namespace Prime31
{
	public static class ActionExtensions
	{
		private static void invoke(global::System.Delegate listener, object[] args)
		{
			if (!listener.Method.IsStatic && (listener.Target == null || listener.Target.Equals(null)))
			{
				global::UnityEngine.Debug.LogError("an event listener is still subscribed to an event with the method " + listener.Method.Name + " even though it is null. Be sure to balance your event subscriptions.");
			}
			else
			{
				listener.Method.Invoke(listener.Target, args);
			}
		}

		public static void fire(this global::System.Action handler)
		{
			if (handler != null)
			{
				object[] args = new object[0];
				global::System.Delegate[] invocationList = handler.GetInvocationList();
				foreach (global::System.Delegate listener in invocationList)
				{
					invoke(listener, args);
				}
			}
		}

		public static void fire<T>(this global::System.Action<T> handler, T param)
		{
			if (handler != null)
			{
				object[] args = new object[1] { param };
				global::System.Delegate[] invocationList = handler.GetInvocationList();
				foreach (global::System.Delegate listener in invocationList)
				{
					invoke(listener, args);
				}
			}
		}

		public static void fire<T, U>(this global::System.Action<T, U> handler, T param1, U param2)
		{
			if (handler != null)
			{
				object[] args = new object[2] { param1, param2 };
				global::System.Delegate[] invocationList = handler.GetInvocationList();
				foreach (global::System.Delegate listener in invocationList)
				{
					invoke(listener, args);
				}
			}
		}

		public static void fire<T, U, V>(this global::System.Action<T, U, V> handler, T param1, U param2, V param3)
		{
			if (handler != null)
			{
				object[] args = new object[3] { param1, param2, param3 };
				global::System.Delegate[] invocationList = handler.GetInvocationList();
				foreach (global::System.Delegate listener in invocationList)
				{
					invoke(listener, args);
				}
			}
		}
	}
}
