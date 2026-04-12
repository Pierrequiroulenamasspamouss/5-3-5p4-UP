namespace Kampai.Game.View
{
	public class ScaffoldingBuildingObject : global::Kampai.Game.View.BuildingObject, global::Kampai.Game.View.IScaffoldingPart
	{
		public global::UnityEngine.GameObject GameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		public void Init(global::Kampai.Game.Building building, global::Kampai.Util.IKampaiLogger logger, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.Init(building, logger, null, definitionService);
		}

		public override void UpdateColliderState(global::Kampai.Game.BuildingState state)
		{
		}
	}
}
