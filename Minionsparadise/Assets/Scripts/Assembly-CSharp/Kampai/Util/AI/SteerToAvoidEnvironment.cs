namespace Kampai.Util.AI
{
	public class SteerToAvoidEnvironment : global::Kampai.Util.AI.SteeringBehaviour
	{
		private int modifier = 4;

		private global::System.Collections.Generic.Queue<global::Kampai.Util.Point> points = new global::System.Collections.Generic.Queue<global::Kampai.Util.Point>();

		[Inject]
		public global::Kampai.Game.Environment environment { get; set; }

		protected virtual global::UnityEngine.Color DebugColor
		{
			get
			{
				return global::UnityEngine.Color.white;
			}
		}

		protected virtual float Strength
		{
			get
			{
				return agent.MaxForce * 0.5f;
			}
		}

		protected virtual float SideDist
		{
			get
			{
				return 0.5f;
			}
		}

		public virtual int Modifier
		{
			get
			{
				return modifier;
			}
			set
			{
				modifier = value;
			}
		}

		public override int Priority
		{
			get
			{
				return 2;
			}
		}

		public override global::UnityEngine.Vector3 Force
		{
			get
			{
				return CalculateForces();
			}
		}

		protected virtual global::UnityEngine.Vector3 CalculateForces()
		{
			global::Kampai.Util.Point point = global::Kampai.Util.Point.FromVector3(agent.Position);
			if (Obstacle(point.x, point.y))
			{
				points.Clear();
				environment.GetClosestGridSquare(point.x, point.y, 1, points, modifier);
				return (points.Dequeue().XZProjection - agent.Position) * Strength;
			}
			global::Kampai.Util.Point hitPoint = global::Kampai.Util.Point.FromVector3(agent.Position - 0.25f * agent.Right);
			global::Kampai.Util.Point hitPoint2 = global::Kampai.Util.Point.FromVector3(agent.Position - 0.25f * agent.Right);
			if (Obstacle(hitPoint.x, hitPoint.y))
			{
				return Hit(1f, hitPoint);
			}
			if (Obstacle(hitPoint2.x, hitPoint2.y))
			{
				return Hit(-1f, hitPoint2);
			}
			global::UnityEngine.Vector3 vector = SideDist * agent.Radius * agent.Right;
			global::Kampai.Util.Point point2 = global::Kampai.Util.Point.FromVector3(agent.Position - vector + agent.Forward);
			global::Kampai.Util.Point point3 = global::Kampai.Util.Point.FromVector3(agent.Position + vector + agent.Forward);
			if (point2 == point)
			{
				point2 = global::Kampai.Util.Point.FromVector3(agent.Position - vector + agent.Forward * 1.5f);
			}
			if (point3 == point)
			{
				point3 = global::Kampai.Util.Point.FromVector3(agent.Position + vector + agent.Forward * 1.5f);
			}
			if (Obstacle(point2.x, point2.y))
			{
				return Hit(1f, point2);
			}
			if (Obstacle(point3.x, point3.y))
			{
				return Hit(-1f, point3);
			}
			return global::UnityEngine.Vector3.zero;
		}

		private global::UnityEngine.Vector3 Hit(float hint, global::Kampai.Util.Point hitPoint)
		{
			return agent.Right * hint * Strength;
		}

		protected virtual bool Obstacle(int x, int y)
		{
			return !environment.CompareModifiers(x, y, modifier) || environment.Definition.IsWater(x, y);
		}
	}
}
