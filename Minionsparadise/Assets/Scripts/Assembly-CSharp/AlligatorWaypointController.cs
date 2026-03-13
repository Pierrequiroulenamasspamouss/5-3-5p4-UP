public class AlligatorWaypointController : global::UnityEngine.MonoBehaviour
{
	public enum PathType
	{
		Minion = 0,
		Alligator = 1
	}

	public global::UnityEngine.Transform FollowCameraMarker;

	public global::UnityEngine.Transform StartingWaypoint;

	public global::UnityEngine.Transform StartingHampoint;

	public global::UnityEngine.TextAsset AlligatorPath;

	public global::UnityEngine.TextAsset MinionPath;

	private global::System.Collections.Generic.List<global::UnityEngine.Vector3> alligatorPoints;

	private global::System.Collections.Generic.List<global::UnityEngine.Vector3> minionPoints;

	private GoSpline alligatorSpline;

	private GoSpline minionSpline;

	private void Awake()
	{
		InitSplines();
	}

	private void InitSplines()
	{
		alligatorPoints = LoadPoints(AlligatorPath);
		alligatorSpline = new GoSpline(alligatorPoints);
		alligatorSpline.buildPath();
		minionPoints = LoadPoints(MinionPath);
		minionSpline = new GoSpline(minionPoints);
		minionSpline.buildPath();
	}

	private global::System.Collections.Generic.List<global::UnityEngine.Vector3> LoadPoints(global::UnityEngine.TextAsset asset)
	{
		if (asset == null)
		{
			return new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
		}
		return GoSpline.bytesToVector3List(asset.bytes);
	}

	public GoSpline GetMinionSpline()
	{
		return minionSpline;
	}

	public GoSpline GetAlligatorSpline()
	{
		return alligatorSpline;
	}

	public global::UnityEngine.Vector3 GetPositionOnMinionSpline(float t)
	{
		return minionSpline.getPointOnPath(t);
	}
}
