namespace Kampai.Util.AI
{
	public class SteerToAvoidNeighbors : global::Kampai.Util.AI.SteeringBehaviour
	{
		public override int Priority
		{
			get
			{
				return 10;
			}
		}

		public override global::UnityEngine.Vector3 Force
		{
			get
			{
				return CalculateForces();
			}
		}

		private global::UnityEngine.Vector3 CalculateForces()
		{
			global::System.Collections.Generic.ICollection<global::Kampai.Util.AI.Agent> collection = global::Kampai.Util.AI.Agent.Agents.WithinRange(agent.Position, 4f * agent.Radius);
			if (collection.Count < 2)
			{
				return global::UnityEngine.Vector3.zero;
			}
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			foreach (global::Kampai.Util.AI.Agent item in collection)
			{
				if (item == agent)
				{
					continue;
				}
				global::UnityEngine.Vector3 lhs = item.Position - agent.Position;
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
			return zero * agent.MaxForce;
		}
	}
}
