namespace Kampai.UI.View
{
	public class CurrencyButtonView : global::Kampai.Util.KampaiView
	{
		public bool PlaySoundOnClick = true;

		public global::UnityEngine.UI.Text Description;

		public global::UnityEngine.UI.Text ItemPrice;

		public global::UnityEngine.UI.Text ItemWorth;

		public global::Kampai.UI.View.KampaiImage ItemImage;

		public global::Kampai.UI.View.KampaiImage CostCurrencyIcon;

		public global::UnityEngine.UI.Image GreyOut;

		public global::UnityEngine.Transform VFXRoot;

		public global::UnityEngine.GameObject VFXPrefab;

		public global::UnityEngine.UI.Button purchaseButton;

		public global::UnityEngine.UI.Button infoButton;

		public global::UnityEngine.UI.Button imageButton;

		public global::UnityEngine.GameObject ValueBanner;

		public global::UnityEngine.UI.Text ValueBannerText;

		public global::Kampai.UI.View.KampaiImage ValueImage;

		public global::UnityEngine.GameObject MoreInfoButton;

		public global::strange.extensions.signal.impl.Signal PurchaseClickedSignal = new global::strange.extensions.signal.impl.Signal();

		public global::strange.extensions.signal.impl.Signal InfoClickedSignal = new global::strange.extensions.signal.impl.Signal();

		public global::UnityEngine.Animator animator;

		public global::UnityEngine.RuntimeAnimatorController controller;

		public bool isStarterPack;

		public global::Kampai.Game.StoreItemDefinition Definition { get; set; }

		public bool isCOPPAGated { get; set; }

		protected override void Start()
		{
			base.Start();
			if (isStarterPack)
			{
				animator.runtimeAnimatorController = controller;
				return;
			}
			MoreInfoButton.gameObject.SetActive(false);
			imageButton.enabled = false;
		}

		public void OnPurchaseClickEvent()
		{
			PurchaseClickedSignal.Dispatch();
		}

		public void OnInfoClickEvent()
		{
			InfoClickedSignal.Dispatch();
		}

		public void UnlockButton(bool isEnabled)
		{
			if (purchaseButton != null)
			{
				purchaseButton.gameObject.SetActive(isEnabled);
				GreyOut.gameObject.SetActive(!isEnabled);
				infoButton.gameObject.SetActive(isEnabled);
			}
		}
	}
}
