namespace Kampai.Game
{
	public class MapAnimationEventCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MapAnimationEventCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.View.AnimEventHandler animEventHandler { get; set; }

		[Inject]
		public int buildingDefinitionId { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Building firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(buildingDefinitionId);
			global::Kampai.Util.VFXScript vFXScriptForBuilding = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>().GetVFXScriptForBuilding(firstInstanceByDefinitionId.ID);
			if (vFXScriptForBuilding != null)
			{
				logger.Info("Binding AnimEventHandler to {0}", buildingDefinitionId);
				animEventHandler.SetSiblingVFXScript(vFXScriptForBuilding);
			}
		}
	}
}
