namespace Kampai.Game
{
	public class CharacterIntroCompleteCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.View.CharacterObject characterObject { get; set; }

		[Inject]
		public int routeIndex { get; set; }

		[Inject]
		public global::Kampai.UI.View.EnablePartyFunMeterSignal enablePartyFunMeterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EndCharacterIntroSignal endCharacterIntroSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ConfirmStartNewMinionPartySignal confirmStartNewMinionPartySignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayLevelUpRewardSignal displayLevelUpRewardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDialogSignal showDialogSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionStateChange { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToPositionSignal cameraAutoMoveToPositionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SetKevinAnimatorCullingModeSignal setKevinAnimatorCullingModeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.FrolicSignal frolicSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockCharacterModel characterModel { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		public override void Execute()
		{
			enablePartyFunMeterSignal.Dispatch(true);
			endCharacterIntroSignal.Dispatch(characterObject, routeIndex);
			if (!playerService.GetMinionPartyInstance().IsPartyHappening)
			{
				showAllWayFindersSignal.Dispatch();
			}
			global::Kampai.Game.Character byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Character>(characterObject.ID);
			global::Kampai.Game.Definition definition = byInstanceId.Definition;
			if (definition.ID == 70003)
			{
				HandleSpecialCharacter(byInstanceId, definition);
			}
			else if (routeIndex >= 0)
			{
				global::Kampai.Game.TikiBarBuilding byInstanceId2 = playerService.GetByInstanceId<global::Kampai.Game.TikiBarBuilding>(313);
				prestigeService.AddMinionToTikiBarSlot(byInstanceId, routeIndex, byInstanceId2);
				if (characterModel.minionUnlocks.Count > 0)
				{
					displayLevelUpRewardSignal.Dispatch(true);
				}
			}
			UpdateMinionParty();
		}

		private void HandleSpecialCharacter(global::Kampai.Game.Character character, global::Kampai.Game.Definition charDef)
		{
			global::Kampai.Game.Prestige prestigeFromMinionInstance = prestigeService.GetPrestigeFromMinionInstance(character);
			bool flag = false;
			if (prestigeFromMinionInstance != null)
			{
				global::Kampai.Game.Minion minion = character as global::Kampai.Game.Minion;
				global::Kampai.Game.PrestigeState targetState = global::Kampai.Game.PrestigeState.Questing;
				global::Kampai.Game.MinionState param = global::Kampai.Game.MinionState.Questing;
				prestigeService.ChangeToPrestigeState(prestigeFromMinionInstance, targetState);
				if (minion != null)
				{
					minion.PrestigeId = prestigeFromMinionInstance.ID;
					minionStateChange.Dispatch(minion.ID, param);
				}
			}
			global::Kampai.Game.KevinCharacterDefinition kevinCharacterDefinition = charDef as global::Kampai.Game.KevinCharacterDefinition;
			if (kevinCharacterDefinition != null)
			{
				global::UnityEngine.Transform transform = characterObject.gameObject.transform;
				transform.position = (global::UnityEngine.Vector3)kevinCharacterDefinition.Location;
				transform.rotation = global::UnityEngine.Quaternion.Euler(kevinCharacterDefinition.RotationEulers);
				cameraAutoMoveToPositionSignal.Dispatch(transform.position, 0.9f, true);
				routineRunner.StartCoroutine(ToggleAnimatorMode());
				flag = true;
			}
			if (flag)
			{
				frolicSignal.Dispatch(character.ID);
			}
		}

		private void UpdateMinionParty()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			if (minionPartyInstance.CharacterUnlocking && minionPartyInstance.IsPartyReady)
			{
				confirmStartNewMinionPartySignal.Dispatch(true);
			}
			minionPartyInstance.CharacterUnlocking = false;
			if (minionPartyInstance.IsPartyHappening || characterModel.stuartFirstTimeHonor || characterModel.characterUnlocks.Count != 0 || characterModel.minionUnlocks.Count != 0)
			{
				return;
			}
			foreach (global::Kampai.Game.QuestDialogSetting item in characterModel.dialogQueue)
			{
				showDialogSignal.Dispatch("AlertPrePrestige", item, new global::Kampai.Util.Tuple<int, int>(0, 0));
			}
			characterModel.dialogQueue.Clear();
		}

		private global::System.Collections.IEnumerator ToggleAnimatorMode()
		{
			setKevinAnimatorCullingModeSignal.Dispatch(global::UnityEngine.AnimatorCullingMode.AlwaysAnimate);
			yield return new global::UnityEngine.WaitForSeconds(1f);
			setKevinAnimatorCullingModeSignal.Dispatch(global::UnityEngine.AnimatorCullingMode.CullUpdateTransforms);
		}
	}
}
