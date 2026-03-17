namespace Kampai.UI.View
{
	public class DisplayMasterPlanBonusCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int componentDefinitionId { get; set; }

		[Inject]
		public global::Kampai.Game.Transaction.TransactionDefinition transaction { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanDefinition planDefinition { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(componentDefinitionId);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_MasterPlanBonus");
			iGUICommand.skrimScreen = "MasterPlanBonus";
			iGUICommand.darkSkrim = true;
			iGUICommand.disableSkrimButton = true;
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(typeof(global::Kampai.Game.MasterPlanComponent), firstInstanceByDefinitionId);
			args.Add(transaction);
			args.Add(false);
			args.Add(planDefinition);
			guiService.Execute(iGUICommand);
		}
	}
}
