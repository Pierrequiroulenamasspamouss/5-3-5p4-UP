namespace Kampai.Game
{
	public class RerouteMinionPathsCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RerouteMinionPathsCommand") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.Generic.Queue<global::Kampai.Util.Point> points = new global::System.Collections.Generic.Queue<global::Kampai.Util.Point>();

		[Inject]
		public global::Kampai.Util.Tuple<global::Kampai.Util.Point, global::Kampai.Util.Point> box { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Game.MinionMoveToSignal moveToSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MinionRunToSignal runToSignal { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.View.MinionManagerView component = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
			Init(component);
			global::Kampai.Util.Point item = box.Item1;
			global::Kampai.Util.Point item2 = box.Item2;
			item.x--;
			item.y--;
			item2.x++;
			item2.y++;
			global::System.Collections.Generic.List<global::Kampai.Game.View.ActionableObject> list = new global::System.Collections.Generic.List<global::Kampai.Game.View.ActionableObject>();
			component.GetObjectsInArea(item, item2, list);
			points.Clear();
			environment.GetClosestWalkableGridSquares(item2.x + 1, item.y - 1, list.Count, points);
			bool type = false;
			foreach (global::Kampai.Game.View.MinionObject item3 in list)
			{
				global::Kampai.Game.MinionState state = playerService.GetByInstanceId<global::Kampai.Game.Minion>(item3.ID).State;
				if (state == global::Kampai.Game.MinionState.Tasking || state == global::Kampai.Game.MinionState.Leisure)
				{
					continue;
				}
				global::Kampai.Game.View.PathAction pathAction = item3.currentAction as global::Kampai.Game.View.PathAction;
				if (pathAction == null)
				{
					if (points.Count == 0)
					{
						break;
					}
					runToSignal.Dispatch(item3.ID, points.Dequeue().XZProjection, 5.5f, type);
					type = true;
				}
			}
		}

		private void Init(global::Kampai.Game.View.MinionManagerView mm)
		{
			global::System.Collections.Generic.List<global::Kampai.Util.Tuple<int, global::UnityEngine.Vector3>> list = new global::System.Collections.Generic.List<global::Kampai.Util.Tuple<int, global::UnityEngine.Vector3>>();
			mm.GetPathingObjects(list);
			foreach (global::Kampai.Util.Tuple<int, global::UnityEngine.Vector3> item3 in list)
			{
				int item = item3.Item1;
				global::UnityEngine.Vector3 item2 = item3.Item2;
				global::UnityEngine.GameObject gameObject = mm.GetGameObject(item);
				if (gameObject == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.CMD_REROUTE_MINION, "Null GameObject: ", item);
					break;
				}
				global::Kampai.Game.View.MinionObject component = gameObject.GetComponent<global::Kampai.Game.View.MinionObject>();
				if (component == null)
				{
					logger.Fatal(global::Kampai.Util.FatalCode.CMD_REROUTE_MINION, "Null MinionObject: ", item);
					break;
				}
				global::System.Collections.Generic.IList<global::UnityEngine.Vector3> list2 = pathFinder.FindPath(gameObject.transform.position, item2, 4);
				if (list2 == null || list2.Count == 1)
				{
					component.ReplaceActionsOfType(new global::Kampai.Game.View.AppearAction(component, item2, logger));
				}
				else if (component.currentAction is global::Kampai.Game.View.ConstantSpeedPathAction)
				{
					global::Kampai.Game.View.ConstantSpeedPathAction constantSpeedPathAction = component.currentAction as global::Kampai.Game.View.ConstantSpeedPathAction;
					float speed = constantSpeedPathAction.Speed;
					component.ReplaceActionsOfType(new global::Kampai.Game.View.ConstantSpeedPathAction(component, list2, speed, logger));
				}
				else if (component.currentAction is global::Kampai.Game.View.PathAction)
				{
					global::Kampai.Game.View.PathAction pathAction = component.currentAction as global::Kampai.Game.View.PathAction;
					float time = pathAction.RemainingTime();
					component.ReplaceActionsOfType(new global::Kampai.Game.View.PathAction(component, list2, time, logger));
				}
				else
				{
					moveToSignal.Dispatch(item, item2, false);
				}
			}
		}
	}
}
