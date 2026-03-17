namespace Kampai.UI.View
{
	public class OrderBoardTicketDetailView : global::Kampai.Util.KampaiView
	{
		private const string requiredItemPrefabPath = "cmp_OrderBoardTicketRequiredItem";

		public global::Kampai.UI.View.LocalizeView TicketName;

		public global::Kampai.UI.View.LocalizeView OrderInstruction;

		public global::UnityEngine.RectTransform ScrollWindow;

		public global::UnityEngine.RectTransform ScrollView;

		public global::UnityEngine.GameObject RequiredItemListBackground;

		public global::UnityEngine.RectTransform RewardPanel;

		public global::UnityEngine.GameObject OrderPanel;

		public global::UnityEngine.GameObject PrestigePanel;

		public global::UnityEngine.GameObject GlowAnimation;

		public global::UnityEngine.RectTransform PrestigeProgressBarFill;

		public global::UnityEngine.UI.Text ProgressBarText;

		public global::UnityEngine.UI.Text XPReward;

		public global::UnityEngine.UI.Text GrindReward;

		public global::UnityEngine.UI.Text PrestigeLevel;

		public global::Kampai.UI.View.MinionSlotModal MinionSlot;

		public global::Kampai.UI.View.KampaiImage FTUEBacking;

		public global::UnityEngine.GameObject FunIcon;

		[global::UnityEngine.Header("Buff Activated")]
		public global::UnityEngine.GameObject BuffInfoGroup;

		public global::UnityEngine.UI.Text BuffMultiplierAmt;

		public global::Kampai.UI.View.KampaiImage BuffTypeIcon;

		public global::UnityEngine.GameObject BuffRewardPanel;

		public global::UnityEngine.UI.Text BuffRewardAmt;

		public global::UnityEngine.GameObject RewardsPanelOutline;

		private bool shouldShowBuff;

		private global::UnityEngine.GameObject requiredItemPrefab;

		private global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject;

		private float height;

		private global::System.Collections.Generic.List<global::Kampai.UI.View.OrderBoardRequiredItemView> itemList;

		private int count;

		private global::Kampai.Main.ILocalizationService localizationService;

		private global::UnityEngine.Vector3 minionSlotScale;

		public void Init(global::Kampai.Main.ILocalizationService localService)
		{
			localizationService = localService;
			requiredItemPrefab = global::Kampai.Util.KampaiResources.Load("cmp_OrderBoardTicketRequiredItem") as global::UnityEngine.GameObject;
			global::UnityEngine.RectTransform rectTransform = requiredItemPrefab.transform as global::UnityEngine.RectTransform;
			height = rectTransform.sizeDelta.y;
			itemList = new global::System.Collections.Generic.List<global::Kampai.UI.View.OrderBoardRequiredItemView>();
			dummyCharacterObject = null;
			minionSlotScale = MinionSlot.transform.localScale;
		}

		internal void ClearDummyObject()
		{
			if (dummyCharacterObject != null)
			{
				dummyCharacterObject.RemoveCoroutine();
				global::UnityEngine.Object.Destroy(dummyCharacterObject.gameObject);
				dummyCharacterObject = null;
			}
		}

		internal global::System.Collections.Generic.List<global::Kampai.UI.View.OrderBoardRequiredItemView> GetItemList()
		{
			return itemList;
		}

		internal void SetFTUEText(string title)
		{
			PrestigePanel.SetActive(false);
			OrderInstruction.text = title;
			OrderInstruction.gameObject.SetActive(true);
			TicketName.gameObject.SetActive(false);
			FTUEBacking.gameObject.SetActive(false);
		}

		internal void SetSlotFullText(string messageLocKey)
		{
			OrderInstruction.LocKey = messageLocKey;
		}

		internal void SetTitle(string title)
		{
			TicketName.text = title;
		}

		internal void SetPrestigeProgress(float currentPrestigePoint, int neededPrestigePoints)
		{
			float num = currentPrestigePoint / (float)neededPrestigePoints;
			num = ((!(num > 1f)) ? num : 1f);
			if (PrestigeProgressBarFill != null && ProgressBarText != null)
			{
				PrestigeProgressBarFill.anchorMax = new global::UnityEngine.Vector2(num, 1f);
				ProgressBarText.text = localizationService.GetString("PrestigeProgress", global::UnityEngine.Mathf.FloorToInt(currentPrestigePoint), neededPrestigePoints);
			}
		}

		internal void SetReward(int grind, int xp, int additionalBuffGrind)
		{
			XPReward.text = xp.ToString();
			GrindReward.text = UIUtils.FormatLargeNumber(grind);
			SetBuffRewards(additionalBuffGrind);
		}

		internal void SetCharacter(global::Kampai.Game.View.DummyCharacterObject characterObject)
		{
			if (dummyCharacterObject != null)
			{
				dummyCharacterObject.RemoveCoroutine();
				global::UnityEngine.Object.Destroy(dummyCharacterObject.gameObject);
			}
			dummyCharacterObject = characterObject;
		}

		internal void SetPanelState(bool isPrestige, int prestigeLevel = 0, global::Kampai.Game.Prestige character = null, bool orderInstructionEnabled = false)
		{
			OrderInstruction.gameObject.SetActive(orderInstructionEnabled);
			TicketName.gameObject.SetActive(true);
			FTUEBacking.gameObject.SetActive(true);
			if (isPrestige)
			{
				string title;
				if (prestigeLevel > 0)
				{
					title = ((character == null) ? localizationService.GetString("RePrestigeText") : localizationService.GetString("RePrestigeText", localizationService.GetString(character.Definition.LocalizedKey)));
					PrestigeLevel.text = (prestigeLevel + 1).ToString();
				}
				else
				{
					title = ((character == null) ? localizationService.GetString("PrestigeText") : localizationService.GetString("PrestigeText", localizationService.GetString(character.Definition.LocalizedKey)));
					PrestigeLevel.text = (prestigeLevel + 1).ToString();
				}
				SetTitle(title);
			}
			OrderPanel.SetActive(!isPrestige);
			PrestigePanel.SetActive(isPrestige);
			ScrollWindow.gameObject.SetActive(!orderInstructionEnabled);
			RewardPanel.gameObject.SetActive(!orderInstructionEnabled);
			RequiredItemListBackground.SetActive(!orderInstructionEnabled);
		}

		internal global::Kampai.UI.View.OrderBoardRequiredItemView CreateRequiredItem(int index, uint itemQuantity, uint itemInInventory, global::UnityEngine.Sprite icon, global::UnityEngine.Sprite mask)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(requiredItemPrefab);
			global::Kampai.UI.View.OrderBoardRequiredItemView component = gameObject.GetComponent<global::Kampai.UI.View.OrderBoardRequiredItemView>();
			component.transform.SetParent(ScrollWindow, false);
			int num = index / 2;
			int num2 = index % 2;
			global::UnityEngine.RectTransform rectTransform = component.transform as global::UnityEngine.RectTransform;
			rectTransform.anchorMin = new global::UnityEngine.Vector2(0.5f * (float)num2, 0f);
			rectTransform.anchorMax = new global::UnityEngine.Vector2(0.5f * (float)(num2 + 1), 0f);
			rectTransform.offsetMin = new global::UnityEngine.Vector2(0f, height * (float)count - height * (float)(num + 1));
			rectTransform.offsetMax = new global::UnityEngine.Vector2(0f, height * (float)count - height * (float)num);
			rectTransform.localPosition = new global::UnityEngine.Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, 0f);
			rectTransform.localScale = global::UnityEngine.Vector3.one;
			bool flag = itemQuantity <= itemInInventory;
			component.CheckMark.SetActive(flag);
			component.XMark.SetActive(!flag);
			component.ItemCount.text = string.Format("{0}/{1}", itemInInventory, itemQuantity);
			component.ItemIcon.sprite = icon;
			component.ItemIcon.maskSprite = mask;
			component.playerHasEnoughItems = flag;
			itemList.Add(component);
			return component;
		}

		internal void SetupItemCount(int count)
		{
			if (itemList.Count != 0)
			{
				foreach (global::Kampai.UI.View.OrderBoardRequiredItemView item in itemList)
				{
					global::UnityEngine.Object.Destroy(item.gameObject);
				}
				itemList.Clear();
			}
			this.count = count;
			ScrollWindow.offsetMin = new global::UnityEngine.Vector2(0f, (0f - height) * (float)count);
			ScrollWindow.offsetMax = new global::UnityEngine.Vector2(0f, 0f);
			if (count < 5)
			{
				ScrollView.GetComponent<global::UnityEngine.UI.ScrollRect>().enabled = false;
			}
			else
			{
				ScrollView.GetComponent<global::UnityEngine.UI.ScrollRect>().enabled = true;
			}
		}

		private void SetBuffRewards(int additionalGrind)
		{
			bool flag = additionalGrind > 0;
			BuffRewardPanel.SetActive(flag);
			SetBuffRewardsPanelGlow(flag);
			if (flag)
			{
				BuffRewardAmt.text = additionalGrind.ToString();
			}
		}

		internal void SetBuffRewardsPanelGlow(bool show)
		{
			RewardsPanelOutline.SetActive(show);
		}

		internal void ActivateBuffIcons(bool newShouldShowBuff, float modifier)
		{
			if (shouldShowBuff != newShouldShowBuff)
			{
				shouldShowBuff = newShouldShowBuff;
				BuffInfoGroup.SetActive(shouldShowBuff);
				if (shouldShowBuff)
				{
					BuffMultiplierAmt.text = localizationService.GetString("partyBuffMultiplier", modifier);
				}
			}
		}

		internal void DeactivateAllBuffVisuals()
		{
			RewardsPanelOutline.SetActive(false);
			BuffInfoGroup.SetActive(false);
			BuffRewardPanel.SetActive(false);
			shouldShowBuff = false;
		}

		internal void toggleMinionSlot(bool active)
		{
			MinionSlot.transform.localScale = ((!active) ? global::UnityEngine.Vector3.zero : minionSlotScale);
		}
	}
}
