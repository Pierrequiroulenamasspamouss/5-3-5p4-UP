namespace Kampai.Game
{
	public class Environment : global::Kampai.Game.Instance<global::Kampai.Game.EnvironmentDefinition>, global::Kampai.Game.IEnvironmentNavigation
	{
		[global::Newtonsoft.Json.JsonIgnore]
		public global::Kampai.Game.EnvironmentGridSquare[,] PlayerGrid;

		public int GetLength(int dimension)
		{
			return PlayerGrid.GetLength(dimension);
		}

		public bool Contains(global::Kampai.Util.Point p)
		{
			return Contains(p.x, p.y);
		}

		public bool Contains(int x, int z)
		{
			return x >= 0 && x < PlayerGrid.GetLength(0) && z >= 0 && z < PlayerGrid.GetLength(1);
		}

		public bool IsOccupied(int x, int z)
		{
			return Contains(x, z) && PlayerGrid[x, z].Occupied;
		}

		public bool IsOccupied(global::Kampai.Game.Location location)
		{
			return IsOccupied(location.x, location.y);
		}

		public bool IsUnlocked(int x, int z)
		{
			return Contains(x, z) && PlayerGrid[x, z].Unlocked;
		}

		public bool IsUnlocked(global::Kampai.Game.Location location)
		{
			return IsUnlocked(location.x, location.y);
		}

		public bool IsWalkable(int x, int z)
		{
			return Contains(x, z) && PlayerGrid[x, z].Walkable;
		}

		public bool IsWalkable(global::Kampai.Game.Location location)
		{
			return IsWalkable(location.x, location.y);
		}

		public bool IsCharacterWalkable(int x, int z)
		{
			return Contains(x, z) && PlayerGrid[x, z].CharacterWalkable;
		}

		public bool IsCharacterWalkable(global::Kampai.Game.Location location)
		{
			return IsCharacterWalkable(location.x, location.y);
		}

		public bool CompareModifiers(global::Kampai.Game.Location location, int modifier)
		{
			return CompareModifiers(location.x, location.y, modifier);
		}

		public bool CompareModifiers(int x, int z, int modifier)
		{
			return Contains(x, z) && (PlayerGrid[x, z].Modifier & modifier) == modifier;
		}

		public global::Kampai.Game.Building GetBuilding(int x, int z)
		{
			if (!Contains(x, z))
			{
				return null;
			}
			return PlayerGrid[x, z].Instance as global::Kampai.Game.Building;
		}

		public void GetClosestWalkableGridSquares(int x, int y, int count, global::System.Collections.Generic.Queue<global::Kampai.Util.Point> points)
		{
			GetClosestGridSquare(x, y, count, points, 4);
		}

		public void GetClosestCharacterWalkableGridSquares(int x, int y, int count, global::System.Collections.Generic.Queue<global::Kampai.Util.Point> points)
		{
			GetClosestGridSquare(x, y, count, points, 8);
		}

		public void GetClosestGridSquare(int x, int y, int count, global::System.Collections.Generic.Queue<global::Kampai.Util.Point> points, int modifier)
		{
			int length = PlayerGrid.GetLength(0);
			int length2 = PlayerGrid.GetLength(1);
			int num = global::UnityEngine.Mathf.Max(length, length2);
			for (int i = 0; i <= num; i++)
			{
				for (int j = -i; j <= i; j++)
				{
					for (int k = -i; k <= i; k++)
					{
						int num2 = x + j;
						int num3 = y + k;
						if (num2 < 0 || num2 >= length || num3 < 0 || num3 >= length2)
						{
							continue;
						}
						global::Kampai.Util.Point item = new global::Kampai.Util.Point(num2, num3);
						if (CompareModifiers(num2, num3, modifier) && !points.Contains(item))
						{
							points.Enqueue(item);
							if (points.Count >= count)
							{
								return;
							}
						}
					}
				}
			}
		}

		public global::System.Collections.Generic.Queue<global::Kampai.Util.Point> GetMagnetFingerGridSquares(int x, int y)
		{
			global::Kampai.Util.Point p = new global::Kampai.Util.Point(x, y + 1);
			if (!Contains(p))
			{
				return new global::System.Collections.Generic.Queue<global::Kampai.Util.Point>();
			}
			global::System.Collections.Generic.List<global::Kampai.Util.Tuple<global::Kampai.Util.Point, float>> list = new global::System.Collections.Generic.List<global::Kampai.Util.Tuple<global::Kampai.Util.Point, float>>();
			global::Kampai.Util.Point a = new global::Kampai.Util.Point(x, y);
			for (int i = 0; i < PlayerGrid.GetLength(0); i++)
			{
				for (int j = 0; j < PlayerGrid.GetLength(1); j++)
				{
					if ((i != x && i != x + 1 && i != x - 1) || (j != y && j != y + 1 && j != y - 1))
					{
						global::UnityEngine.Vector2 position = PlayerGrid[i, j].Position;
						global::Kampai.Util.Point point = new global::Kampai.Util.Point(position.x, position.y);
						if (IsWalkable(point.x, point.y))
						{
							global::Kampai.Util.Point b = point;
							b.y--;
							b.x++;
							list.Add(global::Kampai.Util.Tuple.Create(point, global::Kampai.Util.Point.DistanceSquared(a, b)));
						}
					}
				}
			}
			return SortList(list);
		}

		public global::System.Collections.Generic.Queue<global::Kampai.Util.Point> GetMinionPartyGridSquares(int x, int y)
		{
			global::Kampai.Util.Point p = new global::Kampai.Util.Point(x, y + 1);
			if (!Contains(p))
			{
				return new global::System.Collections.Generic.Queue<global::Kampai.Util.Point>();
			}
			global::System.Collections.Generic.List<global::Kampai.Util.Tuple<global::Kampai.Util.Point, float>> list = new global::System.Collections.Generic.List<global::Kampai.Util.Tuple<global::Kampai.Util.Point, float>>();
			global::Kampai.Util.Point a = new global::Kampai.Util.Point(x, y);
			for (int i = 0; i < PlayerGrid.GetLength(0); i++)
			{
				for (int j = 0; j < PlayerGrid.GetLength(1); j++)
				{
					global::UnityEngine.Vector2 position = PlayerGrid[i, j].Position;
					global::Kampai.Util.Point first = new global::Kampai.Util.Point(position.x, position.y);
					if (IsWalkable(first.x, first.y))
					{
						list.Add(global::Kampai.Util.Tuple.Create(first, global::Kampai.Util.Point.DistanceSquared(a, new global::Kampai.Util.Point(first.x + 1, first.y - 1))));
					}
				}
			}
			return SortList(list);
		}

		private global::System.Collections.Generic.Queue<global::Kampai.Util.Point> SortList(global::System.Collections.Generic.List<global::Kampai.Util.Tuple<global::Kampai.Util.Point, float>> gridTuples)
		{
			global::System.Collections.Generic.Queue<global::Kampai.Util.Point> queue = new global::System.Collections.Generic.Queue<global::Kampai.Util.Point>();
			gridTuples.Sort((global::Kampai.Util.Tuple<global::Kampai.Util.Point, float> square1, global::Kampai.Util.Tuple<global::Kampai.Util.Point, float> square2) => square1.Item2.CompareTo(square2.Item2));
			for (int num = 0; num < gridTuples.Count; num++)
			{
				queue.Enqueue(gridTuples[num].Item1);
			}
			return queue;
		}
	}
}
