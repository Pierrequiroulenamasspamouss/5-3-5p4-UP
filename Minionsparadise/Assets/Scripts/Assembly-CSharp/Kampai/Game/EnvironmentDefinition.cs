namespace Kampai.Game
{
	public class EnvironmentDefinition : global::Kampai.Game.Definition
	{
		[global::Kampai.Util.FastDeserializerIgnore]
		public global::Kampai.Game.EnvironmentGridSquareDefinition[,] DefinitionGrid;

		public override int TypeCode
		{
			get
			{
				return 1086;
			}
		}

		public bool IsUsable(int x, int z)
		{
			return DefinitionGrid[x, z].Usable;
		}

		public bool IsUsable(global::Kampai.Game.Location location)
		{
			return IsUsable(location.x, location.y);
		}

		public bool IsWater(int x, int z)
		{
			return DefinitionGrid[x, z].Water;
		}

		public bool IsWater(global::Kampai.Game.Location location)
		{
			return IsWater(location.x, location.y);
		}
	}
}
