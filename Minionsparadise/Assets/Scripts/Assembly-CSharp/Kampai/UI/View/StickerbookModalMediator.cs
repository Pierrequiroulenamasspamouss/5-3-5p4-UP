namespace Kampai.UI.View
{
	public class StickerbookModalMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		[Inject]
		public global::Kampai.UI.View.StickerbookModalView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIAddedSignal uiAddedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIRemovedSignal uiRemovedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CharacterClickedSignal characterSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideItemPopupSignal hidePopupSignal { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistence { get; set; }

		[Inject]
		public global::Kampai.UI.View.RequestCharacterSelectionSignal requestSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal toggleCharacterAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.FadeBackgroundAudioSignal fadeBackgroundAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomCameraModel { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			soundFXSignal.Dispatch("Play_book_open_01");
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.ToggleStickerbookGlowSignal>().Dispatch(false);
			if (localPersistence.HasKeyPlayer("StickerbookGlow"))
			{
				localPersistence.DeleteKeyPlayer("StickerbookGlow");
			}
			view.Init(prestigeService.GetPrestigedCharacterStates(), definitionService, playerService);
			view.OnMenuClose.AddListener(OnMenuClose);
			characterSignal.AddListener(CharacterClicked);
			uiAddedSignal.Dispatch(view.gameObject, OnHardwareBackButton);
			toggleCharacterAudioSignal.Dispatch(false, view.stickerPanel.transform);
			requestSignal.AddListener(SelectInitialCharacter);
			FadeBackgroundAudio(true);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			view.OnMenuClose.RemoveListener(OnMenuClose);
			characterSignal.RemoveListener(CharacterClicked);
			uiRemovedSignal.Dispatch(view.gameObject);
			requestSignal.RemoveListener(SelectInitialCharacter);
			FadeBackgroundAudio(false);
		}

		private void SelectInitialCharacter()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Sticker> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Sticker>();
			if (instancesByType.Count > 0)
			{
				global::Kampai.Game.StickerDefinition definition = instancesByType[instancesByType.Count - 1].Definition;
				if (definition.IsLimitedTime)
				{
					characterSignal.Dispatch(definition.EventDefinitionID, true);
				}
				else
				{
					characterSignal.Dispatch(definition.CharacterID, false);
				}
			}
			else
			{
				characterSignal.Dispatch(40000, false);
			}
		}

		private void CharacterClicked(int relevantID, bool isLimited)
		{
			if (view.lastSelectedID == relevantID)
			{
				return;
			}
			view.lastSelectedID = relevantID;
			soundFXSignal.Dispatch("Play_button_click_01");
			global::System.Collections.Generic.List<global::Kampai.Game.Sticker> list = new global::System.Collections.Generic.List<global::Kampai.Game.Sticker>();
			global::System.Collections.Generic.List<global::Kampai.Game.StickerDefinition> list2 = new global::System.Collections.Generic.List<global::Kampai.Game.StickerDefinition>();
			if (!isLimited)
			{
				foreach (global::Kampai.Game.StickerDefinition item in definitionService.GetAll<global::Kampai.Game.StickerDefinition>())
				{
					if (item.IsLimitedTime || item.CharacterID != relevantID)
					{
						continue;
					}
					global::Kampai.Game.Sticker firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Sticker>(item.ID);
					if (firstInstanceByDefinitionId != null)
					{
						if (firstInstanceByDefinitionId.isNew)
						{
							firstInstanceByDefinitionId.isNew = false;
						}
						list.Add(firstInstanceByDefinitionId);
					}
					else if (!item.deprecated && !item.IsLimitedTime)
					{
						list2.Add(item);
					}
				}
			}
			else
			{
				foreach (global::Kampai.Game.StickerDefinition item2 in definitionService.GetAll<global::Kampai.Game.StickerDefinition>())
				{
					if (!item2.IsLimitedTime)
					{
						continue;
					}
					global::Kampai.Game.Sticker firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Sticker>(item2.ID);
					if (firstInstanceByDefinitionId2 != null)
					{
						if (firstInstanceByDefinitionId2.isNew)
						{
							firstInstanceByDefinitionId2.isNew = false;
						}
						list.Add(firstInstanceByDefinitionId2);
					}
					else if (!item2.deprecated)
					{
						global::Kampai.Game.SpecialEventItem firstInstanceByDefinitionId3 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.SpecialEventItem>(item2.EventDefinitionID);
						if (firstInstanceByDefinitionId3 != null)
						{
							list2.Add(item2);
						}
					}
				}
			}
			global::System.Collections.Generic.List<int> stickerList = SortStickers(list, list2);
			string stickerPageName = GetStickerPageName(isLimited, relevantID);
			view.PopulateStickersForCurrentCharacter(list.Count, stickerList);
			view.SetCharacterStrings(stickerPageName);
		}

		private global::System.Collections.Generic.List<int> SortStickers(global::System.Collections.Generic.List<global::Kampai.Game.Sticker> unlockedStickers, global::System.Collections.Generic.List<global::Kampai.Game.StickerDefinition> lockedStickers)
		{
			unlockedStickers.Sort((global::Kampai.Game.Sticker x, global::Kampai.Game.Sticker y) => y.UTCTimeEarned.CompareTo(x.UTCTimeEarned));
			global::System.Collections.Generic.List<int> list = new global::System.Collections.Generic.List<int>();
			foreach (global::Kampai.Game.Sticker unlockedSticker in unlockedStickers)
			{
				list.Add(unlockedSticker.Definition.ID);
			}
			foreach (global::Kampai.Game.StickerDefinition lockedSticker in lockedStickers)
			{
				list.Add(lockedSticker.ID);
			}
			return list;
		}

		private string GetStickerPageName(bool isLimited, int relevantID)
		{
			if (!isLimited)
			{
				return localService.GetString(definitionService.Get<global::Kampai.Game.PrestigeDefinition>(relevantID).CollectionTitle);
			}
			return localService.GetString("StickerbookEventName");
		}

		private void CloseButton()
		{
			soundFXSignal.Dispatch("Play_book_close_01");
			toggleCharacterAudioSignal.Dispatch(true, null);
			view.Close();
		}

		private void OnHardwareBackButton()
		{
			CloseButton();
		}

		private void OnMenuClose()
		{
			hidePopupSignal.Dispatch();
			hideSkrim.Dispatch("StickerBookSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_Stickerbook");
		}

		private void FadeBackgroundAudio(bool fade)
		{
			if (zoomCameraModel.ZoomedIn && !zoomCameraModel.ZoomInProgress)
			{
				fadeBackgroundAudioSignal.Dispatch(fade, "Play_tikiBar_snapshotDuck_01");
			}
		}
	}
}
