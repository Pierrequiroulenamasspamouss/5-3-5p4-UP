namespace Kampai.Game
{
	public class UnlockMinionsCommand : global::strange.extensions.command.impl.Command
	{
		private bool isLevelUnlock = true;

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.PhilBeginIntroLoopSignal beginIntroLoopSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnleashCharacterAtShoreSignal unleashCharacterAtShoreSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.PromptReceivedSignal promptReceivedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraZoomBeachSignal cameraZoomBeachSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockCharacterModel characterModel { get; set; }

		[Inject]
		public global::Kampai.Game.CreateNamedCharacterViewSignal createCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CreateMinionSignal createMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CreateInnerTubeSignal createInnerTubeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.EnablePartyFunMeterSignal enablePartyFunMeter { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoPanCompleteSignal cameraAutoPanCompleteSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject namedCharacterManagerView { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Game.GetNewQuestSignal getNewQuestSignal { get; set; }

		public override void Execute()
		{
			if (characterModel.characterUnlocks.Count == 0 && characterModel.minionUnlocks.Count == 0)
			{
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.Character> characterList;
			if (characterModel.characterUnlocks.Count > 0)
			{
				characterList = new global::System.Collections.Generic.List<global::Kampai.Game.Character>(characterModel.characterUnlocks);
				characterModel.characterUnlocks.Clear();
			}
			else
			{
				characterList = new global::System.Collections.Generic.List<global::Kampai.Game.Character>(characterModel.minionUnlocks);
				characterModel.minionUnlocks.Clear();
			}
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			bool waitForCamera = false;
			string key = string.Empty;
			if (quantity == 0)
			{
				key = string.Format("AnnounceMinionSet{0}", 1);
			}
			SetupCharacters(characterList, ref key, ref waitForCamera);
			bool showDialog = !string.IsNullOrEmpty(key);
			bool flag = playerService.IsMinionPartyUnlocked();
			characterModel.stuartFirstTimeHonor = IsFirstStuartHonorGuestParty(characterList);
			if (!flag)
			{
				cameraZoomBeachSignal.Dispatch();
			}
			if (waitForCamera)
			{
				cameraAutoPanCompleteSignal.AddOnce(delegate
				{
					enablePartyFunMeter.Dispatch(false);
					BeginIntroLoop(showDialog, key, characterList);
					HandlePrompt(showDialog, characterList);
					StuartHonorDialog();
				});
			}
			else
			{
				enablePartyFunMeter.Dispatch(false);
				BeginIntroLoop(showDialog, key, characterList);
				HandlePrompt(showDialog, characterList);
				StuartHonorDialog();
			}
		}

		private bool IsFirstStuartHonorGuestParty(global::System.Collections.Generic.IList<global::Kampai.Game.Character> characterList)
		{
			global::Kampai.Game.StuartCharacter firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StuartCharacter>(70001);
			if (firstInstanceByDefinitionId == null)
			{
				return false;
			}
			if (firstInstanceByDefinitionId.WasHonorGuest)
			{
				return false;
			}
			foreach (global::Kampai.Game.Character character in characterList)
			{
				if (character is global::Kampai.Game.StuartCharacter)
				{
					return false;
				}
			}
			firstInstanceByDefinitionId.WasHonorGuest = true;
			return true;
		}

		private void StuartHonorDialog()
		{
			if (!characterModel.stuartFirstTimeHonor)
			{
				return;
			}
			global::System.Action<int, int> PrestigeDialog = delegate
			{
				characterModel.stuartFirstTimeHonor = false;
				if (characterModel.characterUnlocks.Count == 0 && characterModel.minionUnlocks.Count == 0)
				{
					foreach (global::Kampai.Game.QuestDialogSetting item in characterModel.dialogQueue)
					{
						base.injectionBinder.GetInstance<global::Kampai.Game.ShowDialogSignal>().Dispatch("AlertPrePrestige", item, new global::Kampai.Util.Tuple<int, int>(0, 0));
					}
					characterModel.dialogQueue.Clear();
				}
			};
			global::System.Action<bool> action = delegate
			{
				global::Kampai.Game.QuestDialogSetting type = new global::Kampai.Game.QuestDialogSetting
				{
					type = global::Kampai.UI.View.QuestDialogType.MINIONREWARD
				};
				global::Kampai.Util.Tuple<int, int> type2 = new global::Kampai.Util.Tuple<int, int>(-1, -1);
				base.injectionBinder.GetInstance<global::Kampai.Game.ShowDialogSignal>().Dispatch("GOHStuartFirstTime", type, type2);
				promptReceivedSignal.AddOnce(PrestigeDialog);
			};
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			if (minionPartyInstance.IsPartyHappening)
			{
				base.injectionBinder.GetInstance<global::Kampai.Game.EndMinionPartySignal>().AddOnce(action);
			}
			else
			{
				action(true);
			}
		}

		private void SetupCharacters(global::System.Collections.Generic.IList<global::Kampai.Game.Character> characterList, ref string key, ref bool waitForCamera)
		{
			foreach (global::Kampai.Game.Character character in characterList)
			{
				global::Kampai.Game.Minion minion = character as global::Kampai.Game.Minion;
				global::Kampai.Game.NamedCharacter namedCharacter = character as global::Kampai.Game.NamedCharacter;
				global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = null;
				global::Kampai.Game.Prestige prestige = null;
				if (minion != null)
				{
					global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
					if (minionPartyInstance.IsPartyHappening)
					{
						minion.IsInMinionParty = true;
					}
					prestige = prestigeService.GetPrestigeFromMinionInstance(minion);
					createMinionSignal.Dispatch(minion);
					if (prestige != null)
					{
						global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
						global::Kampai.Game.View.MinionObject minionObject = component.Get(minion.ID);
						minionObject.EnableRenderers(false);
						waitForCamera = true;
						isLevelUnlock = false;
						key = string.Format("AnnounceMinion{0}", prestige.Definition.LocalizedKey);
					}
				}
				else if (namedCharacter != null)
				{
					createCharacterSignal.Dispatch(namedCharacter);
					isLevelUnlock = false;
					prestige = prestigeService.GetPrestigeFromMinionInstance(namedCharacter);
					key = string.Format("AnnounceCharacter{0}", character.Name);
					namedCharacterObject = namedCharacterManagerView.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>().Get(namedCharacter.ID);
				}
				if (prestige != null)
				{
					if (namedCharacterObject != null)
					{
						namedCharacterObject.EnableRenderers(false);
						waitForCamera = true;
					}
					uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShowBuddyWelcomePanelUISignal>().Dispatch(new global::Kampai.Util.Boxed<global::Kampai.Game.Prestige>(prestige), global::Kampai.UI.View.CharacterWelcomeState.Welcome, 0);
				}
				base.injectionBinder.GetInstance<global::Kampai.Game.UnveilCharacterSignal>().Dispatch(character);
			}
		}

		private void HandlePrompt(bool showDialog, global::System.Collections.Generic.IList<global::Kampai.Game.Character> characterList)
		{
			global::System.Action<int, int> action = delegate
			{
				foreach (global::Kampai.Game.Character character in characterList)
				{
					unleashCharacterAtShoreSignal.Dispatch(character, characterModel.routeIndex);
				}
				if (!isLevelUnlock)
				{
					characterModel.routeIndex = -1;
				}
				base.injectionBinder.GetInstance<global::Kampai.Common.DeselectAllMinionsSignal>().Dispatch();
			};
			if (showDialog)
			{
				promptReceivedSignal.AddOnce(action);
			}
			else
			{
				action(0, 0);
			}
		}

		private void BeginIntroLoop(bool showDialog, string key, global::System.Collections.Generic.IList<global::Kampai.Game.Character> characterList)
		{
			global::Kampai.Game.QuestDialogSetting questDialogSetting = new global::Kampai.Game.QuestDialogSetting();
			questDialogSetting.type = global::Kampai.UI.View.QuestDialogType.MINIONREWARD;
			global::Kampai.Game.QuestDialogSetting type = questDialogSetting;
			global::Kampai.Util.Tuple<int, int> type2 = new global::Kampai.Util.Tuple<int, int>(-1, -1);
			if (showDialog)
			{
				base.injectionBinder.GetInstance<global::Kampai.Game.ShowDialogSignal>().Dispatch(key, type, type2);
			}
			bool type3 = false;
			int num = 0;
			int num2 = 3;
			foreach (global::Kampai.Game.Character character in characterList)
			{
				global::Kampai.Game.Minion minion = character as global::Kampai.Game.Minion;
				if (isLevelUnlock && num < num2)
				{
					createInnerTubeSignal.Dispatch(num);
				}
				if (character is global::Kampai.Game.NamedCharacter)
				{
					type3 = true;
					global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = namedCharacterManagerView.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>().Get(character.ID);
					if (namedCharacterObject != null)
					{
						namedCharacterObject.EnableRenderers(true);
					}
				}
				else if (minion != null && minion.HasPrestige)
				{
					type3 = true;
					global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
					global::Kampai.Game.View.MinionObject minionObject = component.Get(character.ID);
					minionObject.EnableRenderers(true);
				}
				base.injectionBinder.GetInstance<global::Kampai.Game.BeginCharacterIntroLoopSignal>().Dispatch(character);
				num++;
			}
			if (showDialog)
			{
				beginIntroLoopSignal.Dispatch(type3);
			}
		}
	}
}
