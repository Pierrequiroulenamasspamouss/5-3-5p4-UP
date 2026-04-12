using UnityEngine;
using UnityEngine.UI;
using Kampai.Util;

namespace Kampai.UI.View
{
	public class ChatItemView : KampaiView
	{
		public Text userNameText;
		public Text messageText;
		public Text timestampText;
		public Image avatarImage;

		public void Setup(string user, string text, string timestamp)
		{
			if (userNameText != null)
			{
				userNameText.text = user;
			}

			if (messageText != null)
			{
				messageText.text = text;
			}

			if (timestampText != null)
			{
				timestampText.text = timestamp;
			}
			
			// Optional: load avatar if we had a service for it
		}
	}
}
