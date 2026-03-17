namespace Kampai.UI.View
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Animator))]
	public class OrderBoardRequiredItemView : global::Kampai.UI.View.PopupInfoButtonView
	{
		public global::Kampai.UI.View.KampaiImage ItemIcon;

		public global::UnityEngine.UI.Text ItemCount;

		public global::UnityEngine.GameObject CheckMark;

		public global::UnityEngine.GameObject XMark;

		public global::UnityEngine.Animator IconAnimator;

		public new global::strange.extensions.signal.impl.Signal<global::Kampai.UI.View.OrderBoardRequiredItemView, global::UnityEngine.RectTransform> pointerDownSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.UI.View.OrderBoardRequiredItemView, global::UnityEngine.RectTransform>();

		public int ItemDefinitionID { get; set; }

		public bool playerHasEnoughItems { get; set; }

		public void Init()
		{
		}

		public override void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			pointerDownSignal.Dispatch(this, ItemIcon.rectTransform);
		}
	}
}
