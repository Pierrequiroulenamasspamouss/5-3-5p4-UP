namespace Kampai.UI.View
{
	public class GuestOfHonorSelectionView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.RectTransform characterPanel;

		public float padding = 10f;

		private global::Kampai.UI.View.GOH_InfoPanelView[] characterViewArray;

		public global::UnityEngine.UI.Text titleDescription;

		public global::UnityEngine.UI.Button startButton;

		private global::UnityEngine.GameObject GOHCard_prefab;

		private float GOHCard_width;

		private global::System.Collections.Generic.List<int> characterList;

		internal int characterListCount;

		internal int currentCharacterIndex;

		internal int unlockedCharacterCount;

		private global::Kampai.Game.IPrestigeService prestigeService;

		private global::Kampai.Game.IDefinitionService definitionService;

		private global::Kampai.Game.IPlayerService playerService;

		private global::Kampai.Game.IGuestOfHonorService guestOfHonorService;

		private global::UnityEngine.UI.ScrollRect characterPanelScroll;

		private float desiredPanelPosition = 0.5f;

		private bool characterScrollLocked = true;

		public global::strange.extensions.signal.impl.Signal rushGOHCooldown_Callback = new global::strange.extensions.signal.impl.Signal();

		public void Init(global::Kampai.Game.IPrestigeService prestigeService, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService, global::Kampai.Game.IGuestOfHonorService guestOfHonorService)
		{
			base.Init();
			this.prestigeService = prestigeService;
			this.definitionService = definitionService;
			this.playerService = playerService;
			this.guestOfHonorService = guestOfHonorService;
			resetInternalVariables();
			SetupCharacterList(this.guestOfHonorService.GetAllGOHStates());
			if (unlockedCharacterCount == 1)
			{
				global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
				list.Add(characterList[0]);
				characterList = list;
				base.gameObject.GetComponentInChildren<global::UnityEngine.UI.ScrollRect>().enabled = false;
			}
			GOHCard_prefab = global::Kampai.Util.KampaiResources.Load("cmp_GuestOfHonor_Info") as global::UnityEngine.GameObject;
			GOHCard_width = (GOHCard_prefab.transform as global::UnityEngine.RectTransform).sizeDelta.x;
			characterPanelScroll = base.gameObject.GetComponentInChildren<global::UnityEngine.UI.ScrollRect>();
			characterListCount = characterList.Count;
			PopulateGOHScrollView(0);
			base.Open();
		}

		public override void FinishedOpening()
		{
			characterScrollLocked = false;
		}

		public void Update()
		{
			if (characterScrollLocked)
			{
				SetHorizontalScrollPosition(desiredPanelPosition);
			}
		}

		internal int GetCharacterPrestigeDefID(int index)
		{
			return characterList[index];
		}

		private void SetupCharacterList(global::System.Collections.Generic.Dictionary<int, bool> characters)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Prestige> list = new global::System.Collections.Generic.List<global::Kampai.Game.Prestige>();
			global::System.Collections.Generic.List<global::Kampai.Game.PrestigeDefinition> list2 = new global::System.Collections.Generic.List<global::Kampai.Game.PrestigeDefinition>();
			foreach (global::System.Collections.Generic.KeyValuePair<int, bool> character in characters)
			{
				if (character.Value)
				{
					list.Add(playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(character.Key));
				}
				else
				{
					list2.Add(definitionService.Get<global::Kampai.Game.PrestigeDefinition>(character.Key));
				}
			}
			list.Sort((global::Kampai.Game.Prestige x, global::Kampai.Game.Prestige y) => y.UTCTimeUnlocked.CompareTo(x.UTCTimeUnlocked));
			list2.Sort((global::Kampai.Game.PrestigeDefinition x, global::Kampai.Game.PrestigeDefinition y) => x.PreUnlockLevel.CompareTo(y.PreUnlockLevel));
			foreach (global::Kampai.Game.Prestige item in list)
			{
				characterList.Add(item.Definition.ID);
				unlockedCharacterCount++;
			}
			foreach (global::Kampai.Game.PrestigeDefinition item2 in list2)
			{
				characterList.Add(item2.ID);
			}
		}

		internal void PopulateGOHScrollView(int initiallySelectedIndex)
		{
			characterViewArray = new global::Kampai.UI.View.GOH_InfoPanelView[characterListCount];
			for (int i = 0; i < characterListCount; i++)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(GOHCard_prefab);
				global::Kampai.UI.View.GOH_InfoPanelView component = gameObject.GetComponent<global::Kampai.UI.View.GOH_InfoPanelView>();
				characterViewArray[i] = component;
				global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
				gameObject.transform.SetParent(characterPanel, false);
				int characterPrestigeDefID = GetCharacterPrestigeDefID(i);
				bool flag = i < unlockedCharacterCount;
				int currentCooldownCount = GetCurrentCooldownCount(characterPrestigeDefID);
				component.myIndex = i;
				component.prestigeDefID = characterList[i];
				component.isLocked = !flag;
				component.cooldown = currentCooldownCount;
				component.rushCallBack = rushGOHCooldown_Callback;
				component.initiallySelected = i == initiallySelectedIndex;
				rectTransform.offsetMin = new global::UnityEngine.Vector2(GOHCard_width * (float)i + padding * (float)i, 0f);
				rectTransform.offsetMax = new global::UnityEngine.Vector2(GOHCard_width * (float)(i + 1) + padding * (float)i, 0f);
			}
			characterPanel.sizeDelta = new global::UnityEngine.Vector2(GOHCard_width * (float)characterListCount + padding * (float)characterListCount, 0f);
			characterPanel.localPosition = new global::UnityEngine.Vector2(0f, characterPanel.localPosition.y);
			if (characterListCount > 1)
			{
				desiredPanelPosition = 0f;
			}
		}

		internal float GetHorizontalScrollPosition()
		{
			return characterPanelScroll.horizontalNormalizedPosition;
		}

		internal void SetHorizontalScrollPosition(float position)
		{
			if (characterPanelScroll != null)
			{
				characterPanelScroll.horizontalNormalizedPosition = position;
			}
		}

		internal int GetCurrentCooldownCount(int prestigeDefinitionID)
		{
			global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(prestigeDefinitionID, false);
			if (prestige != null)
			{
				return guestOfHonorService.GetPartyCooldownForPrestige(prestige.ID);
			}
			return -1;
		}

		internal void SetStartButtonUnlocked(bool unlocked)
		{
			startButton.interactable = unlocked;
		}

		internal void RushCurrentCharacterCooldown()
		{
			SetStartButtonUnlocked(true);
			characterViewArray[currentCharacterIndex].RushCharacterCooldown();
		}

		internal void Close()
		{
			removeCharacterView();
			base.Close();
		}

		internal void Hide()
		{
			removeCharacterView();
		}

		private void resetInternalVariables()
		{
			characterViewArray = new global::Kampai.UI.View.GOH_InfoPanelView[1];
			characterList = new global::System.Collections.Generic.List<int>();
			characterListCount = 0;
			currentCharacterIndex = -1;
			unlockedCharacterCount = 0;
		}

		private void removeCharacterView()
		{
			global::Kampai.UI.View.GOH_InfoPanelView[] array = characterViewArray;
			foreach (global::Kampai.UI.View.GOH_InfoPanelView gOH_InfoPanelView in array)
			{
				gOH_InfoPanelView.DestroyDummyObject();
				global::UnityEngine.Object.Destroy(gOH_InfoPanelView);
			}
		}
	}
}
