namespace Kampai.Game
{
	internal sealed class MasterPlanCompleteCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MasterPlanDefinition masterPlanDefinition { get; set; }

		[Inject]
		public global::Kampai.Game.SetMasterPlanRewardAnimatorSignal setAnimatorSingal { get; set; }

		public override void Execute()
		{
			setAnimatorSingal.Dispatch(masterPlanDefinition);
		}
	}
}
