namespace strange.extensions.signal.impl
{
	public class Signal : global::strange.extensions.signal.impl.BaseSignal
	{
		private global::System.Action Listener = delegate
		{
		};

		private global::System.Action OnceListener;

		public virtual void AddListener(global::System.Action callback)
		{
			Listener = AddUnique(Listener, callback);
		}

		public virtual void AddOnce(global::System.Action callback)
		{
			OnceListener = AddUnique(OnceListener, callback);
		}

		public virtual void RemoveListener(global::System.Action callback)
		{
			Listener = (global::System.Action)global::System.Delegate.Remove(Listener, callback);
		}

		public override global::System.Collections.Generic.List<global::System.Type> GetTypes()
		{
			return global::strange.extensions.signal.impl.BaseSignal.emptyTypesList;
		}

		public virtual void Dispatch()
		{
			Listener();
			CallAndClearOnceListener();
			if (base.NonEmpty)
			{
				base.Dispatch(null);
			}
		}

		private global::System.Action AddUnique(global::System.Action listeners, global::System.Action callback)
		{
			if (listeners == null)
			{
				listeners = delegate
				{
				};
			}
			if (!global::System.Linq.Enumerable.Contains(listeners.GetInvocationList(), callback))
			{
				listeners = (global::System.Action)global::System.Delegate.Combine(listeners, callback);
			}
			return listeners;
		}

		private void CallAndClearOnceListener()
		{
			if (OnceListener != null)
			{
				global::System.Action onceListener = OnceListener;
				OnceListener = null;
				onceListener();
			}
		}
	}
	public class Signal<T> : global::strange.extensions.signal.impl.BaseSignal
	{
		private global::System.Action<T> Listener = delegate
		{
		};

		private global::System.Action<T> OnceListener;

		private global::System.Collections.Generic.List<global::System.Type> typesList = new global::System.Collections.Generic.List<global::System.Type> { typeof(T) };

		public virtual void AddListener(global::System.Action<T> callback)
		{
			Listener = AddUnique(Listener, callback);
		}

		public virtual void AddOnce(global::System.Action<T> callback)
		{
			OnceListener = AddUnique(OnceListener, callback);
		}

		public virtual void RemoveListener(global::System.Action<T> callback)
		{
			Listener = (global::System.Action<T>)global::System.Delegate.Remove(Listener, callback);
		}

		public override global::System.Collections.Generic.List<global::System.Type> GetTypes()
		{
			return typesList;
		}

		public virtual void Dispatch(T type1)
		{
			Listener(type1);
			CallAndClearOnceListener(type1);
			if (base.NonEmpty)
			{
				object[] args = new object[1] { type1 };
				base.Dispatch(args);
			}
		}

		private global::System.Action<T> AddUnique(global::System.Action<T> listeners, global::System.Action<T> callback)
		{
			if (listeners == null)
			{
				listeners = delegate
				{
				};
			}
			if (!global::System.Linq.Enumerable.Contains(listeners.GetInvocationList(), callback))
			{
				listeners = (global::System.Action<T>)global::System.Delegate.Combine(listeners, callback);
			}
			return listeners;
		}

		private void CallAndClearOnceListener(T type1)
		{
			if (OnceListener != null)
			{
				global::System.Action<T> onceListener = OnceListener;
				OnceListener = null;
				onceListener(type1);
			}
		}
	}
	public class Signal<T, U> : global::strange.extensions.signal.impl.BaseSignal
	{
		private global::System.Action<T, U> Listener = delegate
		{
		};

		private global::System.Action<T, U> OnceListener;

		private global::System.Collections.Generic.List<global::System.Type> typesList = new global::System.Collections.Generic.List<global::System.Type>
		{
			typeof(T),
			typeof(U)
		};

		public virtual void AddListener(global::System.Action<T, U> callback)
		{
			Listener = AddUnique(Listener, callback);
		}

		public virtual void AddOnce(global::System.Action<T, U> callback)
		{
			OnceListener = AddUnique(OnceListener, callback);
		}

		public virtual void RemoveListener(global::System.Action<T, U> callback)
		{
			Listener = (global::System.Action<T, U>)global::System.Delegate.Remove(Listener, callback);
		}

		public override global::System.Collections.Generic.List<global::System.Type> GetTypes()
		{
			return typesList;
		}

		public virtual void Dispatch(T type1, U type2)
		{
			Listener(type1, type2);
			CallAndClearOnceListener(type1, type2);
			if (base.NonEmpty)
			{
				object[] args = new object[2] { type1, type2 };
				base.Dispatch(args);
			}
		}

		private global::System.Action<T, U> AddUnique(global::System.Action<T, U> listeners, global::System.Action<T, U> callback)
		{
			if (listeners == null)
			{
				listeners = delegate
				{
				};
			}
			if (!global::System.Linq.Enumerable.Contains(listeners.GetInvocationList(), callback))
			{
				listeners = (global::System.Action<T, U>)global::System.Delegate.Combine(listeners, callback);
			}
			return listeners;
		}

		private void CallAndClearOnceListener(T type1, U type2)
		{
			if (OnceListener != null)
			{
				global::System.Action<T, U> onceListener = OnceListener;
				OnceListener = delegate
				{
				};
				onceListener(type1, type2);
			}
		}
	}
	public class Signal<T, U, V> : global::strange.extensions.signal.impl.BaseSignal
	{
		private global::System.Action<T, U, V> Listener = delegate
		{
		};

		private global::System.Action<T, U, V> OnceListener;

		private global::System.Collections.Generic.List<global::System.Type> typesList = new global::System.Collections.Generic.List<global::System.Type>
		{
			typeof(T),
			typeof(U),
			typeof(V)
		};

		public virtual void AddListener(global::System.Action<T, U, V> callback)
		{
			Listener = AddUnique(Listener, callback);
		}

		public virtual void AddOnce(global::System.Action<T, U, V> callback)
		{
			OnceListener = AddUnique(OnceListener, callback);
		}

		public virtual void RemoveListener(global::System.Action<T, U, V> callback)
		{
			Listener = (global::System.Action<T, U, V>)global::System.Delegate.Remove(Listener, callback);
		}

		public override global::System.Collections.Generic.List<global::System.Type> GetTypes()
		{
			return typesList;
		}

		public virtual void Dispatch(T type1, U type2, V type3)
		{
			Listener(type1, type2, type3);
			CallAndClearOnceListener(type1, type2, type3);
			if (base.NonEmpty)
			{
				object[] args = new object[3] { type1, type2, type3 };
				base.Dispatch(args);
			}
		}

		private global::System.Action<T, U, V> AddUnique(global::System.Action<T, U, V> listeners, global::System.Action<T, U, V> callback)
		{
			if (listeners == null)
			{
				listeners = delegate
				{
				};
			}
			if (!global::System.Linq.Enumerable.Contains(listeners.GetInvocationList(), callback))
			{
				listeners = (global::System.Action<T, U, V>)global::System.Delegate.Combine(listeners, callback);
			}
			return listeners;
		}

		private void CallAndClearOnceListener(T type1, U type2, V type3)
		{
			if (OnceListener != null)
			{
				global::System.Action<T, U, V> onceListener = OnceListener;
				OnceListener = null;
				onceListener(type1, type2, type3);
			}
		}
	}
	public class Signal<T, U, V, W> : global::strange.extensions.signal.impl.BaseSignal
	{
		private global::System.Action<T, U, V, W> Listener = delegate
		{
		};

		private global::System.Action<T, U, V, W> OnceListener;

		private global::System.Collections.Generic.List<global::System.Type> typesList = new global::System.Collections.Generic.List<global::System.Type>
		{
			typeof(T),
			typeof(U),
			typeof(V),
			typeof(W)
		};

		public virtual void AddListener(global::System.Action<T, U, V, W> callback)
		{
			Listener = AddUnique(Listener, callback);
		}

		public virtual void AddOnce(global::System.Action<T, U, V, W> callback)
		{
			OnceListener = AddUnique(OnceListener, callback);
		}

		public virtual void RemoveListener(global::System.Action<T, U, V, W> callback)
		{
			Listener = (global::System.Action<T, U, V, W>)global::System.Delegate.Remove(Listener, callback);
		}

		public override global::System.Collections.Generic.List<global::System.Type> GetTypes()
		{
			return typesList;
		}

		public virtual void Dispatch(T type1, U type2, V type3, W type4)
		{
			Listener(type1, type2, type3, type4);
			CallAndClearOnceListener(type1, type2, type3, type4);
			if (base.NonEmpty)
			{
				object[] args = new object[4] { type1, type2, type3, type4 };
				base.Dispatch(args);
			}
		}

		private global::System.Action<T, U, V, W> AddUnique(global::System.Action<T, U, V, W> listeners, global::System.Action<T, U, V, W> callback)
		{
			if (listeners == null)
			{
				listeners = delegate
				{
				};
			}
			if (!global::System.Linq.Enumerable.Contains(listeners.GetInvocationList(), callback))
			{
				listeners = (global::System.Action<T, U, V, W>)global::System.Delegate.Combine(listeners, callback);
			}
			return listeners;
		}

		private void CallAndClearOnceListener(T type1, U type2, V type3, W type4)
		{
			if (OnceListener != null)
			{
				global::System.Action<T, U, V, W> onceListener = OnceListener;
				OnceListener = delegate
				{
				};
				onceListener(type1, type2, type3, type4);
			}
		}
	}
}
