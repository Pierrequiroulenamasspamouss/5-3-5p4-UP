namespace Kampai.UI.View
{
	public class GOH_InfoPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::Kampai.Game.PrestigeDefinition prestigeDef;

		private global::Kampai.Game.BuffDefinition buffDefinition;

		private global::Kampai.Game.GuestOfHonorDefinition GOHDef;

		[Inject]
		public global::Kampai.UI.View.GOH_InfoPanelView view { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal toggleCharacterAudioSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.GOHCardClickedSignal gohCardClickedSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestOfHonorService { get; set; }

		public override void OnRegister()
		{
			view.cooldownRushButton.ClickedSignal.AddListener(RushCooldown);
			view.gohPanelButton.ClickedSignal.AddListener(ClickedCharacterPanel);
			gohCardClickedSignal.AddListener(GOHCardClicked);
			SetUpCharacterInfo();
			SetBuffInfo();
			DetermineAvailability();
			view.Init(localizationService.GetString(prestigeDef.LocalizedKey), gohCardClickedSignal);
			if (view.initiallySelected)
			{
				toggleCharacterAudioSignal.Dispatch(false, view.minionSlot.transform);
				view.RegisterClicked();
			}
		}

		public override void OnRemove()
		{
			view.cooldownRushButton.ClickedSignal.RemoveListener(RushCooldown);
			gohCardClickedSignal.RemoveListener(GOHCardClicked);
			view.gohPanelButton.ClickedSignal.AddListener(ClickedCharacterPanel);
		}

		private void SetUpCharacterInfo()
		{
			prestigeDef = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(view.prestigeDefID);
			GOHDef = definitionService.Get<global::Kampai.Game.GuestOfHonorDefinition>(prestigeDef.GuestOfHonorDefinitionID);
			buffDefinition = definitionService.Get<global::Kampai.Game.BuffDefinition>(GOHDef.buffDefinitionIDs[0]);
		}

		private void SetBuffInfo()
		{
			float buffMultiplierForPrestige = guestOfHonorService.GetBuffMultiplierForPrestige(prestigeDef.ID);
			string currentModifier = localizationService.GetString("partyBuffMultiplier", buffMultiplierForPrestige);
			global::UnityEngine.Sprite buffMaskIcon = UIUtils.LoadSpriteFromPath(buffDefinition.buffSimpleMask);
			string duration = UIUtils.FormatTime(guestOfHonorService.GetBuffDurationForSingleGuestOfHonorOnNextLevel(GOHDef), localizationService);
			view.SetBuffInfo(currentModifier, buffMaskIcon, duration);
		}

		private void DetermineAvailability()
		{
			if (view.isLocked)
			{
				global::UnityEngine.Sprite characterImage = null;
				global::UnityEngine.Sprite characterMask = null;
				prestigeService.GetCharacterImageBasedOnMood(prestigeDef, global::Kampai.Game.CharacterImageType.BigAvatarIcon, out characterImage, out characterMask);
				if (guestOfHonorService.ShouldDisplayUnlockAtLevelText(prestigeDef.PreUnlockLevel, (uint)prestigeDef.ID))
				{
					view.SetCharacterLocked(characterMask, localizationService.GetString("InsufficientLevel", prestigeDef.PreUnlockLevel));
				}
				else
				{
					view.SetCharacterLocked(characterMask, localizationService.GetString("GOHUnlockWithOrders"));
				}
				return;
			}
			view.CreateAnimatedCharacter(fancyUIService);
			string availabilityText = localizationService.GetString(buffDefinition.LocalizedKey);
			view.SetAvailabilityText(availabilityText);
			if (view.cooldown != 0)
			{
				string cooldownText = localizationService.GetString("GOHPartiesNeeded*", view.cooldown);
				global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(prestigeDef.ID);
				string rushCost = guestOfHonorService.GetRushCostForPartyCoolDown(prestige.ID).ToString();
				view.SetCharacterInCooldown(cooldownText, rushCost);
			}
			else
			{
				view.SetCharacterAvailable();
			}
		}

		private void RushCooldown()
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			if (view.cooldownRushButton.isDoubleConfirmed())
			{
				view.RegisterClicked();
				view.rushCallBack.Dispatch();
			}
		}

		private void ClickedCharacterPanel()
		{
			soundFXSignal.Dispatch("Play_button_click_01");
			toggleCharacterAudioSignal.Dispatch(false, view.minionSlot.transform);
			view.RegisterClicked();
		}

		private void GOHCardClicked(int index, bool avail)
		{
			view.IndicateSelected(index == view.myIndex);
		}
	}
}
