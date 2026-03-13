namespace Kampai.UI.View
{
	[global::UnityEngine.RequireComponent(typeof(global::UnityEngine.Animator))]
	public class CurrencyStoreView : global::Kampai.UI.View.PopupMenuView
	{
		private const int minimumAnimItemCount = 4;

		public global::UnityEngine.RectTransform CategoryGridParent;

		public global::UnityEngine.RectTransform CategoryParent;

		public global::UnityEngine.UI.Text title;

		public global::Kampai.UI.View.ButtonView backgroundButton;

		internal bool isOpen;

		internal global::System.Collections.Generic.List<int> viewCounts = new global::System.Collections.Generic.List<int>();

		private global::Kampai.Main.ILocalizationService localService;

		private global::System.Collections.Generic.List<global::Kampai.UI.View.CurrencyStoreCategoryView> currencyStoreCategoryViews = new global::System.Collections.Generic.List<global::Kampai.UI.View.CurrencyStoreCategoryView>();

		private global::System.Collections.Generic.List<global::Kampai.UI.View.CurrencyStoreCategoryButtonView> categoryButtonViews = new global::System.Collections.Generic.List<global::Kampai.UI.View.CurrencyStoreCategoryButtonView>();

		private global::UnityEngine.Transform pulsing;

		private bool openAnimFinished;

		private global::UnityEngine.GameObject storeCategoryButtonPrefab;

		private global::UnityEngine.GameObject storeCategoryGridPrefab;

		private int currentCategoryIndex;

		internal void Init(global::Kampai.Main.ILocalizationService localization)
		{
			base.Init();
			localService = localization;
			storeCategoryButtonPrefab = global::Kampai.Util.KampaiResources.Load("cmp_StoreButton") as global::UnityEngine.GameObject;
			storeCategoryGridPrefab = global::Kampai.Util.KampaiResources.Load("cmp_currencyStoreCategoryGrid") as global::UnityEngine.GameObject;
		}

		internal void ClearViews()
		{
			StopPulsing();
			UIUtils.SafeDestoryViews(currencyStoreCategoryViews);
			UIUtils.SafeDestoryViews(categoryButtonViews);
		}

		internal global::Kampai.UI.View.CurrencyStoreCategoryButtonView BuildCategoryButton(global::Kampai.Game.CurrencyStoreCategoryDefinition categoryDefinition)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(storeCategoryButtonPrefab);
			gameObject.name = string.Format("Category: {0}", categoryDefinition.StoreCategoryType);
			gameObject.transform.SetParent(CategoryParent, false);
			global::Kampai.UI.View.CurrencyStoreCategoryButtonView component = gameObject.GetComponent<global::Kampai.UI.View.CurrencyStoreCategoryButtonView>();
			component.Init(categoryDefinition, localService);
			return component;
		}

		internal global::Kampai.UI.View.CurrencyStoreCategoryView BuildCategoryContainer(global::Kampai.Game.CurrencyStoreCategoryDefinition categoryDefinition, global::Kampai.UI.View.CurrencyStoreCategoryButtonView buttonView)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(storeCategoryGridPrefab);
			gameObject.name = string.Format("Category Grid: {0}", categoryDefinition.StoreCategoryType);
			gameObject.transform.SetParent(CategoryGridParent, false);
			gameObject.SetActive(false);
			global::Kampai.UI.View.CurrencyStoreCategoryView component = gameObject.GetComponent<global::Kampai.UI.View.CurrencyStoreCategoryView>();
			component.Init(categoryDefinition);
			currencyStoreCategoryViews.Add(component);
			categoryButtonViews.Add(buttonView);
			return component;
		}

		internal global::Kampai.UI.View.CurrencyButtonView BuildCategoryItem(global::Kampai.Game.CurrencyItemDefinition currencyItemDef, global::Kampai.Game.StoreItemDefinition storeItemDef, string inputStr, string outputStr, global::Kampai.UI.View.CurrencyStoreCategoryView categoryView, bool hasVFX)
		{
			global::Kampai.UI.View.CurrencyButtonView currencyButtonView = global::Kampai.UI.View.CurrencyButtonBuilder.Build(localService, currencyItemDef, storeItemDef, inputStr, outputStr, categoryView.transform, hasVFX);
			currencyButtonView.Definition = storeItemDef;
			categoryView.AddCurrencyButtonView(currencyButtonView);
			return currencyButtonView;
		}

		internal void OnPremiumCatalogUpdated(global::Kampai.Game.ICurrencyService currencyService, global::Kampai.Game.IDefinitionService definitionService)
		{
			for (int i = 0; i < currencyStoreCategoryViews.Count; i++)
			{
				currencyStoreCategoryViews[i].OnPremiumCatalogUpdated(currencyService, definitionService);
			}
		}

		internal void RefreshButtons(bool forceLocked, global::Kampai.UI.ICurrencyStoreService currencyStoreService, global::Kampai.Main.ILocalizationService localizationService)
		{
			foreach (global::Kampai.UI.View.CurrencyStoreCategoryView currencyStoreCategoryView in currencyStoreCategoryViews)
			{
				currencyStoreCategoryView.RefreshButtons(forceLocked, currencyStoreService, localizationService);
			}
		}

		internal void StopPulsing()
		{
			if (pulsing != null)
			{
				Go.killAllTweensWithTarget(pulsing);
				pulsing.transform.localScale = new global::UnityEngine.Vector3(1f, 1f, 1f);
				pulsing = null;
			}
		}

		private void TrySlideInItems()
		{
			if (openAnimFinished && viewCounts[currentCategoryIndex] > 4)
			{
				base.animator.StopPlayback();
				base.animator.Play("SlideInItems", 0, 0f);
			}
		}

		internal void Cleanup()
		{
			StopPulsing();
			SetScrollableTransform(null);
			isOpen = false;
			base.Close();
		}

		private void DisplayCategory(global::Kampai.UI.View.CurrencyStoreCategoryView categoryView, bool repositionToCenter = false)
		{
			global::UnityEngine.GameObject gameObject = categoryView.gameObject;
			global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
			SetScrollableTransform(rectTransform);
			if (repositionToCenter)
			{
				rectTransform.anchoredPosition = global::UnityEngine.Vector2.zero;
			}
			TrySlideInItems();
			gameObject.SetActive(true);
			title.text = localService.GetString(categoryView.GetCategoryTitle());
		}

		private void SetScrollableTransform(global::UnityEngine.RectTransform rectTransform)
		{
			if (CategoryGridParent != null)
			{
				global::UnityEngine.UI.ScrollRect component = CategoryGridParent.GetComponent<global::UnityEngine.UI.ScrollRect>();
				if (component != null)
				{
					component.content = rectTransform;
				}
			}
		}

		private void UpdateNextCategoryIndex(bool next)
		{
			if (next)
			{
				currentCategoryIndex++;
				if (currentCategoryIndex >= currencyStoreCategoryViews.Count)
				{
					currentCategoryIndex = 0;
				}
			}
			else
			{
				currentCategoryIndex--;
				if (currentCategoryIndex < 0)
				{
					currentCategoryIndex = currencyStoreCategoryViews.Count - 1;
				}
			}
		}

		internal void ShowCategory(int categoryDefinitionID, int amountNeeded = 0, bool firstTime = false)
		{
			StopPulsing();
			int num = currentCategoryIndex;
			for (int i = 0; i < currencyStoreCategoryViews.Count; i++)
			{
				global::Kampai.UI.View.CurrencyStoreCategoryView currencyStoreCategoryView = currencyStoreCategoryViews[i];
				global::Kampai.UI.View.CurrencyStoreCategoryButtonView currencyStoreCategoryButtonView = categoryButtonViews[i];
				if (currencyStoreCategoryView.GetStoreCategoryDefinitionID() == categoryDefinitionID)
				{
					currentCategoryIndex = i;
					currencyStoreCategoryView.gameObject.SetActive(true);
					if (firstTime)
					{
						currencyStoreCategoryButtonView.MarkAsSelected();
					}
				}
				else
				{
					currencyStoreCategoryView.gameObject.SetActive(false);
					currencyStoreCategoryButtonView.MarkAsDeselected();
				}
			}
			if (currentCategoryIndex == num && !firstTime)
			{
				return;
			}
			global::Kampai.UI.View.CurrencyStoreCategoryView currencyStoreCategoryView2 = currencyStoreCategoryViews[currentCategoryIndex];
			int num2 = currencyStoreCategoryViews.Count;
			while (!currencyStoreCategoryView2.CanDisplay() && num2 > 0)
			{
				UpdateNextCategoryIndex(true);
				currencyStoreCategoryView2 = currencyStoreCategoryViews[currentCategoryIndex];
				num2--;
			}
			if (num2 == 0)
			{
				logger.Warning("Failed to find a suitable mtx category view to display.");
				return;
			}
			DisplayCategory(currencyStoreCategoryView2, true);
			if (amountNeeded > 0)
			{
				pulsing = currencyStoreCategoryView2.GetClosestValueView(amountNeeded);
				if (pulsing != null)
				{
					global::UnityEngine.Vector3 originalScale = global::UnityEngine.Vector3.one;
					global::Kampai.Util.TweenUtil.Throb(pulsing, 0.98f, 0.5f, out originalScale);
				}
			}
		}

		public override void FinishedOpening()
		{
			base.FinishedOpening();
			openAnimFinished = true;
			TrySlideInItems();
		}
	}
}
