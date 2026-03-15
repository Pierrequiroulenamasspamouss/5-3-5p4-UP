namespace Kampai.UI.View
{
	public class RequiredItemView : global::Kampai.UI.View.PopupInfoButtonView
	{
		public global::Kampai.UI.View.KampaiImage ItemIcon;

		public global::UnityEngine.GameObject SolidCircle;

		public global::UnityEngine.GameObject DashedCircle;

		public global::UnityEngine.UI.Text ItemQuantity;

		public global::UnityEngine.UI.Text ItemCost;

		public global::UnityEngine.GameObject PurchasePanel;

		public global::Kampai.UI.View.RushButtonView RushBtn;

		public float PaddingInPixels;

		public new global::strange.extensions.signal.impl.Signal<int, global::UnityEngine.RectTransform> pointerDownSignal = new global::strange.extensions.signal.impl.Signal<int, global::UnityEngine.RectTransform>();

		public int ItemNeeded { get; set; }

		public int ItemDefinitionID { get; set; }

		public bool FullyAvailable { get; set; }

		public bool IsIngredient { get; set; }

		public override void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			pointerDownSignal.Dispatch(ItemDefinitionID, ItemIcon.rectTransform);
		}

		protected override void Start()
		{
			base.Start();
			if (PurchasePanel != null)
			{
				PurchasePanel.gameObject.SetActive(false);
			}
		}
	}
}
