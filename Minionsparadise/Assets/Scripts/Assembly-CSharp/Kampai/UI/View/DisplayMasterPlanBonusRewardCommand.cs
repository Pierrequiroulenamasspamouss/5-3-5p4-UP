namespace Kampai.UI.View
{
	public class DisplayMasterPlanBonusRewardCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int transactionDefinitionID { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanComponent component { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Transaction.TransactionDefinition value = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(transactionDefinitionID);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_MasterPlanBonus");
			iGUICommand.skrimScreen = "MasterPlanBonus";
			iGUICommand.darkSkrim = true;
			iGUICommand.disableSkrimButton = true;
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(value);
			args.Add(component);
			args.Add(true);
			guiService.Execute(iGUICommand);
		}
	}
}
