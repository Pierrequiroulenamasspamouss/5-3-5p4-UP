namespace strange.extensions.command.api
{
	public interface ICommandBinding : global::strange.framework.api.IBinding
	{
		bool isOneOff { get; set; }

		bool isSequence { get; set; }

		bool isPooled { get; set; }

		global::strange.extensions.command.api.ICommandBinding Once();

		global::strange.extensions.command.api.ICommandBinding InParallel();

		global::strange.extensions.command.api.ICommandBinding InSequence();

		global::strange.extensions.command.api.ICommandBinding Pooled();

		new global::strange.extensions.command.api.ICommandBinding Bind<T>();

		new global::strange.extensions.command.api.ICommandBinding Bind(object key);

		new global::strange.extensions.command.api.ICommandBinding To<T>();

		new global::strange.extensions.command.api.ICommandBinding To(object o);

		new global::strange.extensions.command.api.ICommandBinding ToName<T>();

		new global::strange.extensions.command.api.ICommandBinding ToName(object o);

		new global::strange.extensions.command.api.ICommandBinding Named<T>();

		new global::strange.extensions.command.api.ICommandBinding Named(object o);
	}
}
