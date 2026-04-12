namespace Kampai.Game
{
	public class SetupSpecialEventCharacterCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupSpecialEventCharacterCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int specialEventItemDefinitionID { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CreateNamedCharacterViewSignal createNamedCharacterViewSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.ShowSpecialEventCharacterSignal showSpecialEventCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateQuestWorldIconsSignal updateWorldIconSignal { get; set; }

		[Inject]
		public global::Kampai.Game.GetNewQuestSignal getNewQuestSignal { get; set; }

		public override void Execute()
		{
			if (specialEventItemDefinitionID != -1)
			{
				global::Kampai.Game.SpecialEventCharacter specialEventCharacter = null;
				global::System.Collections.Generic.ICollection<global::Kampai.Game.SpecialEventCharacter> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.SpecialEventCharacter>();
				if (instancesByType != null)
				{
					foreach (global::Kampai.Game.SpecialEventCharacter item in instancesByType)
					{
						if (item.SpecialEventID == specialEventItemDefinitionID)
						{
							specialEventCharacter = item;
							break;
						}
					}
				}
				if (specialEventCharacter != null)
				{
					routineRunner.StartCoroutine(SetupAndShowSpecialEventCharacter(specialEventCharacter));
					return;
				}
				logger.Error("SetupSpecialEventCharacterCommand: Failed to find Special Event Character instance for Event Definition ID {0}", specialEventItemDefinitionID.ToString());
				return;
			}
			global::Kampai.Game.SpecialEventCharacterDefinition specialEventCharacterDefinition = definitionService.Get<global::Kampai.Game.SpecialEventCharacterDefinition>(70009);
			if (specialEventCharacterDefinition == null)
			{
				logger.Error("SetupSpecialEventCharacterCommand: Failed to find Special Event Character Definition");
				return;
			}
			global::System.Collections.Generic.ICollection<global::Kampai.Game.SpecialEventCharacter> byDefinitionId = playerService.GetByDefinitionId<global::Kampai.Game.SpecialEventCharacter>(70009);
			if (byDefinitionId != null && byDefinitionId.Count > 0)
			{
				routineRunner.StartCoroutine(SetupSpecialEventCharacters(byDefinitionId));
			}
		}

		private global::System.Collections.IEnumerator SetupSpecialEventCharacters(global::System.Collections.Generic.ICollection<global::Kampai.Game.SpecialEventCharacter> specialEventCharacters)
		{
			foreach (global::Kampai.Game.SpecialEventCharacter existingCharacter in specialEventCharacters)
			{
				global::Kampai.Game.SpecialEventItemDefinition eventDefinition = definitionService.Get<global::Kampai.Game.SpecialEventItemDefinition>(existingCharacter.SpecialEventID);
				if (eventDefinition != null && questService.IsQuestCompleted(eventDefinition.UnlockQuestID))
				{
					CreateSpecialEventCharacter(existingCharacter);
				}
			}
			yield return null;
			getNewQuestSignal.Dispatch();
			global::System.Collections.Generic.List<global::Kampai.Game.Quest> quests = playerService.GetInstancesByType<global::Kampai.Game.Quest>();
			foreach (global::Kampai.Game.SpecialEventCharacter existingCharacter2 in specialEventCharacters)
			{
				global::Kampai.Game.SpecialEventItemDefinition eventDefinition2 = definitionService.Get<global::Kampai.Game.SpecialEventItemDefinition>(existingCharacter2.SpecialEventID);
				foreach (global::Kampai.Game.Quest quest in quests)
				{
					if (quest.Definition.SurfaceID == eventDefinition2.PrestigeDefinitionID)
					{
						updateWorldIconSignal.Dispatch(quest);
					}
				}
			}
			showSpecialEventCharacterSignal.Dispatch(3f, -1);
		}

		private global::System.Collections.IEnumerator SetupAndShowSpecialEventCharacter(global::Kampai.Game.SpecialEventCharacter existingCharacter)
		{
			CreateSpecialEventCharacter(existingCharacter);
			yield return null;
			showSpecialEventCharacterSignal.Dispatch(3f, existingCharacter.ID);
		}

		private void CreateSpecialEventCharacter(global::Kampai.Game.SpecialEventCharacter existingCharacter)
		{
			createNamedCharacterViewSignal.Dispatch(existingCharacter);
			global::Kampai.Game.Prestige prestige = prestigeService.GetPrestige(existingCharacter.PrestigeDefinitionID);
			if (prestige == null)
			{
				global::Kampai.Game.PrestigeDefinition prestigeDefinition = definitionService.Get<global::Kampai.Game.PrestigeDefinition>(existingCharacter.PrestigeDefinitionID);
				if (prestigeDefinition != null)
				{
					prestige = new global::Kampai.Game.Prestige(prestigeDefinition);
					prestige.trackedInstanceId = existingCharacter.ID;
					prestige.state = global::Kampai.Game.PrestigeState.Questing;
					prestigeService.AddPrestige(prestige);
					logger.Info("SetupSpecialEventCharacterCommand: Added Prestige ID {0} for Special Event Character ID {1} tracked instance ID to:", existingCharacter.PrestigeDefinitionID, existingCharacter.ID.ToString());
				}
				else
				{
					logger.Warning("SetupSpecialEventCharacterCommand: Added Prestige ID {0} for Special Event Character ID {1} tracked instance ID to:", existingCharacter.PrestigeDefinitionID, existingCharacter.ID.ToString());
				}
			}
		}
	}
}
