namespace Kampai.UI.View
{
	public abstract class AbstractQuestWayFinderView : global::Kampai.UI.View.AbstractWayFinderView, global::Kampai.UI.View.IQuestWayFinderView, global::Kampai.UI.View.IWayFinderView, global::Kampai.UI.View.IWorldToGlassView
	{
		private global::System.Collections.Generic.List<int> allQuests;

		private bool ignoreFirstPriorityUpdate = true;

		private int currentActiveQuestIndex = -1;

		public global::Kampai.Game.Quest CurrentActiveQuest { get; private set; }

		protected override string WayFinderDefaultIcon
		{
			get
			{
				return wayFinderDefinition.NewQuestIcon;
			}
		}

		protected virtual string WayFinderQuestCompleteIcon
		{
			get
			{
				return wayFinderDefinition.QuestCompleteIcon;
			}
		}

		protected virtual string WayFinderTaskCompleteIcon
		{
			get
			{
				return wayFinderDefinition.TaskCompleteIcon;
			}
		}

		protected override void InitSubView()
		{
			allQuests = new global::System.Collections.Generic.List<int>();
			global::Kampai.UI.View.WayFinderSettings wayFinderSettings = m_Settings as global::Kampai.UI.View.WayFinderSettings;
			AddQuest(wayFinderSettings.QuestDefId);
			ignoreFirstPriorityUpdate = false;
		}

		internal override void Clear()
		{
			if (allQuests != null)
			{
				allQuests.Clear();
			}
		}

		protected override bool OnCanUpdate()
		{
			if (m_Prestige != null)
			{
				int iD = m_Prestige.Definition.ID;
				if (iD != 40003 && m_Prestige.Definition.Type != global::Kampai.Game.PrestigeType.SpecialEventMinion && iD != 40004)
				{
					return false;
				}
			}
			return true;
		}

		public void AddQuest(int questDefId)
		{
			int num = allQuests.IndexOf(questDefId);
			if (num != -1)
			{
				global::Kampai.Game.Quest firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Quest>(questDefId);
				if (firstInstanceByDefinitionId.state == global::Kampai.Game.QuestState.Harvestable)
				{
					SetNextQuest(num);
				}
				UpdateQuestIcon();
			}
			else
			{
				allQuests.Add(questDefId);
				SetNextQuest(allQuests.Count - 1);
			}
		}

		public void RemoveQuest(int questDefId)
		{
			int num = allQuests.IndexOf(questDefId);
			if (num != -1)
			{
				allQuests.Remove(questDefId);
				if (allQuests.Count == 0)
				{
					CurrentActiveQuest = null;
					RemoveWayFinderSignal.Dispatch();
				}
				else
				{
					SetNextQuest(num);
				}
			}
		}

		public void SetNextQuest(int indexToSet = -1)
		{
			if (allQuests == null || allQuests.Count == 0)
			{
				return;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.Quest> priorizedQuests = GetPriorizedQuests();
			if (indexToSet == -1)
			{
				for (int i = 0; i < priorizedQuests.Count; i++)
				{
					global::Kampai.Game.Quest quest = priorizedQuests[i];
					global::Kampai.Game.QuestDefinition activeDefinition = quest.GetActiveDefinition();
					if (quest.state == global::Kampai.Game.QuestState.Harvestable || quest.state == global::Kampai.Game.QuestState.Notstarted)
					{
						currentActiveQuestIndex = allQuests.IndexOf(activeDefinition.ID);
						break;
					}
					if (targetObject.ID == 78 && quest.Definition.SurfaceID == 40000)
					{
						currentActiveQuestIndex = allQuests.IndexOf(activeDefinition.ID);
					}
				}
			}
			else
			{
				currentActiveQuestIndex = indexToSet;
			}
			currentActiveQuestIndex %= allQuests.Count;
			CurrentActiveQuest = GetQuestByDefId(allQuests[currentActiveQuestIndex]);
			if (CurrentActiveQuest == null)
			{
				return;
			}
			foreach (global::Kampai.Game.Quest item in priorizedQuests)
			{
				global::Kampai.Game.QuestDefinition activeDefinition2 = item.GetActiveDefinition();
				if (CurrentActiveQuest.GetActiveDefinition().SurfaceID == activeDefinition2.SurfaceID)
				{
					if (CurrentActiveQuest.ID != item.ID && CurrentActiveQuest.GetActiveDefinition().QuestPriority < activeDefinition2.QuestPriority)
					{
						CurrentActiveQuest = item;
						currentActiveQuestIndex = allQuests.IndexOf(activeDefinition2.ID);
					}
					break;
				}
			}
			UpdateQuestIcon();
		}

		public global::Kampai.Game.Quest GetQuestByDefId(int questDefId)
		{
			return playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Quest>(questDefId);
		}

		protected virtual bool CanUpdateQuestIcon()
		{
			return true;
		}

		private void UpdateQuestIcon()
		{
			if (CanUpdateQuestIcon())
			{
				if (IsQuestComplete())
				{
					SetQuestIcon(WayFinderQuestCompleteIcon);
				}
				else if (IsNewQuestAvailable())
				{
					SetQuestIcon(WayFinderDefaultIcon);
				}
				else if (IsTaskReady())
				{
					SetQuestIcon(WayFinderTaskCompleteIcon);
				}
				else if (IsQuestAvailable())
				{
					SetQuestIcon(WayFinderDefaultIcon);
				}
			}
		}

		private void SetQuestIcon(string icon)
		{
			UpdateIcon(icon);
			if (!ignoreFirstPriorityUpdate)
			{
				UpdateWayFinderPrioritySignal.Dispatch();
			}
		}

		public bool IsNewQuestAvailable()
		{
			if (CurrentActiveQuest != null)
			{
				global::Kampai.Game.QuestState state = CurrentActiveQuest.state;
				if (state == global::Kampai.Game.QuestState.Notstarted || state == global::Kampai.Game.QuestState.RunningStartScript)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsQuestAvailable()
		{
			return CurrentActiveQuest != null && CurrentActiveQuest.state == global::Kampai.Game.QuestState.RunningTasks && !IsTaskReady();
		}

		public bool IsTaskReady()
		{
			if (CurrentActiveQuest != null && CurrentActiveQuest.state == global::Kampai.Game.QuestState.RunningTasks)
			{
				foreach (global::Kampai.Game.QuestStep step in CurrentActiveQuest.Steps)
				{
					if (step.state == global::Kampai.Game.QuestStepState.Ready)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsQuestComplete()
		{
			if (CurrentActiveQuest != null)
			{
				switch (CurrentActiveQuest.state)
				{
				case global::Kampai.Game.QuestState.RunningCompleteScript:
				case global::Kampai.Game.QuestState.Harvestable:
				case global::Kampai.Game.QuestState.Complete:
					return true;
				}
			}
			return false;
		}

		private global::System.Collections.Generic.List<global::Kampai.Game.Quest> GetPriorizedQuests()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Quest> list = new global::System.Collections.Generic.List<global::Kampai.Game.Quest>();
			foreach (int allQuest in allQuests)
			{
				global::Kampai.Game.Quest questByDefId = GetQuestByDefId(allQuest);
				list.Add(questByDefId);
			}
			return QuestUtils.ResolveQuests(list);
		}
	}
}
