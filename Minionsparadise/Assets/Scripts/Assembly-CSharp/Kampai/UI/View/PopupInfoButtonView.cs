namespace Kampai.UI.View
{
	public class PopupInfoButtonView : global::Kampai.UI.View.ButtonView, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.IDragHandler, global::UnityEngine.EventSystems.IPointerUpHandler, global::UnityEngine.EventSystems.IEndDragHandler, global::UnityEngine.EventSystems.IEventSystemHandler, global::UnityEngine.EventSystems.IBeginDragHandler
	{
		public global::strange.extensions.signal.impl.Signal pointerDownSignal = new global::strange.extensions.signal.impl.Signal();

		public global::strange.extensions.signal.impl.Signal pointerUpSignal = new global::strange.extensions.signal.impl.Signal();

		private int pointerId;

		private bool dragFinished;

		private global::UnityEngine.Vector2 currentDragDelta;

		private global::UnityEngine.UI.ScrollRect scrollRect;

		public virtual void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			pointerId = eventData.pointerId;
			pointerDownSignal.Dispatch();
		}

		public void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			pointerUpSignal.Dispatch();
		}

		public override void OnClickEvent()
		{
			ClickedSignal.Dispatch();
		}

		public void OnDrag(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (scrollRect != null)
			{
				scrollRect.OnDrag(eventData);
			}
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			currentDragDelta += eventData.delta;
			if (!dragFinished && (global::UnityEngine.Mathf.Abs(currentDragDelta.y) > rectTransform.rect.height / 2f || global::UnityEngine.Mathf.Abs(currentDragDelta.x) > rectTransform.rect.width))
			{
				pointerUpSignal.Dispatch();
				dragFinished = true;
			}
		}

		public void OnBeginDrag(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			dragFinished = false;
			currentDragDelta = global::UnityEngine.Vector2.zero;
			if (scrollRect != null)
			{
				scrollRect.OnBeginDrag(eventData);
			}
		}

		public void OnEndDrag(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (!dragFinished && pointerId == eventData.pointerId)
			{
				pointerUpSignal.Dispatch();
			}
			if (scrollRect != null)
			{
				scrollRect.OnEndDrag(eventData);
			}
		}

		protected virtual void OnEnable()
		{
			scrollRect = base.transform.GetComponentTypeInParent<global::UnityEngine.UI.ScrollRect>();
		}

		protected override void Start()
		{
			base.Start();
			scrollRect = base.transform.GetComponentTypeInParent<global::UnityEngine.UI.ScrollRect>();
		}
	}
}
