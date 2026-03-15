namespace Kampai.UI.View
{
	public class LandExpansionRushDialogView : global::Kampai.UI.View.RushDialogView
	{
		public global::UnityEngine.RectTransform CurrencyScrollViewParent;

		internal override void SetupDialog(global::Kampai.UI.View.RushDialogView.RushDialogType type, bool showPurchaseButton)
		{
			switch (type)
			{
			case global::Kampai.UI.View.RushDialogView.RushDialogType.BRIDGE_QUEST:
				Title.text = localService.GetString("RepairBridge");
				break;
			case global::Kampai.UI.View.RushDialogView.RushDialogType.LAND_EXPANSION:
				Title.text = localService.GetString("ExpandLandPrompt");
				break;
			}
			base.SetupDialog(type, showPurchaseButton);
		}
	}
}
