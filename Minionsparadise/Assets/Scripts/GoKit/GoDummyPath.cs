public class GoDummyPath : global::UnityEngine.MonoBehaviour
{
	public string pathName = string.Empty;

	public global::UnityEngine.Color pathColor = global::UnityEngine.Color.magenta;

	public global::System.Collections.Generic.List<global::UnityEngine.Vector3> nodes = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>
	{
		global::UnityEngine.Vector3.zero,
		global::UnityEngine.Vector3.zero
	};

	public bool useStandardHandles;

	public bool forceStraightLinePath;

	public int pathResolution = 50;

	public void OnDrawGizmos()
	{
		if (!forceStraightLinePath)
		{
			GoSpline goSpline = new GoSpline(nodes);
			global::UnityEngine.Gizmos.color = pathColor;
			goSpline.drawGizmos(pathResolution);
		}
	}
}
