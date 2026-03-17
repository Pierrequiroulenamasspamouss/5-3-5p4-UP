namespace strange.extensions.command.api
{
	public interface ICommandBinder : global::strange.framework.api.IBinder
	{
		void ReactTo(object trigger);

		void ReactTo(object trigger, object data);

		void ReleaseCommand(global::strange.extensions.command.api.ICommand command);

		void Stop(object key);

		new global::strange.extensions.command.api.ICommandBinding Bind<T>();

		new global::strange.extensions.command.api.ICommandBinding Bind(object value);

		new global::strange.extensions.command.api.ICommandBinding GetBinding<T>();
	}
}
