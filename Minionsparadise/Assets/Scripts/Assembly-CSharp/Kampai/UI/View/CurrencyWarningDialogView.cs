namespace Kampai.UI.View
{
	public class CurrencyWarningDialogView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text CurrencyNeededLabel;

		public global::UnityEngine.UI.Text CurrencyNeededButtonLabel;

		public global::Kampai.UI.View.ButtonView CancelButton;

		public global::Kampai.UI.View.DoubleConfirmButtonView PurchaseButton;

		internal void SetCurrencyNeeded(int cost, int amountNeeded)
		{
			CurrencyNeededLabel.text = UIUtils.FormatLargeNumber(amountNeeded);
			CurrencyNeededButtonLabel.text = UIUtils.FormatLargeNumber(cost);
		}
	}
}
