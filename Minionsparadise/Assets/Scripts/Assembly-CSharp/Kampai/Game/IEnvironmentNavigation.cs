namespace Kampai.Game
{
	public interface IEnvironmentNavigation
	{
		bool IsWalkable(int x, int z);

		bool IsOccupied(int x, int z);

		int GetLength(int dimension);
	}
}
