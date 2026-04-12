namespace Kampai.Game
{
	public class WelcomeHutBuildingDefinition : global::Kampai.Game.RepairableBuildingDefinition
	{
		public override int TypeCode
		{
			get
			{
				return 1069;
			}
		}

		public override global::Kampai.Game.Building BuildBuilding()
		{
			return new global::Kampai.Game.WelcomeHutBuilding(this);
		}
	}
}
