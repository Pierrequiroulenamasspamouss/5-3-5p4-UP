namespace Kampai.Game
{
	public class StartMignetteCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("StartMignetteCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int BuildingId { get; set; }

		[Inject]
		public global::Kampai.Common.DeselectAllMinionsSignal deselectAllMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.MignetteGameModel mignetteGameModel { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MignetteCollectionService collectionService { get; set; }

		public override void Execute()
		{
			if (mignetteGameModel.IsMignetteActive)
			{
				return;
			}
			hideSkrimSignal.Dispatch("MignetteSkrim");
			pickControllerModel.ForceDisabled = true;
			mignetteGameModel.IsMignetteActive = true;
			mignetteGameModel.BuildingId = BuildingId;
			mignetteGameModel.CurrentGameScore = 0;
			mignetteGameModel.TriggerCooldownOnComplete = true;
			deselectAllMinionsSignal.Dispatch();
			global::Kampai.Game.MignetteBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(BuildingId);
			global::Kampai.Game.MignetteBuildingDefinition mignetteBuildingDefinition = byInstanceId.MignetteBuildingDefinition;
			global::UnityEngine.GameObject obj = new global::UnityEngine.GameObject(mignetteBuildingDefinition.ContextRootName);
			global::Kampai.Util.GameObjectUtil.AddComponent(obj, mignetteBuildingDefinition.ContextRootName, logger);
			if (mignetteBuildingDefinition.ShowMignetteHUD)
			{
				if (!string.IsNullOrEmpty(mignetteBuildingDefinition.CollectableImage))
				{
					mignetteGameModel.CollectableImage = UIUtils.LoadSpriteFromPath(mignetteBuildingDefinition.CollectableImage);
				}
				if (!string.IsNullOrEmpty(mignetteBuildingDefinition.CollectableImageMask))
				{
					mignetteGameModel.CollectableImageMask = UIUtils.LoadSpriteFromPath(mignetteBuildingDefinition.CollectableImageMask);
				}
				guiService.Execute(global::Kampai.UI.View.GUIOperation.Load, "MignetteHUD");
			}
			collectionService.pendingRewardTransaction = null;
			questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Mignette, global::Kampai.Game.QuestTaskTransition.Start, byInstanceId);
		}
	}
}
