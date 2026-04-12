namespace strange.extensions.command.impl
{
	public class EventCommandBinder : global::strange.extensions.command.impl.CommandBinder
	{
		protected override global::strange.extensions.command.api.ICommand createCommand(object cmd, object data)
		{
			base.injectionBinder.Bind<global::strange.extensions.command.api.ICommand>().To(cmd);
			if (data is global::strange.extensions.dispatcher.eventdispatcher.api.IEvent)
			{
				base.injectionBinder.Bind<global::strange.extensions.dispatcher.eventdispatcher.api.IEvent>().ToValue(data).ToInject(false);
			}
			global::strange.extensions.command.api.ICommand instance = base.injectionBinder.GetInstance<global::strange.extensions.command.api.ICommand>();
			try
			{
				if (instance == null)
				{
					string text = "A Command ";
					if (data is global::strange.extensions.dispatcher.eventdispatcher.api.IEvent)
					{
						global::strange.extensions.dispatcher.eventdispatcher.api.IEvent obj = (global::strange.extensions.dispatcher.eventdispatcher.api.IEvent)data;
						text = text + "tied to event " + obj.type;
					}
					text += " could not be instantiated.\nThis might be caused by a null pointer during instantiation or failing to override Execute (generally you shouldn't have constructor code in Commands).";
					throw new global::strange.extensions.command.impl.CommandException(text, global::strange.extensions.command.api.CommandExceptionType.BAD_CONSTRUCTOR);
				}
				instance.data = data;
				return instance;
			}
			catch (global::System.Exception)
			{
				throw;
			}
			finally
			{
				if (data is global::strange.extensions.dispatcher.eventdispatcher.api.IEvent)
				{
					base.injectionBinder.Unbind<global::strange.extensions.dispatcher.eventdispatcher.api.IEvent>();
				}
				base.injectionBinder.Unbind<global::strange.extensions.command.api.ICommand>();
			}
		}

		protected override void disposeOfSequencedData(object data)
		{
			if (data is global::strange.extensions.pool.api.IPoolable)
			{
				(data as global::strange.extensions.pool.api.IPoolable).Release();
			}
		}
	}
}
