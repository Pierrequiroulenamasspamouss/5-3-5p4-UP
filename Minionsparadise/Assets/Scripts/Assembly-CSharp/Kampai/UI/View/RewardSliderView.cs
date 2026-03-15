namespace Kampai.UI.View
{
	public class RewardSliderView : global::Kampai.Util.KampaiView, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.IDragHandler, global::UnityEngine.EventSystems.IPointerUpHandler, global::UnityEngine.EventSystems.IEventSystemHandler
	{
		public global::UnityEngine.UI.Text description;

		public global::UnityEngine.UI.Text itemQuantity;

		public global::Kampai.UI.View.KampaiImage icon;

		public global::strange.extensions.signal.impl.Signal pointerDownSignal = new global::strange.extensions.signal.impl.Signal();

		public global::strange.extensions.signal.impl.Signal pointerUpSignal = new global::strange.extensions.signal.impl.Signal();

		public int ID { get; set; }

		public global::UnityEngine.UI.ScrollRect scrollRect { get; set; }

		public void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			scrollRect.OnBeginDrag(eventData);
			pointerDownSignal.Dispatch();
		}

		public void OnDrag(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			scrollRect.OnDrag(eventData);
		}

		public void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			scrollRect.OnEndDrag(eventData);
			pointerUpSignal.Dispatch();
		}
	}
}
