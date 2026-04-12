namespace Kampai.Game
{
	public class MasterPlanComponentCompleteCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int componentBuildingId { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalAudioSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.GetWayFinderSignal getWayFinderSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal playerTrainingSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IGhostComponentService ghostService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.MasterPlanComponentBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MasterPlanComponentBuilding>(componentBuildingId);
			if (byInstanceId == null)
			{
				return;
			}
			getWayFinderSignal.Dispatch(componentBuildingId, delegate(int wayFinderId, global::Kampai.UI.View.IWayFinderView wayFinderView)
			{
				if (wayFinderView != null)
				{
					wayFinderView.SetForceHide(false);
				}
			});
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MasterPlanComponent>();
			foreach (global::Kampai.Game.MasterPlanComponent item in instancesByType)
			{
				if (item.buildingDefID == byInstanceId.Definition.ID)
				{
					ghostService.ClearGhostComponentBuildings(true, true);
					global::Kampai.Game.View.MasterPlanComponentBuildingObject masterPlanComponentBuildingObject = component.GetBuildingObject(byInstanceId.ID) as global::Kampai.Game.View.MasterPlanComponentBuildingObject;
					if (masterPlanComponentBuildingObject != null)
					{
						masterPlanComponentBuildingObject.TriggerVFX();
						globalAudioSignal.Dispatch("Play_building_repair_01");
					}
					break;
				}
			}
			global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponentBuilding> instancesByType2 = playerService.GetInstancesByType<global::Kampai.Game.MasterPlanComponentBuilding>();
			if (instancesByType2.Count == 1)
			{
				playerTrainingSignal.Dispatch(19000031, false, new global::strange.extensions.signal.impl.Signal<bool>());
			}
		}
	}
}
