using System.Collections;
using System.Collections.Generic;
using Kampai.Main;
using Kampai.UI.View;
using Kampai.Util;
using UnityEngine;
using Newtonsoft.Json;

namespace Kampai.Game
{
	public class GlobalChatService : IGlobalChatService
	{
		[Inject]
		public IRoutineRunner routineRunner { get; set; }

		[Inject]
		public IPlayerService playerService { get; set; }

		[Inject]
		public GlobalChatUpdateSignal updateSignal { get; set; }

		[Inject]
		public GlobalChatErrorSignal errorSignal { get; set; }

		private List<ChatMessage> m_cachedMessages = new List<ChatMessage>();
		private IEnumerator m_pollingEnumerator;
		private float m_pollInterval = 5f;

		public void StartPolling()
		{
			if (m_pollingEnumerator == null)
			{
				m_pollingEnumerator = PollCoroutine();
				routineRunner.StartCoroutine(m_pollingEnumerator);
			}
		}

		public void StopPolling()
		{
			if (m_pollingEnumerator != null)
			{
				routineRunner.StopCoroutine(m_pollingEnumerator);
				m_pollingEnumerator = null;
			}
		}

		public void SendMessage(string text)
		{
			if (string.IsNullOrEmpty(text)) return;
			routineRunner.StartCoroutine(SendCoroutine(text));
		}

		public List<ChatMessage> GetCachedMessages()
		{
			return m_cachedMessages;
		}

		private IEnumerator PollCoroutine()
		{
			while (true)
			{
				string url = "http://" + GameConstants.Server.SERVER_URL + "/chat";
				WWW www = new WWW(url);
				yield return www;

				if (string.IsNullOrEmpty(www.error))
				{
					try
					{
						ChatResponse response = JsonConvert.DeserializeObject<ChatResponse>(www.text);
						if (response != null && response.messages != null)
						{
							m_cachedMessages = response.messages;
							if (m_cachedMessages.Count > 100)
							{
								m_cachedMessages.RemoveRange(0, m_cachedMessages.Count - 100);
							}
							updateSignal.Dispatch(m_cachedMessages);
						}
					}
					catch (System.Exception ex)
					{
						Debug.LogError("[GlobalChat] Failed to parse chat JSON: " + ex.Message);
					}
				}
				
				yield return new WaitForSeconds(m_pollInterval);
			}
		}

		private IEnumerator SendCoroutine(string text)
		{
			string url = "http://" + GameConstants.Server.SERVER_URL + "/chat";
			string playerName = "Minion " + (playerService != null ? playerService.ID.ToString() : "0");
			
			if (PlayerPrefs.HasKey("PlayerName"))
			{
				playerName = PlayerPrefs.GetString("PlayerName");
			}

			WWWForm form = new WWWForm();
			form.AddField("user", playerName);
			form.AddField("text", text);

			WWW www = new WWW(url, form);
			yield return www;

			if (!string.IsNullOrEmpty(www.error))
			{
				errorSignal.Dispatch("Failed to send message: " + www.error);
			}
			else
			{
				routineRunner.StartCoroutine(FetchOnceCoroutine());
			}
		}

		private IEnumerator FetchOnceCoroutine()
		{
			string url = "http://" + GameConstants.Server.SERVER_URL + "/chat";
			WWW www = new WWW(url);
			yield return www;

			if (string.IsNullOrEmpty(www.error))
			{
				try
				{
					ChatResponse response = JsonConvert.DeserializeObject<ChatResponse>(www.text);
					if (response != null && response.messages != null)
					{
						m_cachedMessages = response.messages;
						updateSignal.Dispatch(m_cachedMessages);
					}
				}
				catch {}
			}
		}
	}
}
