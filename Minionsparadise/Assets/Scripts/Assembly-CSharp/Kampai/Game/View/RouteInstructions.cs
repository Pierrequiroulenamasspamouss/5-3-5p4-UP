namespace Kampai.Game.View
{
	public struct RouteInstructions
	{
		public global::Kampai.Game.View.MinionObject minion;

		public global::System.Collections.Generic.IList<global::UnityEngine.Vector3> Path;

		public float Rotation;

		public global::Kampai.Game.Building TargetBuilding;

		public int StartTime;
	}
}
