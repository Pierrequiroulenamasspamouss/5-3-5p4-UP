namespace Kampai.Game
{
	public interface IBuildingUtilities
	{
		bool ValidateScaffoldingPlacement(global::Kampai.Game.BuildingDefinition buildingDef, global::Kampai.Game.Location location);

		bool ValidateLocation(global::Kampai.Game.Building building, global::Kampai.Game.Location location);

		bool CheckGridBounds(global::Kampai.Game.Location location);

		bool CheckGridBounds(int x, int y);

		bool ValidateGridSquare(global::Kampai.Game.Building building, global::Kampai.Game.Location location);

		bool ValidateGridSquare(global::Kampai.Game.Building building, int x, int y);

		int AvailableLandSpaceCount();
	}
}
