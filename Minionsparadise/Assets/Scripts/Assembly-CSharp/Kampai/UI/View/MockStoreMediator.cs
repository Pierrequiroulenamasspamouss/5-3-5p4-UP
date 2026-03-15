namespace Kampai.UI.View
{
	public class MockStoreMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MockStoreView>
	{
		private string debugSKU;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ICurrencyService currencyService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.ConfirmPurchase.ClickedSignal.AddListener(ConfirmPurchase);
			base.view.DenyPurchase.ClickedSignal.AddListener(DenyPurchase);
			base.view.AskParents.ClickedSignal.AddListener(AskParents);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.ConfirmPurchase.ClickedSignal.RemoveListener(ConfirmPurchase);
			base.view.DenyPurchase.ClickedSignal.RemoveListener(DenyPurchase);
			base.view.AskParents.ClickedSignal.RemoveListener(AskParents);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.Util.Tuple<string, string> tuple = args.Get<global::Kampai.Util.Tuple<string, string>>();
			string item = tuple.Item1;
			string item2 = tuple.Item2;
			Setup(item, item2);
		}

		private void ConfirmPurchase()
		{
			currencyService.PurchaseSucceededAndValidatedCallback(debugSKU);
			Close();
		}

		private void DenyPurchase()
		{
			currencyService.PurchaseCanceledCallback(debugSKU, uint.MaxValue);
			Close();
		}

		private void AskParents()
		{
			currencyService.PurchaseDeferredCallback(debugSKU);
			Close();
		}

		protected override void Close()
		{
			base.uiRemovedSignal.Dispatch(base.view.gameObject);
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "PurchaseAuthorizationWarning");
			soundFXSignal.Dispatch("Play_button_click_01");
		}

		private void Setup(string sku, string cost)
		{
			debugSKU = sku;
			base.view.CostText.text = cost;
		}
	}
}
