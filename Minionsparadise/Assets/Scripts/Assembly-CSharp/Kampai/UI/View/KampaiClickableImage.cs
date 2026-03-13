namespace Kampai.UI.View
{
	public class KampaiClickableImage : global::Kampai.UI.View.KampaiImage, global::UnityEngine.EventSystems.IPointerClickHandler, global::UnityEngine.EventSystems.IEventSystemHandler
	{
		public global::strange.extensions.signal.impl.Signal ClickedSignal = new global::strange.extensions.signal.impl.Signal();

		private bool enableClick;

		public void OnPointerClick(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			if (enableClick)
			{
				ClickedSignal.Dispatch();
			}
		}

		public void EnableClick(bool enable)
		{
			enableClick = enable;
		}
	}
}
