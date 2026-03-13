namespace Kampai.Util.AI
{
	public class SteerCharacterToFollowPath : global::Kampai.Util.AI.SteerToAvoidCollisions
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SteerCharacterToFollowPath") as global::Kampai.Util.IKampaiLogger;

		public float Threshold = 0.25f;

		public float FinalThreshold = 0.1f;

		private global::Kampai.Util.KampaiQueue<global::UnityEngine.Vector3> targetQueue;

		private global::UnityEngine.Vector3 currentTarget;

		private global::Kampai.Game.View.CharacterObject obj;

		[Inject]
		public global::Kampai.Game.CharacterArrivedAtDestinationSignal arrivedSignal { get; set; }

		[Inject]
		public global::Kampai.Util.PathFinder pathFinder { get; set; }

		public override global::UnityEngine.Vector3 Force
		{
			get
			{
				global::UnityEngine.Vector3 vector = CalculateForcesFromNeighbors();
				global::UnityEngine.Vector3 vector2 = CalculateForces();
				float num = global::UnityEngine.Vector3.Dot(vector, vector2);
				if (num < 0f)
				{
					return -agent.Velocity * agent.MaxForce;
				}
				return vector + vector2;
			}
		}

		protected override void Start()
		{
			base.Start();
			obj = GetComponentInParent<global::Kampai.Game.View.CharacterObject>();
		}

		private void OnEnable()
		{
			if (agent == null)
			{
				agent = GetComponent<global::Kampai.Util.AI.Agent>();
			}
			if (obj == null)
			{
				obj = GetComponentInParent<global::Kampai.Game.View.CharacterObject>();
			}
		}

		protected override global::UnityEngine.Vector3 CalculateForces()
		{
			if (agent == null || obj == null)
			{
				logger.Error("MISSING Agent/Character in SteerCharacterToFollowPath");
				return global::UnityEngine.Vector3.zero;
			}
			if (targetQueue == null)
			{
				return global::UnityEngine.Vector3.zero;
			}
			if (targetQueue.Count == 0)
			{
				agent.UpdateInterval = 1;
			}
			else
			{
				agent.UpdateInterval = 3;
			}
			global::UnityEngine.Vector3 position = agent.Position;
			float maxForce = agent.MaxForce;
			global::UnityEngine.Vector3 vector = currentTarget - position;
			float magnitude = vector.magnitude;
			float num = ((targetQueue.Count != 0) ? Threshold : FinalThreshold);
			if (magnitude > num)
			{
				if (magnitude > 1.5f)
				{
					global::System.Collections.Generic.IList<global::UnityEngine.Vector3> list = pathFinder.FindPath(position, currentTarget, Modifier);
					if (list != null)
					{
						global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(list);
						if (list2.Count > 2)
						{
							for (int num2 = list2.Count - 1; num2 > 0; num2--)
							{
								targetQueue.AddFirst(list2[num2]);
							}
							do
							{
								currentTarget = targetQueue.Dequeue();
								vector = currentTarget - position;
								magnitude = vector.magnitude;
							}
							while (magnitude < Threshold && targetQueue.Count > 0);
							return vector / magnitude * maxForce;
						}
					}
				}
				global::UnityEngine.Vector3 vector2 = vector / magnitude;
				float num3 = global::UnityEngine.Vector3.Dot(vector2, agent.Forward);
				if (num3 < 0.4f)
				{
					return (vector2 - agent.Velocity) * maxForce;
				}
				return vector2 * maxForce;
			}
			if (arrivedSignal != null && obj != null && targetQueue.Count == 0)
			{
				arrivedSignal.Dispatch(obj.ID);
				base.enabled = false;
				targetQueue = null;
				return global::UnityEngine.Vector3.zero;
			}
			if (targetQueue.Count > 0)
			{
				currentTarget = targetQueue.Dequeue();
				vector = currentTarget - position;
				magnitude = vector.magnitude;
				num = ((targetQueue.Count != 0) ? Threshold : FinalThreshold);
				if (magnitude > num)
				{
					return vector / magnitude * maxForce;
				}
				return global::UnityEngine.Vector3.zero;
			}
			logger.Error("INVALID targetQueue");
			targetQueue = null;
			return global::UnityEngine.Vector3.zero;
		}

		public void SetTarget(global::UnityEngine.Vector3 target)
		{
			global::System.Collections.Generic.IList<global::UnityEngine.Vector3> list = pathFinder.FindPath(agent.Transform.position, target, Modifier);
			if (list == null)
			{
				logger.Error("Unable to path {0} to {1} ({2})", agent.gameObject.name, target, Modifier);
				return;
			}
			targetQueue = new global::Kampai.Util.KampaiQueue<global::UnityEngine.Vector3>(list);
			if (targetQueue != null && targetQueue.Count > 0)
			{
				float magnitude;
				do
				{
					currentTarget = targetQueue.Dequeue();
					magnitude = (currentTarget - agent.Position).magnitude;
				}
				while (magnitude < Threshold && targetQueue.Count > 0);
			}
		}

		private void OnDrawGizmos()
		{
			if (!base.enabled || targetQueue == null)
			{
				return;
			}
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.cyan;
			global::UnityEngine.Gizmos.DrawSphere(currentTarget, 0.1f);
			global::UnityEngine.Gizmos.DrawLine(agent.Position, currentTarget);
			global::UnityEngine.Vector3 vector = currentTarget;
			global::UnityEngine.Gizmos.color = global::UnityEngine.Color.blue;
			foreach (global::UnityEngine.Vector3 item in targetQueue)
			{
				global::UnityEngine.Gizmos.DrawLine(vector, item);
				vector = item;
				global::UnityEngine.Gizmos.DrawSphere(item, 0.1f);
			}
		}
	}
}
