namespace Kampai.Game.View
{
	public class PathFollower : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.TextAsset Path;

		public global::UnityEngine.Color SplineColor = global::UnityEngine.Color.magenta;

		public float Duration = 1f;

		public bool Reverse;

		private GoSpline BuildSpline(global::UnityEngine.Vector3 relativeOffset)
		{
			if (Path == null)
			{
				return null;
			}
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = GoSpline.bytesToVector3List(Path.bytes);
			if (list.Count == 0)
			{
				return null;
			}
			if (Reverse)
			{
				list.Reverse();
			}
			list.Insert(0, global::UnityEngine.Vector3.zero);
			list.Add(global::UnityEngine.Vector3.zero);
			for (int i = 0; i < list.Count; i++)
			{
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list3;
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = (list3 = list);
				int index2;
				int index = (index2 = i);
				global::UnityEngine.Vector3 vector = list3[index2];
				list2[index] = vector + relativeOffset;
			}
			GoSpline goSpline = new GoSpline(list);
			goSpline.buildPath();
			goSpline.closePath();
			return goSpline;
		}

		private void Start()
		{
			GoSpline goSpline = BuildSpline(global::UnityEngine.Vector3.zero);
			if (goSpline != null)
			{
				Go.to(base.transform, Duration, new GoTweenConfig().positionPath(goSpline, true, GoLookAtType.NextPathNode).setIterations(-1, GoLoopType.RestartFromBeginning));
			}
		}
	}
}
