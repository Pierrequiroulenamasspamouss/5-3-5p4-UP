namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	public class EventBinding : global::strange.framework.impl.Binding, global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding, global::strange.framework.api.IBinding
	{
		private global::System.Collections.Generic.Dictionary<global::System.Delegate, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType> callbackTypes;

		public EventBinding()
			: this(null)
		{
		}

		public EventBinding(global::strange.framework.impl.Binder.BindingResolver resolver)
			: base(resolver)
		{
			base.keyConstraint = global::strange.framework.api.BindingConstraintType.ONE;
			base.valueConstraint = global::strange.framework.api.BindingConstraintType.MANY;
			callbackTypes = new global::System.Collections.Generic.Dictionary<global::System.Delegate, global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType>();
		}

		public global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType TypeForCallback(global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback callback)
		{
			if (callbackTypes.ContainsKey(callback))
			{
				return callbackTypes[callback];
			}
			return global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType.NOT_FOUND;
		}

		public global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType TypeForCallback(global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback callback)
		{
			if (callbackTypes.ContainsKey(callback))
			{
				return callbackTypes[callback];
			}
			return global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType.NOT_FOUND;
		}

		public new global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding Bind(object key)
		{
			return base.Bind(key) as global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding;
		}

		public global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding To(global::strange.extensions.dispatcher.eventdispatcher.api.EventCallback value)
		{
			base.To(value);
			storeMethodType(value);
			return this;
		}

		public global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding To(global::strange.extensions.dispatcher.eventdispatcher.api.EmptyCallback value)
		{
			base.To(value);
			storeMethodType(value);
			return this;
		}

		public new global::strange.extensions.dispatcher.eventdispatcher.api.IEventBinding To(object value)
		{
			base.To(value);
			storeMethodType(value as global::System.Delegate);
			return this;
		}

		public override void RemoveValue(object value)
		{
			base.RemoveValue(value);
			callbackTypes.Remove(value as global::System.Delegate);
		}

		private void storeMethodType(global::System.Delegate value)
		{
			if ((object)value == null)
			{
				throw new global::strange.extensions.dispatcher.impl.DispatcherException("EventDispatcher can't map something that isn't a delegate'", global::strange.extensions.dispatcher.api.DispatcherExceptionType.ILLEGAL_CALLBACK_HANDLER);
			}
			global::System.Reflection.MethodInfo method = value.Method;
			switch (method.GetParameters().Length)
			{
			case 0:
				callbackTypes[value] = global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType.NO_ARGUMENTS;
				break;
			case 1:
				callbackTypes[value] = global::strange.extensions.dispatcher.eventdispatcher.api.EventCallbackType.ONE_ARGUMENT;
				break;
			default:
				throw new global::strange.extensions.dispatcher.impl.DispatcherException("Event callbacks must have either one or no arguments", global::strange.extensions.dispatcher.api.DispatcherExceptionType.ILLEGAL_CALLBACK_HANDLER);
			}
		}
	}
}
