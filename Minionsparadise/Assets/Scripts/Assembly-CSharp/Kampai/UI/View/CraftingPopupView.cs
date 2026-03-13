namespace Kampai.UI.View
{
	public class CraftingPopupView : global::Kampai.UI.View.GenericPopupView
	{
		public global::UnityEngine.UI.Text ingredientsText;

		public global::UnityEngine.UI.Text unlockText;

		public global::UnityEngine.UI.Text ingredientOneQuantity;

		public global::UnityEngine.UI.Text ingredientTwoQuantity;

		public global::UnityEngine.UI.Text ingredientThreeQuantity;

		public global::UnityEngine.UI.Text ingredientFourQuantity;

		public global::Kampai.UI.View.KampaiImage ingredientOneIcon;

		public global::Kampai.UI.View.KampaiImage ingredientTwoIcon;

		public global::Kampai.UI.View.KampaiImage ingredientThreeIcon;

		public global::Kampai.UI.View.KampaiImage ingredientFourIcon;

		public global::UnityEngine.RectTransform ingredientOne;

		public global::UnityEngine.RectTransform ingredientTwo;

		public global::UnityEngine.RectTransform ingredientThree;

		public global::UnityEngine.RectTransform ingredientFour;

		public global::UnityEngine.RectTransform ingredientOneGreen;

		public global::UnityEngine.RectTransform ingredientOneRed;

		public global::UnityEngine.RectTransform ingredientTwoGreen;

		public global::UnityEngine.RectTransform ingredientTwoRed;

		public global::UnityEngine.RectTransform ingredientThreeGreen;

		public global::UnityEngine.RectTransform ingredientThreeRed;

		public global::UnityEngine.RectTransform ingredientFourGreen;

		public global::UnityEngine.RectTransform ingredientFourRed;

		public global::UnityEngine.RectTransform pointer;

		private global::Kampai.Game.IPlayerService playerService;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Main.ILocalizationService localization;

		private float bufferAmt = 0.02f;

		internal void Display(global::UnityEngine.Vector3 itemCenter, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IDefinitionService defService, global::Kampai.Main.ILocalizationService localService, global::UnityEngine.GameObject glassCanvas)
		{
			this.playerService = playerService;
			definitionService = defService;
			localization = localService;
			base.Init();
			SetPosition(itemCenter, 3f, glassCanvas);
			base.Open();
		}

		private void SetPosition(global::UnityEngine.Vector3 itemCenter, float offsetValue, global::UnityEngine.GameObject glassCanvas)
		{
			global::UnityEngine.RectTransform rectTransform = base.transform as global::UnityEngine.RectTransform;
			global::UnityEngine.Vector3 anchoredPosition3D = rectTransform.sizeDelta / offsetValue;
			float x = (glassCanvas.transform as global::UnityEngine.RectTransform).sizeDelta.x;
			float num = rectTransform.sizeDelta.x / x / 2f;
			anchoredPosition3D.x = 0f;
			rectTransform.anchoredPosition3D = anchoredPosition3D;
			float num2 = itemCenter.x - num;
			float num3 = itemCenter.x + num;
			float num4 = 0f;
			if (num2 < 0f)
			{
				num4 = 0f - num2 + bufferAmt;
			}
			else if (num3 > 1f)
			{
				num4 = 1f - num3 - bufferAmt;
			}
			global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector2(num4, 0f);
			rectTransform.anchorMin = itemCenter + vector;
			rectTransform.anchorMax = rectTransform.anchorMin;
			global::UnityEngine.Vector2 vector2 = new global::UnityEngine.Vector2(num4 * x / rectTransform.sizeDelta.x, 0f);
			pointer.anchorMin -= vector2;
			pointer.anchorMax -= vector2;
		}

		internal void PopulateIngredients(global::Kampai.Game.IngredientsItemDefinition itemDef)
		{
			DisableAllIngredients();
			global::System.Collections.Generic.IList<global::Kampai.Util.QuantityItem> inputs = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(itemDef.TransactionId).Inputs;
			int count = inputs.Count;
			if (count < 1 || count > 4)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.DS_INVALID_CRAFT, "Transaction {0} has an invaid number of inputs", itemDef.TransactionId);
			}
			global::Kampai.Game.DynamicIngredientsDefinition dynamicIngredientsDefinition = itemDef as global::Kampai.Game.DynamicIngredientsDefinition;
			int num = 0;
			if (dynamicIngredientsDefinition == null)
			{
				num = definitionService.GetLevelItemUnlocksAt(itemDef.ID);
			}
			if (num > (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID))
			{
				ingredientsText.gameObject.SetActive(false);
				unlockText.gameObject.SetActive(true);
				unlockText.text = localization.GetString("UnlockAt", num);
				return;
			}
			for (int i = 0; i < count; i++)
			{
				int quantityByDefinitionId = (int)playerService.GetQuantityByDefinitionId(inputs[i].ID);
				switch (i)
				{
				case 0:
					SetupSlotOne(inputs[0], quantityByDefinitionId);
					break;
				case 1:
					SetupSlotTwo(inputs[1], quantityByDefinitionId);
					break;
				case 2:
					SetupSlotThree(inputs[2], quantityByDefinitionId);
					break;
				case 3:
					SetupSlotFour(inputs[3], quantityByDefinitionId);
					break;
				}
			}
		}

		private void DisableAllIngredients()
		{
			ingredientOne.gameObject.SetActive(false);
			ingredientTwo.gameObject.SetActive(false);
			ingredientThree.gameObject.SetActive(false);
			ingredientFour.gameObject.SetActive(false);
		}

		private void SetupSlotOne(global::Kampai.Util.QuantityItem input, int playerQuantity)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(input.ID);
			ingredientOneIcon.sprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			ingredientOneIcon.maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
			ingredientOneQuantity.text = string.Format("{0}/{1}", playerQuantity, input.Quantity);
			if (playerQuantity < input.Quantity)
			{
				ingredientOneGreen.gameObject.SetActive(false);
				ingredientOneRed.gameObject.SetActive(true);
			}
			ingredientOne.gameObject.SetActive(true);
		}

		private void SetupSlotTwo(global::Kampai.Util.QuantityItem input, int playerQuantity)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(input.ID);
			ingredientTwoIcon.sprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			ingredientTwoIcon.maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
			ingredientTwoQuantity.text = string.Format("{0}/{1}", playerQuantity, input.Quantity);
			if (playerQuantity < input.Quantity)
			{
				ingredientTwoGreen.gameObject.SetActive(false);
				ingredientTwoRed.gameObject.SetActive(true);
			}
			ingredientTwo.gameObject.SetActive(true);
		}

		private void SetupSlotThree(global::Kampai.Util.QuantityItem input, int playerQuantity)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(input.ID);
			ingredientThreeIcon.sprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			ingredientThreeIcon.maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
			ingredientThreeQuantity.text = string.Format("{0}/{1}", playerQuantity, input.Quantity);
			if (playerQuantity < input.Quantity)
			{
				ingredientThreeGreen.gameObject.SetActive(false);
				ingredientThreeRed.gameObject.SetActive(true);
			}
			ingredientThree.gameObject.SetActive(true);
		}

		private void SetupSlotFour(global::Kampai.Util.QuantityItem input, int playerQuantity)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(input.ID);
			ingredientFourIcon.sprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
			ingredientFourIcon.maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
			ingredientFourQuantity.text = string.Format("{0}/{1}", playerQuantity, input.Quantity);
			if (playerQuantity < input.Quantity)
			{
				ingredientFourGreen.gameObject.SetActive(false);
				ingredientFourRed.gameObject.SetActive(true);
			}
			ingredientFour.gameObject.SetActive(true);
		}
	}
}
