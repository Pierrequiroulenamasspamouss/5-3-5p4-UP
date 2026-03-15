namespace Kampai.UI.View
{
	public class DropAreaMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.DropAreaView DropAreaView { get; set; }

		[Inject]
		public global::Kampai.UI.View.OnDragItemOverDropAreaSignal OnDragItemOverDropAreaSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OnDropItemOverDropAreaSignal OnDropItemOverDropAreaSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OnDragItemSignal OnDragItemSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OnDropItemSignal OnDropItemSignal { get; set; }

		public override void OnRegister()
		{
			OnDragItemSignal.AddListener(OnDragItem);
			OnDropItemSignal.AddListener(OnDropItem);
			DropAreaView.OnDragItemOverDropAreaSignal.AddListener(OnDragItemOverDropArea);
			DropAreaView.OnDropItemOverDropAreaSignal.AddListener(OnDropItemOverDropArea);
		}

		public override void OnRemove()
		{
			OnDragItemSignal.RemoveListener(OnDragItem);
			OnDropItemSignal.RemoveListener(OnDropItem);
			DropAreaView.OnDragItemOverDropAreaSignal.RemoveListener(OnDragItemOverDropArea);
			DropAreaView.OnDropItemOverDropAreaSignal.RemoveListener(OnDropItemOverDropArea);
		}

		private void OnDragItem(global::Kampai.UI.View.DragDropItemView item, global::UnityEngine.EventSystems.PointerEventData data)
		{
			DropAreaView.OnDragItem(item, data);
		}

		private void OnDropItem(global::Kampai.UI.View.DragDropItemView item, global::UnityEngine.EventSystems.PointerEventData data)
		{
			DropAreaView.OnEndDragItem(item, data);
		}

		private void OnDropItemOverDropArea(global::Kampai.UI.View.DragDropItemView item, bool successful)
		{
			OnDropItemOverDropAreaSignal.Dispatch(item, successful);
		}

		private void OnDragItemOverDropArea(global::Kampai.UI.View.DragDropItemView item, bool over)
		{
			OnDragItemOverDropAreaSignal.Dispatch(item, over);
		}
	}
}
