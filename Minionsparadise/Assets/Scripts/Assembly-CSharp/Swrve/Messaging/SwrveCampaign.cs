namespace Swrve.Messaging
{
	public class SwrveCampaign
	{
		protected const string WaitTimeFormat = "HH\\:mm\\:ss zzz";

		protected const int DefaultDelayFirstMessage = 180;

		protected const long DefaultMaxShows = 99999L;

		protected const int DefaultMinDelay = 60;

		protected readonly global::System.Random rnd = new global::System.Random();

		public int Id;

		public global::System.Collections.Generic.List<global::Swrve.Messaging.SwrveMessage> Messages;

		public global::System.Collections.Generic.HashSet<string> Triggers;

		public global::System.DateTime StartDate;

		public global::System.DateTime EndDate;

		public int Impressions;

		public int Next;

		public bool RandomOrder;

		protected readonly global::System.DateTime swrveInitialisedTime;

		protected readonly string assetPath;

		protected global::System.DateTime showMessagesAfterLaunch;

		protected global::System.DateTime showMessagesAfterDelay;

		protected int minDelayBetweenMessage;

		protected int delayFirstMessage = 180;

		protected int maxImpressions;

		private SwrveCampaign(global::System.DateTime initialisedTime, string assetPath)
		{
			swrveInitialisedTime = initialisedTime;
			this.assetPath = assetPath;
			Messages = new global::System.Collections.Generic.List<global::Swrve.Messaging.SwrveMessage>();
			Triggers = new global::System.Collections.Generic.HashSet<string>();
			minDelayBetweenMessage = 60;
			showMessagesAfterLaunch = swrveInitialisedTime + global::System.TimeSpan.FromSeconds(180.0);
		}

		public global::Swrve.Messaging.SwrveMessage GetMessageForEvent(string triggerEvent, global::System.Collections.Generic.Dictionary<int, string> campaignReasons)
		{
			global::System.DateTime utcNow = global::Swrve.Helpers.SwrveHelper.GetUtcNow();
			global::System.DateTime now = global::Swrve.Helpers.SwrveHelper.GetNow();
			int count = Messages.Count;
			if (!HasMessageForEvent(triggerEvent))
			{
				SwrveLog.Log("There is no trigger in " + Id + " that matches " + triggerEvent);
				return null;
			}
			if (count == 0)
			{
				LogAndAddReason(campaignReasons, "No messages in campaign " + Id);
				return null;
			}
			if (StartDate > utcNow)
			{
				LogAndAddReason(campaignReasons, "Campaign " + Id + " has not started yet");
				return null;
			}
			if (EndDate < utcNow)
			{
				LogAndAddReason(campaignReasons, "Campaign" + Id + " has finished");
				return null;
			}
			if (Impressions >= maxImpressions)
			{
				LogAndAddReason(campaignReasons, "{Campaign throttle limit} Campaign " + Id + " has been shown " + maxImpressions + " times already");
				return null;
			}
			if (!string.Equals(triggerEvent, "Swrve.Messages.showAtSessionStart", global::System.StringComparison.OrdinalIgnoreCase) && IsTooSoonToShowMessageAfterLaunch(now))
			{
				LogAndAddReason(campaignReasons, "{Campaign throttle limit} Too soon after launch. Wait until " + showMessagesAfterLaunch.ToString("HH\\:mm\\:ss zzz"));
				return null;
			}
			if (IsTooSoonToShowMessageAfterDelay(now))
			{
				LogAndAddReason(campaignReasons, "{Campaign throttle limit} Too soon after last message. Wait until " + showMessagesAfterDelay.ToString("HH\\:mm\\:ss zzz"));
				return null;
			}
			SwrveLog.Log(triggerEvent + " matches a trigger in " + Id);
			return GetNextMessage(count, campaignReasons);
		}

		protected void LogAndAddReason(global::System.Collections.Generic.Dictionary<int, string> campaignReasons, string reason)
		{
			if (campaignReasons != null)
			{
				campaignReasons.Add(Id, reason);
			}
			SwrveLog.Log(reason);
		}

		public bool HasMessageForEvent(string eventName)
		{
			string item = eventName.ToLower();
			return Triggers != null && Triggers.Contains(item);
		}

		public global::Swrve.Messaging.SwrveMessage GetMessageForId(int id)
		{
			foreach (global::Swrve.Messaging.SwrveMessage message in Messages)
			{
				if (message.Id == id)
				{
					return message;
				}
			}
			return null;
		}

		protected global::Swrve.Messaging.SwrveMessage GetNextMessage(int messagesCount, global::System.Collections.Generic.Dictionary<int, string> campaignReasons)
		{
			if (RandomOrder)
			{
				global::System.Collections.Generic.List<global::Swrve.Messaging.SwrveMessage> list = new global::System.Collections.Generic.List<global::Swrve.Messaging.SwrveMessage>(Messages);
				global::Swrve.Helpers.SwrveHelper.Shuffle(list);
				foreach (global::Swrve.Messaging.SwrveMessage item in list)
				{
					if (item.isDownloaded(assetPath))
					{
						return item;
					}
				}
			}
			else if (Next < messagesCount)
			{
				global::Swrve.Messaging.SwrveMessage swrveMessage = Messages[Next];
				if (swrveMessage.isDownloaded(assetPath))
				{
					return swrveMessage;
				}
			}
			LogAndAddReason(campaignReasons, "Campaign " + Id + " hasn't finished downloading.");
			return null;
		}

		protected void AddMessage(global::Swrve.Messaging.SwrveMessage message)
		{
			Messages.Add(message);
		}

		public static global::Swrve.Messaging.SwrveCampaign LoadFromJSON(SwrveSDK sdk, global::System.Collections.Generic.Dictionary<string, object> campaignData, global::System.DateTime initialisedTime, string assetPath)
		{
			global::Swrve.Messaging.SwrveCampaign swrveCampaign = new global::Swrve.Messaging.SwrveCampaign(initialisedTime, assetPath);
			swrveCampaign.Id = global::Swrve.Helpers.MiniJsonHelper.GetInt(campaignData, "id");
			AssignCampaignTriggers(swrveCampaign, campaignData);
			AssignCampaignRules(swrveCampaign, campaignData);
			AssignCampaignDates(swrveCampaign, campaignData);
			global::System.Collections.Generic.IList<object> list = (global::System.Collections.Generic.IList<object>)campaignData["messages"];
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				global::System.Collections.Generic.Dictionary<string, object> messageData = (global::System.Collections.Generic.Dictionary<string, object>)list[i];
				global::Swrve.Messaging.SwrveMessage swrveMessage = global::Swrve.Messaging.SwrveMessage.LoadFromJSON(sdk, swrveCampaign, messageData);
				if (swrveMessage.Formats.Count > 0)
				{
					swrveCampaign.AddMessage(swrveMessage);
				}
			}
			return swrveCampaign;
		}

		public global::System.Collections.Generic.List<string> ListOfAssets()
		{
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			foreach (global::Swrve.Messaging.SwrveMessage message in Messages)
			{
				list.AddRange(message.ListOfAssets());
			}
			return list;
		}

		protected static void AssignCampaignTriggers(global::Swrve.Messaging.SwrveCampaign campaign, global::System.Collections.Generic.Dictionary<string, object> campaignData)
		{
			global::System.Collections.Generic.IList<object> list = (global::System.Collections.Generic.IList<object>)campaignData["triggers"];
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				string text = (string)list[i];
				campaign.Triggers.Add(text.ToLower());
			}
		}

		protected static void AssignCampaignRules(global::Swrve.Messaging.SwrveCampaign campaign, global::System.Collections.Generic.Dictionary<string, object> campaignData)
		{
			global::System.Collections.Generic.Dictionary<string, object> dictionary = (global::System.Collections.Generic.Dictionary<string, object>)campaignData["rules"];
			campaign.RandomOrder = ((string)dictionary["display_order"]).Equals("random");
			if (dictionary.ContainsKey("dismiss_after_views"))
			{
				int num = global::Swrve.Helpers.MiniJsonHelper.GetInt(dictionary, "dismiss_after_views");
				campaign.maxImpressions = num;
			}
			if (dictionary.ContainsKey("delay_first_message"))
			{
				campaign.delayFirstMessage = global::Swrve.Helpers.MiniJsonHelper.GetInt(dictionary, "delay_first_message");
				campaign.showMessagesAfterLaunch = campaign.swrveInitialisedTime + global::System.TimeSpan.FromSeconds(campaign.delayFirstMessage);
			}
			if (dictionary.ContainsKey("min_delay_between_messages"))
			{
				int num2 = global::Swrve.Helpers.MiniJsonHelper.GetInt(dictionary, "min_delay_between_messages");
				campaign.minDelayBetweenMessage = num2;
			}
		}

		protected static void AssignCampaignDates(global::Swrve.Messaging.SwrveCampaign campaign, global::System.Collections.Generic.Dictionary<string, object> campaignData)
		{
			global::System.DateTime unixEpoch = global::Swrve.Helpers.SwrveHelper.UnixEpoch;
			campaign.StartDate = unixEpoch.AddMilliseconds(global::Swrve.Helpers.MiniJsonHelper.GetLong(campaignData, "start_date"));
			campaign.EndDate = unixEpoch.AddMilliseconds(global::Swrve.Helpers.MiniJsonHelper.GetLong(campaignData, "end_date"));
		}

		public void IncrementImpressions()
		{
			Impressions++;
		}

		protected bool IsTooSoonToShowMessageAfterLaunch(global::System.DateTime now)
		{
			return now < showMessagesAfterLaunch;
		}

		protected bool IsTooSoonToShowMessageAfterDelay(global::System.DateTime now)
		{
			return now < showMessagesAfterDelay;
		}

		protected void SetMessageMinDelayThrottle()
		{
			showMessagesAfterDelay = global::Swrve.Helpers.SwrveHelper.GetNow() + global::System.TimeSpan.FromSeconds(minDelayBetweenMessage);
		}

		public void MessageWasShownToUser(global::Swrve.Messaging.SwrveMessageFormat messageFormat)
		{
			IncrementImpressions();
			if (Messages.Count > 0)
			{
				SetMessageMinDelayThrottle();
				if (!RandomOrder)
				{
					int num = (Next = (Next + 1) % Messages.Count);
					SwrveLog.Log("Round Robin: Next message in campaign " + Id + " is " + num);
				}
				else
				{
					SwrveLog.Log("Next message in campaign " + Id + " is random");
				}
			}
		}

		public void MessageDismissed()
		{
			SetMessageMinDelayThrottle();
		}
	}
}
