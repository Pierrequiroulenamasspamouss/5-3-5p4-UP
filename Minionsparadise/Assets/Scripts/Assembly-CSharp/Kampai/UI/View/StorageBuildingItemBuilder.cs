namespace Kampai.UI.View
{
	public static class StorageBuildingItemBuilder
	{
		public static global::Kampai.UI.View.StorageBuildingItemView Build(global::Kampai.Game.Item item, global::Kampai.Game.Definition definition, int count, global::Kampai.Util.IKampaiLogger logger)
		{
			if (definition == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.EX_NULL_ARG);
			}
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_StorageBuildingItem") as global::UnityEngine.GameObject;
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			global::Kampai.UI.View.StorageBuildingItemView component = gameObject.GetComponent<global::Kampai.UI.View.StorageBuildingItemView>();
			global::Kampai.Game.ItemDefinition itemDefinition = definition as global::Kampai.Game.ItemDefinition;
			if (string.IsNullOrEmpty(itemDefinition.Mask) || string.IsNullOrEmpty(itemDefinition.Image))
			{
				logger.Log(global::Kampai.Util.KampaiLogLevel.Error, "Your ItemDefinition: {0} doesn't have an image/mask image defined", itemDefinition.ID);
				itemDefinition.Mask = "btn_Circle01_mask";
				itemDefinition.Mask = "btn_Circle01_mask";
			}
			global::UnityEngine.Sprite sprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			global::UnityEngine.Sprite maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
			component.StorageItem = item;
			component.ItemIcon.sprite = sprite;
			component.ItemIcon.maskSprite = maskSprite;
			component.ItemQuantity.text = count.ToString();
			return component;
		}
	}
}
