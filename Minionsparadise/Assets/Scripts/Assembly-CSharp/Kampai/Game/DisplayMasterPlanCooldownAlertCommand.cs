namespace Kampai.Game
{
	public class DisplayMasterPlanCooldownAlertCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MasterPlan plan { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		public override void Execute()
		{
			if (!timeEventService.HasEventID(plan.ID))
			{
				plan.displayCooldownAlert = false;
				return;
			}
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_MasterPlanCooldownAlert");
			iGUICommand.skrimScreen = "MasterPlanCooldownAlert";
			iGUICommand.disableSkrimButton = true;
			iGUICommand.darkSkrim = true;
			iGUICommand.Args.Add(plan);
			guiService.Execute(iGUICommand);
		}
	}
}
