namespace Kampai.UI.View
{
	public class StorageBuildingItemMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private bool m_isPointerDown;

		private bool m_isBuyPanelOpened;

		private bool m_isDescriptionDelayed;

		private bool m_canShowSelectAnimation;

		private float itemInfoPopupOffset = -0.2f;

		private global::System.Collections.IEnumerator showInfoCoroutine;

		private global::System.Collections.IEnumerator autoCloseCoroutine;

		[Inject]
		public global::Kampai.UI.View.StorageBuildingItemView view { get; set; }

		[Inject]
		public global::Kampai.Game.IMarketplaceService marketplaceService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetNewSellItemSignal sellItemSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppPauseSignal pauseSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveStorageBuildingItemDescriptionSignal removeItemDescriptionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SelectStorageBuildingItemSignal selectStorageBuildingItemSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.EnableStorageBuildingItemDescriptionSignal enableItemDescriptionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.OpenCreateNewSalePanelSignal openCreateNewSalePanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseCreateNewSalePanelSignal closeCreateNewSalePanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.MarketplaceOpenSalePanelSignal openSalePanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.MarketplaceCloseSalePanelSignal closeSalePanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.MarketplaceOpenBuyPanelSignal openBuyPanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.MarketplaceCloseBuyPanelSignal closeBuyPanelSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			m_canShowSelectAnimation = false;
			openBuyPanelSignal.AddListener(BuyPanelOpened);
			closeBuyPanelSignal.AddListener(BuyPanelClosed);
			openSalePanelSignal.AddListener(SalePanelOpened);
			openCreateNewSalePanelSignal.AddListener(SalePanelOpened);
			closeSalePanelSignal.AddListener(SalePanelClosed);
			closeCreateNewSalePanelSignal.AddListener(CreateNewSalePanelClose);
			view.InfoButtonView.ClickedSignal.AddListener(OnItemClick);
			view.InfoButtonView.pointerDownSignal.AddListener(PointerDown);
			view.InfoButtonView.pointerUpSignal.AddListener(PointerUp);
			pauseSignal.AddListener(OnPause);
			removeItemDescriptionSignal.AddListener(OnItemInfoRemoved);
			selectStorageBuildingItemSignal.AddListener(IsItemSelected);
			enableItemDescriptionSignal.AddListener(EnableDelayOnInfoShow);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			openBuyPanelSignal.RemoveListener(BuyPanelOpened);
			closeBuyPanelSignal.RemoveListener(BuyPanelClosed);
			openSalePanelSignal.RemoveListener(SalePanelOpened);
			openCreateNewSalePanelSignal.RemoveListener(SalePanelOpened);
			closeSalePanelSignal.RemoveListener(SalePanelClosed);
			closeCreateNewSalePanelSignal.RemoveListener(CreateNewSalePanelClose);
			view.InfoButtonView.ClickedSignal.RemoveListener(OnItemClick);
			view.InfoButtonView.pointerDownSignal.RemoveListener(PointerDown);
			view.InfoButtonView.pointerUpSignal.RemoveListener(PointerUp);
			pauseSignal.RemoveListener(OnPause);
			removeItemDescriptionSignal.RemoveListener(OnItemInfoRemoved);
			selectStorageBuildingItemSignal.RemoveListener(IsItemSelected);
			enableItemDescriptionSignal.RemoveListener(EnableDelayOnInfoShow);
		}

		private void OnItemInfoRemoved()
		{
			if (autoCloseCoroutine != null)
			{
				StopCoroutine(autoCloseCoroutine);
				autoCloseCoroutine = null;
			}
		}

		private void EnableDelayOnInfoShow(bool isDelayed)
		{
			m_isDescriptionDelayed = isDelayed;
		}

		private void OnItemClick()
		{
			if (view.StorageItem != null || m_canShowSelectAnimation)
			{
				global::Kampai.Game.DynamicIngredientsDefinition dynamicIngredientsDefinition = view.StorageItem.Definition as global::Kampai.Game.DynamicIngredientsDefinition;
				if (dynamicIngredientsDefinition == null)
				{
					sellItemSignal.Dispatch(view.StorageItem.Definition.ID);
				}
			}
		}

		private void PointerDown()
		{
			if (!m_isBuyPanelOpened)
			{
				if (autoCloseCoroutine != null)
				{
					StopCoroutine(autoCloseCoroutine);
					autoCloseCoroutine = null;
				}
				m_isPointerDown = true;
				showInfoCoroutine = WaitToShowInfoView();
				StartCoroutine(showInfoCoroutine);
			}
		}

		private void IsItemSelected(int itemId)
		{
			if (!(view == null) && view.StorageItem != null)
			{
				view.SelectItem(m_canShowSelectAnimation && view.StorageItem.ID == itemId && HasOpenSlot());
			}
		}

		private void PointerUp()
		{
			if (m_isPointerDown)
			{
				if (showInfoCoroutine != null)
				{
					StopCoroutine(showInfoCoroutine);
					showInfoCoroutine = null;
				}
				m_isPointerDown = false;
				autoCloseCoroutine = CloseInfoView();
				StartCoroutine(autoCloseCoroutine);
			}
		}

		private global::System.Collections.IEnumerator CloseInfoView()
		{
			yield return new global::UnityEngine.WaitForSeconds(1f);
			removeItemDescriptionSignal.Dispatch();
			autoCloseCoroutine = null;
		}

		private global::System.Collections.IEnumerator WaitToShowInfoView()
		{
			global::UnityEngine.RectTransform rt = view.transform as global::UnityEngine.RectTransform;
			if (!(rt == null))
			{
				global::UnityEngine.Vector3[] corners = new global::UnityEngine.Vector3[4];
				rt.GetWorldCorners(corners);
				global::UnityEngine.Vector3 center = default(global::UnityEngine.Vector3);
				global::UnityEngine.Vector3[] array = corners;
				foreach (global::UnityEngine.Vector3 corner in array)
				{
					center += corner;
				}
				center /= 4f;
				center += new global::UnityEngine.Vector3(itemInfoPopupOffset, 0f, 0f);
				global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "cmp_StorageItemInfo");
				global::Kampai.UI.View.GUIArguments args = command.Args;
				args.Add(typeof(global::Kampai.Game.ItemDefinition), view.StorageItem.Definition);
				args.Add(typeof(global::UnityEngine.RectTransform), view.transform);
				args.Add(uiCamera.WorldToViewportPoint(center));
				if (m_isDescriptionDelayed)
				{
					yield return new global::UnityEngine.WaitForSeconds(0.2f);
				}
				removeItemDescriptionSignal.Dispatch();
				if (m_isPointerDown)
				{
					soundFXSignal.Dispatch("Play_menu_popUp_02");
					guiService.Execute(command);
				}
			}
		}

		private void OnPause()
		{
			removeItemDescriptionSignal.Dispatch();
		}

		private bool HasOpenSlot()
		{
			return marketplaceService.GetNextAvailableSlot() != null;
		}

		private void CreateNewSalePanelClose()
		{
			IsItemSelected(0);
		}

		private void SalePanelClosed()
		{
			removeItemDescriptionSignal.Dispatch();
			m_canShowSelectAnimation = false;
			IsItemSelected(0);
		}

		private void SalePanelOpened(bool isInstant)
		{
			m_canShowSelectAnimation = true;
		}

		private void SalePanelOpened(int value)
		{
			m_canShowSelectAnimation = true;
		}

		private void BuyPanelOpened(bool isInstant)
		{
			m_isBuyPanelOpened = true;
		}

		private void BuyPanelClosed()
		{
			m_isBuyPanelOpened = false;
		}
	}
}
