namespace Swrve.Messaging
{
	public class SwrveQAUser
	{
		private const int ApiVersion = 1;

		private const long SessionInterval = 1000L;

		private const long TriggerInterval = 500L;

		private const long PushNotificationInterval = 1000L;

		private readonly SwrveSDK swrve;

		private readonly global::Swrve.REST.IRESTClient restClient;

		private readonly string loggingUrl;

		private long lastSessionRequestTime;

		private long lastTriggerRequestTime;

		private long lastPushNotificationRequestTime;

		public readonly bool ResetDevice;

		public readonly bool Logging;

		public SwrveQAUser(SwrveSDK swrve, global::System.Collections.Generic.Dictionary<string, object> jsonQa)
		{
			this.swrve = swrve;
			ResetDevice = global::Swrve.Helpers.MiniJsonHelper.GetBool(jsonQa, "reset_device_state", false);
			Logging = global::Swrve.Helpers.MiniJsonHelper.GetBool(jsonQa, "logging", false);
			if (Logging)
			{
				restClient = new global::Swrve.REST.RESTClient();
				loggingUrl = global::Swrve.Helpers.MiniJsonHelper.GetString(jsonQa, "logging_url", null);
			}
		}

		public void TalkSession(global::System.Collections.Generic.Dictionary<int, string> campaignsDownloaded)
		{
			try
			{
				if (CanMakeSessionRequest())
				{
					string endpoint = loggingUrl + "/talk/game/" + swrve.ApiKey + "/user/" + swrve.UserId + "/session";
					global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
					global::System.Collections.Generic.IList<object> list = new global::System.Collections.Generic.List<object>();
					global::System.Collections.Generic.Dictionary<int, string>.Enumerator enumerator = campaignsDownloaded.GetEnumerator();
					while (enumerator.MoveNext())
					{
						int key = enumerator.Current.Key;
						string value = enumerator.Current.Value;
						global::System.Collections.Generic.Dictionary<string, object> dictionary2 = new global::System.Collections.Generic.Dictionary<string, object>();
						dictionary2.Add("id", key);
						dictionary2.Add("reason", (value != null) ? value : string.Empty);
						dictionary2.Add("loaded", value == null);
						list.Add(dictionary2);
					}
					dictionary.Add("campaigns", list);
					global::System.Collections.Generic.Dictionary<string, string> deviceInfo = swrve.GetDeviceInfo();
					dictionary.Add("device", deviceInfo);
					MakeRequest(endpoint, dictionary);
				}
			}
			catch (global::System.Exception ex)
			{
				SwrveLog.LogError("QA request talk session failed: " + ex.ToString());
			}
		}

		public void UpdateDeviceInfo()
		{
			try
			{
				if (!CanMakeRequest())
				{
					return;
				}
				string endpoint = loggingUrl + "/talk/game/" + swrve.ApiKey + "/user/" + swrve.UserId + "/device_info";
				global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
				global::System.Collections.Generic.Dictionary<string, string> deviceInfo = swrve.GetDeviceInfo();
				foreach (string key in deviceInfo.Keys)
				{
					dictionary.Add(key, deviceInfo[key]);
				}
				MakeRequest(endpoint, dictionary);
			}
			catch (global::System.Exception ex)
			{
				SwrveLog.LogError("QA request talk device info update failed: " + ex.ToString());
			}
		}

		private void MakeRequest(string endpoint, global::System.Collections.Generic.Dictionary<string, object> json)
		{
			json.Add("version", 1);
			json.Add("client_time", global::System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz"));
			string s = global::SwrveMiniJSON.Json.Serialize(json);
			byte[] bytes = global::System.Text.Encoding.UTF8.GetBytes(s);
			global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
			dictionary.Add("Content-Type", "application/json; charset=utf-8");
			dictionary.Add("Content-Length", bytes.Length.ToString());
			global::System.Collections.Generic.Dictionary<string, string> headers = dictionary;
			swrve.Container.StartCoroutine(restClient.Post(endpoint, bytes, headers, RestListener));
		}

		public void TriggerFailure(string eventName, string globalReason)
		{
			try
			{
				if (CanMakeTriggerRequest())
				{
					string endpoint = loggingUrl + "/talk/game/" + swrve.ApiKey + "/user/" + swrve.UserId + "/trigger";
					global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
					dictionary.Add("trigger_name", eventName);
					dictionary.Add("displayed", false);
					dictionary.Add("reason", globalReason);
					dictionary.Add("campaigns", new global::System.Collections.Generic.List<object>());
					MakeRequest(endpoint, dictionary);
				}
			}
			catch (global::System.Exception ex)
			{
				SwrveLog.LogError("QA request talk session failed: " + ex.ToString());
			}
		}

		public void Trigger(string eventName, global::Swrve.Messaging.SwrveMessage messageShown, global::System.Collections.Generic.Dictionary<int, string> campaignReasons, global::System.Collections.Generic.Dictionary<int, int> campaignMessages)
		{
			try
			{
				if (!CanMakeTriggerRequest())
				{
					return;
				}
				string endpoint = loggingUrl + "/talk/game/" + swrve.ApiKey + "/user/" + swrve.UserId + "/trigger";
				global::System.Collections.Generic.Dictionary<string, object> dictionary = new global::System.Collections.Generic.Dictionary<string, object>();
				dictionary.Add("trigger_name", eventName);
				dictionary.Add("displayed", messageShown != null);
				dictionary.Add("reason", (messageShown != null) ? string.Empty : "The loaded campaigns returned no message");
				global::System.Collections.Generic.IList<object> list = new global::System.Collections.Generic.List<object>();
				global::System.Collections.Generic.Dictionary<int, string>.Enumerator enumerator = campaignReasons.GetEnumerator();
				while (enumerator.MoveNext())
				{
					int key = enumerator.Current.Key;
					string value = enumerator.Current.Value;
					int? num = null;
					if (campaignMessages.ContainsKey(key))
					{
						num = campaignMessages[key];
					}
					global::System.Collections.Generic.Dictionary<string, object> dictionary2 = new global::System.Collections.Generic.Dictionary<string, object>();
					dictionary2.Add("id", key);
					dictionary2.Add("displayed", false);
					dictionary2.Add("message_id", num ?? new int?(-1));
					dictionary2.Add("reason", (value != null) ? value : string.Empty);
					list.Add(dictionary2);
				}
				if (messageShown != null)
				{
					global::System.Collections.Generic.Dictionary<string, object> dictionary3 = new global::System.Collections.Generic.Dictionary<string, object>();
					dictionary3.Add("id", messageShown.Campaign.Id);
					dictionary3.Add("displayed", true);
					dictionary3.Add("message_id", messageShown.Id);
					dictionary3.Add("reason", string.Empty);
					list.Add(dictionary3);
				}
				dictionary.Add("campaigns", list);
				MakeRequest(endpoint, dictionary);
			}
			catch (global::System.Exception ex)
			{
				SwrveLog.LogError("QA request talk session failed: " + ex.ToString());
			}
		}

		private bool CanMakeRequest()
		{
			return swrve != null && Logging;
		}

		private bool CanMakeSessionRequest()
		{
			if (CanMakeRequest())
			{
				long milliseconds = global::Swrve.Helpers.SwrveHelper.GetMilliseconds();
				if (lastSessionRequestTime == 0L || milliseconds - lastSessionRequestTime > 1000)
				{
					lastSessionRequestTime = milliseconds;
					return true;
				}
			}
			return false;
		}

		private bool CanMakeTriggerRequest()
		{
			if (CanMakeRequest())
			{
				long milliseconds = global::Swrve.Helpers.SwrveHelper.GetMilliseconds();
				if (lastTriggerRequestTime == 0L || milliseconds - lastTriggerRequestTime > 500)
				{
					lastTriggerRequestTime = milliseconds;
					return true;
				}
			}
			return false;
		}

		private bool CanMakePushNotificationRequest()
		{
			if (swrve != null && Logging)
			{
				long milliseconds = global::Swrve.Helpers.SwrveHelper.GetMilliseconds();
				if (lastPushNotificationRequestTime == 0L || milliseconds - lastPushNotificationRequestTime > 1000)
				{
					lastPushNotificationRequestTime = milliseconds;
					return true;
				}
			}
			return false;
		}

		private void RestListener(global::Swrve.REST.RESTResponse response)
		{
			if (response.Error != global::Swrve.Helpers.WwwDeducedError.NoError)
			{
				SwrveLog.LogError("QA request to failed with error code " + response.Error.ToString() + ": " + response.Body);
			}
		}
	}
}
