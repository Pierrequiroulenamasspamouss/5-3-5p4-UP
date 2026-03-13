namespace Kampai.Game.Mignette.BalloonBarrage.View
{
	public class BalloonBarrageColliderViewObject : global::UnityEngine.MonoBehaviour
	{
		public enum TargetTypes
		{
			Basket = 0,
			Minion = 1,
			Balloon = 2
		}

		public global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageColliderViewObject.TargetTypes TargetType;

		public global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageTargetAnimatorViewObject ParentTargetBalloonViewObject;
	}
}
