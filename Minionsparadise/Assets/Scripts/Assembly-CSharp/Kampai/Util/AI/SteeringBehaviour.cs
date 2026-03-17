namespace Kampai.Util.AI
{
	[global::UnityEngine.RequireComponent(typeof(global::Kampai.Util.AI.Agent))]
	public abstract class SteeringBehaviour : global::UnityEngine.MonoBehaviour
	{
		protected global::Kampai.Util.AI.Agent agent;

		public abstract int Priority { get; }

		public abstract global::UnityEngine.Vector3 Force { get; }

		protected virtual void Start()
		{
			agent = GetComponent<global::Kampai.Util.AI.Agent>();
		}
	}
}
