namespace Kampai.Game.Mignette.AlligatorSkiing.View
{
	public class AlligatorSkiingObstacleViewObject : global::UnityEngine.MonoBehaviour
	{
		public bool IsCollectable;

		public global::Kampai.Game.Mignette.AlligatorSkiing.View.CollectibleType CollectibleType;

		public bool DestroyOnCollision = true;

		public global::UnityEngine.GameObject ImpactVFX;

		public global::UnityEngine.Transform VFXPositionHolder;

		public int CollectablePoints = 10;

		public bool CollectableObstacleCancel;

		private global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMinionHardpointViewObject minionHardpoint;

		private void OnTriggerEnter(global::UnityEngine.Collider other)
		{
			minionHardpoint = other.GetComponent<global::Kampai.Game.Mignette.AlligatorSkiing.View.AlligatorSkiingMinionHardpointViewObject>();
			if (minionHardpoint != null)
			{
				if (IsCollectable && !CollectableObstacleCancel)
				{
					HandleCollectable();
					minionHardpoint = null;
				}
				else if (!IsCollectable)
				{
					minionHardpoint.OnObstacleCollision();
					minionHardpoint = null;
				}
			}
		}

		private void OnTriggerExit(global::UnityEngine.Collider other)
		{
			if (minionHardpoint != null && IsCollectable && CollectableObstacleCancel && !minionHardpoint.Agent.View.IsOnPenalty)
			{
				HandleCollectable();
				minionHardpoint = null;
			}
		}

		private void HandleCollectable()
		{
			global::UnityEngine.Vector3 vector = ((!(VFXPositionHolder != null)) ? base.transform.position : VFXPositionHolder.position);
			minionHardpoint.OnCollectableCollision(vector, CollectablePoints, CollectibleType);
			if (!minionHardpoint.Agent.View.IsOnPenalty)
			{
				if (DestroyOnCollision)
				{
					global::UnityEngine.Object.Destroy(base.gameObject);
				}
				if (ImpactVFX != null)
				{
					global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(ImpactVFX);
					gameObject.transform.position = vector;
				}
			}
		}
	}
}
