namespace Kampai.Editor
{
	public class KampaiSplineTool : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.Color pathColor = global::UnityEngine.Color.magenta;

		public global::System.Collections.Generic.List<global::UnityEngine.Vector3> nodes = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>
		{
			new global::UnityEngine.Vector3(-5f, 0f, -5f),
			new global::UnityEngine.Vector3(5f, 0f, 5f)
		};

		public bool isLoop;

		public bool useStandardHandles;

		public bool initialized;

		public bool saveRelative;

		public void Initialize()
		{
			if (initialized)
			{
				return;
			}
			initialized = true;
			if (base.transform.childCount < 2)
			{
				nodes = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>
				{
					new global::UnityEngine.Vector3(-5f, 0f, -5f),
					new global::UnityEngine.Vector3(5f, 0f, 5f)
				};
				return;
			}
			nodes.Clear();
			global::UnityEngine.Vector3 position = base.transform.position;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				nodes.Add(base.transform.GetChild(i).transform.position - position);
			}
		}

		public global::System.Collections.Generic.List<global::UnityEngine.Vector3> GetPath()
		{
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>(nodes);
			if (isLoop)
			{
				list.Insert(0, global::UnityEngine.Vector3.zero);
				list.Add(global::UnityEngine.Vector3.zero);
			}
			global::UnityEngine.Vector3 position = base.transform.position;
			for (int i = 0; i < list.Count; i++)
			{
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list3;
				global::System.Collections.Generic.List<global::UnityEngine.Vector3> list2 = (list3 = list);
				int index2;
				int index = (index2 = i);
				global::UnityEngine.Vector3 vector = list3[index2];
				list2[index] = vector + position;
			}
			return list;
		}

		public void OnDrawGizmos()
		{
			GoSpline goSpline = new GoSpline(GetPath());
			goSpline.buildPath();
			if (isLoop)
			{
				goSpline.closePath();
			}
			global::UnityEngine.Gizmos.color = pathColor;
			goSpline.drawGizmos(50f);
		}

		private void Update()
		{
			throw new global::System.NotImplementedException(string.Format("Invalid KampaiSplineTool found on {0}.", base.name));
		}
	}
}
