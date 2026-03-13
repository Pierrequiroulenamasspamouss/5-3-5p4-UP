namespace strange.extensions.sequencer.impl
{
	public class Sequencer : global::strange.extensions.command.impl.CommandBinder, global::strange.extensions.sequencer.api.ISequencer, global::strange.extensions.command.api.ICommandBinder, global::strange.framework.api.IBinder, global::strange.extensions.dispatcher.api.ITriggerable
	{
		public override global::strange.framework.api.IBinding GetRawBinding()
		{
			return new global::strange.extensions.sequencer.impl.SequenceBinding(resolver);
		}

		public override void ReactTo(object key, object data)
		{
			global::strange.extensions.sequencer.api.ISequenceBinding sequenceBinding = GetBinding(key) as global::strange.extensions.sequencer.api.ISequenceBinding;
			if (sequenceBinding != null)
			{
				nextInSequence(sequenceBinding, data, 0);
			}
		}

		private void removeSequence(global::strange.extensions.sequencer.api.ISequenceCommand command)
		{
			if (activeSequences.ContainsKey(command))
			{
				command.Cancel();
				activeSequences.Remove(command);
			}
		}

		private void invokeCommand(global::System.Type cmd, global::strange.extensions.sequencer.api.ISequenceBinding binding, object data, int depth)
		{
			global::strange.extensions.sequencer.api.ISequenceCommand sequenceCommand = createCommand(cmd, data);
			sequenceCommand.sequenceId = depth;
			trackCommand(sequenceCommand, binding);
			executeCommand(sequenceCommand);
			ReleaseCommand(sequenceCommand);
		}

		protected new virtual global::strange.extensions.sequencer.api.ISequenceCommand createCommand(object cmd, object data)
		{
			base.injectionBinder.Bind<global::strange.extensions.sequencer.api.ISequenceCommand>().To(cmd);
			global::strange.extensions.sequencer.api.ISequenceCommand instance = base.injectionBinder.GetInstance<global::strange.extensions.sequencer.api.ISequenceCommand>();
			instance.data = data;
			base.injectionBinder.Unbind<global::strange.extensions.sequencer.api.ISequenceCommand>();
			return instance;
		}

		private void trackCommand(global::strange.extensions.sequencer.api.ISequenceCommand command, global::strange.extensions.sequencer.api.ISequenceBinding binding)
		{
			activeSequences[command] = binding;
		}

		private void executeCommand(global::strange.extensions.sequencer.api.ISequenceCommand command)
		{
			if (command != null)
			{
				command.Execute();
			}
		}

		public void ReleaseCommand(global::strange.extensions.sequencer.api.ISequenceCommand command)
		{
			if (!command.retain && activeSequences.ContainsKey(command))
			{
				global::strange.extensions.sequencer.api.ISequenceBinding binding = activeSequences[command] as global::strange.extensions.sequencer.api.ISequenceBinding;
				object data = command.data;
				activeSequences.Remove(command);
				nextInSequence(binding, data, command.sequenceId + 1);
			}
		}

		private void nextInSequence(global::strange.extensions.sequencer.api.ISequenceBinding binding, object data, int depth)
		{
			object[] array = binding.value as object[];
			if (depth < array.Length)
			{
				global::System.Type cmd = array[depth] as global::System.Type;
				invokeCommand(cmd, binding, data, depth);
			}
			else if (binding.isOneOff)
			{
				Unbind(binding);
			}
		}

		private void failIf(bool condition, string message, global::strange.extensions.sequencer.api.SequencerExceptionType type)
		{
			if (condition)
			{
				throw new global::strange.extensions.sequencer.impl.SequencerException(message, type);
			}
		}

		public new virtual global::strange.extensions.sequencer.api.ISequenceBinding Bind<T>()
		{
			return base.Bind<T>() as global::strange.extensions.sequencer.api.ISequenceBinding;
		}

		public new virtual global::strange.extensions.sequencer.api.ISequenceBinding Bind(object value)
		{
			return base.Bind(value) as global::strange.extensions.sequencer.api.ISequenceBinding;
		}
	}
}
