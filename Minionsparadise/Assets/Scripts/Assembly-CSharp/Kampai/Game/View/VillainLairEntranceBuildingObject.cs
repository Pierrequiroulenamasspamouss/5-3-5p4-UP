namespace Kampai.Game.View
{
	public class VillainLairEntranceBuildingObject : global::Kampai.Game.View.BuildingObject
	{
		public global::Kampai.Game.VillainLairEntranceBuilding portal { get; set; }

		internal override void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::System.Collections.Generic.IDictionary<string, global::UnityEngine.RuntimeAnimatorController> controllers, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, controllers, definitionService);
			portal = building as global::Kampai.Game.VillainLairEntranceBuilding;
			base.gameObject.AddComponent<global::Kampai.Game.View.VolcanoEntranceView>();
		}

		protected override global::UnityEngine.Vector3 GetIndicatorPosition(bool centerY)
		{
			return new global::UnityEngine.Vector3(minColliderY.bounds.center.x, minColliderY.bounds.max.y, minColliderY.bounds.center.z);
		}
	}
}
