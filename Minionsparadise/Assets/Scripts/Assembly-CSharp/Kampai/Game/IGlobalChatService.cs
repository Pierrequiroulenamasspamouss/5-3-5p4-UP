using System.Collections.Generic;

namespace Kampai.Game
{
	[System.Serializable]
	public class ChatMessage
	{
		public string user;
		public string text;
		public string timestamp;
	}

	[System.Serializable]
	public class ChatResponse
	{
		public List<ChatMessage> messages;
	}

	public interface IGlobalChatService
	{
		void SendMessage(string text);
		void StartPolling();
		void StopPolling();
		List<ChatMessage> GetCachedMessages();
	}
}
