namespace strange.extensions.command.impl
{
	public class SignalCommandBinder : global::strange.extensions.command.impl.CommandBinder
	{
		public override void ResolveBinding(global::strange.framework.api.IBinding binding, object key)
		{
			base.ResolveBinding(binding, key);
			if (bindings.ContainsKey(key))
			{
				global::strange.extensions.signal.api.IBaseSignal baseSignal = (global::strange.extensions.signal.api.IBaseSignal)key;
				baseSignal.AddListener(ReactTo);
			}
		}

		public override void OnRemove()
		{
			foreach (object key in bindings.Keys)
			{
				global::strange.extensions.signal.api.IBaseSignal baseSignal = (global::strange.extensions.signal.api.IBaseSignal)key;
				if (baseSignal != null)
				{
					baseSignal.RemoveListener(ReactTo);
				}
			}
		}

		protected override global::strange.extensions.command.api.ICommand invokeCommand(global::System.Type cmd, global::strange.extensions.command.api.ICommandBinding binding, object data, int depth)
		{
			global::strange.extensions.signal.api.IBaseSignal baseSignal = (global::strange.extensions.signal.api.IBaseSignal)binding.key;
			global::strange.extensions.command.api.ICommand command = createCommandForSignal(cmd, data, baseSignal.GetTypes());
			command.sequenceId = depth;
			trackCommand(command, binding);
			executeCommand(command);
			return command;
		}

		protected global::strange.extensions.command.api.ICommand createCommandForSignal(global::System.Type cmd, object data, global::System.Collections.Generic.List<global::System.Type> signalTypes)
		{
			try
			{
				if (data != null)
				{
					bindSignalData(cmd, data, signalTypes);
				}
				global::strange.extensions.command.api.ICommand command = getCommand(cmd);
				command.data = data;
				return command;
			}
			finally
			{
				foreach (global::System.Type signalType in signalTypes)
				{
					base.injectionBinder.Unbind(signalType);
				}
			}
		}

		private void bindSignalData(global::System.Type cmd, object data, global::System.Collections.Generic.List<global::System.Type> signalTypes)
		{
			object[] array = (object[])data;
			int count = signalTypes.Count;
			for (int i = 0; i < count; i++)
			{
				global::System.Type type = signalTypes[i];
				for (int j = i + 1; j < count; j++)
				{
					if (type.Equals(signalTypes[j]))
					{
						throw new global::strange.extensions.signal.impl.SignalException(string.Concat("SignalCommandBinder: You have attempted to map more than one value of type: ", type, " in Command: ", cmd.GetType(), ". Only the first value of a type will be injected. You may want to place your values in a VO, instead."), global::strange.extensions.signal.api.SignalExceptionType.COMMAND_VALUE_CONFLICT);
					}
				}
			}
			int num = array.Length;
			if (num != count)
			{
				throw new global::strange.extensions.signal.impl.SignalException("Values list does not match types list Command: " + cmd.GetType(), global::strange.extensions.signal.api.SignalExceptionType.COMMAND_CORRUPT_STATE);
			}
			for (int k = 0; k < count; k++)
			{
				global::System.Type type2 = signalTypes[k];
				object obj = array[k];
				if (obj != null)
				{
					if (type2.IsAssignableFrom(obj.GetType()))
					{
						base.injectionBinder.Bind(type2).ToValue(obj).ToInject(false);
						continue;
					}
					throw new global::strange.extensions.signal.impl.SignalException("Values list does not match types list Command: " + cmd.GetType(), global::strange.extensions.signal.api.SignalExceptionType.COMMAND_CORRUPT_STATE);
				}
				throw new global::strange.extensions.signal.impl.SignalException(string.Concat("SignalCommandBinder attempted to bind a null value from a signal to Command: ", cmd.GetType(), " to type: ", type2), global::strange.extensions.signal.api.SignalExceptionType.COMMAND_NULL_INJECTION);
			}
		}

		public override global::strange.extensions.command.api.ICommandBinding Bind<T>()
		{
			global::strange.extensions.injector.api.IInjectionBinding binding = base.injectionBinder.GetBinding<T>();
			if (binding == null)
			{
				base.injectionBinder.Bind<T>().ToSingleton();
			}
			T instance = base.injectionBinder.GetInstance<T>();
			return base.Bind(instance);
		}

		public override void Unbind<T>()
		{
			global::strange.extensions.command.api.ICommandBinding commandBinding = (global::strange.extensions.command.api.ICommandBinding)base.injectionBinder.GetBinding<T>();
			if (commandBinding != null)
			{
				T instance = base.injectionBinder.GetInstance<T>();
				Unbind(instance, null);
			}
		}

		public override void Unbind(object key, object name)
		{
			if (bindings.ContainsKey(key))
			{
				global::strange.extensions.signal.api.IBaseSignal baseSignal = (global::strange.extensions.signal.api.IBaseSignal)key;
				baseSignal.RemoveListener(ReactTo);
			}
			base.Unbind(key, name);
		}

		public override global::strange.extensions.command.api.ICommandBinding GetBinding<T>()
		{
			T instance = base.injectionBinder.GetInstance<T>();
			return base.GetBinding(instance) as global::strange.extensions.command.api.ICommandBinding;
		}
	}
}
