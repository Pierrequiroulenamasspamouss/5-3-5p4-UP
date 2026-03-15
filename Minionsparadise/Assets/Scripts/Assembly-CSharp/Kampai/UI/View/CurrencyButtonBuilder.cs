namespace Kampai.UI.View
{
	public static class CurrencyButtonBuilder
	{
		public static global::Kampai.UI.View.CurrencyButtonView Build(global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.CurrencyItemDefinition definition, global::Kampai.Game.StoreItemDefinition storeItemDef, string inputStr, string outputStr, global::UnityEngine.Transform i_parent, bool hasVFX)
		{
			if (definition == null)
			{
				throw new global::System.ArgumentNullException("definition", "CurrencyButtonBuilder: You are passing in null definitions!");
			}
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_purchaseCurrencyButton") as global::UnityEngine.GameObject;
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			global::Kampai.UI.View.CurrencyButtonView component = gameObject.GetComponent<global::Kampai.UI.View.CurrencyButtonView>();
			if (hasVFX)
			{
				global::UnityEngine.GameObject original2 = global::Kampai.Util.KampaiResources.Load(definition.VFX) as global::UnityEngine.GameObject;
				global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(original2);
				global::Kampai.Util.Vector3Serialize vFXOffset = definition.VFXOffset;
				if (vFXOffset != null)
				{
					gameObject2.transform.position = new global::UnityEngine.Vector3(vFXOffset.x, vFXOffset.y, vFXOffset.z);
				}
				gameObject2.transform.SetParent(component.VFXRoot, false);
				component.VFXPrefab = gameObject2;
			}
			component.Description.text = localService.GetStringUpper(definition.LocalizedKey);
			component.isCOPPAGated = definition.COPPAGated;
			component.ItemWorth.text = outputStr;
			if (storeItemDef.Type == global::Kampai.Game.StoreItemType.GrindCurrency)
			{
				component.CostCurrencyIcon.gameObject.SetActive(false);
			}
			else
			{
				component.CostCurrencyIcon.gameObject.SetActive(false);
			}
			if (storeItemDef.Type == global::Kampai.Game.StoreItemType.SalePack)
			{
				component.isStarterPack = true;
			}
			if (storeItemDef.PercentOff > 0)
			{
				string text = localService.GetString("PercentOff", storeItemDef.PercentOff);
				if (text != null && text.Trim().Length > 0)
				{
					component.ValueBanner.SetActive(true);
					component.ValueBannerText.text = text;
					if (storeItemDef.IsFeatured)
					{
						component.ValueImage.color = global::UnityEngine.Color.green;
					}
				}
			}
			global::UnityEngine.Sprite sprite = UIUtils.LoadSpriteFromPath(definition.Image);
			component.ItemImage.sprite = sprite;
			global::UnityEngine.Sprite maskSprite = UIUtils.LoadSpriteFromPath(definition.Mask);
			component.ItemImage.maskSprite = maskSprite;
			component.ItemPrice.text = inputStr;
			global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
			rectTransform.SetParent(i_parent, false);
			return component;
		}
	}
}
