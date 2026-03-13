namespace Kampai.Game.View
{
	public class ActorObject<T> : global::Kampai.Game.View.ActionableObject where T : global::Kampai.Game.Instance
	{
		protected global::Kampai.Util.AI.Agent agent;

		protected float defaultMaxSpeed;

		public global::UnityEngine.GameObject BlobShadow { get; set; }

		public virtual void Init(T instance, global::Kampai.Util.IKampaiLogger logger)
		{
			ID = instance.ID;
			base.logger = logger;
			agent = base.gameObject.GetComponent<global::Kampai.Util.AI.Agent>();
			defaultMaxSpeed = agent.MaxSpeed;
		}

		public global::Kampai.Util.AI.Agent GetAgent()
		{
			return agent;
		}

		public void EnableBlobShadow(bool enabled)
		{
			if (BlobShadow != null)
			{
				BlobShadow.SetActive(enabled);
			}
		}

		public override void ExecuteAction(global::Kampai.Game.View.KampaiAction action)
		{
			agent.MaxSpeed = 0f;
			base.ExecuteAction(action);
		}

		public void setLocation(global::UnityEngine.Vector3 position)
		{
			base.transform.position = position;
		}

		public void setRotation(global::UnityEngine.Vector3 rotation)
		{
			base.transform.localEulerAngles = rotation;
		}
	}
}
