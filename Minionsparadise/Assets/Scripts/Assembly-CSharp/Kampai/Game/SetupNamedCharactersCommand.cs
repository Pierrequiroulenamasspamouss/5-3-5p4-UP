namespace Kampai.Game
{
	public class SetupNamedCharactersCommand : global::strange.extensions.command.impl.Command
	{
		private global::System.Collections.Generic.ICollection<global::Kampai.Game.NamedCharacter> namedCharacters;

		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupNamedCharactersCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService player { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.MinionPrestigeCompleteSignal minionPrestigeCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Main.AppStartCompleteSignal appStartCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RandomFlyOverCompleteSignal randomFlyOverCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreNamedCharacterSignal restoreNamedCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanCooldownAlertSignal displayMasterPlanCooldownAlertSignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.CreateVillainViewSignal createViewSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainGotoCabanaSignal gotoCabanaSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Util.TimeProfiler.StartSection("named characters");
			namedCharacters = player.GetInstancesByType<global::Kampai.Game.NamedCharacter>();
			foreach (global::Kampai.Game.NamedCharacter namedCharacter in namedCharacters)
			{
				global::Kampai.Game.NamedCharacterDefinition definition = namedCharacter.Definition;
				if (definition.Type == global::Kampai.Game.NamedCharacterType.SPECIAL_EVENT || definition.ID == 70004)
				{
					continue;
				}
				global::Kampai.Game.Prestige prestige = GetPrestige(namedCharacter);
				if (prestige == null)
				{
					continue;
				}
				if (definition.Type == global::Kampai.Game.NamedCharacterType.VILLAIN)
				{
					if (prestige.state == global::Kampai.Game.PrestigeState.Questing || prestige.state == global::Kampai.Game.PrestigeState.Taskable || prestige.CurrentPrestigeLevel > 0)
					{
						routineRunner.StartCoroutine(MoveVillainAfterFrame(namedCharacter));
					}
				}
				else
				{
					restoreNamedCharacterSignal.Dispatch(namedCharacter, prestige);
				}
			}
			global::Kampai.Game.OrderBoard firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.OrderBoard>(3022);
			if (firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.HarvestableCharacterDefinitionId != 0)
			{
				global::Kampai.Game.Prestige prestige2 = prestigeService.GetPrestige(firstInstanceByDefinitionId.HarvestableCharacterDefinitionId);
				prestigeService.PostOrderCompletion(prestige2);
			}
			global::Kampai.Util.TimeProfiler.EndSection("named characters");
			prestigeService.CheckCompletedPrestiges();
			CreateCharacters();
		}

		private global::System.Collections.IEnumerator MoveVillainAfterFrame(global::Kampai.Game.NamedCharacter villain)
		{
			yield return null;
			int cabanaDefinitionId = -1;
			switch (villain.Definition.ID)
			{
			case 70005:
				cabanaDefinitionId = 3042;
				break;
			case 70006:
				cabanaDefinitionId = 3043;
				break;
			case 70007:
				cabanaDefinitionId = 3044;
				break;
			}
			global::Kampai.Game.Building cabana = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(cabanaDefinitionId);
			createViewSignal.Dispatch(villain.ID);
			gotoCabanaSignal.Dispatch(villain.ID, cabana.ID);
		}

		private global::Kampai.Game.Prestige GetPrestige(global::Kampai.Game.NamedCharacter namedCharacter)
		{
			global::Kampai.Game.Prestige prestigeFromMinionInstance = prestigeService.GetPrestigeFromMinionInstance(namedCharacter);
			if (prestigeFromMinionInstance == null && !FixPrestige(namedCharacter))
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_RESTORE_NAMED_CHARACTER_NO_PRESTIGE);
			}
			return prestigeFromMinionInstance;
		}

		private bool FixPrestige(global::Kampai.Game.NamedCharacter affectedCharacter)
		{
			int iD = affectedCharacter.Definition.ID;
			foreach (global::Kampai.Game.NamedCharacter namedCharacter in namedCharacters)
			{
				if (namedCharacter == affectedCharacter || namedCharacter.Definition.ID != iD)
				{
					continue;
				}
				player.Remove(affectedCharacter);
				return true;
			}
			return false;
		}

		private void CreateCharacters()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> instancesByDefinition = playerService.GetInstancesByDefinition<global::Kampai.Game.TikiBarBuildingDefinition>();
			if (instancesByDefinition == null || instancesByDefinition.Count == 0)
			{
				return;
			}
			global::Kampai.Game.TikiBarBuilding tikiBar = instancesByDefinition[0] as global::Kampai.Game.TikiBarBuilding;
			if (tikiBar.minionQueue.Count <= 3 || tikiBar.GetOpenSlot() == -1)
			{
				return;
			}
			global::Kampai.Game.Prestige missingCharacter = null;
			int prestigeDefinitionId = -1;
			for (int i = 0; i < tikiBar.minionQueue.Count; i++)
			{
				prestigeDefinitionId = tikiBar.minionQueue[i];
				if (prestigeDefinitionId != -1)
				{
					global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(prestigeDefinitionId);
					if (prestige.trackedInstanceId == 0)
					{
						missingCharacter = prestige;
						break;
					}
				}
			}
			if (missingCharacter != null)
			{
				global::strange.extensions.signal.impl.Signal signal = appStartCompleteSignal;
				if (playerService.GetHighestFtueCompleted() == 999999)
				{
					signal = randomFlyOverCompleteSignal;
				}
				signal.AddOnce(delegate
				{
					tikiBar.minionQueue.Remove(prestigeDefinitionId);
					minionPrestigeCompleteSignal.Dispatch(missingCharacter);
					CheckMasterPlanCooldown();
				});
			}
		}

		private void CheckMasterPlanCooldown()
		{
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			if (currentMasterPlan != null && currentMasterPlan.displayCooldownAlert)
			{
				displayMasterPlanCooldownAlertSignal.Dispatch(currentMasterPlan);
			}
		}
	}
}
