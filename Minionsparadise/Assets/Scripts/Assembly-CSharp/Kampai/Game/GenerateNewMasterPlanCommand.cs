namespace Kampai.Game
{
	public class GenerateNewMasterPlanCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Util.Boxed<global::System.Action> newMasterPlanGeneratedCallback { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		public override void Execute()
		{
			masterPlanService.CreateNewMasterPlan();
			if (newMasterPlanGeneratedCallback.Value != null)
			{
				newMasterPlanGeneratedCallback.Value();
			}
		}
	}
}
