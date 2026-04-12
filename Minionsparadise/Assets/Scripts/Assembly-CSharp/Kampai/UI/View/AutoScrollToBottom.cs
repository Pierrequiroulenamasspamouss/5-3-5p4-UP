using UnityEngine;
using UnityEngine.UI;

namespace Kampai.UI.View
{
	[RequireComponent(typeof(ScrollRect))]
	public class AutoScrollToBottom : MonoBehaviour
	{
		private ScrollRect m_scrollRect;
		private bool m_shouldScroll = false;

		private void Awake()
		{
			m_scrollRect = GetComponent<ScrollRect>();
		}

		public void ScrollToBottom()
		{
			m_shouldScroll = true;
		}

		private void LateUpdate()
		{
			if (m_shouldScroll)
			{
				m_scrollRect.verticalNormalizedPosition = 0;
				m_shouldScroll = false;
			}
		}
	}
}
