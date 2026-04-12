namespace Kampai.Util
{
	public class PathFinder : global::Kampai.Util.IPathFinder
	{
		private enum MoveAction
		{
			Invalid = 0,
			North = 1,
			NorthEast = 2,
			East = 3,
			SouthEast = 4,
			South = 5,
			SouthWest = 6,
			West = 7,
			NorthWest = 8,
			Init = 9
		}

		private global::Kampai.Game.Environment environment;

		private global::Kampai.Util.ScatterList<global::Kampai.Util.Point> partyPoints = new global::Kampai.Util.ScatterList<global::Kampai.Util.Point>(100);

		private global::Kampai.Util.ScatterList<global::Kampai.Util.Point> walkingPoints = new global::Kampai.Util.ScatterList<global::Kampai.Util.Point>(100);

		private global::System.Collections.Generic.List<global::Kampai.Util.Point> stageBuildingPoints = new global::System.Collections.Generic.List<global::Kampai.Util.Point>();

		private float[,] moveScore;

		private global::Kampai.Util.PathFinder.MoveAction[,] lastAction;

		private global::Kampai.Util.SimplePriorityQueue<global::Kampai.Util.Point> moveQueue;

		private int dimX;

		private int dimY;

		private bool allowWalkableUpdate;

		private global::Kampai.Common.IRandomService randomService;

		private global::Kampai.Game.MinionPartyDefinition minionPartyDefinition;

		private global::Kampai.Game.StageBuildingDefinition stageBuildingDefinition;

		public global::Kampai.Util.Point RandomPoint
		{
			get
			{
				return walkingPoints.Pick(randomService);
			}
		}

		public global::Kampai.Util.Point PartyPoint
		{
			get
			{
				return partyPoints.Pick(randomService);
			}
		}

		[Construct]
		public PathFinder(global::Kampai.Game.Environment env, global::Kampai.Common.IRandomService randomService, global::Kampai.Game.IDefinitionService definitionService)
		{
			this.randomService = randomService;
			minionPartyDefinition = definitionService.Get<global::Kampai.Game.MinionPartyDefinition>(80000);
			stageBuildingDefinition = definitionService.Get<global::Kampai.Game.StageBuildingDefinition>(3054);
			SetEnvironment(env);
		}

		public void SetEnvironment(global::Kampai.Game.Environment env)
		{
			environment = env;
			dimX = environment.PlayerGrid.GetLength(0);
			dimY = environment.PlayerGrid.GetLength(1);
			moveScore = new float[dimX, dimY];
			lastAction = new global::Kampai.Util.PathFinder.MoveAction[dimX, dimY];
			moveQueue = new global::Kampai.Util.SimplePriorityQueue<global::Kampai.Util.Point>();
			UpdateWalkableRegion();
		}

		public void AllowWalkableUpdates()
		{
			allowWalkableUpdate = true;
		}

		public void UpdateWalkableRegion()
		{
			if (!allowWalkableUpdate)
			{
				return;
			}
			walkingPoints.Clear();
			stageBuildingPoints.Clear();
			for (int i = 0; i < dimX; i++)
			{
				for (int j = 0; j < dimY; j++)
				{
					if (environment.IsWalkable(i, j))
					{
						global::Kampai.Util.Point point = new global::Kampai.Util.Point(i, j);
						walkingPoints.Add(point);
						if (minionPartyDefinition.Contains(point))
						{
							partyPoints.Add(point);
						}
						if (stageBuildingDefinition.Contains(point))
						{
							stageBuildingPoints.Add(point);
						}
					}
				}
			}
		}

		public global::UnityEngine.Vector3 RandomPosition(bool party)
		{
			global::UnityEngine.Vector3 xZProjection = ((!party) ? RandomPoint : PartyPoint).XZProjection;
			if (!party)
			{
				xZProjection.x -= randomService.NextFloat() - 0.5f;
				xZProjection.z -= randomService.NextFloat() - 0.5f;
			}
			return xZProjection;
		}

		public void ShuffleLists()
		{
			walkingPoints.ShuffleContents(randomService);
			partyPoints.ShuffleContents(randomService);
		}

		public global::UnityEngine.Vector3 GetStageBuildingPosition(int index)
		{
			if (index > stageBuildingPoints.Count)
			{
				return RandomPoint.XZProjection;
			}
			global::UnityEngine.Vector3 xZProjection = stageBuildingPoints[index].XZProjection;
			xZProjection.x += randomService.NextFloat(0f - stageBuildingDefinition.maxMinionOffsetX, stageBuildingDefinition.maxMinionOffsetX);
			xZProjection.z += randomService.NextFloat(0f - stageBuildingDefinition.maxMinionOffsetY, stageBuildingDefinition.maxMinionOffsetY);
			return xZProjection;
		}

		public int GetStageBuildingCapacity()
		{
			return stageBuildingPoints.Count;
		}

		public global::System.Collections.Generic.IList<global::UnityEngine.Vector3> RandomPath(global::UnityEngine.Vector3 startPos, bool party, int mask)
		{
			return FindPath(startPos, RandomPosition(party), mask);
		}

		public global::System.Collections.Generic.IList<global::UnityEngine.Vector3> FindPath(global::UnityEngine.Vector3 startPos, global::UnityEngine.Vector3 goalPos, int modifier, bool forceDestination = false)
		{
			global::Kampai.Util.Point point = new global::Kampai.Util.Point(global::UnityEngine.Mathf.RoundToInt(startPos.x), global::UnityEngine.Mathf.RoundToInt(startPos.z));
			global::Kampai.Util.Point point2 = new global::Kampai.Util.Point(global::UnityEngine.Mathf.RoundToInt(goalPos.x), global::UnityEngine.Mathf.RoundToInt(goalPos.z));
			bool flag = false;
			if (!environment.CompareModifiers(point.x, point.y, modifier))
			{
				global::System.Collections.Generic.List<global::Kampai.Util.Point> list = global::Kampai.Util.Bresenham.Line(point, point2) as global::System.Collections.Generic.List<global::Kampai.Util.Point>;
				foreach (global::Kampai.Util.Point item in list)
				{
					if (environment.CompareModifiers(item.x, item.y, modifier))
					{
						point = item;
						break;
					}
				}
				flag = true;
			}
			bool flag2 = false;
			if (forceDestination && !environment.CompareModifiers(point2.x, point2.y, modifier))
			{
				global::System.Collections.Generic.List<global::Kampai.Util.Point> list2 = global::Kampai.Util.Bresenham.Line(point2, point) as global::System.Collections.Generic.List<global::Kampai.Util.Point>;
				foreach (global::Kampai.Util.Point item2 in list2)
				{
					if (environment.CompareModifiers(item2.x, item2.y, modifier))
					{
						point2 = item2;
						break;
					}
				}
				flag2 = true;
			}
			if (point == point2)
			{
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list3;
				if (flag2)
				{
					list3 = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
					list3.Add(startPos);
					return list3;
				}
				list3 = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
				list3.Add(startPos);
				list3.Add(goalPos);
				return list3;
			}
			Reset();
			lastAction[point.x, point.y] = global::Kampai.Util.PathFinder.MoveAction.Init;
			moveScore[point.x, point.y] = 0f;
			moveQueue.Enqueue(point, global::UnityEngine.Mathf.RoundToInt(global::Kampai.Util.Point.Distance(point, point2)));
			while (moveQueue.Count > 0)
			{
				global::Kampai.Util.Point point3 = moveQueue.Dequeue();
				float num = moveScore[point3.x, point3.y];
				if (point3 == point2)
				{
					global::System.Collections.Generic.List<global::UnityEngine.Vector3> list4 = ReconstructPath(point2);
					if (flag)
					{
						list4.Insert(0, startPos);
					}
					else
					{
						list4[0] = startPos;
					}
					if (!flag2)
					{
						list4[list4.Count - 1] = goalPos;
					}
					return list4;
				}
				float nextScore = num + 1f;
				EnqueueMove(point3.x, point3.y + 1, nextScore, global::Kampai.Util.PathFinder.MoveAction.North, point2, modifier);
				EnqueueMove(point3.x + 1, point3.y, nextScore, global::Kampai.Util.PathFinder.MoveAction.East, point2, modifier);
				EnqueueMove(point3.x, point3.y - 1, nextScore, global::Kampai.Util.PathFinder.MoveAction.South, point2, modifier);
				EnqueueMove(point3.x - 1, point3.y, nextScore, global::Kampai.Util.PathFinder.MoveAction.West, point2, modifier);
				nextScore = num + 1.4f;
				EnqueueMove(point3.x + 1, point3.y + 1, nextScore, global::Kampai.Util.PathFinder.MoveAction.NorthEast, point2, modifier);
				EnqueueMove(point3.x - 1, point3.y + 1, nextScore, global::Kampai.Util.PathFinder.MoveAction.NorthWest, point2, modifier);
				EnqueueMove(point3.x + 1, point3.y - 1, nextScore, global::Kampai.Util.PathFinder.MoveAction.SouthEast, point2, modifier);
				EnqueueMove(point3.x - 1, point3.y - 1, nextScore, global::Kampai.Util.PathFinder.MoveAction.SouthWest, point2, modifier);
			}
			if (forceDestination)
			{
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list3 = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
				list3.Add(goalPos);
				return list3;
			}
			return null;
		}

		private void Reset()
		{
			global::System.Array.Clear(lastAction, 0, dimX * dimY);
			global::System.Array.Clear(moveScore, 0, dimX * dimY);
			moveQueue.Clear();
		}

		private global::System.Collections.Generic.List<global::UnityEngine.Vector3> ReconstructPath(global::Kampai.Util.Point goal)
		{
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
			list.Insert(0, goal.XZProjection);
			global::Kampai.Util.Point point = goal;
			for (global::Kampai.Util.PathFinder.MoveAction moveAction = lastAction[point.x, point.y]; moveAction != global::Kampai.Util.PathFinder.MoveAction.Init; moveAction = lastAction[point.x, point.y])
			{
				lastAction[point.x, point.y] = global::Kampai.Util.PathFinder.MoveAction.Invalid;
				switch (moveAction)
				{
				case global::Kampai.Util.PathFinder.MoveAction.North:
					point.y--;
					break;
				case global::Kampai.Util.PathFinder.MoveAction.NorthEast:
					point.x--;
					point.y--;
					break;
				case global::Kampai.Util.PathFinder.MoveAction.East:
					point.x--;
					break;
				case global::Kampai.Util.PathFinder.MoveAction.SouthEast:
					point.x--;
					point.y++;
					break;
				case global::Kampai.Util.PathFinder.MoveAction.South:
					point.y++;
					break;
				case global::Kampai.Util.PathFinder.MoveAction.SouthWest:
					point.x++;
					point.y++;
					break;
				case global::Kampai.Util.PathFinder.MoveAction.West:
					point.x++;
					break;
				case global::Kampai.Util.PathFinder.MoveAction.NorthWest:
					point.x++;
					point.y--;
					break;
				default:
					return null;
				}
				list.Insert(0, point.XZProjection);
			}
			return list;
		}

		private void EnqueueMove(int x, int y, float nextScore, global::Kampai.Util.PathFinder.MoveAction action, global::Kampai.Util.Point goal, int modifier)
		{
			if (x >= 0 && x < dimX && y >= 0 && y < dimY && environment.CompareModifiers(x, y, modifier) && lastAction[x, y] == global::Kampai.Util.PathFinder.MoveAction.Invalid)
			{
				lastAction[x, y] = action;
				moveScore[x, y] = nextScore;
				global::Kampai.Util.Point point = new global::Kampai.Util.Point(x, y);
				moveQueue.Enqueue(point, global::UnityEngine.Mathf.RoundToInt(nextScore + global::Kampai.Util.Point.Distance(point, goal)));
			}
		}

		public bool IsOccupiable(global::Kampai.Game.Location location)
		{
			return !environment.IsOccupied(location);
		}
	}
}
