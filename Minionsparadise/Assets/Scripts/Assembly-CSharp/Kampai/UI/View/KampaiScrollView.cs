namespace Kampai.UI.View
{
	public class KampaiScrollView : global::Kampai.Util.KampaiView, global::System.Collections.IEnumerable, global::System.Collections.Generic.IEnumerable<global::UnityEngine.MonoBehaviour>
	{
		public enum MoveDirection
		{
			Start = 0,
			End = 1,
			LastLocation = 2
		}

		[global::UnityEngine.SerializeField]
		private float m_colunmNumber = 1f;

		[global::UnityEngine.SerializeField]
		private float m_rowNumber = 1f;

		public global::UnityEngine.UI.ScrollRect ScrollRect;

		public global::UnityEngine.RectTransform ItemContainer;

		public global::System.Collections.Generic.IList<global::UnityEngine.MonoBehaviour> ItemViewList = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();

		public float ColumnNumber
		{
			get
			{
				return m_colunmNumber;
			}
			set
			{
				m_colunmNumber = value;
			}
		}

		public float RowNumber
		{
			get
			{
				return m_rowNumber;
			}
			set
			{
				m_rowNumber = value;
			}
		}

		public global::UnityEngine.MonoBehaviour this[int index]
		{
			get
			{
				return ItemViewList[index];
			}
			set
			{
				ItemViewList[index] = value;
			}
		}

		public float ItemSize { get; private set; }

		public bool isVertical
		{
			get
			{
				return ScrollRect.vertical;
			}
		}

		global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		internal void SetupScrollView(float columns = -1f, global::Kampai.UI.View.KampaiScrollView.MoveDirection moveDirection = global::Kampai.UI.View.KampaiScrollView.MoveDirection.LastLocation)
		{
			if (columns == -1f)
			{
				columns = ((!isVertical) ? m_rowNumber : m_colunmNumber);
			}
			if (ItemViewList == null)
			{
				return;
			}
			float y = ItemContainer.anchoredPosition.y;
			int count = ItemViewList.Count;
			ScrollRect.verticalNormalizedPosition = 1f;
			int num = count % global::UnityEngine.Mathf.FloorToInt(columns);
			float num2 = ItemSize * (float)(count / global::UnityEngine.Mathf.FloorToInt(columns) + ((num != 0) ? 1 : 0));
			if (isVertical)
			{
				ItemContainer.offsetMin = new global::UnityEngine.Vector2(0f, 0f - num2);
				ItemContainer.offsetMax = new global::UnityEngine.Vector2(0f, 0f);
			}
			else
			{
				ItemContainer.offsetMin = new global::UnityEngine.Vector2(0f, 0f);
				ItemContainer.offsetMax = new global::UnityEngine.Vector2(num2, 0f);
			}
			if ((float)count <= columns * m_rowNumber)
			{
				ScrollRect.movementType = global::UnityEngine.UI.ScrollRect.MovementType.Clamped;
				moveDirection = global::Kampai.UI.View.KampaiScrollView.MoveDirection.Start;
			}
			else
			{
				ScrollRect.movementType = global::UnityEngine.UI.ScrollRect.MovementType.Elastic;
			}
			switch (moveDirection)
			{
			case global::Kampai.UI.View.KampaiScrollView.MoveDirection.Start:
				ItemContainer.anchoredPosition = ((!isVertical) ? new global::UnityEngine.Vector2(0f, 0f) : new global::UnityEngine.Vector2(ItemContainer.anchoredPosition.x, 0f));
				break;
			case global::Kampai.UI.View.KampaiScrollView.MoveDirection.End:
				ItemContainer.anchoredPosition = ((!isVertical) ? new global::UnityEngine.Vector2(y, ItemContainer.anchoredPosition.y) : new global::UnityEngine.Vector2(ItemContainer.anchoredPosition.x, y));
				if (num != 0)
				{
					TweenToPosition(new global::UnityEngine.Vector2(0f, 0f), 1f);
				}
				break;
			case global::Kampai.UI.View.KampaiScrollView.MoveDirection.LastLocation:
				ItemContainer.anchoredPosition = ((!isVertical) ? new global::UnityEngine.Vector2(y, 0f) : new global::UnityEngine.Vector2(ItemContainer.anchoredPosition.x, y));
				break;
			}
		}

		internal global::UnityEngine.Vector3 GetViewSize()
		{
			global::UnityEngine.RectTransform rectTransform = ScrollRect.transform as global::UnityEngine.RectTransform;
			return (!(rectTransform == null)) ? new global::UnityEngine.Vector3(rectTransform.rect.width / m_colunmNumber, rectTransform.rect.height / m_rowNumber) : global::UnityEngine.Vector3.zero;
		}

		private global::UnityEngine.RectTransform PositionItem(global::UnityEngine.MonoBehaviour view, int index)
		{
			global::UnityEngine.RectTransform rectTransform = view.transform as global::UnityEngine.RectTransform;
			if (rectTransform == null)
			{
				return null;
			}
			float num = 1f;
			if (isVertical)
			{
				int num2 = global::UnityEngine.Mathf.FloorToInt(m_colunmNumber);
				int num3 = index % num2;
				int num4 = index / num2;
				rectTransform.sizeDelta = GetViewSize();
				ItemSize = rectTransform.sizeDelta.y;
				rectTransform.SetParent(ItemContainer, false);
				rectTransform.offsetMin = new global::UnityEngine.Vector2(0f, (float)(-num4 - 1) * ItemSize);
				rectTransform.offsetMax = new global::UnityEngine.Vector2(0f, (float)(-num4) * ItemSize);
				rectTransform.anchorMin = new global::UnityEngine.Vector2(num / m_colunmNumber * (float)num3, 1f);
				rectTransform.anchorMax = new global::UnityEngine.Vector2(num / m_colunmNumber * (float)(num3 + 1), 1f);
			}
			else
			{
				int num5 = global::UnityEngine.Mathf.FloorToInt(m_rowNumber);
				int num6 = index % num5;
				int num7 = index / num5;
				rectTransform.sizeDelta = GetViewSize();
				ItemSize = rectTransform.sizeDelta.x;
				rectTransform.SetParent(ItemContainer, false);
				rectTransform.offsetMin = new global::UnityEngine.Vector2((float)num7 * ItemSize, 1f);
				rectTransform.offsetMax = new global::UnityEngine.Vector2((float)(num7 + 1) * ItemSize, 1f);
				rectTransform.anchorMin = new global::UnityEngine.Vector2(0f, num / m_rowNumber * (float)num6);
				rectTransform.anchorMax = new global::UnityEngine.Vector2(0f, num / m_rowNumber * (float)(num6 + 1));
			}
			rectTransform.localScale = global::UnityEngine.Vector3.one;
			return rectTransform;
		}

		public void ClearItems()
		{
			foreach (global::UnityEngine.MonoBehaviour itemView in ItemViewList)
			{
				global::UnityEngine.Object.Destroy(itemView.gameObject);
			}
			ItemViewList.Clear();
		}

		public void AddList<T>(global::System.Collections.Generic.IList<T> items, global::System.Func<int, T, global::UnityEngine.MonoBehaviour> createItemFunc, global::System.Func<T, bool> hasItemFunc = null, bool setupScrollAfter = true) where T : global::Kampai.Game.Instance
		{
			if (createItemFunc == null)
			{
				SetupScrollView();
				return;
			}
			foreach (T item in items)
			{
				if (hasItemFunc == null || hasItemFunc(item))
				{
					global::UnityEngine.MonoBehaviour slotView = createItemFunc(ItemViewList.Count, item);
					AddItem(slotView);
				}
			}
			if (setupScrollAfter)
			{
				SetupScrollView();
			}
		}

		public void AddItem(global::UnityEngine.MonoBehaviour slotView)
		{
			if (!(slotView == null))
			{
				PositionItem(slotView, ItemViewList.Count);
				ItemViewList.Add(slotView);
			}
		}

		public void TweenToPosition(global::UnityEngine.Vector2 newPosition, float tweenTime)
		{
			if (tweenTime > 0f)
			{
				GoTweenConfig config = new GoTweenConfig().vector2Prop("normalizedPosition", newPosition).setEaseType(GoEaseType.SineOut);
				GoTween tween = new GoTween(ScrollRect, tweenTime, config);
				Go.addTween(tween);
			}
			else
			{
				ScrollRect.normalizedPosition = newPosition;
			}
		}

		public void EnableScrolling(bool horizontal, bool vertial)
		{
			ScrollRect.horizontal = horizontal;
			ScrollRect.vertical = vertial;
		}

		public global::System.Collections.Generic.IEnumerator<global::UnityEngine.MonoBehaviour> GetEnumerator()
		{
			return ItemViewList.GetEnumerator();
		}
	}
}
