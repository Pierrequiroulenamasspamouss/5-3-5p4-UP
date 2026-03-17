namespace Kampai.Game
{
	public class PanAndOpenQuestCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PanAndOpenQuestCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int questID { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject namedCharacterManager { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveSignal autoMoveSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanQuestService masterPlanQuestService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.Quest questByInstanceId = masterPlanQuestService.GetQuestByInstanceId(questID);
			if (questByInstanceId == null)
			{
				logger.Error("No quest found with id: {0}", questID);
				return;
			}
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(questByInstanceId.QuestIconTrackedInstanceId);
			if (buildingObject != null)
			{
				zero = buildingObject.transform.position;
			}
			else
			{
				global::Kampai.Game.View.NamedCharacterManagerView component2 = namedCharacterManager.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>();
				global::Kampai.Game.View.NamedCharacterObject namedCharacterObject = component2.Get(questByInstanceId.QuestIconTrackedInstanceId);
				if (!(namedCharacterObject != null))
				{
					logger.Warning("Unsupported type of object to pan to");
					return;
				}
				zero = namedCharacterObject.transform.position;
			}
			logger.Info("Pan and open quest id: {0} - {1}", questID, zero);
			global::Kampai.Game.ScreenPosition value = null;
			global::Kampai.Game.TaskableBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.TaskableBuilding>(questByInstanceId.QuestIconTrackedInstanceId);
			if (byInstanceId != null)
			{
				value = byInstanceId.Definition.ScreenPosition;
			}
			autoMoveSignal.Dispatch(zero, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(value), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.Quest, byInstanceId, questByInstanceId), false);
		}
	}
}
