namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	public class EventDispatcher : global::strange.framework.impl.Binder, global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher, global::strange.extensions.dispatcher.api.IDispatcher, global::strange.extensions.dispatcher.api.ITriggerProvider, global::strange.extensions.dispatcher.api.ITriggerable
	{
		protected global::System.Collections.Generic.HashSet<global::strange.extensions.dispatcher.api.ITriggerable> triggerClients;

		protected global::System.Collections.Generic.HashSet<global::strange.extensions.dispatcher.api.ITriggerable> triggerClientRemovals;

		protected bool isTriggeringClients;

		public static global::strange.extensions.pool.api.IPool<global::strange.extensions.dispatcher.eventdispatcher.impl.TmEvent> eventPool;

		public int Triggerables
		{
			get
			{
				if (triggerClients == null)
				{
					return 0;
				}
				return triggerClients.Count;
			}
		}

		public EventDispatcher()
		{
			if (eventPool == null)
			{
				eventPool = new global::strange.extensions.pool.impl.Pool<global::strange.extensions.dispatcher.eventdispatcher.impl.TmEvent>();
				eventPool.instanceProvider = new global::strange.extensions.dispatcher.eventdispatcher.impl.EventInstanceProvider();
			}
		}

		public override global::strange.framework.api.IBinding GetRawBinding()
		{
			return new global::strange.extensions.dispatcher.eventdispatcher.impl.EventBinding(resolver);
		}

		public new global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding Bind(object key)
		{
			return base.Bind(key) as global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding;
		}

		public void Dispatch(object eventType)
		{
			Dispatch(eventType, null);
		}

		public void Dispatch(object eventType, object data)
		{
			global::strange.extensions.dispatcher.eventdispatcher.api.IEvent obj = conformDataToEvent(eventType, data);
			if (obj is global::strange.extensions.pool.api.IPoolable)
			{
				(obj as global::strange.extensions.pool.api.IPoolable).Retain();
			}
			bool flag = true;
			if (triggerClients != null)
			{
				isTriggeringClients = true;
				foreach (global::strange.extensions.dispatcher.api.ITriggerable triggerClient in triggerClients)
				{
					try
					{
						if (!triggerClient.Trigger(obj.type, obj))
						{
							flag = false;
							break;
						}
					}
					catch (global::System.Exception)
					{
						internalReleaseEvent(obj);
						throw;
					}
				}
				if (triggerClientRemovals != null)
				{
					flushRemovals();
				}
				isTriggeringClients = false;
			}
			if (!flag)
			{
				internalReleaseEvent(obj);
				return;
			}
			global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding eventBinding = GetBinding(obj.type) as global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding;
			if (eventBinding == null)
			{
				internalReleaseEvent(obj);
				return;
			}
			object[] array = (eventBinding.value as object[]).Clone() as object[];
			if (array == null)
			{
				internalReleaseEvent(obj);
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				object obj2 = array[i];
				if (obj2 == null)
				{
					continue;
				}
				array[i] = null;
				object[] array2 = eventBinding.value as object[];
				if (global::System.Array.IndexOf(array2, obj2) != -1)
				{
					if (obj2 is global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback)
					{
						invokeEventCallback(obj, obj2 as global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback);
					}
					else if (obj2 is global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback)
					{
						(obj2 as global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback)();
					}
				}
			}
			internalReleaseEvent(obj);
		}

		protected virtual global::strange.extensions.dispatcher.eventdispatcher.api.IEvent conformDataToEvent(object eventType, object data)
		{
			global::strange.extensions.dispatcher.eventdispatcher.api.IEvent obj = null;
			if (eventType == null)
			{
				throw new global::strange.extensions.dispatcher.eventdispatcher.impl.EventDispatcherException("Attempt to Dispatch to null.\ndata: " + data, global::strange.extensions.dispatcher.eventdispatcher.api.EventDispatcherExceptionType.EVENT_KEY_NULL);
			}
			if (eventType is global::strange.extensions.dispatcher.eventdispatcher.api.IEvent)
			{
				return (global::strange.extensions.dispatcher.eventdispatcher.api.IEvent)eventType;
			}
			if (data == null)
			{
				return createEvent(eventType, null);
			}
			if (data is global::strange.extensions.dispatcher.eventdispatcher.api.IEvent)
			{
				return (global::strange.extensions.dispatcher.eventdispatcher.api.IEvent)data;
			}
			return createEvent(eventType, data);
		}

		protected virtual global::strange.extensions.dispatcher.eventdispatcher.api.IEvent createEvent(object eventType, object data)
		{
			global::strange.extensions.dispatcher.eventdispatcher.api.IEvent instance = eventPool.GetInstance();
			instance.type = eventType;
			instance.target = this;
			instance.data = data;
			return instance;
		}

		protected virtual void invokeEventCallback(object data, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback)
		{
			try
			{
				callback(data as global::strange.extensions.dispatcher.eventdispatcher.api.IEvent);
			}
			catch (global::System.InvalidCastException)
			{
				object target = callback.Target;
				string name = callback.Method.Name;
				string message = string.Concat("An EventCallback is attempting an illegal cast. One possible reason is not typing the payload to IEvent in your callback. Another is illegal casting of the data.\nTarget class: ", target, " method: ", name);
				throw new global::strange.extensions.dispatcher.eventdispatcher.impl.EventDispatcherException(message, global::strange.extensions.dispatcher.eventdispatcher.api.EventDispatcherExceptionType.TARGET_INVOCATION);
			}
		}

		public void AddListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback)
		{
			global::strange.framework.api.IBinding binding = GetBinding(evt);
			if (binding == null)
			{
				Bind(evt).To(callback);
			}
			else
			{
				binding.To(callback);
			}
		}

		public void AddListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback)
		{
			global::strange.framework.api.IBinding binding = GetBinding(evt);
			if (binding == null)
			{
				Bind(evt).To(callback);
			}
			else
			{
				binding.To(callback);
			}
		}

		public void RemoveListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback)
		{
			global::strange.framework.api.IBinding binding = GetBinding(evt);
			RemoveValue(binding, callback);
		}

		public void RemoveListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback)
		{
			global::strange.framework.api.IBinding binding = GetBinding(evt);
			RemoveValue(binding, callback);
		}

		public bool HasListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback)
		{
			global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding eventBinding = GetBinding(evt) as global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding;
			if (eventBinding == null)
			{
				return false;
			}
			return eventBinding.TypeForCallback(callback) != global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType.NOT_FOUND;
		}

		public bool HasListener(object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback)
		{
			global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding eventBinding = GetBinding(evt) as global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding;
			if (eventBinding == null)
			{
				return false;
			}
			return eventBinding.TypeForCallback(callback) != global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType.NOT_FOUND;
		}

		public void UpdateListener(bool toAdd, object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback)
		{
			if (toAdd)
			{
				AddListener(evt, callback);
			}
			else
			{
				RemoveListener(evt, callback);
			}
		}

		public void UpdateListener(bool toAdd, object evt, global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback)
		{
			if (toAdd)
			{
				AddListener(evt, callback);
			}
			else
			{
				RemoveListener(evt, callback);
			}
		}

		public void AddTriggerable(global::strange.extensions.dispatcher.api.ITriggerable target)
		{
			if (triggerClients == null)
			{
				triggerClients = new global::System.Collections.Generic.HashSet<global::strange.extensions.dispatcher.api.ITriggerable>();
			}
			triggerClients.Add(target);
		}

		public void RemoveTriggerable(global::strange.extensions.dispatcher.api.ITriggerable target)
		{
			if (triggerClients.Contains(target))
			{
				if (triggerClientRemovals == null)
				{
					triggerClientRemovals = new global::System.Collections.Generic.HashSet<global::strange.extensions.dispatcher.api.ITriggerable>();
				}
				triggerClientRemovals.Add(target);
				if (!isTriggeringClients)
				{
					flushRemovals();
				}
			}
		}

		protected void flushRemovals()
		{
			if (triggerClientRemovals == null)
			{
				return;
			}
			foreach (global::strange.extensions.dispatcher.api.ITriggerable triggerClientRemoval in triggerClientRemovals)
			{
				if (triggerClients.Contains(triggerClientRemoval))
				{
					triggerClients.Remove(triggerClientRemoval);
				}
			}
			triggerClientRemovals = null;
		}

		public bool Trigger<T>(object data)
		{
			return Trigger(typeof(T), data);
		}

		public bool Trigger(object key, object data)
		{
			if ((data is global::strange.extensions.dispatcher.eventdispatcher.api.IEvent && !object.ReferenceEquals((data as global::strange.extensions.dispatcher.eventdispatcher.api.IEvent).target, this)) || (key is global::strange.extensions.dispatcher.eventdispatcher.api.IEvent && !object.ReferenceEquals((data as global::strange.extensions.dispatcher.eventdispatcher.api.IEvent).target, this)))
			{
				Dispatch(key, data);
			}
			return true;
		}

		protected void internalReleaseEvent(global::strange.extensions.dispatcher.eventdispatcher.api.IEvent evt)
		{
			if (evt is global::strange.extensions.pool.api.IPoolable)
			{
				(evt as global::strange.extensions.pool.api.IPoolable).Release();
			}
		}

		public void ReleaseEvent(global::strange.extensions.dispatcher.eventdispatcher.api.IEvent evt)
		{
			if (!(evt as global::strange.extensions.pool.api.IPoolable).retain)
			{
				cleanEvent(evt);
				eventPool.ReturnInstance(evt);
			}
		}

		protected void cleanEvent(global::strange.extensions.dispatcher.eventdispatcher.api.IEvent evt)
		{
			evt.target = null;
			evt.data = null;
			evt.type = null;
		}
	}
}
