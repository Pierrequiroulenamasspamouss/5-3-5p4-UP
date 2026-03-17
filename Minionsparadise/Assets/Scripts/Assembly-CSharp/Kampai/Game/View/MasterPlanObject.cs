namespace Kampai.Game.View
{
	public class MasterPlanObject : global::Kampai.Game.View.ActionableObject
	{
		private global::UnityEngine.Collider lairCollider;

		public void Init(int masterPlanInstanceID)
		{
			ID = masterPlanInstanceID;
		}

		public global::UnityEngine.Vector3 GetIndicatorPosition()
		{
			if (lairCollider == null)
			{
				lairCollider = base.gameObject.GetComponent<global::UnityEngine.Collider>();
			}
			return new global::UnityEngine.Vector3(lairCollider.bounds.center.x, lairCollider.bounds.max.y, lairCollider.bounds.center.z);
		}
	}
}
