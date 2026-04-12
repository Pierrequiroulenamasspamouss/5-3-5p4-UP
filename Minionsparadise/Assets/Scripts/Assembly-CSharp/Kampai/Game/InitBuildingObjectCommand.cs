namespace Kampai.Game
{
	public class InitBuildingObjectCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("InitBuildingObjectCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.View.BuildingObject buildingObject { get; set; }

		[Inject]
		public global::Kampai.Game.Building building { get; set; }

		[Inject]
		public global::System.Collections.Generic.Dictionary<string, global::UnityEngine.RuntimeAnimatorController> animatorControllers { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.ActionableObject component = buildingObject.GetComponent<global::Kampai.Game.View.ActionableObject>();
			if (component != null)
			{
				base.injectionBinder.injector.Inject(component, false);
			}
			buildingObject.Init(building, logger, animatorControllers, definitionService);
		}
	}
}
