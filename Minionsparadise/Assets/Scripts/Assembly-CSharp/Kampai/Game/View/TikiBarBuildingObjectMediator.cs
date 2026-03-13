namespace Kampai.Game.View
{
	public class TikiBarBuildingObjectMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("TikiBarBuildingObjectMediator") as global::Kampai.Util.IKampaiLogger;

		private global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject, int> addToTikiBarSignal;

		[Inject]
		public global::Kampai.Game.View.TikiBarBuildingObjectView view { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreMinionAtTikiBarSignal restoreMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionChangeStateSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowQuestPanelSignal showPanelSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowQuestRewardSignal showQuestRewardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PathCharacterToTikiBarSignal pathCharacterToTikibarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TeleportCharacterToTikiBarSignal teleportCharacterToTikibarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnveilCharacterObjectSignal unveilCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BeginCharacterLoopAnimationSignal characterLoopAnimationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PopUnleashedCharacterToTikiBarSignal popUnleashedCharacterToTikibarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ReleaseMinionFromTikiBarSignal releaseMinionFromTikiBarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.NamedCharacterRemovedFromTikiBarSignal removedFromTikibarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EndCharacterIntroSignal endCharacterIntroSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CharacterIntroCompleteSignal introCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CharacterDrinkingCompleteSignal drinkingCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ToggleStickerbookGlowSignal glowSignal { get; set; }

		[Inject]
		public global::Kampai.Game.GetNewQuestSignal getNewQuestSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ToggleHitboxSignal toggleHitboxSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TikiBarSetAnimParamSignal setAnimParamSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TikiBarResetAnimParamSignal resetAnimParamSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockMinionsSignal unlockMinionsSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void OnRegister()
		{
			addToTikiBarSignal = new global::strange.extensions.signal.impl.Signal<global::Kampai.Game.View.CharacterObject, int>();
			unveilCharacterSignal.AddListener(UnveilCharacter);
			characterLoopAnimationSignal.AddListener(BeginCharacterIntroLoop);
			popUnleashedCharacterToTikibarSignal.AddListener(UnleashCharacterToTikiBar);
			pathCharacterToTikibarSignal.AddListener(PathCharacterToTikiBar);
			teleportCharacterToTikibarSignal.AddListener(TeleportCharacterToTikiBar);
			endCharacterIntroSignal.AddListener(EndCharacterIntro);
			drinkingCompleteSignal.AddListener(CharacterDrinkingComplete);
			releaseMinionFromTikiBarSignal.AddListener(ReleaseMinionFromTikiBar);
			restoreMinionSignal.AddListener(RestoreMinion);
			StartCoroutine(Init());
			toggleHitboxSignal.AddListener(ToggleHitbox);
			setAnimParamSignal.AddListener(PlayAnimation);
			glowSignal.AddListener(ToggleStickerbookGlow);
			addToTikiBarSignal.AddListener(AddCharacterToTikiBar);
			resetAnimParamSignal.AddListener(ResetAnimationParameters);
			view.SetupInjections(minionChangeStateSignal, removedFromTikibarSignal, introCompleteSignal, drinkingCompleteSignal);
		}

		public override void OnRemove()
		{
			unveilCharacterSignal.RemoveListener(UnveilCharacter);
			characterLoopAnimationSignal.RemoveListener(BeginCharacterIntroLoop);
			popUnleashedCharacterToTikibarSignal.RemoveListener(UnleashCharacterToTikiBar);
			pathCharacterToTikibarSignal.RemoveListener(PathCharacterToTikiBar);
			endCharacterIntroSignal.RemoveListener(EndCharacterIntro);
			drinkingCompleteSignal.RemoveListener(CharacterDrinkingComplete);
			teleportCharacterToTikibarSignal.RemoveListener(TeleportCharacterToTikiBar);
			releaseMinionFromTikiBarSignal.RemoveListener(ReleaseMinionFromTikiBar);
			restoreMinionSignal.RemoveListener(RestoreMinion);
			toggleHitboxSignal.RemoveListener(ToggleHitbox);
			setAnimParamSignal.RemoveListener(PlayAnimation);
			glowSignal.RemoveListener(ToggleStickerbookGlow);
			addToTikiBarSignal.RemoveListener(AddCharacterToTikiBar);
			resetAnimParamSignal.RemoveListener(ResetAnimationParameters);
		}

		private void ResetAnimationParameters(bool didSkip)
		{
			view.didSkipParty = didSkip;
			view.ResetAnimationParameters();
		}

		private void AddCharacterToTikiBar(global::Kampai.Game.View.CharacterObject characterObject, int routeIndex)
		{
			view.AddCharacterToBuildingActions(characterObject, playerService, routeIndex, prestigeService, getNewQuestSignal);
			if (routeIndex > 0)
			{
				unlockMinionsSignal.Dispatch();
			}
		}

		private void ToggleHitbox(global::Kampai.Game.BuildingZoomType zoomBuildingType, bool enable)
		{
			if (zoomBuildingType == global::Kampai.Game.BuildingZoomType.TIKIBAR)
			{
				view.ToggleHitbox(enable);
			}
		}

		private void ReleaseMinionFromTikiBar(global::Kampai.Game.Character character, bool forceRelease)
		{
			if (character is global::Kampai.Game.KevinCharacter || forceRelease)
			{
				CharacterDrinkingComplete(character.ID);
			}
			else
			{
				view.RemoveCharacterFromTikiBar(character.ID);
			}
		}

		private void UnveilCharacter(global::Kampai.Game.View.CharacterObject characterObject)
		{
			view.SetupCharacter(characterObject, playerService, prestigeService);
		}

		private void BeginCharacterIntroLoop(global::Kampai.Game.View.CharacterObject characterObject)
		{
			global::Kampai.Game.View.MinionObject minionObject = characterObject as global::Kampai.Game.View.MinionObject;
			if (!(minionObject != null) || minionObject.GetMinion() == null || minionObject.GetMinion().HasPrestige)
			{
				hideAllWayFindersSignal.Dispatch();
			}
			bool waitForLoop = playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) == 0 || minionObject == null || (minionObject != null && minionObject.GetMinion().HasPrestige);
			view.BeginCharacterIntroLoop(waitForLoop, characterObject);
		}

		private void UnleashCharacterToTikiBar(global::Kampai.Game.View.CharacterObject characterObject, int routeIndex)
		{
			global::Kampai.Game.View.MinionObject minionObject = characterObject as global::Kampai.Game.View.MinionObject;
			bool waitForLoop = playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID) == 0 || minionObject == null || (minionObject != null && minionObject.GetMinion().HasPrestige);
			view.BeginCharacterIntro(waitForLoop, characterObject, routeIndex);
			if (playerService.GetMinionCount() <= 4 && minionObject != null)
			{
				StartCoroutine(PanCameraToBeach());
			}
		}

		private global::System.Collections.IEnumerator PanCameraToBeach()
		{
			yield return new global::UnityEngine.WaitForSeconds(9.7f);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveToPositionSignal>().Dispatch(new global::UnityEngine.Vector3(140.2076f, 13.75177f, 158.1269f), 0.9557783f, false);
		}

		private void EndCharacterIntro(global::Kampai.Game.View.CharacterObject characterObject, int routeIndex)
		{
			view.EndCharacterIntro(characterObject, routeIndex);
		}

		private void CharacterDrinkingComplete(int instanceID)
		{
			view.UntrackChild(instanceID);
		}

		private void PathCharacterToTikiBar(global::Kampai.Game.View.CharacterObject characterObject, global::Kampai.Game.View.RouteInstructions ri, int routeIndex)
		{
			view.PathCharacterToTikiBar(characterObject, ri.Path, ri.Rotation, routeIndex, addToTikiBarSignal);
		}

		private void TeleportCharacterToTikiBar(global::Kampai.Game.View.CharacterObject characterObject, int routeIndex)
		{
			if (!view.ContainsCharacter(characterObject.ID) && routeIndex <= 2)
			{
				if (characterObject.ID == 78)
				{
					global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(40000);
					prestigeService.ChangeToPrestigeState(prestige, global::Kampai.Game.PrestigeState.Questing);
				}
				AddCharacterToTikiBar(characterObject, routeIndex);
			}
		}

		public void RestoreMinion(global::Kampai.Game.Character character)
		{
			global::Kampai.Game.Prestige prestigeFromMinionInstance = prestigeService.GetPrestigeFromMinionInstance(character);
			if (prestigeFromMinionInstance == null)
			{
				logger.Error("RestoreMinion: Prestige == null for minion ID: {0}", character.ID);
				return;
			}
			int iD = prestigeFromMinionInstance.Definition.ID;
			int minionSlotIndex = view.tikiBar.GetMinionSlotIndex(iD);
			if (minionSlotIndex != -1)
			{
				prestigeService.AddMinionToTikiBarSlot(character, minionSlotIndex, view.tikiBar);
			}
		}

		private global::System.Collections.IEnumerator Init()
		{
			yield return null;
			if (view.tikiBar.minionQueue.Count == 0)
			{
				view.tikiBar.minionQueue.Add(40000);
				view.tikiBar.minionQueue.Add(-1);
				view.tikiBar.minionQueue.Add(-1);
			}
		}

		public void MinionButtonClicked(int questInstanceID)
		{
			global::Kampai.Game.Quest byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Quest>(questInstanceID);
			switch (byInstanceId.state)
			{
			case global::Kampai.Game.QuestState.Notstarted:
			case global::Kampai.Game.QuestState.RunningStartScript:
			case global::Kampai.Game.QuestState.RunningTasks:
			case global::Kampai.Game.QuestState.RunningCompleteScript:
				showPanelSignal.Dispatch(questInstanceID);
				break;
			case global::Kampai.Game.QuestState.Harvestable:
				showQuestRewardSignal.Dispatch(questInstanceID);
				break;
			}
		}

		private void PlayAnimation(string animation, global::System.Type type, object obj)
		{
			view.PlayAnimation(animation, type, obj);
		}

		private void ToggleStickerbookGlow(bool enable)
		{
			view.ToggleStickerbookGlow(enable);
		}
	}
}
