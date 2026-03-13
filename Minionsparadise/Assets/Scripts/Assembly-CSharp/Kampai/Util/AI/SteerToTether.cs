namespace Kampai.Util.AI
{
	public class SteerToTether : global::Kampai.Util.AI.SteeringBehaviour
	{
		public global::UnityEngine.Vector3 Tether;

		public float MaxDist = 15f;

		public override int Priority
		{
			get
			{
				return 90;
			}
		}

		public override global::UnityEngine.Vector3 Force
		{
			get
			{
				global::UnityEngine.Vector3 result = global::UnityEngine.Vector3.zero;
				global::UnityEngine.Vector3 vector = Tether - agent.Position;
				if (vector.sqrMagnitude > MaxDist * MaxDist)
				{
					result = (vector + agent.Velocity) * 0.5f;
				}
				return result;
			}
		}
	}
}
