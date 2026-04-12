namespace Kampai.UI.View
{
	public class StorageBuildingSaleSlotView : global::Kampai.Util.KampaiView
	{
		private enum SaleSlotState
		{
			CreateNew = 0,
			Pending = 1,
			Sold = 2,
			Discord = 3,
			Premium = 4
		}

		public global::Kampai.UI.View.KampaiImage IconImage;

		public global::UnityEngine.Animator FlipAnimator;

		public global::UnityEngine.UI.Text QuantityText;

		public global::UnityEngine.UI.Text TitleText;

		public global::UnityEngine.UI.Text ConfirmText;

		public global::UnityEngine.Color TicketColor;

		public global::Kampai.UI.View.ButtonView CreateButtonView;

		public global::Kampai.UI.View.ButtonView FacebookButtonView;

		public ScrollableButtonView PendingPanel;

		public global::UnityEngine.GameObject PendingAmountPanel;

		public global::UnityEngine.UI.Text PendingAmountText;

		public global::UnityEngine.UI.Text DeleteAmountText;

		public ScrollableButtonView CancelPendingButtonView;

		public global::Kampai.UI.View.KampaiImage TrashIcon;

		public global::UnityEngine.GameObject SoldPanel;

		public global::Kampai.UI.View.ButtonView CollectButtonView;

		public global::Kampai.UI.View.KampaiImage SoldCurrencyImage;

		public global::UnityEngine.UI.Text SoldCurrencyText;

		public ScrollableButtonView PremiumButtonView;

		public global::UnityEngine.UI.Text PremiumCostText;

		public float FadeInScaleTime = 0.5f;

		public float FadeOutScaleTime = 0.1666666f;

		public global::UnityEngine.Vector2 FadeOutSize = new global::UnityEngine.Vector2(0.75f, 0.75f);

		internal global::strange.extensions.signal.impl.Signal CheckIfValidItemsSignal = new global::strange.extensions.signal.impl.Signal();

		private global::System.Collections.IEnumerator waitFrame;

		private bool showTimer;

		private float remainingTime;

		private global::Kampai.Game.ItemDefinition itemDefinition;

		private global::Kampai.Game.MarketplaceSaleItem item;

		private int cancelSalePrice;

		private global::Kampai.Main.ILocalizationService localizationService;

		public int slotId { get; set; }

		internal void Init(global::Kampai.Main.ILocalizationService localizationService, int cancelSalePrice)
		{
			this.localizationService = localizationService;
			this.cancelSalePrice = cancelSalePrice;
			PremiumButtonView.EnableDoubleConfirm();
			ConfirmText.text = localizationService.GetString("confirmButtonText");
		}

		internal void UpdateSlot(global::Kampai.Game.MarketplaceSaleSlot slot)
		{
			switch (slot.state)
			{
			case global::Kampai.Game.MarketplaceSaleSlot.State.LOCKED:
			{
				bool flag = slot.Definition.type == global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType.FACEBOOK_UNLOCKABLE;
				bool flag2 = slot.Definition.type == global::Kampai.Game.MarketplaceSaleSlotDefinition.SlotType.PREMIUM_UNLOCKABLE;
				if (flag)
				{
					SetSlotState(global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.Discord);
				}
				else if (flag2)
				{
					PremiumButtonView.ResetTapState();
					SetSlotState(global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.Premium, slot.premiumCost);
				}
				break;
			}
			case global::Kampai.Game.MarketplaceSaleSlot.State.UNLOCKED:
				SetSlotState(global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.CreateNew);
				CheckIfValidItemsSignal.Dispatch();
				break;
			}
		}

		internal void UpdateItem(global::Kampai.Game.MarketplaceSaleItem item, global::Kampai.Game.ItemDefinition itemDefinition, global::Kampai.Game.ItemDefinition quantityItemDef, int rewardValue)
		{
			if (item != null && itemDefinition != null && quantityItemDef != null)
			{
				this.item = item;
				this.itemDefinition = itemDefinition;
				switch (item.state)
				{
				case global::Kampai.Game.MarketplaceSaleItem.State.PENDING:
					SetSlotState(global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.Pending, rewardValue);
					break;
				case global::Kampai.Game.MarketplaceSaleItem.State.SOLD:
					SetSlotState(global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.Sold, rewardValue);
					break;
				}
			}
		}

		internal void Flip(global::Kampai.Main.PlayGlobalSoundFXSignal soundFxSignal)
		{
			soundFxSignal.Dispatch("Play_marketplace_sellCardFlip_01");
			FlipAnimator.Play("Flip");
		}

		internal void EnableDebugTimer(bool isEnabled, int timeRemaining)
		{
			showTimer = isEnabled;
			remainingTime = timeRemaining;
		}

		private void Update()
		{
			if (showTimer)
			{
				remainingTime -= global::UnityEngine.Time.deltaTime;
				global::System.TimeSpan timeSpan = global::System.TimeSpan.FromSeconds(remainingTime);
				TitleText.text = UIUtils.FormatTime(timeSpan.TotalSeconds, localizationService);
			}
		}

		internal void FadeOut()
		{
			Go.killAllTweensWithTarget(base.transform);
			GoTween tween = new GoTween(base.transform, FadeInScaleTime, new GoTweenConfig().scale(FadeOutSize));
			Go.addTween(tween);
		}

		internal void FadeIn()
		{
			Go.killAllTweensWithTarget(base.transform);
			base.transform.localScale = new global::UnityEngine.Vector3(0.75f, 0.75f);
			GoTween tween = new GoTween(base.transform, FadeOutScaleTime, new GoTweenConfig().scale(global::UnityEngine.Vector3.one));
			Go.addTween(tween);
		}

		public void FlipSwitchToDeleteState()
		{
			string text = localizationService.GetString("SellSaleDeleteSale");
			if (TitleText.text.Equals(text))
			{
				SetItemTitleText();
				TrashIcon.gameObject.SetActive(true);
				PendingAmountPanel.gameObject.SetActive(true);
				CancelPendingButtonView.gameObject.SetActive(false);
			}
			else
			{
				SetTitleText(text);
				TrashIcon.gameObject.SetActive(false);
				PendingAmountPanel.gameObject.SetActive(false);
				CancelPendingButtonView.gameObject.SetActive(true);
			}
		}

		private void SetSlotState(global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState state, int costValue = 0)
		{
			CreateButtonView.gameObject.SetActive(false);
			FacebookButtonView.gameObject.SetActive(false);
			PendingPanel.gameObject.SetActive(false);
			SoldPanel.gameObject.SetActive(false);
			if (state != global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.Premium && waitFrame == null)
			{
				waitFrame = WaitAFrame();
				StartCoroutine(waitFrame);
			}
			switch (state)
			{
			case global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.CreateNew:
				SetupCreateState();
				CreateButtonView.gameObject.SetActive(true);
				break;
			case global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.Discord:
			case global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.Premium:
				SetupCreateState();
				if (state == global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.Discord)
				{
					FacebookButtonView.gameObject.SetActive(true);
					break;
				}
				if (waitFrame != null)
				{
					StopCoroutine(waitFrame);
					waitFrame = null;
				}
				PremiumButtonView.gameObject.SetActive(true);
				PremiumCostText.text = costValue.ToString();
				break;
			case global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.Pending:
				SetupPendingState(costValue);
				PendingPanel.gameObject.SetActive(true);
				break;
			case global::Kampai.UI.View.StorageBuildingSaleSlotView.SaleSlotState.Sold:
				SetupSoldState(costValue);
				SoldPanel.gameObject.SetActive(true);
				break;
			}
		}

		private void SetupCreateState()
		{
			SetTitleCreateNewSale();
			SetIconToTag();
			SetQuantityTextEnabled(false);
		}

		private void SetupPendingState(int rewardValue)
		{
			QuantityText.gameObject.SetActive(true);
			PendingPanel.gameObject.SetActive(true);
			PendingAmountPanel.gameObject.SetActive(true);
			CancelPendingButtonView.gameObject.SetActive(false);
			TrashIcon.gameObject.SetActive(true);
			DeleteAmountText.text = cancelSalePrice.ToString();
			PendingAmountText.text = rewardValue.ToString();
			QuantityText.text = string.Format("x{0}", item.QuantitySold);
			IconImage.color = global::UnityEngine.Color.white;
			SetImage(IconImage, itemDefinition.Image, itemDefinition.Mask);
			SetItemTitleText();
		}

		private void SetupSoldState(int rewardValue)
		{
			SoldCurrencyText.text = rewardValue.ToString();
			IconImage.gameObject.SetActive(false);
			SetItemTitleText();
			IconImage.color = global::UnityEngine.Color.white;
		}

		internal global::System.Collections.IEnumerator WaitAFrame()
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			if (!(this == null) && !(PremiumButtonView == null))
			{
				PremiumButtonView.gameObject.SetActive(false);
				waitFrame = null;
			}
		}

		private void SetIconToTag()
		{
			SetImage(IconImage, "btn_Main01_overlay_fill", "icn_nav_salesMinion_mask");
			IconImage.color = TicketColor;
		}

		private void SetQuantityTextEnabled(bool isEnabled)
		{
			QuantityText.gameObject.SetActive(isEnabled);
		}

		private void SetItemTitleText()
		{
			SetTitleText(LocalizeTitle(itemDefinition));
		}

		private void SetTitleText(string title)
		{
			TitleText.text = title;
		}

		private void SetTitleCreateNewSale()
		{
			SetTitleText(localizationService.GetString("SellPanelCreateNewSale"));
		}

		private string LocalizeTitle(global::Kampai.Game.ItemDefinition itemDefinition)
		{
			if (item != null)
			{
				return localizationService.GetString(itemDefinition.LocalizedKey);
			}
			if (item == null || item.Definition == null)
			{
				return "ITEM";
			}
			return localizationService.GetString(item.Definition.LocalizedKey);
		}

		private void SetImage(global::Kampai.UI.View.KampaiImage image, string iconPath, string maskPath)
		{
			image.gameObject.SetActive(true);
			if (string.IsNullOrEmpty(iconPath))
			{
				iconPath = "btn_Main01_fill";
			}
			if (string.IsNullOrEmpty(maskPath))
			{
				maskPath = "btn_Main01_mask";
			}
			image.sprite = UIUtils.LoadSpriteFromPath(iconPath);
			image.maskSprite = UIUtils.LoadSpriteFromPath(maskPath);
		}
	}
}
