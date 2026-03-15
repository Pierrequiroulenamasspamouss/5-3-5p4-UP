namespace Kampai.Game
{
	public class LandExpansionBuildingDefinition : global::Kampai.Game.BuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1051;
			}
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.LandExpansionBuilding(this);
		}
	}
}
