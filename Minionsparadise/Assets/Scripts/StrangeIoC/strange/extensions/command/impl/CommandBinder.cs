namespace strange.extensions.command.impl
{
	public class CommandBinder : global::strange.framework.impl.Binder, global::strange.extensions.command.api.ICommandBinder, global::strange.framework.api.IBinder, global::strange.extensions.command.api.IPooledCommandBinder, global::strange.extensions.dispatcher.api.ITriggerable
	{
		protected global::System.Collections.Generic.Dictionary<global::System.Type, global::strange.extensions.pool.impl.Pool> pools = new global::System.Collections.Generic.Dictionary<global::System.Type, global::strange.extensions.pool.impl.Pool>();

		protected global::System.Collections.Generic.HashSet<global::strange.extensions.command.api.ICommand> activeCommands = new global::System.Collections.Generic.HashSet<global::strange.extensions.command.api.ICommand>();

		protected global::System.Collections.Generic.Dictionary<global::strange.extensions.command.api.ICommand, global::strange.extensions.command.api.ICommandBinding> activeSequences = new global::System.Collections.Generic.Dictionary<global::strange.extensions.command.api.ICommand, global::strange.extensions.command.api.ICommandBinding>();

		[Inject]
		public global::strange.extensions.injector.api.IInjectionBinder injectionBinder { get; set; }

		public bool usePooling { get; set; }

		public CommandBinder()
		{
			usePooling = true;
		}

		public override global::strange.framework.api.IBinding GetRawBinding()
		{
			return new global::strange.extensions.command.impl.CommandBinding(resolver);
		}

		public virtual void ReactTo(object trigger)
		{
			ReactTo(trigger, null);
		}

		public virtual void ReactTo(object trigger, object data)
		{
			if (data is global::strange.extensions.pool.api.IPoolable)
			{
				(data as global::strange.extensions.pool.api.IPoolable).Retain();
			}
			global::strange.extensions.command.api.ICommandBinding commandBinding = GetBinding(trigger) as global::strange.extensions.command.api.ICommandBinding;
			if (commandBinding == null)
			{
				return;
			}
			if (commandBinding.isSequence)
			{
				next(commandBinding, data, 0);
				return;
			}
			object[] array = commandBinding.value as object[];
			int num = array.Length + 1;
			for (int i = 0; i < num; i++)
			{
				next(commandBinding, data, i);
			}
		}

		protected void next(global::strange.extensions.command.api.ICommandBinding binding, object data, int depth)
		{
			object[] array = binding.value as object[];
			if (depth < array.Length)
			{
				global::System.Type cmd = array[depth] as global::System.Type;
				global::strange.extensions.command.api.ICommand command = invokeCommand(cmd, binding, data, depth);
				ReleaseCommand(command);
				return;
			}
			disposeOfSequencedData(data);
			if (binding.isOneOff)
			{
				Unbind(binding);
			}
		}

		protected virtual void disposeOfSequencedData(object data)
		{
		}

		protected virtual global::strange.extensions.command.api.ICommand invokeCommand(global::System.Type cmd, global::strange.extensions.command.api.ICommandBinding binding, object data, int depth)
		{
			global::strange.extensions.command.api.ICommand command = createCommand(cmd, data);
			command.sequenceId = depth;
			trackCommand(command, binding);
			executeCommand(command);
			return command;
		}

		protected virtual global::strange.extensions.command.api.ICommand createCommand(object cmd, object data)
		{
			global::strange.extensions.command.api.ICommand command = getCommand(cmd as global::System.Type);
			if (command == null)
			{
				string text = "A Command ";
				if (data != null)
				{
					text = text + "tied to data " + data.ToString();
				}
				text += " could not be instantiated.\nThis might be caused by a null pointer during instantiation or failing to override Execute (generally you shouldn't have constructor code in Commands).";
				throw new global::strange.extensions.command.impl.CommandException(text, global::strange.extensions.command.api.CommandExceptionType.BAD_CONSTRUCTOR);
			}
			command.data = data;
			return command;
		}

		protected global::strange.extensions.command.api.ICommand getCommand(global::System.Type type)
		{
			if (usePooling && pools.ContainsKey(type))
			{
				global::strange.extensions.pool.impl.Pool pool = pools[type];
				global::strange.extensions.command.api.ICommand command = pool.GetInstance() as global::strange.extensions.command.api.ICommand;
				if (command.IsClean)
				{
					injectionBinder.injector.Inject(command);
					command.IsClean = false;
				}
				return command;
			}
			injectionBinder.Bind<global::strange.extensions.command.api.ICommand>().To(type);
			try
			{
				return injectionBinder.GetInstance<global::strange.extensions.command.api.ICommand>();
			}
			finally
			{
				injectionBinder.Unbind<global::strange.extensions.command.api.ICommand>();
			}
		}

		protected void trackCommand(global::strange.extensions.command.api.ICommand command, global::strange.extensions.command.api.ICommandBinding binding)
		{
			if (binding.isSequence)
			{
				activeSequences.Add(command, binding);
			}
			else
			{
				activeCommands.Add(command);
			}
		}

		protected void executeCommand(global::strange.extensions.command.api.ICommand command)
		{
			if (command != null)
			{
				command.Execute();
			}
		}

		public virtual void Stop(object key)
		{
			if (key is global::strange.extensions.command.api.ICommand && activeSequences.ContainsKey(key as global::strange.extensions.command.api.ICommand))
			{
				removeSequence(key as global::strange.extensions.command.api.ICommand);
				return;
			}
			global::strange.extensions.command.api.ICommandBinding commandBinding = GetBinding(key) as global::strange.extensions.command.api.ICommandBinding;
			if (commandBinding == null || !activeSequences.ContainsValue(commandBinding))
			{
				return;
			}
			foreach (global::System.Collections.Generic.KeyValuePair<global::strange.extensions.command.api.ICommand, global::strange.extensions.command.api.ICommandBinding> activeSequence in activeSequences)
			{
				if (activeSequence.Value == commandBinding)
				{
					global::strange.extensions.command.api.ICommand key2 = activeSequence.Key;
					removeSequence(key2);
				}
			}
		}

		public virtual void ReleaseCommand(global::strange.extensions.command.api.ICommand command)
		{
			if (!command.retain)
			{
				global::System.Type type = command.GetType();
				if (usePooling && pools.ContainsKey(type))
				{
					pools[type].ReturnInstance(command);
				}
				if (activeCommands.Contains(command))
				{
					activeCommands.Remove(command);
				}
				else if (activeSequences.ContainsKey(command))
				{
					global::strange.extensions.command.api.ICommandBinding binding = activeSequences[command];
					object data = command.data;
					activeSequences.Remove(command);
					next(binding, data, command.sequenceId + 1);
				}
			}
		}

		public global::strange.extensions.pool.impl.Pool<T> GetPool<T>()
		{
			global::System.Type typeFromHandle = typeof(T);
			if (pools.ContainsKey(typeFromHandle))
			{
				return pools[typeFromHandle] as global::strange.extensions.pool.impl.Pool<T>;
			}
			return null;
		}

		private void removeSequence(global::strange.extensions.command.api.ICommand command)
		{
			if (activeSequences.ContainsKey(command))
			{
				command.Cancel();
				activeSequences.Remove(command);
			}
		}

		public bool Trigger<T>(object data)
		{
			return Trigger(typeof(T), data);
		}

		public bool Trigger(object key, object data)
		{
			ReactTo(key, data);
			return true;
		}

		public new virtual global::strange.extensions.command.api.ICommandBinding Bind<T>()
		{
			return base.Bind<T>() as global::strange.extensions.command.api.ICommandBinding;
		}

		public new virtual global::strange.extensions.command.api.ICommandBinding Bind(object value)
		{
			return base.Bind(value) as global::strange.extensions.command.api.ICommandBinding;
		}

		protected override void resolver(global::strange.framework.api.IBinding binding)
		{
			base.resolver(binding);
			if (!usePooling || !(binding as global::strange.extensions.command.api.ICommandBinding).isPooled || binding.value == null)
			{
				return;
			}
			object[] array = binding.value as object[];
			object[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				global::System.Type type = (global::System.Type)array2[i];
				if (!pools.ContainsKey(type))
				{
					global::strange.extensions.pool.impl.Pool value = makePoolFromType(type);
					pools[type] = value;
				}
			}
		}

		protected virtual global::strange.extensions.pool.impl.Pool makePoolFromType(global::System.Type type)
		{
			global::System.Type o = typeof(global::strange.extensions.pool.impl.Pool<>).MakeGenericType(type);
			injectionBinder.Bind(type).To(type);
			injectionBinder.Bind<global::strange.extensions.pool.impl.Pool>().To(o).ToName(global::strange.extensions.command.api.CommandKeys.COMMAND_POOL);
			global::strange.extensions.pool.impl.Pool instance = injectionBinder.GetInstance<global::strange.extensions.pool.impl.Pool>(global::strange.extensions.command.api.CommandKeys.COMMAND_POOL);
			injectionBinder.Unbind<global::strange.extensions.pool.impl.Pool>(global::strange.extensions.command.api.CommandKeys.COMMAND_POOL);
			return instance;
		}

		public new virtual global::strange.extensions.command.api.ICommandBinding GetBinding<T>()
		{
			return base.GetBinding<T>() as global::strange.extensions.command.api.ICommandBinding;
		}
	}
}
