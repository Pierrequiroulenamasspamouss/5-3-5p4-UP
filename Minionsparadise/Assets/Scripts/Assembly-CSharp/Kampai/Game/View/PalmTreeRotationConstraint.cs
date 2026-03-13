namespace Kampai.Game.View
{
	public class PalmTreeRotationConstraint : global::UnityEngine.MonoBehaviour
	{
		public enum Orientation
		{
			NONE = 0,
			UP = 1,
			DOWN = 2,
			LEFT = 3,
			RIGHT = 4
		}

		public global::UnityEngine.Transform Ground;

		public global::Kampai.Game.View.PalmTreeRotationConstraint.Orientation OrentationDirection;

		public global::UnityEngine.Vector2 RandomXRotationRange;

		public global::UnityEngine.Vector2 RandomYRotationRange;

		public global::UnityEngine.Vector2 RandomZRotationRange;

		private void Start()
		{
			switch (OrentationDirection)
			{
			case global::Kampai.Game.View.PalmTreeRotationConstraint.Orientation.UP:
				base.transform.up = Ground.up;
				break;
			case global::Kampai.Game.View.PalmTreeRotationConstraint.Orientation.DOWN:
				base.transform.up = -Ground.up;
				break;
			case global::Kampai.Game.View.PalmTreeRotationConstraint.Orientation.LEFT:
				base.transform.up = -Ground.right;
				break;
			case global::Kampai.Game.View.PalmTreeRotationConstraint.Orientation.RIGHT:
				base.transform.up = Ground.right;
				break;
			}
			base.transform.Rotate(global::UnityEngine.Random.Range(RandomXRotationRange.x, RandomXRotationRange.y), global::UnityEngine.Random.Range(RandomYRotationRange.x, RandomYRotationRange.y), global::UnityEngine.Random.Range(RandomZRotationRange.x, RandomZRotationRange.y));
		}
	}
}
