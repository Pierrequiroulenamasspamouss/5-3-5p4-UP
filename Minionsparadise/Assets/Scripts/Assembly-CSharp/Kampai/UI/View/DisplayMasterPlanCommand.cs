namespace Kampai.UI.View
{
	public class DisplayMasterPlanCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int componentIDSelectedFromPlatform { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllMessageDialogs closeAllMessageDialogs { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			global::Kampai.Game.MasterPlanDefinition definition = currentMasterPlan.Definition;
			int iD = definition.ID;
			bool flag = false;
			int definitionId = definition.ComponentDefinitionIDs[0];
			if (playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(definitionId) == null)
			{
				masterPlanService.CreateMasterPlanComponents(currentMasterPlan);
				flag = true;
			}
			closeAllMessageDialogs.Dispatch();
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "screen_MasterPlanComponentSelection");
			iGUICommand.skrimScreen = "MasterPlan";
			iGUICommand.darkSkrim = false;
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(new global::Kampai.Util.Tuple<int, int>(iD, componentIDSelectedFromPlatform));
			args.Add(flag);
			guiService.Execute(iGUICommand);
		}
	}
}
