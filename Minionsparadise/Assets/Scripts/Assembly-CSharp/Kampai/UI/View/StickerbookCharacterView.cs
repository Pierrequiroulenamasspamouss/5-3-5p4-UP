namespace Kampai.UI.View
{
	public class StickerbookCharacterView : global::Kampai.Util.KampaiView
	{
		public ScrollableButtonView character;

		public ScrollableButtonView lockedCharacter;

		public ScrollableButtonView limitedEvent;

		public global::Kampai.UI.View.MinionSlotModal MinionSlot;

		public global::Kampai.UI.View.KampaiImage lockedImage;

		public global::Kampai.UI.View.KampaiImage limitedImage;

		public global::UnityEngine.RectTransform characterBadge;

		public global::UnityEngine.RectTransform LTEBadge;

		public global::UnityEngine.RectTransform selectionIcon;

		public global::UnityEngine.RectTransform unlockedPanel;

		public global::UnityEngine.RectTransform lockedPanel;

		public global::UnityEngine.UI.Text title;

		public global::UnityEngine.UI.Text characterBadgeText;

		public global::UnityEngine.UI.Text LTEBadgeText;

		private global::Kampai.Game.Building building;

		private global::Kampai.Game.View.BuildingObject buildingObj;

		private global::System.Collections.Generic.IList<global::Kampai.Game.View.MinionObject> minionsList;

		private global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject;

		private bool isSelected;

		private global::Kampai.UI.IFancyUIService fancyService;

		public int prestigeID { get; set; }

		public int limitedID { get; set; }

		public bool isLimited { get; set; }

		public bool isLocked { get; set; }

		internal void Init(int numNewStickers, global::Kampai.UI.IFancyUIService fancyUIService, global::Kampai.Game.IPrestigeService prestigeService, global::Kampai.Main.ILocalizationService localService, global::Kampai.Game.IDefinitionService definitionService)
		{
			fancyService = fancyUIService;
			if (!isLimited)
			{
				global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(prestigeID);
				string text = localService.GetString(prestigeDefinition.LocalizedKey);
				title.text = text;
				if (!isLocked)
				{
					if (prestigeID != 0)
					{
						global::Kampai.UI.DummyCharacterType characterType = fancyUIService.GetCharacterType(prestigeID);
						dummyCharacterObject = fancyUIService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.Idle, MinionSlot.transform, MinionSlot.VillainScale, MinionSlot.VillainPositionOffset, prestigeID);
					}
					if (numNewStickers > 0)
					{
						characterBadge.gameObject.SetActive(true);
						characterBadgeText.text = numNewStickers.ToString();
					}
				}
				else
				{
					unlockedPanel.gameObject.SetActive(false);
					lockedPanel.gameObject.SetActive(true);
					global::UnityEngine.Sprite characterImage = null;
					global::UnityEngine.Sprite characterMask = null;
					prestigeService.GetCharacterImageBasedOnMood(prestigeDefinition, global::Kampai.Game.CharacterImageType.BigAvatarIcon, out characterImage, out characterMask);
					lockedImage.maskSprite = characterMask;
				}
			}
			else
			{
				limitedImage.gameObject.SetActive(false);
				title.text = localService.GetString("StickerbookEventHeader");
				global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get<global::Kampai.Game.BuildingDefinition>(1000012712);
				MinionSlot.transform.localScale = new global::UnityEngine.Vector3(buildingDefinition.UiScale, buildingDefinition.UiScale, buildingDefinition.UiScale);
				MinionSlot.transform.localPosition = buildingDefinition.UiPosition;
				minionsList = new global::System.Collections.Generic.List<global::Kampai.Game.View.MinionObject>();
				buildingObj = fancyUIService.CreateDummyBuildingObject(buildingDefinition, MinionSlot.transform.gameObject, out building, minionsList, false);
				if (numNewStickers > 0)
				{
					LTEBadge.gameObject.SetActive(true);
					LTEBadgeText.text = numNewStickers.ToString();
				}
			}
		}

		internal void RemoveCoroutine()
		{
			if (dummyCharacterObject != null)
			{
				dummyCharacterObject.RemoveCoroutine();
				global::UnityEngine.Object.Destroy(dummyCharacterObject.gameObject);
			}
			if (buildingObj != null)
			{
				fancyService.ReleaseBuildingObject(buildingObj, building, minionsList);
			}
		}

		internal void UpdateBadge()
		{
			if (isLimited)
			{
				LTEBadge.gameObject.SetActive(false);
			}
			else
			{
				characterBadge.gameObject.SetActive(false);
			}
		}

		internal void OnCharacterClicked(int id, bool isLimitedEvent)
		{
			if (dummyCharacterObject == null && !isLimited)
			{
				return;
			}
			if (!isLimited && id == prestigeID)
			{
				if (!isSelected)
				{
					selectionIcon.gameObject.SetActive(true);
					isSelected = true;
					if (dummyCharacterObject != null)
					{
						dummyCharacterObject.RemoveCoroutine(false);
						dummyCharacterObject.StartingState(global::Kampai.UI.DummyCharacterAnimationState.SelectedHappy);
						global::UnityEngine.RectTransform rectTransform = MinionSlot.transform as global::UnityEngine.RectTransform;
						global::UnityEngine.Vector3 anchoredPosition3D = rectTransform.anchoredPosition3D;
						rectTransform.anchoredPosition3D = new global::UnityEngine.Vector3(anchoredPosition3D.x, anchoredPosition3D.y, anchoredPosition3D.z + -900f);
					}
				}
			}
			else if (isLimited && isLimitedEvent)
			{
				if (!isSelected)
				{
					selectionIcon.gameObject.SetActive(true);
					isSelected = true;
					if (dummyCharacterObject != null)
					{
						dummyCharacterObject.RemoveCoroutine(false);
						dummyCharacterObject.StartingState(global::Kampai.UI.DummyCharacterAnimationState.SelectedHappy);
						global::UnityEngine.RectTransform rectTransform2 = MinionSlot.transform as global::UnityEngine.RectTransform;
						global::UnityEngine.Vector3 anchoredPosition3D2 = rectTransform2.anchoredPosition3D;
						rectTransform2.anchoredPosition3D = new global::UnityEngine.Vector3(anchoredPosition3D2.x, anchoredPosition3D2.y, anchoredPosition3D2.z + -900f);
					}
				}
			}
			else if (isSelected)
			{
				selectionIcon.gameObject.SetActive(false);
				isSelected = false;
				if (dummyCharacterObject != null)
				{
					global::UnityEngine.RectTransform rectTransform3 = MinionSlot.transform as global::UnityEngine.RectTransform;
					global::UnityEngine.Vector3 anchoredPosition3D3 = rectTransform3.anchoredPosition3D;
					rectTransform3.anchoredPosition3D = new global::UnityEngine.Vector3(anchoredPosition3D3.x, anchoredPosition3D3.y, anchoredPosition3D3.z - -900f);
					dummyCharacterObject.RemoveCoroutine(false);
					dummyCharacterObject.StartingState(global::Kampai.UI.DummyCharacterAnimationState.Idle, true);
				}
			}
		}
	}
}
