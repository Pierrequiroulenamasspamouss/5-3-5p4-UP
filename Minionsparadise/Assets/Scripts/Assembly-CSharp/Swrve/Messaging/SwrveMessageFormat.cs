namespace Swrve.Messaging
{
	public class SwrveMessageFormat
	{
		public string Name;

		public global::System.Collections.Generic.List<global::Swrve.Messaging.SwrveButton> Buttons;

		public global::System.Collections.Generic.List<global::Swrve.Messaging.SwrveImage> Images;

		public string Language;

		public global::Swrve.Messaging.SwrveOrientation Orientation;

		public global::Swrve.Messaging.Point Size = new global::Swrve.Messaging.Point(0, 0);

		public global::Swrve.Messaging.SwrveMessage Message;

		public float Scale = 1f;

		public global::UnityEngine.Color? BackgroundColor;

		public global::Swrve.Messaging.ISwrveInstallButtonListener InstallButtonListener;

		public global::Swrve.Messaging.ISwrveCustomButtonListener CustomButtonListener;

		public global::Swrve.Messaging.ISwrveMessageListener MessageListener;

		public bool Closing;

		public bool Dismissed;

		public bool Rotate;

		private SwrveMessageFormat(global::Swrve.Messaging.SwrveMessage message)
		{
			Message = message;
			Buttons = new global::System.Collections.Generic.List<global::Swrve.Messaging.SwrveButton>();
			Images = new global::System.Collections.Generic.List<global::Swrve.Messaging.SwrveImage>();
		}

		public static global::Swrve.Messaging.SwrveMessageFormat LoadFromJSON(SwrveSDK sdk, global::Swrve.Messaging.SwrveMessage message, global::System.Collections.Generic.Dictionary<string, object> messageFormatData)
		{
			global::Swrve.Messaging.SwrveMessageFormat swrveMessageFormat = new global::Swrve.Messaging.SwrveMessageFormat(message);
			swrveMessageFormat.Name = (string)messageFormatData["name"];
			swrveMessageFormat.Language = (string)messageFormatData["language"];
			if (messageFormatData.ContainsKey("scale"))
			{
				swrveMessageFormat.Scale = global::Swrve.Helpers.MiniJsonHelper.GetFloat(messageFormatData, "scale", 1f);
			}
			if (messageFormatData.ContainsKey("orientation"))
			{
				swrveMessageFormat.Orientation = global::Swrve.Messaging.SwrveOrientationHelper.Parse((string)messageFormatData["orientation"]);
			}
			swrveMessageFormat.BackgroundColor = sdk.DefaultBackgroundColor;
			if (messageFormatData.ContainsKey("color"))
			{
				string text = (string)messageFormatData["color"];
				global::UnityEngine.Color? backgroundColor = swrveMessageFormat.BackgroundColor;
				if (text.Length == 8)
				{
					byte a = byte.Parse(text.Substring(0, 2), global::System.Globalization.NumberStyles.HexNumber);
					byte r = byte.Parse(text.Substring(2, 2), global::System.Globalization.NumberStyles.HexNumber);
					byte g = byte.Parse(text.Substring(4, 2), global::System.Globalization.NumberStyles.HexNumber);
					byte b = byte.Parse(text.Substring(6, 2), global::System.Globalization.NumberStyles.HexNumber);
					backgroundColor = new global::UnityEngine.Color32(r, g, b, a);
				}
				else if (text.Length == 6)
				{
					byte r2 = byte.Parse(text.Substring(0, 2), global::System.Globalization.NumberStyles.HexNumber);
					byte g2 = byte.Parse(text.Substring(2, 2), global::System.Globalization.NumberStyles.HexNumber);
					byte b2 = byte.Parse(text.Substring(4, 2), global::System.Globalization.NumberStyles.HexNumber);
					backgroundColor = new global::UnityEngine.Color32(r2, g2, b2, byte.MaxValue);
				}
				swrveMessageFormat.BackgroundColor = backgroundColor;
			}
			global::System.Collections.Generic.Dictionary<string, object> dictionary = (global::System.Collections.Generic.Dictionary<string, object>)messageFormatData["size"];
			swrveMessageFormat.Size.X = global::Swrve.Helpers.MiniJsonHelper.GetInt((global::System.Collections.Generic.Dictionary<string, object>)dictionary["w"], "value");
			swrveMessageFormat.Size.Y = global::Swrve.Helpers.MiniJsonHelper.GetInt((global::System.Collections.Generic.Dictionary<string, object>)dictionary["h"], "value");
			global::System.Collections.Generic.IList<object> list = (global::System.Collections.Generic.List<object>)messageFormatData["buttons"];
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				global::Swrve.Messaging.SwrveButton item = LoadButtonFromJSON(message, (global::System.Collections.Generic.Dictionary<string, object>)list[i]);
				swrveMessageFormat.Buttons.Add(item);
			}
			global::System.Collections.Generic.IList<object> list2 = (global::System.Collections.Generic.List<object>)messageFormatData["images"];
			int j = 0;
			for (int count2 = list2.Count; j < count2; j++)
			{
				global::Swrve.Messaging.SwrveImage item2 = LoadImageFromJSON(message, (global::System.Collections.Generic.Dictionary<string, object>)list2[j]);
				swrveMessageFormat.Images.Add(item2);
			}
			return swrveMessageFormat;
		}

		protected static int IntValueFromAttribute(global::System.Collections.Generic.Dictionary<string, object> data, string attribute)
		{
			return global::Swrve.Helpers.MiniJsonHelper.GetInt((global::System.Collections.Generic.Dictionary<string, object>)data[attribute], "value");
		}

		protected static string StringValueFromAttribute(global::System.Collections.Generic.Dictionary<string, object> data, string attribute)
		{
			return (string)((global::System.Collections.Generic.Dictionary<string, object>)data[attribute])["value"];
		}

		protected static global::Swrve.Messaging.SwrveButton LoadButtonFromJSON(global::Swrve.Messaging.SwrveMessage message, global::System.Collections.Generic.Dictionary<string, object> buttonData)
		{
			global::Swrve.Messaging.SwrveButton swrveButton = new global::Swrve.Messaging.SwrveButton();
			swrveButton.Position.X = IntValueFromAttribute(buttonData, "x");
			swrveButton.Position.Y = IntValueFromAttribute(buttonData, "y");
			swrveButton.Size.X = IntValueFromAttribute(buttonData, "w");
			swrveButton.Size.Y = IntValueFromAttribute(buttonData, "h");
			swrveButton.Image = StringValueFromAttribute(buttonData, "image_up");
			swrveButton.Message = message;
			if (buttonData.ContainsKey("name"))
			{
				swrveButton.Name = (string)buttonData["name"];
			}
			string text = StringValueFromAttribute(buttonData, "type");
			global::Swrve.Messaging.SwrveActionType actionType = global::Swrve.Messaging.SwrveActionType.Dismiss;
			if (text.ToLower().Equals("install"))
			{
				actionType = global::Swrve.Messaging.SwrveActionType.Install;
			}
			else if (text.ToLower().Equals("custom"))
			{
				actionType = global::Swrve.Messaging.SwrveActionType.Custom;
			}
			swrveButton.ActionType = actionType;
			swrveButton.Action = StringValueFromAttribute(buttonData, "action");
			if (swrveButton.ActionType == global::Swrve.Messaging.SwrveActionType.Install)
			{
				string text2 = StringValueFromAttribute(buttonData, "game_id");
				if (text2 != null && text2 != string.Empty)
				{
					swrveButton.GameId = int.Parse(text2);
				}
			}
			return swrveButton;
		}

		protected static global::Swrve.Messaging.SwrveImage LoadImageFromJSON(global::Swrve.Messaging.SwrveMessage message, global::System.Collections.Generic.Dictionary<string, object> imageData)
		{
			global::Swrve.Messaging.SwrveImage swrveImage = new global::Swrve.Messaging.SwrveImage();
			swrveImage.Position.X = IntValueFromAttribute(imageData, "x");
			swrveImage.Position.Y = IntValueFromAttribute(imageData, "y");
			swrveImage.Size.X = IntValueFromAttribute(imageData, "w");
			swrveImage.Size.Y = IntValueFromAttribute(imageData, "h");
			swrveImage.File = StringValueFromAttribute(imageData, "image");
			return swrveImage;
		}

		public void Dismiss()
		{
			if (!Closing)
			{
				Closing = true;
				CustomButtonListener = null;
				InstallButtonListener = null;
				Message.Campaign.MessageDismissed();
				if (MessageListener != null)
				{
					MessageListener.OnDismiss(this);
					MessageListener = null;
				}
			}
		}

		public void Init(global::Swrve.Messaging.Point startPoint, global::Swrve.Messaging.Point endPoint)
		{
			Closing = false;
			Dismissed = false;
			Message.Position = startPoint;
			Message.TargetPosition = endPoint;
			if (MessageListener != null)
			{
				MessageListener.OnShow(this);
			}
		}

		public void UnloadAssets()
		{
			foreach (global::Swrve.Messaging.SwrveImage image in Images)
			{
				global::UnityEngine.Object.Destroy(image.Texture);
				image.Texture = null;
			}
			foreach (global::Swrve.Messaging.SwrveButton button in Buttons)
			{
				global::UnityEngine.Object.Destroy(button.Texture);
				button.Texture = null;
			}
		}
	}
}
