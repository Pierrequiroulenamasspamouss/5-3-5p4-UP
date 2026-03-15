namespace Kampai.UI.View
{
	public class StickerbookModalView : global::Kampai.UI.View.PopupMenuView
	{
		public global::UnityEngine.RectTransform characterPanel;

		public global::UnityEngine.RectTransform stickerPanel;

		public global::UnityEngine.UI.Text stickerTitle;

		public float padding = 10f;

		private global::System.Collections.Generic.List<int> characterList = new global::System.Collections.Generic.List<int>();

		private global::System.Collections.Generic.List<global::Kampai.UI.View.StickerbookCharacterView> characterViewList = new global::System.Collections.Generic.List<global::Kampai.UI.View.StickerbookCharacterView>();

		private global::System.Collections.Generic.List<global::UnityEngine.GameObject> stickerInstanceList = new global::System.Collections.Generic.List<global::UnityEngine.GameObject>();

		private global::System.Collections.Generic.List<int> charactersWithUnlockedStickers;

		private global::UnityEngine.GameObject stickerPackPrefab;

		private global::UnityEngine.GameObject stickerPrefab;

		private float characterWidth;

		private float stickerWidth;

		private bool firstTime = true;

		internal int lastSelectedID;

		private int unlockedCharacterCount;

		public void Init(global::System.Collections.Generic.Dictionary<int, bool> characters, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService)
		{
			SetupCharacterList(characters, definitionService, playerService);
			base.Init();
			CachePrefabs();
			PopulateCharacterScrollView(definitionService, playerService);
			base.Open();
		}

		internal void Close()
		{
			foreach (global::Kampai.UI.View.StickerbookCharacterView characterView in characterViewList)
			{
				characterView.RemoveCoroutine();
			}
			base.Close();
		}

		private void SetupCharacterList(global::System.Collections.Generic.Dictionary<int, bool> characters, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Prestige> list = new global::System.Collections.Generic.List<global::Kampai.Game.Prestige>();
			global::System.Collections.Generic.List<global::Kampai.Game.PrestigeDefinition> list2 = new global::System.Collections.Generic.List<global::Kampai.Game.PrestigeDefinition>();
			foreach (global::System.Collections.Generic.KeyValuePair<int, bool> character in characters)
			{
				if (character.Value)
				{
					global::Kampai.Game.Prestige firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(character.Key);
					if (firstInstanceByDefinitionId.Definition.StickerbookDisplayableType != global::Kampai.Game.StickerbookCharacterDisplayableType.Never && CharacterIsDisplayable(firstInstanceByDefinitionId, definitionService, playerService))
					{
						list.Add(firstInstanceByDefinitionId);
						unlockedCharacterCount++;
					}
				}
				else
				{
					global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(character.Key);
					if (prestigeDefinition.StickerbookDisplayableType == global::Kampai.Game.StickerbookCharacterDisplayableType.Always)
					{
						list2.Add(prestigeDefinition);
					}
				}
			}
			list.Sort((global::Kampai.Game.Prestige x, global::Kampai.Game.Prestige y) => y.UTCTimeUnlocked.CompareTo(x.UTCTimeUnlocked));
			list2.Sort((global::Kampai.Game.PrestigeDefinition x, global::Kampai.Game.PrestigeDefinition y) => x.PreUnlockLevel.CompareTo(y.PreUnlockLevel));
			foreach (global::Kampai.Game.Prestige item in list)
			{
				characterList.Add(item.Definition.ID);
			}
			foreach (global::Kampai.Game.PrestigeDefinition item2 in list2)
			{
				characterList.Add(item2.ID);
			}
		}

		private bool CharacterIsDisplayable(global::Kampai.Game.Prestige prestige, global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService)
		{
			if (prestige.Definition.StickerbookDisplayableType == global::Kampai.Game.StickerbookCharacterDisplayableType.Always)
			{
				return true;
			}
			if (charactersWithUnlockedStickers == null)
			{
				global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
				charactersWithUnlockedStickers = new global::System.Collections.Generic.List<int>();
				foreach (global::Kampai.Game.PrestigeDefinition item in definitionService.GetAll<global::Kampai.Game.PrestigeDefinition>())
				{
					if (item.StickerbookDisplayableType == global::Kampai.Game.StickerbookCharacterDisplayableType.OnlyWithUnlockedStickers)
					{
						list.Add(item.ID);
					}
				}
				foreach (global::Kampai.Game.StickerDefinition item2 in definitionService.GetAll<global::Kampai.Game.StickerDefinition>())
				{
					if (list.Contains(item2.CharacterID))
					{
						global::Kampai.Game.Sticker firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Sticker>(item2.ID);
						if (firstInstanceByDefinitionId != null)
						{
							charactersWithUnlockedStickers.Add(item2.CharacterID);
						}
					}
				}
			}
			if (charactersWithUnlockedStickers.Count == 0)
			{
				return false;
			}
			if (charactersWithUnlockedStickers.Contains(prestige.Definition.ID))
			{
				return true;
			}
			return false;
		}

		private void CachePrefabs()
		{
			stickerPackPrefab = global::Kampai.Util.KampaiResources.Load("cmp_StickerPackCharacters") as global::UnityEngine.GameObject;
			stickerPrefab = global::Kampai.Util.KampaiResources.Load("cmp_Sticker") as global::UnityEngine.GameObject;
			characterWidth = (stickerPackPrefab.transform as global::UnityEngine.RectTransform).sizeDelta.x;
			stickerWidth = (stickerPrefab.transform as global::UnityEngine.RectTransform).sizeDelta.x;
		}

		private void PopulateCharacterScrollView(global::Kampai.Game.IDefinitionService definitionService, global::Kampai.Game.IPlayerService playerService)
		{
			int num = 1;
			int count = characterList.Count;
			bool flag = false;
			bool flag2 = false;
			int num2 = 0;
			foreach (global::Kampai.Game.SpecialEventItemDefinition item in definitionService.GetAll<global::Kampai.Game.SpecialEventItemDefinition>())
			{
				global::Kampai.Game.SpecialEventItem firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.SpecialEventItem>(item.ID);
				if (item.IsActive && firstInstanceByDefinitionId != null && !firstInstanceByDefinitionId.HasEnded)
				{
					flag = true;
					break;
				}
			}
			foreach (global::Kampai.Game.SpecialEventItemDefinition item2 in definitionService.GetAll<global::Kampai.Game.SpecialEventItemDefinition>())
			{
				global::Kampai.Game.SpecialEventItem firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.SpecialEventItem>(item2.ID);
				if (firstInstanceByDefinitionId2 != null)
				{
					flag2 = true;
					break;
				}
			}
			if (flag)
			{
				num2 = 1;
				characterViewList.Add(CreateEventView(0));
			}
			for (int i = 0; i < count; i++)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(stickerPackPrefab);
				global::Kampai.UI.View.StickerbookCharacterView component = gameObject.GetComponent<global::Kampai.UI.View.StickerbookCharacterView>();
				global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
				gameObject.transform.SetParent(characterPanel, false);
				component.prestigeID = characterList[i];
				component.isLimited = false;
				if (i < unlockedCharacterCount)
				{
					component.isLocked = false;
				}
				else
				{
					component.isLocked = true;
				}
				rectTransform.offsetMin = new global::UnityEngine.Vector2(characterWidth * (float)(i + num2) + padding * (float)(i + num2), 0f);
				rectTransform.offsetMax = new global::UnityEngine.Vector2(characterWidth * (float)(i + num2 + 1) + padding * (float)(i + num2), 0f);
				characterViewList.Add(component);
			}
			if (!flag && flag2)
			{
				characterViewList.Add(CreateEventView(count));
			}
			characterPanel.sizeDelta = new global::UnityEngine.Vector2(characterWidth * (float)(count + num) + padding * (float)(count + num), 0f);
			characterPanel.localPosition = new global::UnityEngine.Vector2(0f, characterPanel.localPosition.y);
		}

		private global::Kampai.UI.View.StickerbookCharacterView CreateEventView(int index)
		{
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(stickerPackPrefab);
			global::Kampai.UI.View.StickerbookCharacterView component = gameObject.GetComponent<global::Kampai.UI.View.StickerbookCharacterView>();
			global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
			gameObject.transform.SetParent(characterPanel, false);
			component.character.gameObject.SetActive(false);
			component.limitedEvent.gameObject.SetActive(true);
			component.isLimited = true;
			rectTransform.offsetMin = new global::UnityEngine.Vector2(characterWidth * (float)index + padding * (float)index, 0f);
			rectTransform.offsetMax = new global::UnityEngine.Vector2(characterWidth * (float)(index + 1) + padding * (float)index, 0f);
			return component;
		}

		internal void PopulateStickersForCurrentCharacter(int unlockedStickerCount, global::System.Collections.Generic.List<int> stickerList)
		{
			CleanupExistingStickers();
			for (int i = 0; i < stickerList.Count; i++)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(stickerPrefab);
				global::Kampai.UI.View.StickerbookStickerView component = gameObject.GetComponent<global::Kampai.UI.View.StickerbookStickerView>();
				global::UnityEngine.RectTransform rectTransform = gameObject.transform as global::UnityEngine.RectTransform;
				gameObject.transform.SetParent(stickerPanel, false);
				if (i < unlockedStickerCount)
				{
					component.locked = false;
				}
				else
				{
					component.locked = true;
				}
				component.stickerDefinitionID = stickerList[i];
				rectTransform.offsetMin = new global::UnityEngine.Vector2(stickerWidth * (float)i, 0f);
				rectTransform.offsetMax = new global::UnityEngine.Vector2(stickerWidth * (float)(i + 1), 0f);
				stickerInstanceList.Add(gameObject);
			}
			stickerPanel.sizeDelta = new global::UnityEngine.Vector2(stickerWidth * (float)stickerList.Count, 0f);
			stickerPanel.localPosition = new global::UnityEngine.Vector2(0f, stickerPanel.localPosition.y);
		}

		private void CleanupExistingStickers()
		{
			foreach (global::UnityEngine.GameObject stickerInstance in stickerInstanceList)
			{
				global::UnityEngine.Object.Destroy(stickerInstance);
			}
		}

		internal void SetCharacterStrings(string characterCollection)
		{
			if (firstTime)
			{
				firstTime = false;
				stickerTitle.gameObject.SetActive(true);
			}
			stickerTitle.text = characterCollection;
		}
	}
}
