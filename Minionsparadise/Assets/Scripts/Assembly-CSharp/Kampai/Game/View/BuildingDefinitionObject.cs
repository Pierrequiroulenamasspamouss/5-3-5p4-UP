namespace Kampai.Game.View
{
	public abstract class BuildingDefinitionObject : global::Kampai.Game.View.ActionableObject, FootprintProperties
	{
		public int Width { get; protected set; }

		public int Depth { get; protected set; }

		public bool HasSidewalk { get; protected set; }

		public global::UnityEngine.Vector3 ResourceIconPosition
		{
			get
			{
				return GetResourceIconPosition();
			}
		}

		private global::UnityEngine.Vector3 GetResourceIconPosition()
		{
			global::UnityEngine.Vector3 position = base.transform.position;
			return new global::UnityEngine.Vector3(position.x + (float)Width / 2f, 0f, position.z - (float)Depth / 2f);
		}

		public void Init(global::Kampai.Game.Definition definition, global::Kampai.Game.IDefinitionService definitionService)
		{
			base.DefinitionID = definition.ID;
			UpdateFootprint(definitionService);
			base.Init();
		}

		private void UpdateFootprint(global::Kampai.Game.IDefinitionService definitionService)
		{
			global::Kampai.Game.BuildingDefinition buildingDefinition = definitionService.Get<global::Kampai.Game.BuildingDefinition>(base.DefinitionID);
			string buildingFootprint = definitionService.GetBuildingFootprint(buildingDefinition.FootprintID);
			Width = BuildingUtil.GetFootprintWidth(buildingFootprint);
			Depth = BuildingUtil.GetFootprintDepth(buildingFootprint);
			HasSidewalk = buildingFootprint.Contains(".");
		}

		public override void OnDefinitionsHotSwap(global::Kampai.Game.IDefinitionService definitionService)
		{
			base.OnDefinitionsHotSwap(definitionService);
			UpdateFootprint(definitionService);
		}
	}
}
