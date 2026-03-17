namespace Swrve.Messaging
{
	public class SwrveMessage
	{
		public int Id;

		public string Name;

		public int Priority = 9999;

		public global::Swrve.Messaging.SwrveCampaign Campaign;

		public global::System.Collections.Generic.List<global::Swrve.Messaging.SwrveMessageFormat> Formats;

		public global::Swrve.Messaging.Point Position = new global::Swrve.Messaging.Point(0, 0);

		public global::Swrve.Messaging.Point TargetPosition = new global::Swrve.Messaging.Point(0, 0);

		public float BackgroundAlpha = 1f;

		public float AnimationScale = 1f;

		private SwrveMessage(global::Swrve.Messaging.SwrveCampaign campaign)
		{
			Campaign = campaign;
			Formats = new global::System.Collections.Generic.List<global::Swrve.Messaging.SwrveMessageFormat>();
		}

		public global::Swrve.Messaging.SwrveMessageFormat GetFormat(global::Swrve.Messaging.SwrveOrientation orientation)
		{
			global::System.Collections.Generic.IEnumerator<global::Swrve.Messaging.SwrveMessageFormat> enumerator = Formats.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Orientation == orientation)
				{
					return enumerator.Current;
				}
			}
			return null;
		}

		public static global::Swrve.Messaging.SwrveMessage LoadFromJSON(SwrveSDK sdk, global::Swrve.Messaging.SwrveCampaign campaign, global::System.Collections.Generic.Dictionary<string, object> messageData)
		{
			global::Swrve.Messaging.SwrveMessage swrveMessage = new global::Swrve.Messaging.SwrveMessage(campaign);
			swrveMessage.Id = global::Swrve.Helpers.MiniJsonHelper.GetInt(messageData, "id");
			swrveMessage.Name = (string)messageData["name"];
			if (messageData.ContainsKey("priority"))
			{
				swrveMessage.Priority = global::Swrve.Helpers.MiniJsonHelper.GetInt(messageData, "priority");
			}
			global::System.Collections.Generic.Dictionary<string, object> dictionary = (global::System.Collections.Generic.Dictionary<string, object>)messageData["template"];
			global::System.Collections.Generic.IList<object> list = (global::System.Collections.Generic.List<object>)dictionary["formats"];
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				global::System.Collections.Generic.Dictionary<string, object> messageFormatData = (global::System.Collections.Generic.Dictionary<string, object>)list[i];
				global::Swrve.Messaging.SwrveMessageFormat item = global::Swrve.Messaging.SwrveMessageFormat.LoadFromJSON(sdk, swrveMessage, messageFormatData);
				swrveMessage.Formats.Add(item);
			}
			return swrveMessage;
		}

		public bool SupportsOrientation(global::Swrve.Messaging.SwrveOrientation orientation)
		{
			return GetFormat(orientation) != null;
		}

		public global::System.Collections.Generic.List<string> ListOfAssets()
		{
			global::System.Collections.Generic.List<string> list = new global::System.Collections.Generic.List<string>();
			foreach (global::Swrve.Messaging.SwrveMessageFormat format in Formats)
			{
				foreach (global::Swrve.Messaging.SwrveImage image in format.Images)
				{
					if (!string.IsNullOrEmpty(image.File))
					{
						list.Add(image.File);
					}
				}
				foreach (global::Swrve.Messaging.SwrveButton button in format.Buttons)
				{
					if (!string.IsNullOrEmpty(button.Image))
					{
						list.Add(button.Image);
					}
				}
			}
			return list;
		}

		public bool isDownloaded(string assetPath)
		{
			global::System.Collections.Generic.List<string> list = ListOfAssets();
			foreach (string item in list)
			{
				if (!global::Swrve.CrossPlatformFile.Exists(assetPath + "/" + item))
				{
					return false;
				}
			}
			return true;
		}
	}
}
