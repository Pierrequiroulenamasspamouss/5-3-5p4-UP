namespace Kampai.UI.View
{
	public class MoveBuildingMenuView : global::Kampai.UI.View.WorldToGlassView
	{
		internal global::Kampai.UI.View.ButtonView InventoryButton;

		internal global::Kampai.UI.View.ButtonView AcceptButton;

		internal global::Kampai.UI.View.ButtonView CloseButton;

		private global::UnityEngine.UI.Button inventoryButton;

		private global::UnityEngine.UI.Button acceptButton;

		private global::UnityEngine.UI.Button closeButton;

		private global::Kampai.UI.View.MoveBuildingModal myModal;

		private global::UnityEngine.Vector3 originalScale = new global::UnityEngine.Vector3(1f, 1f, 1f);

		protected override string UIName
		{
			get
			{
				return "MoveBuildingMenu";
			}
		}

		protected override void LoadModalData(global::Kampai.UI.View.WorldToGlassUIModal modal)
		{
			myModal = modal as global::Kampai.UI.View.MoveBuildingModal;
			global::Kampai.UI.View.MoveBuildingSetting moveBuildingSetting = myModal.Settings as global::Kampai.UI.View.MoveBuildingSetting;
			if (targetObject == null)
			{
				global::Kampai.Game.View.BuildingManagerMediator component = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER).GetComponent<global::Kampai.Game.View.BuildingManagerMediator>();
				targetObject = component.GetCurrentDummyBuilding();
			}
			InventoryButton = myModal.InventoryButton;
			AcceptButton = myModal.AcceptButton;
			CloseButton = myModal.CloseButton;
			inventoryButton = InventoryButton.GetComponent<global::UnityEngine.UI.Button>();
			acceptButton = AcceptButton.GetComponent<global::UnityEngine.UI.Button>();
			closeButton = CloseButton.GetComponent<global::UnityEngine.UI.Button>();
			myModal.gameObject.transform.SetAsLastSibling();
			if (moveBuildingSetting.Mask == 1)
			{
				inventoryButton.interactable = false;
			}
			if (moveBuildingSetting.pulseAcceptButton)
			{
				PulseAcceptButton(true);
			}
			ToggleCostPanel(moveBuildingSetting.ShowCostPanel);
		}

		internal void ReloadModal(global::Kampai.UI.View.WorldToGlassUIModal modal)
		{
			global::Kampai.Game.View.BuildingManagerMediator component = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER).GetComponent<global::Kampai.Game.View.BuildingManagerMediator>();
			targetObject = component.GetCurrentDummyBuilding();
			LoadModalData(modal);
		}

		public override global::UnityEngine.Vector3 GetIndicatorPosition()
		{
			global::Kampai.Game.View.BuildingDefinitionObject buildingDefinitionObject = targetObject as global::Kampai.Game.View.BuildingDefinitionObject;
			if (buildingDefinitionObject != null)
			{
				return buildingDefinitionObject.ResourceIconPosition;
			}
			return global::UnityEngine.Vector3.zero;
		}

		internal void DisableInventory()
		{
			inventoryButton.interactable = false;
		}

		internal void SetButtonState(int mask)
		{
			if ((mask & 1) > 0)
			{
				inventoryButton.interactable = false;
			}
			else
			{
				inventoryButton.interactable = true;
			}
			if ((mask & 4) > 0)
			{
				acceptButton.interactable = false;
			}
			else
			{
				acceptButton.interactable = true;
			}
			if ((mask & 8) > 0)
			{
				closeButton.interactable = false;
			}
			else
			{
				closeButton.interactable = true;
			}
		}

		internal void UpdateValidity(bool enable)
		{
			acceptButton.interactable = enable;
		}

		internal void ToggleCostPanel(bool enable)
		{
			if (!enable)
			{
				return;
			}
			global::Kampai.Game.Scaffolding instance = gameContext.injectionBinder.GetInstance<global::Kampai.Game.Scaffolding>();
			if (instance.Building != null)
			{
				myModal.ItemCostPanel.SetActive(false);
				return;
			}
			myModal.ItemCostPanel.SetActive(true);
			global::Kampai.Game.IDefinitionService instance2 = gameContext.injectionBinder.GetInstance<global::Kampai.Game.IDefinitionService>();
			global::Kampai.Game.BuildingDefinition definition = instance.Definition;
			global::System.Collections.Generic.ICollection<global::Kampai.Game.Building> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.Building>(definition.ID);
			int num = byDefinitionId.Count * definition.IncrementalCost;
			foreach (global::Kampai.Util.QuantityItem input in instance.Transaction.Inputs)
			{
				if (input.ID == 0 || input.ID == 1)
				{
					global::Kampai.Game.ItemDefinition itemDefinition = instance2.Get<global::Kampai.Game.ItemDefinition>(input.ID);
					int num2 = (int)input.Quantity + num;
					SetItemCost(num2, itemDefinition);
					int quantityByDefinitionId = (int)playerService.GetQuantityByDefinitionId(input.ID);
					if (quantityByDefinitionId < num2)
					{
						myModal.ItemBacking.color = global::Kampai.Util.GameConstants.UI.UI_DARK_RED;
					}
					break;
				}
			}
		}

		internal void SetItemCost(int cost, global::Kampai.Game.ItemDefinition itemDefinition)
		{
			myModal.ItemCost.text = cost.ToString();
			UIUtils.SetItemIcon(myModal.ItemIcon, itemDefinition);
			myModal.ItemIcon.gameObject.SetActive(true);
			myModal.ItemCost.gameObject.SetActive(true);
		}

		internal void SetInventoryCount(int inventoryCount)
		{
			if (inventoryCount != 0)
			{
				myModal.InventoryCount.text = inventoryCount.ToString();
				myModal.ItemIcon.gameObject.SetActive(false);
				myModal.ItemCost.gameObject.SetActive(false);
				myModal.InventoryBacking.SetActive(true);
				myModal.InventoryCount.gameObject.SetActive(true);
			}
		}

		private void PulseAcceptButton(bool enablePulse)
		{
			if (AcceptButton.enabled)
			{
				global::UnityEngine.Animator component = AcceptButton.GetComponent<global::UnityEngine.Animator>();
				component.enabled = false;
				global::UnityEngine.Animator[] componentsInChildren = AcceptButton.GetComponentsInChildren<global::UnityEngine.Animator>();
				global::UnityEngine.Animator[] array = componentsInChildren;
				foreach (global::UnityEngine.Animator animator in array)
				{
					animator.enabled = false;
				}
				if (enablePulse)
				{
					global::Kampai.Util.TweenUtil.Throb(AcceptButton.transform, 0.85f, 0.5f, out originalScale);
					return;
				}
				Go.killAllTweensWithTarget(AcceptButton.transform);
				AcceptButton.transform.localScale = originalScale;
			}
		}
	}
}
