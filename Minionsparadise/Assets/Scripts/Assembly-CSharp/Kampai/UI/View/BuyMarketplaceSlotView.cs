namespace Kampai.UI.View
{
	public class BuyMarketplaceSlotView : global::Kampai.Util.KampaiView
	{
		public enum State
		{
			Pending = 0,
			Sold = 1,
			Facebook = 2
		}

		public global::UnityEngine.UI.Text ItemTitleText;

		public global::Kampai.UI.View.KampaiImage ItemImage;

		public global::UnityEngine.UI.Text QuantityText;

		public global::Kampai.UI.View.KampaiImage SoldIconImage;

		public ScrollableButtonView BuyButtonView;

		public ScrollableButtonView FacebookButtonView;

		public global::Kampai.UI.View.KampaiImage BuyButtonFill;

		public global::Kampai.UI.View.KampaiImage FadeOutImage;

		public global::UnityEngine.UI.Text PriceText;

		public global::UnityEngine.GameObject FacebookPanel;

		public global::UnityEngine.GameObject PricePanel;

		public global::Kampai.UI.View.KampaiImage SlotBlurImage1;

		public global::Kampai.UI.View.KampaiImage SlotBlurImage2;

		private global::Kampai.Main.ILocalizationService localizationService;

		private global::Kampai.Game.IDefinitionService definitionService;

		public global::UnityEngine.Animation shakeIconAnimation;

		public bool isSlotAnimationPlaying;

		private bool playTickingSound;

		private bool isBlurSlotMoving = true;

		private bool isFirstUpdate = true;

		private global::UnityEngine.Vector2 originialIconPosition;

		private global::UnityEngine.Vector2 targetIconPosition;

		private global::UnityEngine.Vector2 originalBlurPosition1;

		private global::UnityEngine.Vector2 originalBlurPosition2;

		private global::System.Collections.Generic.List<AbstractGoTween> runningSpinnerTweens = new global::System.Collections.Generic.List<AbstractGoTween>();

		public global::Kampai.Game.MarketplaceBuyItem BuyItem { get; set; }

		internal global::Kampai.UI.View.BuyMarketplaceSlotView.State CurrentState { get; set; }

		public int slotIndex { get; internal set; }

		public global::Kampai.Game.ISocialService facebookService { get; set; }

		public int slotId { get; set; }

		internal void Init()
		{
			BuyButtonView.EnableDoubleConfirm();
			FacebookButtonView.DisableDoubleConfirm();
		}

		public bool SetupBuyItem(global::Kampai.Main.ILocalizationService localizationService, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.ISocialService facebookService, bool isCOPPAGated, global::Kampai.Game.MarketplaceBuyItem buyItem, global::strange.extensions.signal.impl.Signal<string> playSFXSignal, global::Kampai.Game.IMarketplaceService marketPlaceService)
		{
			this.localizationService = localizationService;
			this.facebookService = facebookService;
			this.definitionService = definitionService;
			bool result = false;
			if (buyItem == BuyItem)
			{
				isFirstUpdate = true;
			}
			else
			{
				result = true;
			}
			BuyItem = buyItem;
			global::Kampai.Game.MarketplaceDefinition marketplaceDefinition = this.definitionService.Get<global::Kampai.Game.MarketplaceDefinition>();
			if (isCOPPAGated || facebookService.isLoggedIn || slotIndex < marketplaceDefinition.StartingBuyAds || marketPlaceService.DebugFacebook)
			{
				SetMarketplaceBuyItem(BuyItem, playSFXSignal);
			}
			else if (slotIndex >= marketplaceDefinition.StartingBuyAds)
			{
				SetupFacebookSlot(playSFXSignal);
			}
			return result;
		}

		private void MoveBlurSlot(global::Kampai.UI.View.KampaiImage img, global::UnityEngine.Vector2 endPosition, global::Kampai.UI.View.KampaiImage otherImage)
		{
			float magnitude = (endPosition - img.rectTransform.anchoredPosition).magnitude;
			float duration = magnitude / 900f;
			if (magnitude < 0.1f)
			{
				duration = 0.00011111111f;
			}
			GoTween item = Go.to(img.rectTransform, duration, new GoTweenConfig().setEaseType(GoEaseType.Linear).vector2Prop("anchoredPosition", endPosition).onComplete(delegate(AbstractGoTween thisTween)
			{
				thisTween.destroy();
				if (isBlurSlotMoving)
				{
					img.rectTransform.anchoredPosition = otherImage.rectTransform.anchoredPosition + new global::UnityEngine.Vector2(0f, otherImage.rectTransform.sizeDelta.y);
					MoveBlurSlot(img, endPosition, otherImage);
				}
			}));
			runningSpinnerTweens.Add(item);
		}

		private GoTween FadeInBlurImage(global::Kampai.UI.View.KampaiImage img)
		{
			return new GoTween(img, 1f, new GoTweenConfig().onInit(delegate
			{
				img.gameObject.SetActive(true);
				img.color = new global::UnityEngine.Color(1f, 1f, 1f, 0f);
			}).setEaseType(GoEaseType.QuadOut).colorProp("color", new global::UnityEngine.Color(1f, 1f, 1f, 1f)));
		}

		private GoTween FadeOutBlurImage(global::Kampai.UI.View.KampaiImage img)
		{
			return new GoTween(img, 1f, new GoTweenConfig().setEaseType(GoEaseType.QuadOut).colorProp("color", new global::UnityEngine.Color(1f, 1f, 1f, 0f)).onComplete(delegate
			{
				isBlurSlotMoving = false;
			}));
		}

		private void SetMarketplaceBuyItem(global::Kampai.Game.MarketplaceBuyItem marketplaceBuyItem, global::strange.extensions.signal.impl.Signal<string> playSFXSignal)
		{
			BuyButtonView.ResetTapState();
			BuyItem = marketplaceBuyItem;
			FadeOutImage.gameObject.SetActive(false);
			PricePanel.gameObject.SetActive(true);
			FacebookPanel.gameObject.SetActive(false);
			SetIsSold(marketplaceBuyItem.BoughtFlag);
			if (isFirstUpdate)
			{
				DisplayItemInfo();
				originialIconPosition = ItemImage.rectTransform.anchorMin;
				targetIconPosition = originialIconPosition - new global::UnityEngine.Vector2(0f, 1f);
				isFirstUpdate = false;
				originalBlurPosition1 = SlotBlurImage1.rectTransform.anchoredPosition;
				originalBlurPosition2 = SlotBlurImage2.rectTransform.anchoredPosition;
			}
			else if (slotIndex > 7)
			{
				DisplayItemInfo();
			}
			else
			{
				StopAllCoroutines();
				StartCoroutine(PlaySpinningSound(playSFXSignal));
				StartCoroutine(StartSlotMachineAnimation(playSFXSignal));
			}
		}

		private void DisplayItemInfo()
		{
			global::Kampai.Game.Item item = new global::Kampai.Game.Item(definitionService.Get<global::Kampai.Game.ItemDefinition>(BuyItem.Definition.ItemID));
			SetIcon(item.Definition.Image, item.Definition.Mask);
			SetTitleText(item);
			QuantityText.text = string.Format("x{0}", BuyItem.BuyQuantity);
			PriceText.text = UIUtils.FormatLargeNumber(BuyItem.BuyPrice);
		}

		public void StopSlotMachine()
		{
			StopAllCoroutines();
			ClearOldTweens();
			DisplayItemInfo();
			if (BuyButtonView.isActiveAndEnabled && BuyButtonView.animator != null)
			{
				BuyButtonView.animator.enabled = true;
			}
		}

		private global::System.Collections.IEnumerator PlaySpinningSound(global::strange.extensions.signal.impl.Signal<string> playSFXSignal)
		{
			yield return new global::UnityEngine.WaitForSeconds(0.5f);
			while (playTickingSound)
			{
				playSFXSignal.Dispatch("Play_marketplace_slotTick_01");
				yield return new global::UnityEngine.WaitForSeconds(0.12f);
			}
		}

		internal void ClearOldTweens()
		{
			isSlotAnimationPlaying = false;
			playTickingSound = false;
			foreach (AbstractGoTween runningSpinnerTween in runningSpinnerTweens)
			{
				runningSpinnerTween.complete();
			}
			runningSpinnerTweens.Clear();
		}

		private global::System.Collections.IEnumerator StartSlotMachineAnimation(global::strange.extensions.signal.impl.Signal<string> playSFXSignal)
		{
			BuyButtonView.ResetAnim();
			yield return new global::UnityEngine.WaitForSeconds(0.2f * (float)(slotIndex + 1));
			if (BuyButtonView.isActiveAndEnabled && BuyButtonView.animator != null)
			{
				BuyButtonView.animator.enabled = false;
			}
			global::UnityEngine.Color visibleColor = new global::UnityEngine.Color(1f, 1f, 1f, 1f);
			global::UnityEngine.Color transparentColor = new global::UnityEngine.Color(1f, 1f, 1f, 0f);
			global::UnityEngine.Color originalTextColor = global::UnityEngine.Color.black;
			global::UnityEngine.Vector2 anchorDiff = ItemImage.rectTransform.anchorMax - ItemImage.rectTransform.anchorMin;
			ClearOldTweens();
			isSlotAnimationPlaying = true;
			playTickingSound = true;
			GoTweenFlow slotMachineFlow = new GoTweenFlow();
			slotMachineFlow.insert(0f, new GoTween(ItemImage, 0.35f, new GoTweenConfig().setEaseType(GoEaseType.Linear).colorProp("color", transparentColor)));
			slotMachineFlow.insert(0f, new GoTween(QuantityText, 0.35f, new GoTweenConfig().colorProp("color", transparentColor)));
			slotMachineFlow.insert(0f, new GoTween(ItemTitleText, 0.35f, new GoTweenConfig().colorProp("color", transparentColor)));
			slotMachineFlow.insert(0f, new GoTween(ItemImage.rectTransform, 1f, new GoTweenConfig().setEaseType(GoEaseType.Linear).vector2Prop("anchorMin", targetIconPosition).vector2Prop("anchorMax", targetIconPosition + anchorDiff)));
			slotMachineFlow.insert(0f, new GoTween(PriceText.rectTransform, 0.4f, new GoTweenConfig().vector2Prop("anchorMin", new global::UnityEngine.Vector2(0.5f, -1f)).vector2Prop("anchorMax", new global::UnityEngine.Vector2(0.5f, 0f))));
			slotMachineFlow.insert(0.35f, FadeInBlurImage(SlotBlurImage1));
			slotMachineFlow.insert(0.35f, FadeInBlurImage(SlotBlurImage2));
			slotMachineFlow.insert(0.95f, FadeOutBlurImage(SlotBlurImage1));
			slotMachineFlow.insert(0.95f, FadeOutBlurImage(SlotBlurImage2));
			global::strange.extensions.signal.impl.Signal<string> playSFXSignal2 = default(global::strange.extensions.signal.impl.Signal<string>);
			slotMachineFlow.insert(0.95f, new GoTween(ItemImage.rectTransform, 1f, new GoTweenConfig().onInit(delegate
			{
				ItemImage.color = new global::UnityEngine.Color(1f, 1f, 1f, 0f);
				ItemImage.rectTransform.anchorMin = originialIconPosition + new global::UnityEngine.Vector2(0f, 2f);
				ItemImage.rectTransform.anchorMax = originialIconPosition + new global::UnityEngine.Vector2(0f, 2f) + anchorDiff;
				PriceText.rectTransform.anchorMin = new global::UnityEngine.Vector2(0.5f, 1f);
				PriceText.rectTransform.anchorMax = new global::UnityEngine.Vector2(0.5f, 2f);
				DisplayItemInfo();
			}).onComplete(delegate
			{
				playTickingSound = false;
				if (isSlotAnimationPlaying)
				{
					playSFXSignal2.Dispatch("Play_marketplace_slotEnd_01");
				}
			}).vector2Prop("anchorMin", originialIconPosition)
				.vector2Prop("anchorMax", originialIconPosition + anchorDiff)));
			slotMachineFlow.insert(1.7f, new GoTween(ItemImage, 0.5f, new GoTweenConfig().setEaseType(GoEaseType.Linear).colorProp("color", visibleColor).onComplete(delegate
			{
				isSlotAnimationPlaying = false;
			})));
			GoTweenChain priceTextChain = new GoTweenChain();
			priceTextChain.append(new GoTween(PriceText.rectTransform, 0.25f, new GoTweenConfig().vector2Prop("anchorMin", new global::UnityEngine.Vector2(0.5f, 0f)).vector2Prop("anchorMax", new global::UnityEngine.Vector2(0.5f, 1f)))).append(new GoTween(PriceText.transform, 0.25f, new GoTweenConfig().scale(1.2f))).append(new GoTween(PriceText.transform, 0.25f, new GoTweenConfig().scale(1f)));
			slotMachineFlow.insert(1.95f, priceTextChain);
			slotMachineFlow.insert(1.7f, new GoTween(QuantityText, 0.25f, new GoTweenConfig().setEaseType(GoEaseType.Linear).colorProp("color", originalTextColor)));
			slotMachineFlow.insert(1.7f, new GoTween(ItemTitleText, 0.25f, new GoTweenConfig().setEaseType(GoEaseType.Linear).colorProp("color", originalTextColor)));
			slotMachineFlow.play();
			runningSpinnerTweens.Add(slotMachineFlow);
			global::UnityEngine.Vector2 targetPos = originalBlurPosition1;
			targetPos.y = -347f;
			SlotBlurImage1.rectTransform.anchoredPosition = originalBlurPosition1;
			SlotBlurImage2.rectTransform.anchoredPosition = originalBlurPosition2;
			isBlurSlotMoving = true;
			MoveBlurSlot(SlotBlurImage1, targetPos, SlotBlurImage2);
			MoveBlurSlot(SlotBlurImage2, targetPos, SlotBlurImage1);
		}

		internal void SetupFacebookSlot(global::strange.extensions.signal.impl.Signal<string> playSFXSignal)
		{
			SetMarketplaceBuyItem(BuyItem, playSFXSignal);
			CurrentState = global::Kampai.UI.View.BuyMarketplaceSlotView.State.Facebook;
			FacebookPanel.gameObject.SetActive(true);
			PricePanel.gameObject.SetActive(false);
			SoldIconImage.gameObject.SetActive(false);
			FadeOutImage.gameObject.SetActive(true);
			if (BuyItem == null)
			{
				SetIcon("btn_Main01_fill", "icn_nav_salesMinion_mask");
			}
		}

		private void SetIcon(string iconPath, string maskPath)
		{
			if (string.IsNullOrEmpty(iconPath))
			{
				iconPath = "btn_Main01_fill";
			}
			global::UnityEngine.Sprite sprite = UIUtils.LoadSpriteFromPath(iconPath);
			ItemImage.sprite = sprite;
			if (string.IsNullOrEmpty(maskPath))
			{
				maskPath = "btn_Main01_mask";
			}
			global::UnityEngine.Sprite maskSprite = UIUtils.LoadSpriteFromPath(maskPath);
			ItemImage.maskSprite = maskSprite;
		}

		internal void SetIsSold(bool isSold)
		{
			SoldIconImage.gameObject.SetActive(isSold);
			SetBuyButtonInteractable(!isSold);
			if (isSold)
			{
				PriceText.color = global::UnityEngine.Color.black;
				CurrentState = global::Kampai.UI.View.BuyMarketplaceSlotView.State.Sold;
			}
			else
			{
				PriceText.color = global::UnityEngine.Color.white;
				CurrentState = global::Kampai.UI.View.BuyMarketplaceSlotView.State.Pending;
			}
		}

		internal void ShakeIcon()
		{
			if (shakeIconAnimation != null)
			{
				shakeIconAnimation.Play();
			}
		}

		private void SetTitleText(global::Kampai.Game.Item item)
		{
			ItemTitleText.text = LocalizeTitle(item);
		}

		private string LocalizeTitle(global::Kampai.Game.Item item)
		{
			if (item != null)
			{
				return localizationService.GetString(item.Definition.LocalizedKey);
			}
			if (BuyItem == null || BuyItem.Definition == null)
			{
				return "ITEM";
			}
			return localizationService.GetString(BuyItem.Definition.LocalizedKey);
		}

		private void SetBuyButtonInteractable(bool isEnabled)
		{
			global::UnityEngine.UI.Button component = BuyButtonView.GetComponent<global::UnityEngine.UI.Button>();
			if (!(component == null))
			{
				component.interactable = isEnabled;
				BuyButtonFill.enabled = isEnabled;
			}
		}
	}
}
