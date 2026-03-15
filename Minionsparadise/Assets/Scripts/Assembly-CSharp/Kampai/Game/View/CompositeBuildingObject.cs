namespace Kampai.Game.View
{
	public class CompositeBuildingObject : global::Kampai.Game.View.BuildingObject
	{
		public global::Kampai.Game.CompositeBuilding compositeBuilding { get; set; }

		internal override void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.IDictionary<string, global::UnityEngine.RuntimeAnimatorController> controllers, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, controllers, definitionService);
			compositeBuilding = (global::Kampai.Game.CompositeBuilding)building;
		}
	}
}
