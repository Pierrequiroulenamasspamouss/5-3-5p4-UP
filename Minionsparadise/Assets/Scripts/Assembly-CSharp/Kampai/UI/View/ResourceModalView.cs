namespace Kampai.UI.View
{
	public class ResourceModalView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.RectTransform scrollViewContent;

		public global::Kampai.UI.View.KampaiImage IconImage;

		public global::Kampai.UI.View.ButtonView LeftArrow;

		public global::Kampai.UI.View.ButtonView RightArrow;

		public global::UnityEngine.GameObject PartyPanel;

		public global::UnityEngine.UI.Text modalName;

		public global::UnityEngine.UI.Text descriptionText;

		public global::UnityEngine.UI.Text ownedResourceText;

		public global::UnityEngine.UI.Text partyBoostText;

		private global::Kampai.Game.ResourceBuilding building;

		private global::System.Collections.Generic.List<global::Kampai.UI.View.MinionSliderView> views;

		private global::Kampai.Main.ILocalizationService localService;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.IPlayerService playerService;

		private int transactionId;

		internal global::strange.extensions.signal.impl.Signal<int> resetDoubleTapSignal = new global::strange.extensions.signal.impl.Signal<int>();

		internal void Init(global::Kampai.Game.ResourceBuilding building, global::System.Collections.Generic.List<global::Kampai.Game.Minion> minions, global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::Kampai.UI.View.ModalSettings modalSettings, global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData)
		{
			InitProgrammatic(buildingPopupPositionData);
			this.localService = localService;
			this.definitionService = definitionService;
			this.playerService = playerService;
			views = new global::System.Collections.Generic.List<global::Kampai.UI.View.MinionSliderView>();
			SetupModalInfo(building, minions, modalSettings);
			base.Open();
		}

		internal void RecreateModal(global::Kampai.Game.ResourceBuilding building, global::System.Collections.Generic.List<global::Kampai.Game.Minion> minions, global::Kampai.UI.View.ModalSettings modalSettings)
		{
			SetupModalInfo(building, minions, modalSettings);
		}

		private void SetupModalInfo(global::Kampai.Game.ResourceBuilding building, global::System.Collections.Generic.List<global::Kampai.Game.Minion> minions, global::Kampai.UI.View.ModalSettings modalSettings)
		{
			this.building = building;
			global::Kampai.Game.ResourceBuildingDefinition definition = building.Definition;
			modalName.text = localService.GetString(definition.LocalizedKey);
			int harvestTimeForTaskableBuilding = BuildingUtil.GetHarvestTimeForTaskableBuilding(building, definitionService);
			global::Kampai.Game.ItemDefinition itemDefinition = definitionService.Get<global::Kampai.Game.ItemDefinition>(definition.ItemId);
			descriptionText.text = localService.GetString("ResourceProd", localService.GetString(itemDefinition.LocalizedKey, 1), UIUtils.FormatTime(harvestTimeForTaskableBuilding, localService));
			transactionId = building.GetTransactionID(definitionService);
			int harvestItemDefinitionIdFromTransactionId = definitionService.GetHarvestItemDefinitionIdFromTransactionId(transactionId);
			global::Kampai.Game.ItemDefinition itemDefinition2 = definitionService.Get<global::Kampai.Game.ItemDefinition>(harvestItemDefinitionIdFromTransactionId);
			IconImage.sprite = UIUtils.LoadSpriteFromPath(itemDefinition2.Image);
			IconImage.maskSprite = UIUtils.LoadSpriteFromPath(itemDefinition2.Mask);
			UpdateDisplay();
			global::UnityEngine.GameObject gameObject = global::Kampai.Util.KampaiResources.Load("cmp_buildInfo") as global::UnityEngine.GameObject;
			global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
			float y = rectTransform.sizeDelta.y;
			for (int i = 0; i < building.GetMaxSlotCount(); i++)
			{
				if (views.Count < building.GetMaxSlotCount())
				{
					global::Kampai.UI.View.MinionSliderView item = CreateView(i, y, gameObject);
					views.Add(item);
				}
				UpdateView(i, minions, modalSettings);
			}
			scrollViewContent.offsetMin = new global::UnityEngine.Vector2(0f, (float)building.GetMaxSlotCount() * (0f - y));
		}

		internal void SetPartyInfo(float boost, string boostString, bool isOn = true)
		{
			partyBoostText.text = boostString;
			bool flag = isOn && (int)(boost * 100f) != 100;
			PartyPanel.SetActive(isOn && flag);
			foreach (global::Kampai.UI.View.MinionSliderView view in views)
			{
				view.isCorrectBuffType = flag;
			}
		}

		internal void SetArrowButtonState(bool enable)
		{
			LeftArrow.GetComponent<global::UnityEngine.UI.Button>().interactable = enable;
			RightArrow.GetComponent<global::UnityEngine.UI.Button>().interactable = enable;
		}

		private global::Kampai.UI.View.MinionSliderView CreateView(int index, float height, global::UnityEngine.GameObject prefab)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(prefab);
			global::UnityEngine.Transform transform = gameObject.transform;
			global::Kampai.UI.View.MinionSliderView component = gameObject.GetComponent<global::Kampai.UI.View.MinionSliderView>();
			component.Init(localService, definitionService);
			float paddingInPixels = component.PaddingInPixels;
			transform.SetParent(scrollViewContent, false);
			global::UnityEngine.RectTransform rectTransform = transform as global::UnityEngine.RectTransform;
			rectTransform.localPosition = global::UnityEngine.Vector3.zero;
			rectTransform.offsetMin = new global::UnityEngine.Vector2(0f, (0f - (height + paddingInPixels)) * (float)index - height);
			rectTransform.offsetMax = new global::UnityEngine.Vector2(0f, (0f - (height + paddingInPixels)) * (float)index);
			rectTransform.localScale = global::UnityEngine.Vector3.one;
			return component;
		}

		private void UpdateView(int index, global::System.Collections.Generic.List<global::Kampai.Game.Minion> minions, global::Kampai.UI.View.ModalSettings modalSettings)
		{
			global::Kampai.UI.View.MinionSliderView minionSliderView = views[index];
			minionSliderView.SetModalSettings(modalSettings);
			minionSliderView.building = building;
			minionSliderView.identifier = index;
			minionSliderView.playerService = playerService;
			minionSliderView.buttonImage.sprite = IconImage.sprite;
			minionSliderView.buttonImage.maskSprite = IconImage.maskSprite;
			minionSliderView.UpdateHarvestTime();
			int count = minions.Count;
			int availableHarvest = building.GetAvailableHarvest();
			int num = index - availableHarvest;
			bool flag = index < availableHarvest;
			bool flag2 = num < count && num >= 0;
			bool flag3 = index < building.MinionSlotsOwned;
			if (flag && flag3)
			{
				minionSliderView.minionID = -1;
				minionSliderView.SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Harvestable);
				return;
			}
			if (flag2 && flag3)
			{
				minionSliderView.minionID = minions[num].ID;
				minionSliderView.SetRushCost();
				minionSliderView.costText.text = string.Format("{0}", building.Definition.RushCost);
				minionSliderView.startTime = minions[num].UTCTaskStartTime;
				minionSliderView.SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Working);
				return;
			}
			minionSliderView.minionID = -1;
			if (flag3)
			{
				minionSliderView.SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Available);
				return;
			}
			minionSliderView.lockedText.text = localService.GetString("BRBLockedSlot", building.GetSlotUnlockLevelByIndex(index));
			minionSliderView.lockedCostText.text = string.Format("{0}", building.GetSlotCostByIndex(index));
			minionSliderView.UpdateLockedButton();
			minionSliderView.SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Locked);
		}

		public void LevelUpUnlock(uint playerLevel)
		{
			if (views == null)
			{
				return;
			}
			foreach (global::Kampai.UI.View.MinionSliderView view in views)
			{
				if (view.identifier >= building.MinionSlotsOwned)
				{
					int slotUnlockLevelByIndex = building.GetSlotUnlockLevelByIndex(view.identifier);
					if (playerLevel < slotUnlockLevelByIndex)
					{
						view.lockedButton.GetComponent<global::UnityEngine.UI.Button>().interactable = true;
						break;
					}
					view.SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Available);
				}
			}
		}

		internal void UpdateDisplay()
		{
			global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(transactionId);
			if (transactionDefinition.Outputs[0] != null)
			{
				ownedResourceText.text = playerService.GetQuantityByDefinitionId(transactionDefinition.Outputs[0].ID).ToString();
			}
		}

		public void ResetRushButtonsState()
		{
			foreach (global::Kampai.UI.View.MinionSliderView view in views)
			{
				view.rushButton.ResetAnim();
			}
		}
	}
}
