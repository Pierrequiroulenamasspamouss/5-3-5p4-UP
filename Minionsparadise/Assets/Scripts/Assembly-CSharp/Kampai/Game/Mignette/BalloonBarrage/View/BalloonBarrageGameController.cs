namespace Kampai.Game.Mignette.BalloonBarrage.View
{
	public class BalloonBarrageGameController : global::UnityEngine.MonoBehaviour
	{
		[global::System.Serializable]
		public class StaticBasketAndPoints
		{
			public int ScoreValue;

			public global::UnityEngine.GameObject[] BasketLocators;

			public int BasketMaterialIndex;
		}

		public global::UnityEngine.GameObject MignetteObjects;

		public global::UnityEngine.Transform FloatingMinionLocator;

		public global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageGameController.StaticBasketAndPoints[] MinionStaticTargetLocators;

		public global::UnityEngine.GameObject MangoToShowForPrepareThrow;
	}
}
