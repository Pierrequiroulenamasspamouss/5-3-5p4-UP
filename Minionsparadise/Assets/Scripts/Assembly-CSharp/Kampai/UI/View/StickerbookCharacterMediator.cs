namespace Kampai.UI.View
{
	public class StickerbookCharacterMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.StickerbookCharacterView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.CharacterClickedSignal characterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RequestCharacterSelectionSignal requestSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal toggleCharacterAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		public override void OnRegister()
		{
			view.Init(GetNumNewStickers(), fancyUIService, prestigeService, localService, definitionService);
			view.character.ClickedSignal.AddListener(CharacterClicked);
			view.lockedCharacter.ClickedSignal.AddListener(LockedClicked);
			view.limitedEvent.ClickedSignal.AddListener(LimitedClicked);
			characterSignal.AddListener(OnCharacterClicked);
			requestSignal.Dispatch();
		}

		public override void OnRemove()
		{
			view.character.ClickedSignal.RemoveListener(CharacterClicked);
			view.lockedCharacter.ClickedSignal.RemoveListener(LockedClicked);
			view.limitedEvent.ClickedSignal.RemoveListener(LimitedClicked);
			characterSignal.RemoveListener(OnCharacterClicked);
		}

		private int GetNumNewStickers()
		{
			int num = 0;
			foreach (global::Kampai.Game.Sticker item in playerService.GetInstancesByType<global::Kampai.Game.Sticker>())
			{
				if (view.isLimited)
				{
					if (item.isNew && item.Definition.IsLimitedTime)
					{
						num++;
					}
				}
				else if (item.isNew && !item.Definition.IsLimitedTime && item.Definition.CharacterID == view.prestigeID)
				{
					num++;
				}
			}
			return num;
		}

		private void OnCharacterClicked(int id, bool isLimited)
		{
			view.OnCharacterClicked(id, isLimited);
		}

		private void CharacterClicked()
		{
			view.UpdateBadge();
			toggleCharacterAudioSignal.Dispatch(false, view.MinionSlot.transform);
			characterSignal.Dispatch(view.prestigeID, false);
		}

		private void LockedClicked()
		{
			playSFXSignal.Dispatch("Play_action_locked_01");
		}

		private void LimitedClicked()
		{
			view.UpdateBadge();
			characterSignal.Dispatch(view.limitedID, true);
		}
	}
}
