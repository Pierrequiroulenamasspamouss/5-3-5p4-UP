namespace Kampai.Game
{
	public class BuildingCooldownCompleteCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int buildingID { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingChangeStateSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Building>(buildingID);
			if (byInstanceId != null)
			{
				buildingChangeStateSignal.Dispatch(buildingID, global::Kampai.Game.BuildingState.Idle);
			}
		}
	}
}
