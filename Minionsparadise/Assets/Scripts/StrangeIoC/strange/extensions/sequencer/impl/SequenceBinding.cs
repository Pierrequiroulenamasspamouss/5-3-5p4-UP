namespace strange.extensions.sequencer.impl
{
	public class SequenceBinding : global::strange.extensions.command.impl.CommandBinding, global::strange.extensions.sequencer.api.ISequenceBinding, global::strange.extensions.command.api.ICommandBinding, global::strange.framework.api.IBinding
	{
		public new bool isOneOff { get; set; }

		public SequenceBinding()
		{
		}

		public SequenceBinding(global::strange.framework.impl.Binder.BindingResolver resolver)
			: base(resolver)
		{
		}

		public new global::strange.extensions.sequencer.api.ISequenceBinding Once()
		{
			isOneOff = true;
			return this;
		}

		public new global::strange.extensions.sequencer.api.ISequenceBinding Bind<T>()
		{
			return Bind<T>();
		}

		public new global::strange.extensions.sequencer.api.ISequenceBinding Bind(object key)
		{
			return Bind(key);
		}

		public new global::strange.extensions.sequencer.api.ISequenceBinding To<T>()
		{
			return To(typeof(T));
		}

		public new global::strange.extensions.sequencer.api.ISequenceBinding To(object o)
		{
			global::System.Type type = o as global::System.Type;
			global::System.Type typeFromHandle = typeof(global::strange.extensions.sequencer.api.ISequenceCommand);
			if (!typeFromHandle.IsAssignableFrom(type))
			{
				throw new global::strange.extensions.sequencer.impl.SequencerException("Attempt to bind a non SequenceCommand to a Sequence. Perhaps your command needs to extend SequenceCommand or implement ISequenCommand?\n\tType: " + type.ToString(), global::strange.extensions.sequencer.api.SequencerExceptionType.COMMAND_USED_IN_SEQUENCE);
			}
			return base.To(o) as global::strange.extensions.sequencer.api.ISequenceBinding;
		}

		public new global::strange.extensions.sequencer.api.ISequenceBinding ToName<T>()
		{
			return base.ToName<T>() as global::strange.extensions.sequencer.api.ISequenceBinding;
		}

		public new global::strange.extensions.sequencer.api.ISequenceBinding ToName(object o)
		{
			return base.ToName(o) as global::strange.extensions.sequencer.api.ISequenceBinding;
		}

		public new global::strange.extensions.sequencer.api.ISequenceBinding Named<T>()
		{
			return base.Named<T>() as global::strange.extensions.sequencer.api.ISequenceBinding;
		}

		public new global::strange.extensions.sequencer.api.ISequenceBinding Named(object o)
		{
			return base.Named(o) as global::strange.extensions.sequencer.api.ISequenceBinding;
		}
	}
}
