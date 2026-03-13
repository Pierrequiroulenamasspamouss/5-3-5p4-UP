namespace Kampai.UI.View
{
	public class BuffButtonView : global::Kampai.Util.KampaiView, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.IEventSystemHandler
	{
		public bool pulse;

		public float popupOffset;

		public global::strange.extensions.signal.impl.Signal pointerDownSignal = new global::strange.extensions.signal.impl.Signal();

		public virtual void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			pointerDownSignal.Dispatch();
		}
	}
}
