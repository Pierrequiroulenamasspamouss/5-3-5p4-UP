namespace Kampai.Game
{
	public class MasterPlanLeftOverBuildingDefinition : global::Kampai.Game.BuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1055;
			}
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.MasterPlanLeftOverBuilding(this);
		}
	}
}
