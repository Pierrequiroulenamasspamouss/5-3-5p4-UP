namespace Kampai.Game
{
	public class RelocateCharacterCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RelocateCharacterCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.View.CharacterObject characterObject { get; set; }

		[Inject(global::Kampai.Game.GameElement.RELOCATION_POINTS)]
		public global::System.Collections.Generic.List<global::Kampai.Util.Point> usedRelocationPoints { get; set; }

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner RoutineRunner { get; set; }

		public override void Execute()
		{
			global::UnityEngine.Transform transform = characterObject.gameObject.transform;
			global::Kampai.Util.Point point = global::Kampai.Util.Point.FromVector3(transform.position);
			global::System.Collections.Generic.Queue<global::Kampai.Util.Point> queue = new global::System.Collections.Generic.Queue<global::Kampai.Util.Point>();
			environment.GetClosestWalkableGridSquares(point.x, point.y, 20, queue);
			while (queue.Count > 0)
			{
				point = queue.Dequeue();
				if (!IsOccupied(point))
				{
					if (usedRelocationPoints.Count == 0)
					{
						RoutineRunner.StartCoroutine(ClearPoints(usedRelocationPoints));
					}
					usedRelocationPoints.Add(point);
					transform.position = point.XZProjection;
					logger.Debug("Relocating {0} to ({1}, {2}) {3}", characterObject.name, point.x, point.y, queue.Count);
					return;
				}
			}
			logger.Debug("Gave up relocating {0} to ({1}, {2})", characterObject.name, point.x, point.y);
			transform.position = point.XZProjection;
		}

		private bool IsOccupied(global::Kampai.Util.Point point)
		{
			for (int i = 0; i < usedRelocationPoints.Count; i++)
			{
				if (usedRelocationPoints[i] == point)
				{
					return true;
				}
			}
			global::UnityEngine.Collider[] array = global::UnityEngine.Physics.OverlapSphere(new global::UnityEngine.Vector3(point.x, 1f, point.y), 1f);
			global::UnityEngine.Collider[] array2 = array;
			foreach (global::UnityEngine.Collider collider in array2)
			{
				if (collider.gameObject.GetComponentInParent<global::Kampai.Game.View.CharacterObject>() != null)
				{
					return true;
				}
			}
			return false;
		}

		private global::System.Collections.IEnumerator ClearPoints(global::System.Collections.Generic.List<global::Kampai.Util.Point> points)
		{
			yield return new global::UnityEngine.WaitForEndOfFrame();
			points.Clear();
		}
	}
}
