namespace Kampai.Util
{
	public interface IPathFinder
	{
		global::System.Collections.Generic.IList<global::UnityEngine.Vector3> FindPath(global::UnityEngine.Vector3 startPos, global::UnityEngine.Vector3 goalPos, int modifier, bool forceDestination = false);

		bool IsOccupiable(global::Kampai.Game.Location location);
	}
}
