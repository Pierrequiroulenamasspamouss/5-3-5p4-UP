namespace Kampai.UI.View
{
	public class SellQuantityButtonView : global::Kampai.UI.View.ButtonView, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.IPointerUpHandler, global::UnityEngine.EventSystems.IEventSystemHandler
	{
		public class HeldDownSignal : global::strange.extensions.signal.impl.Signal<int>
		{
		}

		internal readonly float COUNT_WAIT_TIME = 0.25f;

		internal readonly float PRICE_INIT_WAIT_TIME = 0.1f;

		internal readonly float PRICE_MAX_WAIT_TIME = 1.5f;

		public global::Kampai.UI.View.SellQuantityButtonView.HeldDownSignal heldDownSignal = new global::Kampai.UI.View.SellQuantityButtonView.HeldDownSignal();

		public global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData> OnPointerUpSignal = new global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData>();

		public global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData> OnPointerDownSignal = new global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData>();

		private bool m_isEnabled;

		internal bool IsHeldDown { get; set; }

		public int MaxValue { get; set; }

		public int MinValue { get; set; }

		public bool IsPriceButton { get; set; }

		protected override void Awake()
		{
			global::Kampai.Util.KampaiView.BubbleToContextOnAwake(this, ref currentContext, true);
		}

		public override void OnClickEvent()
		{
			ClickedSignal.Dispatch();
		}

		public void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			IsHeldDown = true;
			global::UnityEngine.UI.Button component = GetComponent<global::UnityEngine.UI.Button>();
			m_isEnabled = component != null && component.interactable;
			OnPointerDownSignal.Dispatch(eventData);
		}

		public void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			IsHeldDown = false;
			OnPointerUpSignal.Dispatch(eventData);
			if (m_isEnabled)
			{
				m_isEnabled = false;
			}
		}

		public void SetSize(float height)
		{
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			if (!(rectTransform == null))
			{
				rectTransform.sizeDelta = new global::UnityEngine.Vector2(height, height);
			}
		}
	}
}
