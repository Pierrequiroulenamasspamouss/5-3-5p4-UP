namespace Kampai.UI.View
{
	public class PlayerTrainingView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.UI.Text trainingTitle;

		public global::System.Collections.Generic.List<global::UnityEngine.UI.Text> cardTitleTexts;

		public global::System.Collections.Generic.List<global::UnityEngine.UI.Text> cardDescriptionTexts;

		public global::Kampai.UI.View.ButtonView confirmButton;

		public global::UnityEngine.Animator animator;

		public global::UnityEngine.RectTransform cardOneCompositePanel;

		public global::UnityEngine.RectTransform cardTwoCompositePanel;

		public global::UnityEngine.RectTransform cardThreeCompositePanel;

		public global::System.Collections.Generic.List<global::Kampai.UI.View.MinionSlotModal> minionSlots;

		public global::Kampai.UI.View.KampaiImage transitionOne;

		public global::Kampai.UI.View.KampaiImage transitionTwo;

		public global::Kampai.UI.View.KampaiImage cardOneSingleImage;

		public global::Kampai.UI.View.KampaiImage cardTwoSingleImage;

		public global::Kampai.UI.View.KampaiImage cardThreeSingleImage;

		public global::System.Collections.Generic.List<global::Kampai.UI.View.KampaiImage> cardOneCompositeImages;

		public global::System.Collections.Generic.List<global::Kampai.UI.View.KampaiImage> cardTwoCompositeImages;

		public global::System.Collections.Generic.List<global::Kampai.UI.View.KampaiImage> cardThreeCompositeImages;

		internal global::strange.extensions.signal.impl.Signal completeSignal = new global::strange.extensions.signal.impl.Signal();

		internal global::strange.extensions.signal.impl.Signal audioSignal = new global::strange.extensions.signal.impl.Signal();

		internal global::System.Collections.Generic.List<int> prestigeDefinitionIDs = new global::System.Collections.Generic.List<int>();

		internal global::System.Collections.Generic.List<int> buildingDefinitionIDs = new global::System.Collections.Generic.List<int>();

		private global::System.Collections.Generic.List<global::Kampai.Game.Building> buildings = new global::System.Collections.Generic.List<global::Kampai.Game.Building>();

		private global::System.Collections.Generic.IList<global::Kampai.Game.View.MinionObject> minionsList;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.UI.IFancyUIService fancyUIService;

		private global::System.Collections.Generic.List<global::Kampai.Game.View.DummyCharacterObject> dummyCharacters = new global::System.Collections.Generic.List<global::Kampai.Game.View.DummyCharacterObject>();

		private global::System.Collections.Generic.List<global::Kampai.Game.View.BuildingObject> dummyBuildings = new global::System.Collections.Generic.List<global::Kampai.Game.View.BuildingObject>();

		internal void Init(global::Kampai.Game.IDefinitionService defService, global::Kampai.UI.IFancyUIService UIService)
		{
			definitionService = defService;
			fancyUIService = UIService;
		}

		internal void SetTitle(string title)
		{
			trainingTitle.text = title;
		}

		internal void SetCardTitles(string titleOne, string titleTwo, string titleThree)
		{
			cardTitleTexts[0].text = titleOne;
			cardTitleTexts[1].text = titleTwo;
			cardTitleTexts[2].text = titleThree;
		}

		internal void SetCardDescriptions(string descriptionOne, string descriptionTwo, string descriptionThree)
		{
			cardDescriptionTexts[0].text = descriptionOne;
			cardDescriptionTexts[1].text = descriptionTwo;
			cardDescriptionTexts[2].text = descriptionThree;
		}

		internal void SetTransitionOne(string mask)
		{
			transitionOne.maskSprite = UIUtils.LoadSpriteFromPath(mask);
		}

		internal void SetTransitionTwo(string mask)
		{
			transitionTwo.maskSprite = UIUtils.LoadSpriteFromPath(mask);
		}

		internal void SetCardOneImages(global::System.Collections.Generic.List<global::Kampai.Game.ImageMaskCombo> images)
		{
			PopulateImages(cardOneSingleImage, cardOneCompositeImages, images, cardOneCompositePanel);
		}

		internal void SetCardTwoImages(global::System.Collections.Generic.List<global::Kampai.Game.ImageMaskCombo> images)
		{
			PopulateImages(cardTwoSingleImage, cardTwoCompositeImages, images, cardTwoCompositePanel);
		}

		internal void SetCardThreeImages(global::System.Collections.Generic.List<global::Kampai.Game.ImageMaskCombo> images)
		{
			PopulateImages(cardThreeSingleImage, cardThreeCompositeImages, images, cardThreeCompositePanel);
		}

		private void PopulateImages(global::Kampai.UI.View.KampaiImage singleImage, global::System.Collections.Generic.List<global::Kampai.UI.View.KampaiImage> compositeImages, global::System.Collections.Generic.List<global::Kampai.Game.ImageMaskCombo> images, global::UnityEngine.RectTransform compositePanel)
		{
			if (images.Count == 1)
			{
				if (images[0].image.Equals("img_fill_128"))
				{
					singleImage.color = global::Kampai.Util.GameConstants.UI.UI_TEXT_LIGHT_BLUE;
				}
				singleImage.sprite = UIUtils.LoadSpriteFromPath(images[0].image);
				if (!string.IsNullOrEmpty(images[0].mask))
				{
					singleImage.maskSprite = UIUtils.LoadSpriteFromPath(images[0].mask);
				}
				compositePanel.gameObject.SetActive(false);
				return;
			}
			singleImage.gameObject.SetActive(false);
			for (int i = 0; i < images.Count; i++)
			{
				if (images[i].image.Equals("img_fill_128"))
				{
					compositeImages[i].color = global::Kampai.Util.GameConstants.UI.UI_TEXT_LIGHT_BLUE;
				}
				compositeImages[i].sprite = UIUtils.LoadSpriteFromPath(images[i].image);
				if (!string.IsNullOrEmpty(images[i].mask))
				{
					compositeImages[i].maskSprite = UIUtils.LoadSpriteFromPath(images[i].mask);
				}
			}
		}

		public void SetMinionCard(int cardNumber)
		{
			int num = prestigeDefinitionIDs[cardNumber];
			int num2 = buildingDefinitionIDs[cardNumber];
			if (num != 0)
			{
				CreateCharacter(num, cardNumber);
			}
			if (num2 != 0)
			{
				CreateBuilding(num2, cardNumber);
			}
		}

		private void CreateCharacter(int prestigeID, int cardNumber)
		{
			if (prestigeID == 99 || prestigeID == 191 || prestigeID == 192 || prestigeID == 193)
			{
				int[] mINION_DEFINITION_IDS = global::Kampai.Util.GameConstants.MINION_DEFINITION_IDS;
				global::System.Random random = new global::System.Random();
				int num = random.Next(mINION_DEFINITION_IDS.Length);
				int minionLevel = 0;
				switch (prestigeID)
				{
				case 191:
					minionLevel = 1;
					break;
				case 192:
					minionLevel = 2;
					break;
				case 193:
					minionLevel = 3;
					break;
				}
				dummyCharacters.Add(fancyUIService.BuildMinion(mINION_DEFINITION_IDS[num], global::Kampai.UI.DummyCharacterAnimationState.Idle, minionSlots[cardNumber].transform, true, true, minionLevel));
			}
			else
			{
				global::Kampai.UI.DummyCharacterType characterType = fancyUIService.GetCharacterType(prestigeID);
				dummyCharacters.Add(fancyUIService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.Idle, minionSlots[cardNumber].transform, minionSlots[cardNumber].VillainScale, minionSlots[cardNumber].VillainPositionOffset, prestigeID));
			}
		}

		private void CreateBuilding(int buildingID, int cardNumber)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get<global::Kampai.Game.BuildingDefinition>(buildingID);
			minionSlots[cardNumber].transform.localScale = new global::UnityEngine.Vector3(buildingDefinition.UiScale, buildingDefinition.UiScale, buildingDefinition.UiScale);
			minionSlots[cardNumber].transform.localPosition = buildingDefinition.UiPosition;
			minionsList = new global::System.Collections.Generic.List<global::Kampai.Game.View.MinionObject>();
			global::Kampai.Game.Building building;
			dummyBuildings.Add(fancyUIService.CreateDummyBuildingObject(buildingDefinition, minionSlots[cardNumber].transform.gameObject, out building, minionsList, false));
			buildings.Add(building);
		}

		public void AnimationComplete()
		{
			completeSignal.Dispatch();
		}

		internal void RemoveCoroutine()
		{
			foreach (global::Kampai.Game.View.DummyCharacterObject dummyCharacter in dummyCharacters)
			{
				if (dummyCharacter != null)
				{
					dummyCharacter.RemoveCoroutine();
					global::UnityEngine.Object.Destroy(dummyCharacter.gameObject);
				}
			}
			for (int i = 0; i < dummyBuildings.Count; i++)
			{
				if (dummyBuildings[i] != null)
				{
					fancyUIService.ReleaseBuildingObject(dummyBuildings[i], buildings[i], minionsList);
				}
			}
		}

		public void TriggerAudio()
		{
			audioSignal.Dispatch();
		}
	}
}
