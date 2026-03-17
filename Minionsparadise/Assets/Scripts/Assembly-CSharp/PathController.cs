public class PathController : global::UnityEngine.MonoBehaviour
{
	public global::UnityEngine.Transform PathParent;

	public global::UnityEngine.Color TrackEditorColor = global::UnityEngine.Color.cyan;

	private void Start()
	{
	}

	public global::System.Collections.Generic.IList<global::UnityEngine.Vector3> GetPathList()
	{
		global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
		global::UnityEngine.Transform transform = null;
		transform = PathParent;
		if (transform != null)
		{
			foreach (global::UnityEngine.Transform item in transform)
			{
				list.Add(item.position);
			}
		}
		return list;
	}

	public GoSpline GetPathSpline()
	{
		global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = GetPathList() as global::System.Collections.Generic.List<global::UnityEngine.Vector3>;
		GoSpline goSpline = null;
		if (list != null)
		{
			goSpline = new GoSpline(list);
			goSpline.buildPath();
		}
		return goSpline;
	}

	public global::UnityEngine.Vector3 GetPositionOnSpline(float t)
	{
		return GetPathSpline().getPointOnPath(t);
	}
}
