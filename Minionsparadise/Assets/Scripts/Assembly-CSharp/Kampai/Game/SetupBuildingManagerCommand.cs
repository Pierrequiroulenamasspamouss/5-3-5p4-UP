namespace Kampai.Game
{
	internal sealed class SetupBuildingManagerCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupBuildingManagerCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.ISpecialEventService specialEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("Buildings");
			global::Kampai.Game.View.BuildingManagerView buildingManagerView = gameObject.AddComponent<global::Kampai.Game.View.BuildingManagerView>();
			buildingManagerView.Init(logger, definitionService, masterPlanService, specialEventService.IsSpecialEventActive());
			base.injectionBinder.Bind<global::UnityEngine.GameObject>().ToValue(gameObject).ToName(global::Kampai.Game.GameElement.BUILDING_MANAGER)
				.CrossContext();
			gameObject.transform.parent = contextView.transform;
			logger.Debug("SetupBuildingManagerCommand: Building Manager created.");
			routineRunner.StartCoroutine(WaitAFrame());
		}

		private global::System.Collections.IEnumerator WaitAFrame()
		{
			yield return null;
			base.injectionBinder.GetInstance<global::Kampai.Game.PopulateBuildingSignal>().Dispatch();
		}
	}
}
