# Guide: Global Chat Unity Implementation

This guide describes how to implement the Global Chat client in Unity using `UnityWebRequest`.

## 1. Fetching Global Chat Messages

Use a `Coroutine` to periodically fetch the latest messages from the server.

```csharp
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GlobalChatManager : MonoBehaviour
{
    private string chatUrl = "http://your-server-ip:44733/api/globalchat";

    IEnumerator GetChatMessages()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(chatUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string jsonResponse = webRequest.downloadHandler.text;
                // Parse the JSON array into your message data structures
                Debug.Log("Chat Messages Received: " + jsonResponse);
            }
            else
            {
                Debug.LogError("Error fetching chat: " + webRequest.error);
            }
        }
    }
}
```

## 2. Posting a Chat Message

Send a `POST` request with a JSON body containing the `userId` and `message`.

```csharp
    public void SendChatMessage(string userId, string message)
    {
        StartCoroutine(PostChatMessage(userId, message));
    }

    IEnumerator PostChatMessage(string userId, string message)
    {
        string json = JsonUtility.ToJson(new ChatMessage { userId = userId, message = message });
        
        using (UnityWebRequest webRequest = new UnityWebRequest(chatUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Message sent successfully!");
            }
            else
            {
                Debug.LogError("Error sending message: " + webRequest.error);
            }
        }
    }

    [System.Serializable]
    public class ChatMessage
    {
        public string userId;
        public string message;
    }
```

## 3. UI Implementation Tips

- **Polling:** Since this is a simple REST API (not WebSockets), poll the `GET` endpoint every 5-10 seconds while the chat window is open.
- **Limit:** Use the `limit` query parameter (e.g., `?limit=20`) to avoid fetching too many messages at once.
- **Styling:** Use the `avatar` URL returned in the message objects to display player avatars.
- **Formatting:** Ensure the message length is limited to 200 characters in your UI's input field.
