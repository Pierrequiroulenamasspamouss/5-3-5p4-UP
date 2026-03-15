namespace Kampai.UI.View
{
	public class CurrencyStoreCategoryView : global::strange.extensions.mediation.impl.View
	{
		private global::System.Collections.Generic.List<global::Kampai.UI.View.CurrencyButtonView> currencyButtonViews;

		private global::Kampai.Game.CurrencyStoreCategoryDefinition definition;

		internal void Init(global::Kampai.Game.CurrencyStoreCategoryDefinition definition)
		{
			currencyButtonViews = new global::System.Collections.Generic.List<global::Kampai.UI.View.CurrencyButtonView>();
			this.definition = definition;
		}

		internal int GetStoreCategoryDefinitionID()
		{
			return definition.ID;
		}

		internal string GetCategoryTitle()
		{
			return definition.Description;
		}

		internal void RefreshButtons(bool forceLocked, global::Kampai.UI.ICurrencyStoreService currencyStoreService, global::Kampai.Main.ILocalizationService localizationService)
		{
			foreach (global::Kampai.UI.View.CurrencyButtonView currencyButtonView in currencyButtonViews)
			{
				global::Kampai.Game.StoreItemType type = currencyButtonView.Definition.Type;
				if (type == global::Kampai.Game.StoreItemType.SalePack)
				{
					UpdateSalePackButton(currencyButtonView, forceLocked, currencyStoreService, localizationService);
					continue;
				}
				bool flag = !forceLocked;
				currencyButtonView.UnlockButton(flag);
				if (!flag)
				{
					currencyButtonView.ItemWorth.text = localizationService.GetStringUpper("StarterPackMTXLockedBanner");
				}
			}
		}

		private void UpdateSalePackButton(global::Kampai.UI.View.CurrencyButtonView btnView, bool forceLocked, global::Kampai.UI.ICurrencyStoreService currencyStoreService, global::Kampai.Main.ILocalizationService localizationService)
		{
			global::Kampai.Game.CurrencyStorePackDefinition currencyStorePackDefinition = currencyStoreService.GetCurrencyStorePackDefinition(btnView.Definition.ReferencedDefID);
			if (currencyStorePackDefinition != null)
			{
				if (currencyStoreService.HasPurchasedEnough(currencyStorePackDefinition))
				{
					btnView.gameObject.SetActive(false);
					return;
				}
				bool flag = forceLocked || currencyStoreService.ShouldPackBeVisuallyLocked(currencyStorePackDefinition);
				UpdateButtonUnlock(btnView, !flag, currencyStorePackDefinition, localizationService);
			}
		}

		private void UpdateButtonUnlock(global::Kampai.UI.View.CurrencyButtonView btnView, bool unlocked, global::Kampai.Game.CurrencyStorePackDefinition packDef, global::Kampai.Main.ILocalizationService localService)
		{
			if (btnView != null)
			{
				if (unlocked)
				{
					btnView.UnlockButton(true);
					string text = ((!string.IsNullOrEmpty(packDef.SaleBanner)) ? localService.GetString(packDef.SaleBanner) : localService.GetStringUpper("StarterPackMTXDiscountButton"));
					btnView.ItemWorth.text = text;
				}
				else
				{
					btnView.UnlockButton(false);
					btnView.ItemWorth.text = localService.GetStringUpper("StarterPackMTXLockedBanner");
				}
			}
		}

		internal global::UnityEngine.Transform GetClosestValueView(int amount)
		{
			foreach (global::Kampai.UI.View.CurrencyButtonView currencyButtonView in currencyButtonViews)
			{
				int result;
				bool flag = int.TryParse(currencyButtonView.ItemWorth.text, out result);
				if (currencyButtonView.gameObject.activeSelf && flag && result > amount)
				{
					return currencyButtonView.transform;
				}
			}
			return null;
		}

		internal bool CanDisplay()
		{
			foreach (global::Kampai.UI.View.CurrencyButtonView currencyButtonView in currencyButtonViews)
			{
				if (currencyButtonView.gameObject.activeSelf)
				{
					return true;
				}
			}
			return false;
		}

		internal void AddCurrencyButtonView(global::Kampai.UI.View.CurrencyButtonView currencyButtonView)
		{
			currencyButtonViews.Add(currencyButtonView);
		}

		internal void OnPremiumCatalogUpdated(global::Kampai.Game.ICurrencyService currencyService, global::Kampai.Game.IDefinitionService definitionService)
		{
			for (int i = 0; i < currencyButtonViews.Count; i++)
			{
				global::Kampai.UI.View.CurrencyButtonView currencyButtonView = currencyButtonViews[i];
				global::Kampai.Game.CurrencyItemDefinition currencyItemDefinition;
				if (definitionService.TryGet<global::Kampai.Game.CurrencyItemDefinition>(currencyButtonView.Definition.ReferencedDefID, out currencyItemDefinition))
				{
					global::Kampai.Game.PremiumCurrencyItemDefinition premiumCurrencyItemDefinition = currencyItemDefinition as global::Kampai.Game.PremiumCurrencyItemDefinition;
					if (premiumCurrencyItemDefinition != null)
					{
						currencyButtonView.ItemPrice.text = currencyService.GetPriceWithCurrencyAndFormat(premiumCurrencyItemDefinition.SKU);
					}
				}
			}
		}
	}
}
