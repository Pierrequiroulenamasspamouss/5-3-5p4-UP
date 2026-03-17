namespace Kampai.UI.View
{
	public class DragDropItemMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.DragDropItemView View { get; set; }

		[Inject]
		public global::Kampai.UI.View.OnDragItemSignal OnDragItemSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OnDropItemSignal OnDropItemSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		public override void OnRegister()
		{
			View.OnDragSignal.AddListener(OnDrag);
			View.OnDropSignal.AddListener(OnDrop);
			View.OnStartSignal.AddListener(OnStart);
			View.Init();
		}

		public override void OnRemove()
		{
			View.OnDragSignal.RemoveListener(OnDrag);
			View.OnDropSignal.RemoveListener(OnDrop);
			View.OnStartSignal.AddListener(OnStart);
		}

		private void OnStart(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			playSFXSignal.Dispatch("Play_pick_item_01");
		}

		private void OnDrag(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			OnDragItemSignal.Dispatch(View, eventData);
		}

		private void OnDrop(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			playSFXSignal.Dispatch("Play_place_item_01");
			OnDropItemSignal.Dispatch(View, eventData);
		}
	}
}
