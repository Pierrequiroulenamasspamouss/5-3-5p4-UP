namespace Kampai.Util.AI
{
	public class SteerToSeek : global::Kampai.Util.AI.SteeringBehaviour
	{
		public global::UnityEngine.Vector3 Target;

		public override int Priority
		{
			get
			{
				return 80;
			}
		}

		public override global::UnityEngine.Vector3 Force
		{
			get
			{
				global::UnityEngine.Vector3 vector = Target - agent.Position;
				return vector - agent.Velocity;
			}
		}
	}
}
