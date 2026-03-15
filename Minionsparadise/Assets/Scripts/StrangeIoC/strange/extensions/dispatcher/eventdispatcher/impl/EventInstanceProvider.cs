namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	internal class EventInstanceProvider : global::strange.framework.api.IInstanceProvider
	{
		public T GetInstance<T>()
		{
			object obj = new global::strange.extensions.dispatcher.eventdispatcher.impl.TmEvent();
			return (T)obj;
		}

		public object GetInstance(global::System.Type key)
		{
			return new global::strange.extensions.dispatcher.eventdispatcher.impl.TmEvent();
		}
	}
}
