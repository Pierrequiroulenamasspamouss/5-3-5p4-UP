namespace Kampai.Util.AI
{
	public class SteerToAvoidCollisions : global::Kampai.Util.AI.SteerToAvoidEnvironment
	{
		public override int Priority
		{
			get
			{
				return 1;
			}
		}

		protected override global::UnityEngine.Color DebugColor
		{
			get
			{
				return global::UnityEngine.Color.yellow;
			}
		}

		protected override float Strength
		{
			get
			{
				return agent.MaxForce;
			}
		}

		protected override float SideDist
		{
			get
			{
				return 1f;
			}
		}

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

		protected global::UnityEngine.Vector3 CalculateForcesFromNeighbors()
		{
			global::System.Collections.Generic.List<global::Kampai.Util.AI.Agent> list = global::Kampai.Util.AI.Agent.Agents.WithinRange(agent.Position, 4f * agent.Radius);
			if (list.Count < 2)
			{
				return global::UnityEngine.Vector3.zero;
			}
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			global::System.Collections.Generic.List<global::Kampai.Util.AI.Agent>.Enumerator enumerator = list.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					global::Kampai.Util.AI.Agent current = enumerator.Current;
					if (current == agent)
					{
						continue;
					}
					global::UnityEngine.Vector3 lhs = current.Position - agent.Position;
					float num = global::UnityEngine.Vector3.Dot(lhs, agent.Forward);
					float num2 = global::UnityEngine.Vector3.Dot(lhs, agent.Right);
					if (num > 0f)
					{
						if (num2 >= 0f)
						{
							zero += agent.Right * (0f - agent.MaxForce) - agent.Forward;
						}
						else
						{
							zero += agent.Right * agent.MaxForce - agent.Forward;
						}
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			return zero * agent.MaxForce;
		}

		protected override bool Obstacle(int x, int y)
		{
			return !base.environment.IsWalkable(x, y) && base.environment.IsOccupied(x, y);
		}
	}
}
