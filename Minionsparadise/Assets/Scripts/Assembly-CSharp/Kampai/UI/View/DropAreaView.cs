namespace Kampai.UI.View
{
	public class DropAreaView : global::Kampai.Util.KampaiView
	{
		internal global::strange.extensions.signal.impl.Signal<global::Kampai.UI.View.DragDropItemView, bool> OnDragItemOverDropAreaSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.UI.View.DragDropItemView, bool>();

		internal global::strange.extensions.signal.impl.Signal<global::Kampai.UI.View.DragDropItemView, bool> OnDropItemOverDropAreaSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.UI.View.DragDropItemView, bool>();

		internal void OnDragItem(global::Kampai.UI.View.DragDropItemView item, global::UnityEngine.EventSystems.PointerEventData pointerEventData)
		{
			global::UnityEngine.GameObject gameObject = pointerEventData.pointerCurrentRaycast.gameObject;
			if (gameObject != null)
			{
				OnDragItemOverDropAreaSignal.Dispatch(item, base.name == gameObject.name);
			}
			else
			{
				OnDragItemOverDropAreaSignal.Dispatch(item, false);
			}
		}

		internal void OnEndDragItem(global::Kampai.UI.View.DragDropItemView item, global::UnityEngine.EventSystems.PointerEventData pointerEventData)
		{
			global::UnityEngine.GameObject gameObject = pointerEventData.pointerCurrentRaycast.gameObject;
			if (gameObject != null)
			{
				OnDropItemOverDropAreaSignal.Dispatch(item, base.name == gameObject.name);
			}
			else
			{
				OnDropItemOverDropAreaSignal.Dispatch(item, false);
			}
		}
	}
}
