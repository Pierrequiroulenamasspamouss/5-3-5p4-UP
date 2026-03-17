namespace Kampai.Game.Mignette.AlligatorSkiing.View
{
	public class AlligatorSkiingMinionHardpointViewObject : global::UnityEngine.MonoBehaviour
	{
		public AlligatorAgent Agent;

		public global::UnityEngine.Transform MinionTriggerVFXMarker;

		public void OnObstacleCollision()
		{
			Agent.OnMinionHitObstacle();
		}

		public void OnCollectableCollision(global::UnityEngine.Vector3 collectablePosition, int collectablePoints, global::Kampai.Game.Mignette.AlligatorSkiing.View.CollectibleType type)
		{
			Agent.OnMinionHitCollectable(collectablePosition, collectablePoints, type);
		}
	}
}
