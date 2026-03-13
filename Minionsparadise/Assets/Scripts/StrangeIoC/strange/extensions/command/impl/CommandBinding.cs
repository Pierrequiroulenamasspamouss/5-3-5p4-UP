namespace strange.extensions.command.impl
{
	public class CommandBinding : global::strange.framework.impl.Binding, global::strange.extensions.command.api.ICommandBinding, global::strange.framework.api.IBinding
	{
		public bool isOneOff { get; set; }

		public bool isSequence { get; set; }

		public bool isPooled { get; set; }

		public CommandBinding()
		{
		}

		public CommandBinding(global::strange.framework.impl.Binder.BindingResolver resolver)
			: base(resolver)
		{
		}

		public global::strange.extensions.command.api.ICommandBinding Once()
		{
			isOneOff = true;
			return this;
		}

		public global::strange.extensions.command.api.ICommandBinding InParallel()
		{
			isSequence = false;
			return this;
		}

		public global::strange.extensions.command.api.ICommandBinding InSequence()
		{
			isSequence = true;
			return this;
		}

		public global::strange.extensions.command.api.ICommandBinding Pooled()
		{
			isPooled = true;
			resolver(this);
			return this;
		}

		public new global::strange.extensions.command.api.ICommandBinding Bind<T>()
		{
			return base.Bind<T>() as global::strange.extensions.command.api.ICommandBinding;
		}

		public new global::strange.extensions.command.api.ICommandBinding Bind(object key)
		{
			return base.Bind(key) as global::strange.extensions.command.api.ICommandBinding;
		}

		public new global::strange.extensions.command.api.ICommandBinding To<T>()
		{
			return base.To<T>() as global::strange.extensions.command.api.ICommandBinding;
		}

		public new global::strange.extensions.command.api.ICommandBinding To(object o)
		{
			return base.To(o) as global::strange.extensions.command.api.ICommandBinding;
		}

		public new global::strange.extensions.command.api.ICommandBinding ToName<T>()
		{
			return base.ToName<T>() as global::strange.extensions.command.api.ICommandBinding;
		}

		public new global::strange.extensions.command.api.ICommandBinding ToName(object o)
		{
			return base.ToName(o) as global::strange.extensions.command.api.ICommandBinding;
		}

		public new global::strange.extensions.command.api.ICommandBinding Named<T>()
		{
			return base.Named<T>() as global::strange.extensions.command.api.ICommandBinding;
		}

		public new global::strange.extensions.command.api.ICommandBinding Named(object o)
		{
			return base.Named(o) as global::strange.extensions.command.api.ICommandBinding;
		}
	}
}
