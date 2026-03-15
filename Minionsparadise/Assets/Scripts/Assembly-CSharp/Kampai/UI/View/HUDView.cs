namespace Kampai.UI.View
{
	public class HUDView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.ParticleSystem PremiumStarVFX;

		public global::UnityEngine.ParticleSystem PremiumImageVFX;

		public global::UnityEngine.ParticleSystem GrindStarVFX;

		public global::UnityEngine.ParticleSystem GrindImageVFX;

		public global::UnityEngine.ParticleSystem StorageStarVFX;

		public global::UnityEngine.ParticleSystem StorageImageVFX;

		public global::Kampai.UI.View.ButtonView PremiumMenuButton;

		public global::Kampai.UI.View.ButtonView PremiumIconButton;

		public global::Kampai.UI.View.ButtonView PremiumTextButton;

		public global::Kampai.UI.View.ButtonView GrindMenuButton;

		public global::Kampai.UI.View.ButtonView GrindIconButton;

		public global::Kampai.UI.View.ButtonView GrindTextButton;

		public global::Kampai.UI.View.ButtonView StorageButton;

		public global::Kampai.UI.View.ButtonView StorageExpandButton;

		public global::UnityEngine.GameObject SettingsButton;

		public global::Kampai.UI.View.ButtonView PetsButton;

		public global::Kampai.UI.View.ButtonView BackgroundButton;

		public global::Kampai.UI.View.ButtonView StoreMenuButton;

		public global::Kampai.UI.View.ButtonView ExitLairButton;

		public global::UnityEngine.RectTransform CurrencyStore;

		public global::UnityEngine.RectTransform StorageFillBar;

		public global::UnityEngine.RectTransform PointsPanel;

		public global::UnityEngine.RectTransform PartyMeterPanel;

		public global::UnityEngine.RectTransform ExitLairPanel;

		public global::Kampai.UI.View.WayFinderPanelView WayFinder;

		public global::Kampai.UI.View.BuildMenuView BuildMenu;

		public global::UnityEngine.GameObject StoreMenu;

		public global::Kampai.UI.View.SalePackHUDPanelView salePackPanel;

		public global::UnityEngine.UI.Text GrindCurrency;

		public global::UnityEngine.UI.Text PremiumCurrency;

		public global::UnityEngine.GameObject SaleBadge;

		public global::UnityEngine.UI.Text SaleCount;

		public global::UnityEngine.UI.Text StorageAmount;

		public global::UnityEngine.Animator StorageAmountAnim;

		public global::UnityEngine.GameObject DarkSkrim;

		public global::strange.extensions.signal.impl.Signal<bool> MenuMoved = new global::strange.extensions.signal.impl.Signal<bool>();

		private global::UnityEngine.Animator animator;

		private int darkSkrimCount;

		private bool lastPopupState;

		private int popupsOpened;

		private bool playStorageVFX;

		private global::Kampai.UI.View.HUDChangedSiblingIndexSignal hudChangedSiblingIndexSignal;

		internal GoTween storageTextTween;

		internal GoTween storageFillTween;

		internal bool isInForeground;

		private bool shown;

		public int expTweenCount { get; set; }

		public int storageTweenCount { get; set; }

		public int premiumTweenCount { get; set; }

		public int grindTweenCount { get; set; }

		public void Init(global::Kampai.UI.View.HUDChangedSiblingIndexSignal hudChangedSiblingIndexSignal)
		{
			this.hudChangedSiblingIndexSignal = hudChangedSiblingIndexSignal;
			(base.transform as global::UnityEngine.RectTransform).offsetMin = global::UnityEngine.Vector2.zero;
			(base.transform as global::UnityEngine.RectTransform).offsetMax = global::UnityEngine.Vector2.zero;
			animator = GetComponent<global::UnityEngine.Animator>();
			DisableSoundForMenuButtons();
			BuildMenu.transform.SetAsLastSibling();
		}

		private void DisableSoundForMenuButtons()
		{
			PremiumMenuButton.PlaySoundOnClick = (PremiumIconButton.PlaySoundOnClick = (PremiumTextButton.PlaySoundOnClick = false));
			GrindMenuButton.PlaySoundOnClick = (GrindIconButton.PlaySoundOnClick = (GrindTextButton.PlaySoundOnClick = false));
			StorageButton.PlaySoundOnClick = false;
			BackgroundButton.PlaySoundOnClick = false;
			StorageExpandButton.PlaySoundOnClick = false;
			PetsButton.PlaySoundOnClick = false;
			global::Kampai.UI.View.ButtonView component = SettingsButton.GetComponent<global::Kampai.UI.View.ButtonView>();
			if (component != null)
			{
				component.PlaySoundOnClick = false;
			}
		}

		internal void SetStorage(uint current, uint max)
		{
			if (storageTextTween != null)
			{
				storageTextTween.destroy();
				storageTextTween = null;
			}
			storageTextTween = Go.to(this, 1f, new GoTweenConfig().intProp("storageTweenCount", (int)current).onUpdate(delegate
			{
				SetStorageText((uint)storageTweenCount, max);
			}).onComplete(delegate
			{
				storageTextTween.destroy();
				storageTextTween = null;
			}));
			if (StorageFillBar != null && max != 0)
			{
				if (storageFillTween != null)
				{
					storageFillTween.destroy();
					storageFillTween = null;
				}
				if (current > max)
				{
					current = max;
				}
				storageFillTween = Go.to(StorageFillBar, 1f, new GoTweenConfig().vector2Prop("anchorMax", new global::UnityEngine.Vector2((float)current / (float)max, 1f)).onComplete(delegate
				{
					storageFillTween.destroy();
					storageFillTween = null;
				}));
			}
		}

		internal void SetStorageText(uint current, uint max)
		{
			StorageAmount.text = string.Format("{0}/{1}", current, max);
			int num = global::System.Math.Abs((int)(max - current));
			bool flag = num < 10;
			bool flag2 = current >= max;
			if (flag || flag2)
			{
				EnableOutline(true, StorageAmount, global::UnityEngine.Color.white);
				StorageAmountAnim.Play("AlmostFull");
			}
			else
			{
				EnableOutline(false, StorageAmount, global::UnityEngine.Color.white);
				StorageAmountAnim.Play("Init");
			}
		}

		private static void EnableOutline(bool enable, global::UnityEngine.UI.Text text, global::UnityEngine.Color outlineColor)
		{
			global::UnityEngine.UI.Outline outline = text.GetComponent<global::UnityEngine.UI.Outline>();
			if (enable)
			{
				if (outline == null)
				{
					outline = text.gameObject.AddComponent<global::UnityEngine.UI.Outline>();
				}
				outline.effectColor = outlineColor;
			}
			else if (outline != null)
			{
				global::UnityEngine.Object.Destroy(outline);
				outline = null;
			}
		}

		public void SetGrindCurrency(uint amount)
		{
			Go.to(this, 1f, new GoTweenConfig().intProp("grindTweenCount", (int)amount).onUpdate(delegate
			{
				GrindCurrency.text = UIUtils.FormatLargeNumber(grindTweenCount);
			}));
		}

		public void SetPremiumCurrency(uint amount)
		{
			if (amount < premiumTweenCount)
			{
				PremiumCurrency.text = UIUtils.FormatLargeNumber((int)amount);
				return;
			}
			Go.to(this, 1f, new GoTweenConfig().intProp("premiumTweenCount", (int)amount).onUpdate(delegate
			{
				PremiumCurrency.text = UIUtils.FormatLargeNumber(premiumTweenCount);
			}));
		}

		public void ActivateBackgroundButton()
		{
			if (darkSkrimCount == 0)
			{
				BackgroundButton.gameObject.SetActive(true);
				ToggleDarkSkrim(true);
			}
		}

		public void MoveMenu(bool show)
		{
			if (show != shown)
			{
				shown = show;
				if (show)
				{
					BringToForeground();
					MenuMoved.Dispatch(true);
					animator.SetBool("OnHide", false);
					animator.SetBool("OnPopup", false);
				}
				else
				{
					BackgroundButton.gameObject.SetActive(false);
					BringToBackground();
					MenuMoved.Dispatch(false);
					animator.SetBool("OnPopup", lastPopupState);
					ToggleDarkSkrim(false);
				}
			}
		}

		internal void EnableVillainHud(bool isEnabled)
		{
			ExitLairPanel.gameObject.SetActive(isEnabled);
			animator.SetBool("OnVillainLair", isEnabled);
		}

		internal void ToggleDarkSkrim(bool show)
		{
			if (show)
			{
				darkSkrimCount++;
			}
			else
			{
				darkSkrimCount--;
				darkSkrimCount = global::UnityEngine.Mathf.Max(0, darkSkrimCount);
			}
			DarkSkrim.SetActive((darkSkrimCount > 0) ? true : false);
		}

		internal void TogglePopup(bool show)
		{
			if (show)
			{
				popupsOpened++;
				ToggleDarkSkrim(true);
				if (popupsOpened == 1)
				{
					BringToForeground();
				}
			}
			else
			{
				popupsOpened--;
				popupsOpened = global::UnityEngine.Mathf.Max(0, popupsOpened);
				ToggleDarkSkrim(false);
				if (popupsOpened == 0)
				{
					BringToBackground();
				}
			}
			lastPopupState = popupsOpened > 0;
			animator.SetBool("OnPopup", lastPopupState);
		}

		internal void Toggle(bool show)
		{
			animator.SetBool("OnHide", !show);
		}

		internal void ToggleSettings(bool show)
		{
			SettingsButton.SetActive(show);
		}

		internal void TogglePetsButton(bool visible)
		{
			PetsButton.gameObject.SetActive(visible);
		}

		internal bool IsHiding()
		{
			return animator.GetBool("OnHide");
		}

		public void SetStorageButtonVisible(bool visible)
		{
			StorageButton.gameObject.SetActive(visible);
		}

		public void SetButtonsVisible(bool visible)
		{
			SettingsButton.SetActive(visible);
		}

		internal void PlayPremiumVFX()
		{
			PremiumStarVFX.Play();
			PremiumImageVFX.Play();
		}

		internal void PlayGrindVFX()
		{
			GrindStarVFX.Play();
			GrindImageVFX.Play();
		}

		internal void PlayStorageVFX()
		{
			if (playStorageVFX)
			{
				StorageStarVFX.Play();
				StorageImageVFX.Play();
			}
			playStorageVFX = true;
		}

		private void BringToForeground()
		{
			isInForeground = true;
			base.transform.SetAsLastSibling();
			hudChangedSiblingIndexSignal.Dispatch(base.transform.GetSiblingIndex());
			salePackPanel.transform.SetSiblingIndex(WayFinder.transform.GetSiblingIndex() + 1);
			BuildMenu.transform.SetSiblingIndex(WayFinder.transform.GetSiblingIndex() + 1);
		}

		private void BringToBackground()
		{
			isInForeground = false;
			base.transform.SetAsFirstSibling();
			hudChangedSiblingIndexSignal.Dispatch(base.transform.GetSiblingIndex());
			salePackPanel.transform.SetAsLastSibling();
			BuildMenu.transform.SetAsLastSibling();
		}

		internal void EnableStoreMenuButton(bool enable)
		{
			global::UnityEngine.UI.Button component = StoreMenuButton.gameObject.GetComponent<global::UnityEngine.UI.Button>();
			if (component != null)
			{
				component.interactable = enable;
			}
		}
	}
}
