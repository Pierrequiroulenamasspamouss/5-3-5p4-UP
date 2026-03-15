namespace Kampai.UI.View
{
	public class DisplayMasterPlanComponentBonusDialogCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MasterPlanDefinition masterPlanDefinition { get; set; }

		[Inject]
		public int componentIndex { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void Execute()
		{
			int definitionId = masterPlanDefinition.ComponentDefinitionIDs[componentIndex];
			global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(definitionId);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_MasterPlanComponentBonusReward");
			iGUICommand.skrimScreen = "MasterPlanComponentBonus";
			iGUICommand.darkSkrim = false;
			iGUICommand.Args.Add(firstInstanceByDefinitionId);
			guiService.Execute(iGUICommand);
		}
	}
}
