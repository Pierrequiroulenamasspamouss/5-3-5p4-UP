namespace Kampai.UI.View
{
	public class CraftingRecipeView : global::Kampai.Util.KampaiView, global::UnityEngine.EventSystems.IPointerDownHandler, global::UnityEngine.EventSystems.IDragHandler, global::UnityEngine.EventSystems.IPointerUpHandler, global::UnityEngine.EventSystems.IEventSystemHandler
	{
		public global::UnityEngine.RectTransform lockedPanel;

		public global::UnityEngine.RectTransform unlockedPanel;

		public global::UnityEngine.RectTransform greenCircle;

		public global::UnityEngine.RectTransform redCircle;

		public global::UnityEngine.RectTransform groupQuantity;

		public global::Kampai.UI.View.KampaiImage lockedImage;

		public global::Kampai.UI.View.KampaiImage unlockedImage;

		public global::UnityEngine.UI.Text itemQuantity;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.IPlayerService playerService;

		private global::Kampai.Game.ItemDefinition itemDefinition;

		private global::Kampai.Game.IngredientsItemDefinition ingredientItemDefinition;

		private global::Kampai.Game.Transaction.TransactionDefinition transactionDef;

		private global::UnityEngine.Vector3 originalScaleUnlocked;

		public global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData, global::Kampai.Game.IngredientsItemDefinition> pointerDownSignal = new global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData, global::Kampai.Game.IngredientsItemDefinition>();

		public global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData> pointerDragSignal = new global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData>();

		public global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData, global::Kampai.Game.IngredientsItemDefinition> pointerUpSignal = new global::strange.extensions.signal.impl.Signal<global::UnityEngine.EventSystems.PointerEventData, global::Kampai.Game.IngredientsItemDefinition>();

		public global::strange.extensions.signal.impl.Signal<bool, bool> IsValidDragAreaSignal = new global::strange.extensions.signal.impl.Signal<bool, bool>();

		public bool isHighlighted { get; set; }

		public bool isUnlocked { get; set; }

		public int recipeID { get; set; }

		public int instanceID { get; set; }

		internal void Init(global::Kampai.Game.IDefinitionService defService, global::Kampai.Game.IPlayerService pService)
		{
			definitionService = defService;
			playerService = pService;
			itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(recipeID);
			ingredientItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(recipeID);
			SetItemImageAndQuantity();
			SetImageBorder();
			if (unlockedImage != null)
			{
				originalScaleUnlocked = unlockedImage.gameObject.transform.localScale;
			}
			SetHighlight(isHighlighted);
		}

		private void SetItemImageAndQuantity()
		{
			if (ValidRecipe())
			{
				isUnlocked = true;
				unlockedImage.sprite = UIUtils.LoadSpriteFromPath(itemDefinition.Image);
				unlockedImage.maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
				transactionDef = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(ingredientItemDefinition.TransactionId);
				SetQuantity();
			}
			else
			{
				isUnlocked = false;
				lockedPanel.gameObject.SetActive(true);
				unlockedPanel.gameObject.SetActive(false);
				lockedImage.maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition.Mask);
			}
		}

		private bool ValidRecipe()
		{
			global::Kampai.Game.DynamicIngredientsDefinition definition;
			if (definitionService.TryGet<global::Kampai.Game.DynamicIngredientsDefinition>(recipeID, out definition))
			{
				return true;
			}
			return playerService.GetUnlockedQuantityOfID(recipeID) > 0;
		}

		public void SetQuantity()
		{
			itemQuantity.text = playerService.GetQuantityByDefinitionId(recipeID).ToString();
		}

		public void SetImageBorder()
		{
			if (!isUnlocked)
			{
				return;
			}
			bool flag = true;
			foreach (global::Kampai.Util.QuantityItem input in transactionDef.Inputs)
			{
				flag &= playerService.GetQuantityByDefinitionId(input.ID) >= input.Quantity;
			}
			if (flag)
			{
				greenCircle.gameObject.SetActive(true);
				redCircle.gameObject.SetActive(false);
			}
			else
			{
				greenCircle.gameObject.SetActive(false);
				redCircle.gameObject.SetActive(true);
			}
		}

		internal void SetHighlight(bool isHighlighted)
		{
			if (isUnlocked && !(unlockedImage == null))
			{
				if (isHighlighted)
				{
					global::Kampai.Util.TweenUtil.Throb(unlockedImage.gameObject.transform, 0.85f, 0.5f, out originalScaleUnlocked);
					return;
				}
				Go.killAllTweensWithTarget(unlockedImage.gameObject.transform);
				unlockedImage.gameObject.transform.localScale = originalScaleUnlocked;
			}
		}

		public void OnPointerDown(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			pointerDownSignal.Dispatch(eventData, ingredientItemDefinition);
		}

		public void OnDrag(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			pointerDragSignal.Dispatch(eventData);
		}

		public void OnPointerUp(global::UnityEngine.EventSystems.PointerEventData eventData)
		{
			pointerUpSignal.Dispatch(eventData, ingredientItemDefinition);
		}
	}
}
