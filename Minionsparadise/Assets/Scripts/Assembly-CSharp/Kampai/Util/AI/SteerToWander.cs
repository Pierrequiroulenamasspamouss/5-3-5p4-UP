namespace Kampai.Util.AI
{
	public class SteerToWander : global::Kampai.Util.AI.SteeringBehaviour
	{
		private float wanderScalar;

		public override int Priority
		{
			get
			{
				return 100;
			}
		}

		public override global::UnityEngine.Vector3 Force
		{
			get
			{
				float num = 12f * global::UnityEngine.Time.deltaTime;
				wanderScalar += (global::UnityEngine.Random.value * 2f - 1f) * num;
				wanderScalar = global::UnityEngine.Mathf.Clamp(wanderScalar, -1f, 1f);
				return agent.Right * wanderScalar;
			}
		}
	}
}
