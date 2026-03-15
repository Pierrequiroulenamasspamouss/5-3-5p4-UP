namespace Kampai.UI.Controller
{
	public class DisplayMasterPlanOnboardingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int onboardDefinitionId { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraMoveToCustomPositionSignal customCameraPositionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IGhostComponentService ghostService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MasterPlanOnboardDefinition masterPlanOnboardDefinition = definitionService.Get<global::Kampai.Game.MasterPlanOnboardDefinition>(onboardDefinitionId);
			customCameraPositionSignal.Dispatch(masterPlanOnboardDefinition.CustomCameraPosID, new global::Kampai.Util.Boxed<global::System.Action>(null));
			ghostService.RunBeginGhostComponentFunctionFromDefinition(masterPlanOnboardDefinition.ghostFunction.startType, masterPlanOnboardDefinition.ghostFunction.componentBuildingDefID);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "screen_MasterPlanOnboarding");
			iGUICommand.Args.Add(masterPlanOnboardDefinition);
			iGUICommand.skrimScreen = "MasterPlanOnboarding";
			iGUICommand.darkSkrim = false;
			iGUICommand.disableSkrimButton = true;
			guiService.Execute(iGUICommand);
		}
	}
}
