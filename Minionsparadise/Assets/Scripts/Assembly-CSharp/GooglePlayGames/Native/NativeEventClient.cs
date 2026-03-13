namespace GooglePlayGames.Native
{
	internal class NativeEventClient : global::GooglePlayGames.BasicApi.Events.IEventsClient
	{
		private readonly global::GooglePlayGames.Native.PInvoke.EventManager mEventManager;

		internal NativeEventClient(global::GooglePlayGames.Native.PInvoke.EventManager manager)
		{
			mEventManager = global::GooglePlayGames.OurUtils.Misc.CheckNotNull(manager);
		}

		public void FetchAllEvents(global::GooglePlayGames.BasicApi.DataSource source, global::System.Action<global::GooglePlayGames.BasicApi.ResponseStatus, global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Events.IEvent>> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			callback = global::GooglePlayGames.Native.CallbackUtils.ToOnGameThread(callback);
			mEventManager.FetchAll(global::GooglePlayGames.Native.ConversionUtils.AsDataSource(source), delegate(global::GooglePlayGames.Native.PInvoke.EventManager.FetchAllResponse response)
			{
				global::GooglePlayGames.BasicApi.ResponseStatus arg = global::GooglePlayGames.Native.ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (!response.RequestSucceeded())
				{
					callback(arg, new global::System.Collections.Generic.List<global::GooglePlayGames.BasicApi.Events.IEvent>());
				}
				else
				{
					callback(arg, global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Cast<global::GooglePlayGames.BasicApi.Events.IEvent>(response.Data())));
				}
			});
		}

		public void FetchEvent(global::GooglePlayGames.BasicApi.DataSource source, string eventId, global::System.Action<global::GooglePlayGames.BasicApi.ResponseStatus, global::GooglePlayGames.BasicApi.Events.IEvent> callback)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(eventId);
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(callback);
			mEventManager.Fetch(global::GooglePlayGames.Native.ConversionUtils.AsDataSource(source), eventId, delegate(global::GooglePlayGames.Native.PInvoke.EventManager.FetchResponse response)
			{
				global::GooglePlayGames.BasicApi.ResponseStatus arg = global::GooglePlayGames.Native.ConversionUtils.ConvertResponseStatus(response.ResponseStatus());
				if (!response.RequestSucceeded())
				{
					callback(arg, null);
				}
				else
				{
					callback(arg, response.Data());
				}
			});
		}

		public void IncrementEvent(string eventId, uint stepsToIncrement)
		{
			global::GooglePlayGames.OurUtils.Misc.CheckNotNull(eventId);
			mEventManager.Increment(eventId, stepsToIncrement);
		}
	}
}
