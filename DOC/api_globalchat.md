# Global Chat API Documentation

The Global Chat API allows players to communicate in real-time by sending and receiving messages.

## Endpoints

### 1. GET `/api/globalchat`
Retrieves the latest messages from the global chat.

- **Method**: `GET`
- **Query Parameters**:
  - `limit` (optional): Number of messages to retrieve (default: 50, max: 100).
- **Response**: `JSON Array` of message objects.
  - `userId`: The sender's unique ID.
  - `username`: The sender's display name (Discord name or default minion name).
  - `message`: The chat message content.
  - `timestamp`: UTC time the message was sent.
  - `avatar`: URL to the sender's Discord avatar (if available).

#### Example Response:
```json
[
  {
    "userId": "1000000284",
    "username": "Stuart",
    "message": "Hello everyone!",
    "timestamp": "2026-03-27 12:00:00",
    "avatar": "https://cdn.discordapp.com/avatars/..."
  }
]
```

---

### 2. POST `/api/globalchat`
Submits a new message to the global chat.

- **Method**: `POST`
- **Body**: `JSON Object`
  - `userId` (required): The sender's ID.
  - `message` (required): The message content (max 200 characters).
- **Responses**:
  - `200 OK`: `{"success": true}`
  - `400 Bad Request`: `{"success": false, "error": "Missing userId or message"}` or `{"success": false, "error": "Message too long"}`
  - `500 Server Error`: `{"success": false, "error": "Internal server error"}`

#### Request Example:
```json
{
  "userId": "1000000284",
  "message": "Banana!"
}
```
