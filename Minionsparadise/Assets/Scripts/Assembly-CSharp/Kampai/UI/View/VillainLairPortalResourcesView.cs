namespace Kampai.UI.View
{
	public class VillainLairPortalResourcesView : global::Kampai.UI.View.PopupMenuView
	{
		public global::Kampai.UI.View.ButtonView enterLair;

		public global::UnityEngine.UI.Text resourceProductionDescription;

		public global::UnityEngine.UI.Text resourceItemAmount;

		public global::Kampai.UI.View.KampaiImage resourceItemImage;

		public global::UnityEngine.GameObject partyPanel;

		public global::UnityEngine.UI.Text partyBoostText;

		public global::UnityEngine.UI.ScrollRect scrollView;

		public global::UnityEngine.GameObject LockStateGameObject;

		private global::Kampai.Main.ILocalizationService localizationService;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.IPlayerService playerService;

		private global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour> sliderViews;

		private global::Kampai.Game.VillainLair lair;

		internal void Init(global::Kampai.Game.VillainLair lair, global::System.Collections.Generic.List<global::Kampai.Game.VillainLairResourcePlot> plots, global::Kampai.Main.ILocalizationService localizationService, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::Kampai.UI.View.ModalSettings modalSettings, global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData)
		{
			InitProgrammatic(buildingPopupPositionData);
			this.localizationService = localizationService;
			this.definitionService = definitionService;
			this.playerService = playerService;
			this.lair = lair;
			sliderViews = new global::System.Collections.Generic.List<global::UnityEngine.MonoBehaviour>();
			SetupModalInfo(plots, modalSettings);
			base.Open();
		}

		internal void SetEnterLairButtonActive(bool active)
		{
			enterLair.GetComponent<global::UnityEngine.UI.Button>().interactable = active;
		}

		internal void SetupModalInfo(global::System.Collections.Generic.List<global::Kampai.Game.VillainLairResourcePlot> plots, global::Kampai.UI.View.ModalSettings modalSettings)
		{
			SetUpResourceInformation();
			int count = plots.Count;
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load("cmp_Resource_Lair") as global::UnityEngine.GameObject;
			for (int i = 0; i < count; i++)
			{
				global::Kampai.Game.VillainLairResourcePlot villainLairResourcePlot = plots[i];
				if (sliderViews.Count < count)
				{
					if (villainLairResourcePlot.State != global::Kampai.Game.BuildingState.Inaccessible)
					{
						global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
						global::Kampai.UI.View.MinionSliderView component = gameObject.GetComponent<global::Kampai.UI.View.MinionSliderView>();
						sliderViews.Add(component);
						component.transform.SetParent(scrollView.content, false);
						UpdateView(i, modalSettings, plots[i]);
						continue;
					}
					global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(LockStateGameObject, global::UnityEngine.Vector3.zero, global::UnityEngine.Quaternion.identity) as global::UnityEngine.GameObject;
					if (gameObject2 == null)
					{
						continue;
					}
					gameObject2.SetActive(true);
					gameObject2.transform.SetParent(scrollView.content, false);
					sliderViews.Add(gameObject2.GetComponent<global::Kampai.UI.View.KampaiImage>());
				}
				if (villainLairResourcePlot.State != global::Kampai.Game.BuildingState.Inaccessible)
				{
					UpdateView(i, modalSettings, plots[i]);
				}
			}
		}

		internal void SetPartyInfo(float boost, string boostString, bool isOn = true)
		{
			partyBoostText.text = boostString;
			bool flag = isOn && (int)(boost * 100f) != 100;
			partyPanel.SetActive(isOn && flag);
			foreach (global::UnityEngine.MonoBehaviour sliderView in sliderViews)
			{
				global::Kampai.UI.View.MinionSliderView minionSliderView = sliderView as global::Kampai.UI.View.MinionSliderView;
				if (minionSliderView != null)
				{
					minionSliderView.isCorrectBuffType = flag;
				}
			}
		}

		internal void UpdateDisplay()
		{
			resourceItemAmount.text = playerService.GetQuantityByDefinitionId(lair.Definition.ResourceItemID).ToString();
		}

		internal void UpdateView(int index, global::Kampai.UI.View.ModalSettings modalSettings, global::Kampai.Game.VillainLairResourcePlot currentPlot)
		{
			global::Kampai.UI.View.MinionSliderView minionSliderView = sliderViews[index] as global::Kampai.UI.View.MinionSliderView;
			if (!(minionSliderView == null))
			{
				minionSliderView.SetModalSettings(modalSettings);
				minionSliderView.resourcePlot = currentPlot;
				minionSliderView.isResourcePlotSlider = true;
				minionSliderView.identifier = index;
				minionSliderView.playerService = playerService;
				minionSliderView.buttonImage.sprite = resourceItemImage.sprite;
				minionSliderView.buttonImage.maskSprite = resourceItemImage.maskSprite;
				minionSliderView.UpdateHarvestTime();
				bool flag = currentPlot.State == global::Kampai.Game.BuildingState.Harvestable;
				bool flag2 = currentPlot.MinionIsTaskedToBuilding();
				if (flag)
				{
					minionSliderView.minionID = -1;
					minionSliderView.SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Harvestable);
				}
				else if (flag2)
				{
					minionSliderView.minionID = currentPlot.MinionIDInBuilding;
					minionSliderView.SetRushCost();
					minionSliderView.startTime = currentPlot.UTCLastTaskingTimeStarted;
					minionSliderView.SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Working);
				}
				else
				{
					minionSliderView.minionID = -1;
					minionSliderView.SetMinionSliderState(global::Kampai.UI.View.MinionSliderState.Available);
				}
			}
		}

		internal void SetUpResourceInformation()
		{
			global::Kampai.Game.IngredientsItemDefinition ingredientsItemDefinition = definitionService.Get<global::Kampai.Game.IngredientsItemDefinition>(lair.Definition.ResourceItemID);
			int secondsToHarvest = lair.Definition.SecondsToHarvest;
			int quantityByDefinitionId = (int)playerService.GetQuantityByDefinitionId(ingredientsItemDefinition.ID);
			resourceProductionDescription.text = localizationService.GetString("ResourceProd", localizationService.GetString(ingredientsItemDefinition.LocalizedKey, 1), UIUtils.FormatTime(secondsToHarvest, localizationService));
			resourceItemAmount.text = quantityByDefinitionId.ToString();
			resourceItemImage.sprite = UIUtils.LoadSpriteFromPath(ingredientsItemDefinition.Image);
			resourceItemImage.maskSprite = UIUtils.LoadSpriteFromPath(ingredientsItemDefinition.Mask);
		}
	}
}
