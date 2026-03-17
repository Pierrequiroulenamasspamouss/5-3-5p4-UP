namespace strange.extensions.dispatcher.eventdispatcher.impl
{
	public class TmEvent : global::strange.extensions.dispatcher.eventdispatcher.api.IEvent, global::strange.extensions.pool.api.IPoolable
	{
		protected int retainCount;

		public object type { get; set; }

		public global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher target { get; set; }

		public object data { get; set; }

		public bool retain
		{
			get
			{
				return retainCount > 0;
			}
		}

		public TmEvent()
		{
		}

		public TmEvent(object type, global::strange.extensions.dispatcher.eventdispatcher.api.IEventDispatcher target, object data)
		{
			this.type = type;
			this.target = target;
			this.data = data;
		}

		public void Restore()
		{
			type = null;
			target = null;
			data = null;
		}

		public void Retain()
		{
			retainCount++;
		}

		public void Release()
		{
			retainCount--;
			if (retainCount == 0)
			{
				target.ReleaseEvent(this);
			}
		}
	}
}
