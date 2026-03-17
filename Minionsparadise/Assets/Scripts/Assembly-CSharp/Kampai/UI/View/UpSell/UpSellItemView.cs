namespace Kampai.UI.View.UpSell
{
	public class UpSellItemView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.GameObject Backing;

		public global::UnityEngine.GameObject buildingStencil;

		public global::Kampai.UI.View.KampaiImage itemSlot;

		public global::UnityEngine.GameObject buildingSlot;

		public global::Kampai.UI.View.MinionSlotModal minionSlot;

		public global::Kampai.UI.View.LocalizeView MTXItemQuantity;

		public global::Kampai.UI.View.LocalizeView MTXItemTitle;

		public global::UnityEngine.UI.Text PartyPointsTitle;

		public global::Kampai.UI.View.KampaiImage MTXGlowImage;

		public global::UnityEngine.GameObject MTXPanel;

		public global::UnityEngine.GameObject Exclusive;

		public global::Kampai.UI.View.LocalizeView itemQuantity;

		private int showMtxID;

		private global::Kampai.Game.Building building;

		private global::Kampai.Game.View.BuildingObject buildingObj;

		private global::System.Collections.Generic.IList<global::Kampai.Game.View.MinionObject> minionsList;

		private global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject;

		private bool isAudible;

		private bool isSet;

		protected global::Kampai.Main.ILocalizationService localizationService;

		protected global::Kampai.Game.IDefinitionService definitionService;

		protected global::Kampai.Util.IKampaiLogger logger;

		protected global::Kampai.UI.IFancyUIService fancyUiService;

		protected global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal;

		private float additionalScale = 1f;

		public global::Kampai.Util.QuantityItem Item;

		public float AdditionalUIScale
		{
			get
			{
				return additionalScale;
			}
			set
			{
				additionalScale = value;
			}
		}

		internal virtual void Init(global::Kampai.Main.ILocalizationService localizationService, global::Kampai.UI.IFancyUIService fancyUiService, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Main.MoveAudioListenerSignal moveAudioListenerSignal)
		{
			this.localizationService = localizationService;
			this.fancyUiService = fancyUiService;
			this.definitionService = definitionService;
			this.logger = logger;
			this.moveAudioListenerSignal = moveAudioListenerSignal;
			SetupItem();
		}

		public void SetupItem()
		{
			if (isSet || Item == null || definitionService == null)
			{
				return;
			}
			if (Backing != null)
			{
				Backing.SetActive(true);
			}
			global::Kampai.Game.Definition definition = definitionService.Get(Item.ID);
			global::Kampai.Game.BuildingDefinition buildingDefinition = definition as global::Kampai.Game.BuildingDefinition;
			if (buildingDefinition != null)
			{
				SetupBuildingItem(buildingDefinition);
			}
			global::Kampai.Game.MinionDefinition minionDefinition = definition as global::Kampai.Game.MinionDefinition;
			if (minionDefinition != null)
			{
				SetupMinion(minionDefinition);
			}
			global::Kampai.Game.ItemDefinition itemDefinition = definition as global::Kampai.Game.ItemDefinition;
			global::Kampai.Game.CurrencyItemDefinition currencyItemDefinition = null;
			if (itemDefinition != null)
			{
				if (showMtxID > 0 && (Item.ID == 0 || Item.ID == 1))
				{
					currencyItemDefinition = definitionService.Get<global::Kampai.Game.CurrencyItemDefinition>(showMtxID);
				}
				SetupItem(itemDefinition, currencyItemDefinition);
			}
			isSet = true;
			if (Item.Quantity > 1)
			{
				if (currencyItemDefinition != null)
				{
					MTXPanel.SetActive(true);
					MTXItemQuantity.text = Item.Quantity.ToString();
				}
				else
				{
					itemQuantity.gameObject.SetActive(true);
					itemQuantity.text = Item.Quantity.ToString();
				}
			}
		}

		public void ShowMtxID(int showMtxId)
		{
			showMtxID = showMtxId;
		}

		internal void SetAudible(bool isAudible)
		{
			this.isAudible = isAudible;
		}

		internal void Release()
		{
			if (fancyUiService != null)
			{
				fancyUiService.ReleaseBuildingObject(buildingObj, building, minionsList);
				moveAudioListenerSignal.Dispatch(true, null);
			}
			if (!(dummyCharacterObject == null))
			{
				dummyCharacterObject.RemoveCoroutine();
				global::UnityEngine.Object.Destroy(dummyCharacterObject.gameObject);
			}
		}

		private void SetupItem(global::Kampai.Game.DisplayableDefinition itemDefinition, global::Kampai.Game.CurrencyItemDefinition currencyItemDefinition)
		{
			itemSlot.gameObject.SetActive(true);
			string iconPath = string.Empty;
			string maskPath = string.Empty;
			if (currencyItemDefinition != null)
			{
				MTXGlowImage.gameObject.SetActive(true);
				iconPath = currencyItemDefinition.Image;
				maskPath = currencyItemDefinition.Mask;
				MTXItemTitle.LocKey = currencyItemDefinition.LocalizedKey;
				global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load(currencyItemDefinition.VFX) as global::UnityEngine.GameObject;
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
				gameObject.transform.SetParent(itemSlot.transform, false);
			}
			else if (itemDefinition != null)
			{
				int iD = itemDefinition.ID;
				if (iD == 0 || iD == 1)
				{
					MTXGlowImage.gameObject.SetActive(true);
					iconPath = itemDefinition.Image;
					maskPath = itemDefinition.Mask;
					MTXItemTitle.LocKey = itemDefinition.LocalizedKey;
					global::UnityEngine.RectTransform rectTransform = MTXGlowImage.gameObject.transform as global::UnityEngine.RectTransform;
					if (rectTransform != null)
					{
						rectTransform.localScale = new global::UnityEngine.Vector3(1.1f, 1.1f, 1f);
					}
				}
				else
				{
					iconPath = itemDefinition.Image;
					maskPath = itemDefinition.Mask;
				}
			}
			fancyUiService.SetKampaiImage(itemSlot, iconPath, maskPath);
		}

		private void SetupMinion(global::Kampai.Game.MinionDefinition minionDefinition)
		{
			if (minionDefinition == null)
			{
				logger.Error("Minion Definition is null for item {0}", Item.ID);
				return;
			}
			minionSlot.gameObject.SetActive(true);
			dummyCharacterObject = fancyUiService.BuildMinion(minionDefinition.ID, global::Kampai.UI.DummyCharacterAnimationState.SelectedIdle, minionSlot.transform, true, isAudible);
			int childCount = dummyCharacterObject.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				global::UnityEngine.Transform child = dummyCharacterObject.transform.GetChild(i);
				if (child.name.Contains("LOD"))
				{
					global::UnityEngine.SkinnedMeshRenderer component = child.GetComponent<global::UnityEngine.SkinnedMeshRenderer>();
					component.updateWhenOffscreen = true;
				}
			}
			if (isAudible)
			{
				moveAudioListenerSignal.Dispatch(false, dummyCharacterObject.transform);
			}
		}

		private void SetupBuildingItem(global::Kampai.Game.BuildingDefinition buildingDef)
		{
			if (buildingDef == null)
			{
				logger.Error("Building definition is null {0}", Item.ID);
				return;
			}
			float num = buildingDef.UiScale;
			if (num < float.Epsilon)
			{
				num = 100f;
			}
			buildingSlot.transform.localScale = new global::UnityEngine.Vector3(num, num, num);
			buildingSlot.transform.localPosition = buildingDef.UiPosition;
			SetupBuildingObject(buildingDef, num);
			buildingStencil.SetActive(true);
			buildingSlot.gameObject.SetActive(true);
			minionsList = new global::System.Collections.Generic.List<global::Kampai.Game.View.MinionObject>();
			buildingObj = fancyUiService.CreateDummyBuildingObject(buildingDef, buildingSlot, out building, minionsList, isAudible);
			fancyUiService.SetStenciledShaderOnBuilding(buildingSlot);
			if (buildingObj == null)
			{
				logger.Error("Failed to create a building object from building def {0}", buildingDef.ID);
				return;
			}
			if (isAudible)
			{
				moveAudioListenerSignal.Dispatch(false, buildingObj.transform);
			}
			if (!string.IsNullOrEmpty(buildingDef.PartyPointsLocalizedKey) && building != null)
			{
				global::Kampai.Game.LeisureBuildingDefintiion leisureBuildingDefintiion = building.Definition as global::Kampai.Game.LeisureBuildingDefintiion;
				if (leisureBuildingDefintiion != null)
				{
					PartyPointsTitle.transform.parent.gameObject.SetActive(true);
					PartyPointsTitle.text = localizationService.GetString(buildingDef.PartyPointsLocalizedKey, leisureBuildingDefintiion.PartyPointsReward);
				}
			}
		}

		private void SetupBuildingObject(global::Kampai.Game.BuildingDefinition buildingDef, float uiScale)
		{
			if (additionalScale < 1f)
			{
				buildingSlot.transform.localScale = new global::UnityEngine.Vector3(uiScale, uiScale, uiScale) * additionalScale;
				buildingSlot.transform.localPosition = buildingDef.UiPosition * additionalScale;
			}
		}

		public void SetIsExclusive(bool isExclusive)
		{
			if (!(Exclusive == null))
			{
				Exclusive.SetActive(isExclusive);
			}
		}
	}
}
