namespace Kampai.Game
{
	public class DecoGridModel
	{
		private const int POINT_TAKEN = 100;

		private global::System.Collections.Generic.Dictionary<global::Kampai.Util.Point, int> decoGrid = new global::System.Collections.Generic.Dictionary<global::Kampai.Util.Point, int>();

		private readonly global::Kampai.Util.Point North = new global::Kampai.Util.Point(0, 1);

		private readonly global::Kampai.Util.Point East = new global::Kampai.Util.Point(1, 0);

		private readonly global::Kampai.Util.Point South = new global::Kampai.Util.Point(0, -1);

		private readonly global::Kampai.Util.Point West = new global::Kampai.Util.Point(-1, 0);

		private global::Kampai.Util.Point lastPlacedDeco = new global::Kampai.Util.Point(-1, -1);

		public bool AddDeco(int x, int y, int id)
		{
			global::Kampai.Util.Point p = new global::Kampai.Util.Point(x, y);
			return AddDeco(p, id);
		}

		public bool AddDeco(global::Kampai.Util.Point p, int id)
		{
			if (!decoGrid.ContainsKey(p))
			{
				decoGrid[p] = id;
				return true;
			}
			return false;
		}

		public void RemoveDeco(int x, int y)
		{
			global::Kampai.Util.Point key = new global::Kampai.Util.Point(x, y);
			if (decoGrid.ContainsKey(key))
			{
				decoGrid.Remove(key);
			}
		}

		public global::UnityEngine.Vector3 GetNewPieceLocation(int x, int y, int id, global::Kampai.Game.Environment environment)
		{
			global::Kampai.Util.Point point = new global::Kampai.Util.Point(x, y);
			int neighbors = GetNeighbors(point, id, false, environment);
			global::Kampai.Util.Point point2 = new global::Kampai.Util.Point(0, 0);
			if (lastPlacedDeco.x > -1)
			{
				point2 = new global::Kampai.Util.Point(lastPlacedDeco.x - point.x, lastPlacedDeco.y - point.y);
			}
			global::UnityEngine.Vector3 right = global::UnityEngine.Vector3.right;
			global::UnityEngine.Vector3 result;
			switch (neighbors)
			{
			case 15:
				result = right;
				break;
			case 0:
			case 8:
			case 13:
				result = right;
				break;
			case 3:
			case 6:
			case 9:
			case 12:
				if (point2 == North)
				{
					result = global::UnityEngine.Vector3.back;
					break;
				}
				if (point2 == South)
				{
					result = global::UnityEngine.Vector3.forward;
					break;
				}
				if (point2 == East)
				{
					result = global::UnityEngine.Vector3.left;
					break;
				}
				if (point2 == West)
				{
					result = right;
					break;
				}
				switch (neighbors)
				{
				case 9:
				case 12:
					result = right;
					break;
				case 6:
					result = global::UnityEngine.Vector3.forward;
					break;
				default:
					result = global::UnityEngine.Vector3.left;
					break;
				}
				break;
			case 10:
				result = ((!(point2 == West)) ? global::UnityEngine.Vector3.forward : global::UnityEngine.Vector3.back);
				break;
			case 5:
				result = ((!(point2 == South)) ? global::UnityEngine.Vector3.left : right);
				break;
			case 4:
			case 14:
				result = global::UnityEngine.Vector3.forward;
				break;
			case 2:
			case 7:
				result = global::UnityEngine.Vector3.left;
				break;
			case 1:
			case 11:
				result = global::UnityEngine.Vector3.back;
				break;
			default:
				result = right;
				break;
			}
			lastPlacedDeco = point;
			return result;
		}

		private int GetNeighbors(global::Kampai.Util.Point point, int id, bool checkIdentical, global::Kampai.Game.Environment environment = null)
		{
			int num = 0;
			foreach (int value in global::System.Enum.GetValues(typeof(global::Kampai.Game.AdjacentDirection)))
			{
				int neighboringPieceId = GetNeighboringPieceId(point, (global::Kampai.Game.AdjacentDirection)value, environment);
				if ((checkIdentical && neighboringPieceId == id) || (!checkIdentical && neighboringPieceId == 100))
				{
					num |= value;
				}
			}
			return num;
		}

		public global::Kampai.Game.ConnectableBuildingPieceType GetConnectablePieceType(int x, int y, int id, out int outDirection)
		{
			global::Kampai.Util.Point point = new global::Kampai.Util.Point(x, y);
			int neighbors = GetNeighbors(point, id, true);
			outDirection = 0;
			switch (neighbors)
			{
			case 15:
				return global::Kampai.Game.ConnectableBuildingPieceType.CROSS;
			case 7:
			case 11:
			case 13:
			case 14:
				outDirection = RotateTShape(neighbors);
				return global::Kampai.Game.ConnectableBuildingPieceType.TSHAPE;
			case 5:
			case 10:
				outDirection = RotateStraightPiece(neighbors);
				return global::Kampai.Game.ConnectableBuildingPieceType.STRAIGHT;
			case 3:
			case 6:
			case 9:
			case 12:
				outDirection = RotateCorner(neighbors);
				return global::Kampai.Game.ConnectableBuildingPieceType.CORNER;
			case 1:
			case 2:
			case 4:
			case 8:
				outDirection = RotateEndCap(neighbors);
				return global::Kampai.Game.ConnectableBuildingPieceType.ENDCAP;
			case 0:
				return global::Kampai.Game.ConnectableBuildingPieceType.POST;
			default:
				return global::Kampai.Game.ConnectableBuildingPieceType.POST;
			}
		}

		private int RotateTShape(int neighbors)
		{
			switch (neighbors)
			{
			case 14:
				return 0;
			case 13:
				return 90;
			case 11:
				return 180;
			case 7:
				return 270;
			default:
				return 0;
			}
		}

		private int RotateStraightPiece(int neighbors)
		{
			switch (neighbors)
			{
			case 10:
				return 0;
			case 5:
				return 90;
			default:
				return 0;
			}
		}

		private int RotateCorner(int neighbors)
		{
			switch (neighbors)
			{
			case 12:
				return 90;
			case 9:
				return 180;
			case 6:
				return 0;
			case 3:
				return 270;
			default:
				return 0;
			}
		}

		private int RotateEndCap(int neighbors)
		{
			switch (neighbors)
			{
			case 8:
				return 0;
			case 1:
				return 90;
			case 2:
				return 180;
			case 4:
				return 270;
			default:
				return 0;
			}
		}

		public int GetNeighboringPieceId(global::Kampai.Util.Point point, global::Kampai.Game.AdjacentDirection dir, global::Kampai.Game.Environment environment)
		{
			global::Kampai.Util.Point point2;
			switch (dir)
			{
			case global::Kampai.Game.AdjacentDirection.North:
				point2 = North;
				break;
			case global::Kampai.Game.AdjacentDirection.South:
				point2 = South;
				break;
			case global::Kampai.Game.AdjacentDirection.East:
				point2 = East;
				break;
			case global::Kampai.Game.AdjacentDirection.West:
				point2 = West;
				break;
			default:
				return 0;
			}
			int x = point.x + point2.x;
			int num = point.y + point2.y;
			if (environment != null)
			{
				if (!environment.IsUnlocked(x, num) || environment.IsOccupied(x, num))
				{
					return 100;
				}
				return 0;
			}
			point.x = x;
			point.y = num;
			if (!decoGrid.ContainsKey(point))
			{
				return 0;
			}
			return decoGrid[point];
		}
	}
}
