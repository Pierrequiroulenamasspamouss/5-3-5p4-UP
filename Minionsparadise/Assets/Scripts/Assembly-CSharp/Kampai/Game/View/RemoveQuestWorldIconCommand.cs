namespace Kampai.Game.View
{
	public class RemoveQuestWorldIconCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Quest Quest { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveQuestFromExistingWayFinderSignal removeQuestFromExistingWayFinderSignal { get; set; }

		public override void Execute()
		{
			if (Quest.state != global::Kampai.Game.QuestState.Complete && Quest.GetActiveDefinition().QuestVersion != -1)
			{
				int questIconTrackedInstanceId = Quest.QuestIconTrackedInstanceId;
				int num = 0;
				if (questIconTrackedInstanceId != 0)
				{
					num = prestigeService.ResolveTrackedId(questIconTrackedInstanceId);
				}
				int type = ((num != 0) ? num : questIconTrackedInstanceId);
				removeQuestFromExistingWayFinderSignal.Dispatch(Quest.GetActiveDefinition().ID, type);
			}
		}
	}
}
