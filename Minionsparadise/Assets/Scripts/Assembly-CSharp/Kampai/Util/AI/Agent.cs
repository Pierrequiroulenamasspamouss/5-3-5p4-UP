namespace Kampai.Util.AI
{
	public class Agent : global::UnityEngine.MonoBehaviour, global::Kampai.Util.SpatiallySortable
	{
		public const int DEFAULT_UPDATE_INTERVAL = 3;

		[global::UnityEngine.SerializeField]
		protected float mass = 1f;

		[global::UnityEngine.SerializeField]
		protected float radius = 0.5f;

		[global::UnityEngine.SerializeField]
		protected float maxForce = 8f;

		[global::UnityEngine.SerializeField]
		protected float maxSpeed = 2f;

		private float speed;

		private global::UnityEngine.Vector3 forward = global::UnityEngine.Vector3.forward;

		private global::UnityEngine.Vector3 right = global::UnityEngine.Vector3.right;

		protected global::Kampai.Util.AI.SteeringBehaviour[] steeringBehaviours;

		private float stuckTime;

		private static global::System.Collections.Generic.List<global::Kampai.Util.AI.Agent> agentList = new global::System.Collections.Generic.List<global::Kampai.Util.AI.Agent>();

		public static global::Kampai.Util.KDTree<global::Kampai.Util.AI.Agent> Agents;

		private static int lastUpdateFrame = -1;

		private global::System.Collections.Generic.Queue<global::Kampai.Util.Point> points = new global::System.Collections.Generic.Queue<global::Kampai.Util.Point>();

		private global::UnityEngine.Vector3 PrevForce = global::UnityEngine.Vector3.zero;

		private int updateInterval = 3;

		private int InstanceID;

		public float Mass
		{
			get
			{
				return mass;
			}
			set
			{
				mass = value;
			}
		}

		public float Radius
		{
			get
			{
				return radius;
			}
			set
			{
				radius = value;
			}
		}

		public float MaxForce
		{
			get
			{
				return maxForce;
			}
			set
			{
				maxForce = value;
			}
		}

		public float MaxSpeed
		{
			get
			{
				return maxSpeed;
			}
			set
			{
				maxSpeed = value;
			}
		}

		public global::UnityEngine.Vector3 Position { get; protected set; }

		public float Speed
		{
			get
			{
				return speed;
			}
			protected set
			{
				speed = value;
				Velocity = Forward * speed;
			}
		}

		public global::UnityEngine.Vector3 Velocity { get; private set; }

		public global::UnityEngine.Vector3 Acceleration { get; protected set; }

		public global::UnityEngine.Transform Transform { get; private set; }

		public global::UnityEngine.Vector3 Forward
		{
			get
			{
				return forward;
			}
			protected set
			{
				forward = value;
				right = global::UnityEngine.Vector3.Cross(global::UnityEngine.Vector3.up, forward);
				Velocity = forward * speed;
			}
		}

		public global::UnityEngine.Vector3 Right
		{
			get
			{
				return right;
			}
			protected set
			{
				right = value;
				forward = global::UnityEngine.Vector3.Cross(right, global::UnityEngine.Vector3.up);
				Velocity = forward * speed;
			}
		}

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		public int UpdateInterval
		{
			get
			{
				return updateInterval;
			}
			set
			{
				if (value > 0)
				{
					updateInterval = value;
				}
			}
		}

		private void OnEnable()
		{
			agentList.Add(this);
			if (Transform == null)
			{
				Transform = GetComponent<global::UnityEngine.Transform>();
			}
			Position = Transform.position;
			Forward = VectorUtils.ZeroY(Transform.forward);
		}

		private void OnDisable()
		{
			agentList.Remove(this);
		}

		protected virtual void Start()
		{
			Transform = GetComponent<global::UnityEngine.Transform>();
			Transform.parent = null;
			UpdateSteeringBehaviours();
			Speed = MaxSpeed;
			InstanceID = base.gameObject.GetInstanceID();
		}

		public void UpdateSteeringBehaviours()
		{
			steeringBehaviours = GetComponents<global::Kampai.Util.AI.SteeringBehaviour>();
			global::System.Array.Sort(steeringBehaviours, (global::Kampai.Util.AI.SteeringBehaviour a, global::Kampai.Util.AI.SteeringBehaviour b) => a.Priority.CompareTo(b.Priority));
		}

		protected virtual void Update()
		{
			if (lastUpdateFrame != global::UnityEngine.Time.frameCount)
			{
				if (Agents == null)
				{
					Agents = new global::Kampai.Util.KDTree<global::Kampai.Util.AI.Agent>(agentList);
				}
				else
				{
					Agents.Rebuild(agentList);
				}
				lastUpdateFrame = global::UnityEngine.Time.frameCount;
			}
			Position = Transform.position;
			Forward = VectorUtils.ZeroY(Transform.forward);
			if (MaxSpeed > 0f)
			{
				global::Kampai.Util.Point p = global::Kampai.Util.Point.FromVector3(Position);
				if (environment.IsOccupied(p.x, p.y) && !environment.IsWalkable(p.x, p.y))
				{
					MoveToNavigable(p);
				}
				else
				{
					UpdateNormally(p);
				}
			}
			else
			{
				Acceleration = global::UnityEngine.Vector3.zero;
				Speed = 0f;
			}
		}

		protected virtual global::UnityEngine.Vector3 CalculateForces(float deltaTime)
		{
			global::UnityEngine.Vector3 vector = global::UnityEngine.Vector3.zero;
			if (PrevForce == global::UnityEngine.Vector3.zero || (global::UnityEngine.Time.frameCount + InstanceID) % UpdateInterval == 0)
			{
				for (int i = 0; i < steeringBehaviours.Length; i++)
				{
					global::Kampai.Util.AI.SteeringBehaviour steeringBehaviour = steeringBehaviours[i];
					if (steeringBehaviour.enabled && 0.1f < global::UnityEngine.Random.value)
					{
						global::UnityEngine.Vector3 force = steeringBehaviour.Force;
						if (force != global::UnityEngine.Vector3.zero)
						{
							vector += force;
							PrevForce = vector;
							break;
						}
					}
				}
			}
			else
			{
				vector = PrevForce;
			}
			vector.y = 0f;
			return vector;
		}

		protected virtual void ApplyForce(global::UnityEngine.Vector3 force, float deltaTime)
		{
			global::UnityEngine.Vector3 v = AdjustRawForce(force);
			global::UnityEngine.Vector3 vector = v.Truncate(MaxForce);
			global::UnityEngine.Vector3 b = vector / Mass;
			global::UnityEngine.Vector3 velocity = Velocity;
			if (deltaTime > 0f)
			{
				float value = global::UnityEngine.Mathf.Clamp(9f * deltaTime, 0.15f, 0.4f);
				Acceleration = global::UnityEngine.Vector3.Lerp(Acceleration, b, global::UnityEngine.Mathf.Clamp01(value));
			}
			velocity += Acceleration * deltaTime;
			velocity = velocity.Truncate(MaxSpeed);
			speed = velocity.magnitude;
			Position += velocity * deltaTime;
			if (Speed > 0f)
			{
				Forward = velocity.normalized;
			}
		}

		protected virtual global::UnityEngine.Vector3 AdjustRawForce(global::UnityEngine.Vector3 force)
		{
			if (MaxSpeed <= 0.0001f)
			{
				return global::UnityEngine.Vector3.zero;
			}
			float num = 0.2f * MaxSpeed;
			if (Speed > num || force == global::UnityEngine.Vector3.zero)
			{
				return force;
			}
			float f = Speed / num;
			float cosMaxAngle = global::UnityEngine.Mathf.Lerp(1f, -1f, global::UnityEngine.Mathf.Pow(f, 20f));
			return LimitVectorDeviation(force, cosMaxAngle, Forward, Right, true);
		}

		private void MoveToNavigable(global::Kampai.Util.Point p)
		{
			points.Clear();
			environment.GetClosestWalkableGridSquares(p.x, p.y, 1, points);
			speed = maxSpeed;
			Forward = (points.Dequeue().XZProjection - Position).normalized;
			Position += Velocity * global::UnityEngine.Time.deltaTime;
			UpdateTransform();
		}

		private void UpdateNormally(global::Kampai.Util.Point p)
		{
			global::UnityEngine.Vector3 force = CalculateForces(global::UnityEngine.Time.deltaTime);
			ApplyForce(force, global::UnityEngine.Time.deltaTime);
			p = global::Kampai.Util.Point.FromVector3(Position);
			if (environment.IsWalkable(p.x, p.y) && !environment.Definition.IsWater(p.x, p.y))
			{
				UpdateTransform();
				return;
			}
			if (global::UnityEngine.Time.time - stuckTime > 2f)
			{
				MoveToNavigable(p);
				return;
			}
			Position = Transform.position;
			Speed = 0f;
			Acceleration = global::UnityEngine.Vector3.zero;
			Forward = -Forward;
			UpdateTransform();
		}

		private global::UnityEngine.Vector3 LimitVectorDeviation(global::UnityEngine.Vector3 source, float cosMaxAngle, global::UnityEngine.Vector3 basis, global::UnityEngine.Vector3 orthoBasis, bool interiorConstraint)
		{
			float magnitude = source.magnitude;
			if (magnitude < 1E-05f)
			{
				return source;
			}
			global::UnityEngine.Vector3 lhs = source / magnitude;
			float num = global::UnityEngine.Vector3.Dot(lhs, basis);
			if (num == -1f)
			{
				return source + orthoBasis * 0.1f;
			}
			if (interiorConstraint)
			{
				if (num >= cosMaxAngle)
				{
					return source;
				}
			}
			else if (num <= cosMaxAngle)
			{
				return source;
			}
			float num2 = global::UnityEngine.Vector3.Dot(source, basis);
			global::UnityEngine.Vector3 vector = basis * num2;
			global::UnityEngine.Vector3 vector2 = source - vector;
			vector2.Normalize();
			float num3 = global::UnityEngine.Mathf.Sqrt(1f - cosMaxAngle * cosMaxAngle);
			global::UnityEngine.Vector3 vector3 = basis * cosMaxAngle;
			global::UnityEngine.Vector3 vector4 = vector2 * num3;
			return (vector3 + vector4) * magnitude;
		}

		private void UpdateTransform()
		{
			Transform.position = Position;
			Transform.forward = Forward;
		}

		private void DebugDraw()
		{
			global::Kampai.Util.DebugUtil.DrawXZCircle(Position, Radius);
			global::UnityEngine.Debug.DrawLine(Position, Position + Velocity * (3f / MaxSpeed), new global::UnityEngine.Color(0.4f, 0.4f, 1f, 1f));
			global::UnityEngine.Debug.DrawLine(Position, Position + Acceleration * (3f / MaxForce), new global::UnityEngine.Color(1f, 0.4f, 1f, 1f));
			if (global::UnityEngine.Time.frameCount % 20 == 0)
			{
				global::UnityEngine.Debug.DrawLine(Position, Position - Velocity * 0.1f, global::UnityEngine.Color.gray, 2f);
			}
		}
	}
}
