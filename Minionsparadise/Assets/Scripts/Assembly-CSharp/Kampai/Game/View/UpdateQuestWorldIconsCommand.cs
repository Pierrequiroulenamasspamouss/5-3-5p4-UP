namespace Kampai.Game.View
{
	public class UpdateQuestWorldIconsCommand : global::strange.extensions.command.impl.Command
	{
		private int wayfinderTrackedId;

		[Inject]
		public global::Kampai.Game.Quest Quest { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService PrestigeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.GetWayFinderSignal GetWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CreateWayFinderSignal CreateWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveWayFinderSignal RemoveWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.AddQuestToExistingWayFinderSignal AddQuestToExistingWayFinderSignal { get; set; }

		public override void Execute()
		{
			foreach (global::Kampai.Game.QuestStep step in Quest.Steps)
			{
				if (step.state == global::Kampai.Game.QuestStepState.Complete && CanShowWayfinder(Quest, step))
				{
					RemoveWayFinderSignal.Dispatch(step.TrackedID);
				}
			}
			if (Quest.state != global::Kampai.Game.QuestState.Complete && Quest.GetActiveDefinition().QuestVersion != -1)
			{
				int questIconTrackedInstanceId = Quest.QuestIconTrackedInstanceId;
				if (questIconTrackedInstanceId != 0)
				{
					wayfinderTrackedId = PrestigeService.ResolveTrackedId(questIconTrackedInstanceId);
				}
				GetWayFinderSignal.Dispatch(wayfinderTrackedId, GetWayFinder);
			}
		}

		private bool CanShowWayfinder(global::Kampai.Game.Quest quest, global::Kampai.Game.QuestStep step)
		{
			global::Kampai.Game.QuestDefinition definition = quest.Definition;
			if (definition.QuestSteps == null || definition.QuestSteps.Count <= 0 || quest.state == global::Kampai.Game.QuestState.Notstarted)
			{
				return false;
			}
			int num = quest.Steps.IndexOf(step);
			return num >= 0 && definition.QuestSteps[num].ShowWayfinder;
		}

		internal void GetWayFinder(int trackedId, global::Kampai.UI.View.IWayFinderView wayFinderView)
		{
			if (wayFinderView == null)
			{
				if (wayfinderTrackedId != 0)
				{
					if (Quest.Definition.ID == 77777)
					{
						CreateWayFinderSignal.Dispatch(new global::Kampai.UI.View.WayFinderSettings(Quest.Definition.ID, wayfinderTrackedId));
					}
					else
					{
						CreateWayFinderSignal.Dispatch(new global::Kampai.UI.View.WayFinderSettings(Quest.GetActiveDefinition().ID, wayfinderTrackedId));
					}
				}
			}
			else
			{
				AddQuestToExistingWayFinderSignal.Dispatch(Quest.GetActiveDefinition().ID, trackedId);
			}
			foreach (global::Kampai.Game.QuestStep step in Quest.Steps)
			{
				if (step.state != global::Kampai.Game.QuestStepState.Complete && CanShowWayfinder(Quest, step))
				{
					global::Kampai.UI.View.WayFinderSettings type = new global::Kampai.UI.View.WayFinderSettings(step.TrackedID);
					CreateWayFinderSignal.Dispatch(type);
				}
			}
		}
	}
}
