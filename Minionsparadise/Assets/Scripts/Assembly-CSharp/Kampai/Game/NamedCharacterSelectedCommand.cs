namespace Kampai.Game
{
	public class NamedCharacterSelectedCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.View.CharacterObject characterObject { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PopupMessageSignal popupMessageSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Character firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Character>(characterObject.DefinitionID);
			if (firstInstanceByDefinitionId != null)
			{
				string text = TryGetQuestAspirationMessage(firstInstanceByDefinitionId);
				if (text != null)
				{
					popupMessageSignal.Dispatch(text, global::Kampai.UI.View.PopupMessageType.NORMAL);
				}
			}
		}

		private string TryGetQuestAspirationMessage(global::Kampai.Game.Character character)
		{
			string result = null;
			global::Kampai.Game.Prestige prestigeFromMinionInstance = prestigeService.GetPrestigeFromMinionInstance(character);
			if (prestigeFromMinionInstance == null || (!(character is global::Kampai.Game.SpecialEventCharacter) && (prestigeFromMinionInstance.state == global::Kampai.Game.PrestigeState.TaskableWhileQuesting || prestigeFromMinionInstance.state == global::Kampai.Game.PrestigeState.Questing)))
			{
				return null;
			}
			global::Kampai.Game.PrestigeDefinition definition = prestigeFromMinionInstance.Definition;
			int iD = definition.ID;
			if (IsExpiredEventMinion(iD))
			{
				return null;
			}
			if (questService.HasActiveQuest(iD))
			{
				return null;
			}
			int quantity = (int)playerService.GetQuantity(global::Kampai.Game.StaticItem.LEVEL_ID);
			int num = prestigeFromMinionInstance.CurrentPrestigeLevel;
			uint num2 = 0u;
			bool flag = definition.PrestigeLevelSettings != null;
			if (flag)
			{
				bool flag2 = num == definition.PrestigeLevelSettings.Count - 1;
				if (prestigeFromMinionInstance.state == global::Kampai.Game.PrestigeState.Taskable && flag2)
				{
					result = localService.GetString("AspirationalMessageCharacterLevelComplete", localService.GetString(prestigeFromMinionInstance.Definition.LocalizedKey));
				}
				else
				{
					if (prestigeFromMinionInstance.state != global::Kampai.Game.PrestigeState.Prestige && !flag2)
					{
						num++;
					}
					num2 = definition.PrestigeLevelSettings[num].UnlockLevel;
				}
			}
			else
			{
				num2 = GetNextQuestLevelFromQuestline(iD);
			}
			if (num2 != 0)
			{
				result = ((num2 > quantity) ? localService.GetString("AspirationalMessageCharacterLevelPre", localService.GetString(prestigeFromMinionInstance.Definition.LocalizedKey), num2.ToString()) : ((!flag) ? localService.GetString("AspirationalMessageSpecialEventCharacterLevelAvailable", localService.GetString(prestigeFromMinionInstance.Definition.LocalizedKey)) : localService.GetString("AspirationalMessageCharacterLevelAvailable", localService.GetString(prestigeFromMinionInstance.Definition.LocalizedKey))));
			}
			return result;
		}

		private bool IsExpiredEventMinion(int prestigeDefinitionID)
		{
			global::System.Collections.Generic.List<global::Kampai.Game.SpecialEventItem> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.SpecialEventItem>();
			foreach (global::Kampai.Game.SpecialEventItem item in instancesByType)
			{
				if ((item.Definition as global::Kampai.Game.SpecialEventItemDefinition).PrestigeDefinitionID == prestigeDefinitionID && item.HasEnded)
				{
					return true;
				}
			}
			return false;
		}

		public uint GetNextQuestLevelFromQuestline(int prestigeDefinitionID)
		{
			uint result = 0u;
			global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.QuestLine> questLines = questService.GetQuestLines();
			foreach (global::Kampai.Game.QuestLine value in questLines.Values)
			{
				if (value.Quests.Count == 0 || value.state == global::Kampai.Game.QuestLineState.Finished || value.state != global::Kampai.Game.QuestLineState.NotStarted || GetSurfaceIDFromQuestLine(value) != prestigeDefinitionID)
				{
					continue;
				}
				foreach (global::Kampai.Game.QuestDefinition quest in value.Quests)
				{
					if (quest.ID == value.QuestLineID && quest.QuestVersion != -1)
					{
						result = (uint)quest.UnlockLevel;
						break;
					}
				}
				break;
			}
			return result;
		}

		private int GetSurfaceIDFromQuestLine(global::Kampai.Game.QuestLine questLine)
		{
			int result = -1;
			if (questLine != null && questLine.Quests != null)
			{
				for (int i = 0; i < questLine.Quests.Count; i++)
				{
					if (questLine.Quests[i].SurfaceID > 0)
					{
						result = questLine.Quests[i].SurfaceID;
						break;
					}
				}
			}
			return result;
		}
	}
}
