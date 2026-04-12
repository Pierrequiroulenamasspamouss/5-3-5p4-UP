namespace Kampai.UI.View
{
	public class RushDialogView : global::Kampai.UI.View.PopupMenuView
	{
		public enum RushDialogType
		{
			DEFAULT = 0,
			STORAGE_EXPAND = 1,
			SOCIAL = 2,
			DEBRIS = 3,
			LAND_EXPANSION = 4,
			BRIDGE_QUEST = 5,
			VILLAIN_LAIR_PORTAL_REPAIR = 6,
			VILLAIN_LAIR_RESOURCE_PLOT = 7
		}

		public global::Kampai.UI.View.DoubleConfirmButtonView PurchaseButtonView;

		public global::Kampai.UI.View.ButtonView UpgradeButton;

		public global::UnityEngine.RectTransform ScrollViewParent;

		public global::UnityEngine.GameObject PurchasePanel;

		public global::UnityEngine.GameObject UpgradePanel;

		public global::UnityEngine.UI.Text RushCost;

		public global::UnityEngine.UI.Text Title;

		protected global::System.Collections.Generic.IList<global::Kampai.UI.View.RequiredItemView> items;

		protected global::Kampai.Main.ILocalizationService localService;

		protected float requiredItemWidth;

		protected float requiredItemPadding;

		internal void Init(global::Kampai.Main.ILocalizationService localService)
		{
			base.Init();
			PostInit(localService);
		}

		internal void InitProgrammatic(global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData, global::Kampai.Main.ILocalizationService localService)
		{
			InitProgrammatic(buildingPopupPositionData);
			PostInit(localService);
		}

		private void PostInit(global::Kampai.Main.ILocalizationService localService)
		{
			this.localService = localService;
			items = new global::System.Collections.Generic.List<global::Kampai.UI.View.RequiredItemView>();
			base.Open();
		}

		internal virtual void SetupDialog(global::Kampai.UI.View.RushDialogView.RushDialogType type, bool showPurchaseButton)
		{
			if (showPurchaseButton)
			{
				PurchasePanel.SetActive(true);
				UpgradePanel.SetActive(false);
				return;
			}
			PurchasePanel.SetActive(false);
			UpgradePanel.SetActive(true);
			global::Kampai.UI.View.RushButtonView rushButtonView = UpgradeButton as global::Kampai.UI.View.RushButtonView;
			if (rushButtonView != null)
			{
				rushButtonView.SkipDoubleConfirm = true;
			}
		}

		internal virtual void SetupItemCount(int count)
		{
			float num = (requiredItemWidth + requiredItemPadding) * (float)count;
			global::UnityEngine.RectTransform rectTransform = ScrollViewParent.parent.transform as global::UnityEngine.RectTransform;
			ScrollViewParent.sizeDelta = new global::UnityEngine.Vector2(num, 0f);
			if (num < rectTransform.rect.width)
			{
				rectTransform.GetComponent<global::UnityEngine.UI.ScrollRect>().enabled = false;
				ScrollViewParent.anchoredPosition = global::UnityEngine.Vector2.zero;
			}
			else
			{
				ScrollViewParent.pivot = new global::UnityEngine.Vector2(0.5f, 0.5f);
			}
		}

		internal void AddRequiredItem(global::Kampai.UI.View.RequiredItemView view, int index, global::UnityEngine.RectTransform parent)
		{
			requiredItemWidth = (view.transform as global::UnityEngine.RectTransform).sizeDelta.x;
			requiredItemPadding = view.PaddingInPixels;
			global::UnityEngine.RectTransform rectTransform = view.transform as global::UnityEngine.RectTransform;
			rectTransform.SetParent(parent, false);
			if (index == -1)
			{
				rectTransform.offsetMin = new global::UnityEngine.Vector2((0f - requiredItemWidth) / 2f, 0f);
				rectTransform.offsetMax = new global::UnityEngine.Vector2(requiredItemWidth / 2f, 0f);
			}
			else
			{
				rectTransform.offsetMin = new global::UnityEngine.Vector2((float)index * (requiredItemWidth + requiredItemPadding), 0f);
				rectTransform.offsetMax = new global::UnityEngine.Vector2((float)index * (requiredItemWidth + requiredItemPadding) + requiredItemWidth, 0f);
			}
			items.Add(view);
		}

		internal void MoveRequiredItem()
		{
			int num = 0;
			for (int i = 0; i < items.Count; i++)
			{
				global::Kampai.UI.View.RequiredItemView requiredItemView = items[i];
				if (!(requiredItemView == null) && requiredItemView.ItemDefinitionID != 0)
				{
					global::UnityEngine.RectTransform rectTransform = requiredItemView.transform as global::UnityEngine.RectTransform;
					if (!(rectTransform == null))
					{
						requiredItemWidth = rectTransform.sizeDelta.x;
						requiredItemPadding = requiredItemView.PaddingInPixels;
						rectTransform.offsetMin = new global::UnityEngine.Vector2((float)num * (requiredItemWidth + requiredItemPadding), 0f);
						rectTransform.offsetMax = new global::UnityEngine.Vector2((float)num * (requiredItemWidth + requiredItemPadding) + requiredItemWidth, 0f);
						num++;
					}
				}
			}
		}

		internal global::System.Collections.Generic.IList<global::Kampai.UI.View.RequiredItemView> GetItemList()
		{
			return items;
		}

		internal void SetupItemCost(int cost)
		{
			RushCost.text = UIUtils.FormatLargeNumber(cost);
		}

		internal void resetAllExceptRequiredItemTapState(int itemDefID)
		{
			foreach (global::Kampai.UI.View.RequiredItemView item in items)
			{
				if (item.RushBtn.Item == null || item.RushBtn.Item.ID != itemDefID)
				{
					item.RushBtn.ResetTapState();
				}
			}
			PurchaseButtonView.ResetTapState();
		}

		internal void resetAllRequiredItemsTapState()
		{
			foreach (global::Kampai.UI.View.RequiredItemView item in items)
			{
				if (item.RushBtn != null)
				{
					item.RushBtn.ResetTapState();
					item.RushBtn.ResetAnim();
				}
			}
		}

		internal void DeleteItem(int itemDefID)
		{
			foreach (global::Kampai.UI.View.RequiredItemView item in items)
			{
				markDelete(item, itemDefID);
			}
		}

		private void markDelete(global::Kampai.UI.View.RequiredItemView riv, int itemDefID)
		{
			if (riv.RushBtn.Item != null && riv.RushBtn.Item.ID == itemDefID)
			{
				riv.PurchasePanel.SetActive(false);
				riv.ItemQuantity.text = string.Format("{0}/{1}", riv.ItemNeeded, riv.ItemNeeded);
				riv.ItemQuantity.color = global::UnityEngine.Color.black;
				riv.ItemCost.gameObject.SetActive(false);
				riv.RushBtn.gameObject.SetActive(false);
			}
		}

		internal void DeleteAllItems()
		{
			foreach (global::Kampai.UI.View.RequiredItemView item in items)
			{
				if (item.IsInvoking())
				{
					item.CancelInvoke();
				}
				global::UnityEngine.Object.Destroy(item.gameObject);
			}
			items.Clear();
		}
	}
}
