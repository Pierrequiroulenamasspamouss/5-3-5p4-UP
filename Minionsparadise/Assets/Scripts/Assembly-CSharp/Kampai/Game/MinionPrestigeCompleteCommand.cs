namespace Kampai.Game
{
	public class MinionPrestigeCompleteCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MinionPrestigeCompleteCommand") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.Character character;

		private global::Kampai.Game.TikiBarBuilding tikiBar;

		private int slotIndex;

		private int prestigeDefId;

		[Inject]
		public global::Kampai.Game.Prestige prestige { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.AddMinionToTikiBarSignal addMinionToTikiBarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockCharacterModel unlockCharacterModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal removeWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnlockMinionsSignal unlockMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displaySignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeAllMenuSignal { get; set; }

		public override void Execute()
		{
			prestigeDefId = prestige.Definition.ID;
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> instancesByDefinition = playerService.GetInstancesByDefinition<global::Kampai.Game.TikiBarBuildingDefinition>();
			if (instancesByDefinition != null && instancesByDefinition.Count != 0)
			{
				tikiBar = instancesByDefinition[0] as global::Kampai.Game.TikiBarBuilding;
				int openSlot = tikiBar.GetOpenSlot();
				if (prestigeDefId == 40004)
				{
					if (prestige.CurrentPrestigeLevel <= 0)
					{
						pickControllerModel.SetIgnoreInstance(313, true);
						CreateMinion();
					}
				}
				else if (openSlot != -1)
				{
					slotIndex = openSlot;
					if (prestige.CurrentPrestigeLevel > 0)
					{
						character = playerService.GetByInstanceId<global::Kampai.Game.Character>(prestige.trackedInstanceId);
						if (character is global::Kampai.Game.Minion || prestigeDefId == 40002 || prestigeDefId == 40003)
						{
							tikiBar.minionQueue[openSlot] = prestigeDefId;
							RemoveWayFinderIfBob(prestigeDefId);
							addMinionToTikiBarSignal.Dispatch(tikiBar, character, prestige, slotIndex);
						}
						else if (character is global::Kampai.Game.NamedCharacter)
						{
							prestigeService.ChangeToPrestigeState(prestige, global::Kampai.Game.PrestigeState.Questing);
						}
						uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShowBuddyWelcomePanelUISignal>().Dispatch(new global::Kampai.Util.Boxed<global::Kampai.Game.Prestige>(prestige), global::Kampai.UI.View.CharacterWelcomeState.Welcome, prestige.CurrentPrestigeLevel);
					}
					else
					{
						unlockCharacterModel.routeIndex = slotIndex;
						tikiBar.minionQueue[slotIndex] = prestigeDefId;
						CreateMinion();
					}
				}
				else
				{
					HandleNoRoomAtTikiBar();
				}
			}
			telemetryService.Send_TelemetryCharacterPrestiged(prestige);
		}

		private void HandleNoRoomAtTikiBar()
		{
			int count = tikiBar.minionQueue.Count;
			if (this.prestige.CurrentPrestigeLevel > 0)
			{
				character = playerService.GetByInstanceId<global::Kampai.Game.Character>(this.prestige.trackedInstanceId);
				if (character is global::Kampai.Game.Minion || prestigeDefId == 40002)
				{
					tikiBar.minionQueue.Add(prestigeDefId);
				}
				else if (character is global::Kampai.Game.NamedCharacter)
				{
					prestigeService.ChangeToPrestigeState(this.prestige, global::Kampai.Game.PrestigeState.Questing);
				}
				return;
			}
			int index = count;
			for (int i = 3; i < count; i++)
			{
				global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(tikiBar.minionQueue[i]);
				if (prestige != null && prestige.CurrentPrestigeLevel > 0)
				{
					index = i;
					break;
				}
			}
			tikiBar.minionQueue.Insert(index, prestigeDefId);
		}

		private void RemoveWayFinderIfBob(int prestigeDefId)
		{
			if (prestigeDefId == 40002)
			{
				removeWayFinderSignal.Dispatch(character.ID);
			}
		}

		private void CreateMinion()
		{
			closeAllMenuSignal.Dispatch(null);
			global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(prestigeDefId);
			global::Kampai.Game.Definition definition = definitionService.Get<global::Kampai.Game.Definition>(prestigeDefinition.TrackedDefinitionID);
			global::Kampai.Game.NamedCharacterDefinition namedCharacterDefinition = definition as global::Kampai.Game.NamedCharacterDefinition;
			if (namedCharacterDefinition != null)
			{
				CreateNamedCharacter(namedCharacterDefinition);
			}
			global::Kampai.Game.CostumeItemDefinition costumeItemDefinition = definition as global::Kampai.Game.CostumeItemDefinition;
			if (costumeItemDefinition != null)
			{
				CreateCostumedMinion();
			}
			playerService.GetMinionPartyInstance().CharacterUnlocking = true;
		}

		private void CreateCostumedMinion()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MinionDefinition> all = definitionService.GetAll<global::Kampai.Game.MinionDefinition>();
			int count = all.Count;
			int index = randomService.NextInt(count);
			global::Kampai.Game.MinionDefinition def = all[index];
			global::Kampai.Game.Minion minion = new global::Kampai.Game.Minion(def);
			int costumeDefinitionID = prestige.Definition.CostumeDefinitionID;
			playerService.Add(minion);
			prestige.trackedInstanceId = minion.ID;
			minion.PrestigeId = prestige.ID;
			character = minion;
			if (costumeDefinitionID == 99)
			{
				unlockCharacterModel.minionUnlocks.Add(minion);
			}
			else
			{
				unlockCharacterModel.characterUnlocks.Add(minion);
			}
			routineRunner.StartCoroutine(WaitAFrame());
		}

		private void CreateNamedCharacter(global::Kampai.Game.NamedCharacterDefinition namedCharacterDefinition)
		{
			global::Kampai.Game.NamedCharacter namedCharacter = CreateNamedCharacterBasedOnDefinition(namedCharacterDefinition);
			prestige.trackedInstanceId = namedCharacter.ID;
			character = namedCharacter;
			unlockCharacterModel.characterUnlocks.Add(namedCharacter);
			global::strange.extensions.signal.impl.Signal<bool> signal = new global::strange.extensions.signal.impl.Signal<bool>();
			signal.AddListener(delegate(bool wasShown)
			{
				if (!wasShown)
				{
					unlockMinionSignal.Dispatch();
				}
			});
			int playerTrainingReprestigeDefinitionId = prestige.Definition.PlayerTrainingReprestigeDefinitionId;
			if (prestige.CurrentPrestigeLevel >= 1 && playerTrainingReprestigeDefinitionId > 0)
			{
				displaySignal.Dispatch(prestige.Definition.PlayerTrainingReprestigeDefinitionId, false, signal);
			}
			else
			{
				displaySignal.Dispatch(prestige.Definition.PlayerTrainingPrestigeDefinitionId, false, signal);
			}
			prestigeService.ChangeToPrestigeState(prestige, global::Kampai.Game.PrestigeState.Questing, 0, false);
		}

		private global::Kampai.Game.NamedCharacter CreateNamedCharacterBasedOnDefinition(global::Kampai.Game.NamedCharacterDefinition namedCharacterDefinition)
		{
			global::Kampai.Game.NamedCharacter namedCharacter = null;
			global::Kampai.Game.PhilCharacterDefinition philCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.PhilCharacterDefinition;
			if (philCharacterDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.PhilCharacter(philCharacterDefinition);
			}
			global::Kampai.Game.BobCharacterDefinition bobCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.BobCharacterDefinition;
			if (bobCharacterDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.BobCharacter(bobCharacterDefinition);
			}
			global::Kampai.Game.KevinCharacterDefinition kevinCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.KevinCharacterDefinition;
			if (kevinCharacterDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.KevinCharacter(kevinCharacterDefinition);
			}
			global::Kampai.Game.StuartCharacterDefinition stuartCharacterDefinition = namedCharacterDefinition as global::Kampai.Game.StuartCharacterDefinition;
			if (stuartCharacterDefinition != null)
			{
				namedCharacter = new global::Kampai.Game.StuartCharacter(stuartCharacterDefinition);
			}
			global::System.Collections.Generic.List<global::Kampai.Game.NamedCharacter> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.NamedCharacter>();
			foreach (global::Kampai.Game.NamedCharacter item in instancesByType)
			{
				if (item.Definition.ID == namedCharacter.Definition.ID)
				{
					logger.Error("You are trying to create a character that already exists! We are just gonna re-prestige the minion, sorry");
					prestigeService.ChangeToPrestigeState(prestige, global::Kampai.Game.PrestigeState.Questing);
					uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShowBuddyWelcomePanelUISignal>().Dispatch(new global::Kampai.Util.Boxed<global::Kampai.Game.Prestige>(prestige), global::Kampai.UI.View.CharacterWelcomeState.Welcome, prestige.CurrentPrestigeLevel);
					return item;
				}
			}
			playerService.Add(namedCharacter);
			return namedCharacter;
		}

		private global::System.Collections.IEnumerator WaitAFrame()
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			prestigeService.ChangeToPrestigeState(prestige, global::Kampai.Game.PrestigeState.Questing, 0, false);
			unlockMinionSignal.Dispatch();
		}
	}
}
