using strange.extensions.signal.impl;
using System.Collections.Generic;
using Kampai.Game;

namespace Kampai.UI.View
{
	public class GlobalChatUpdateSignal : Signal<List<ChatMessage>>
	{
	}

	public class GlobalChatErrorSignal : Signal<string>
	{
	}
}
