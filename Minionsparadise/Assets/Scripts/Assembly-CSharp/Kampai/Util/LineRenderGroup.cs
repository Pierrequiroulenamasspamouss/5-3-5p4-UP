namespace Kampai.Util
{
	public class LineRenderGroup : global::UnityEngine.MonoBehaviour
	{
		public global::UnityEngine.Transform pathParent;

		public global::System.Collections.Generic.List<global::UnityEngine.Transform> pathNodes = new global::System.Collections.Generic.List<global::UnityEngine.Transform>();

		public global::UnityEngine.Color pathColor = global::UnityEngine.Color.yellow;

		public bool closePath;

		public global::UnityEngine.LineRenderer lineRndr;

		public bool hideNodesAtRuntime = true;

		private void Awake()
		{
			if (lineRndr == null)
			{
				lineRndr = base.gameObject.GetComponent<global::UnityEngine.LineRenderer>();
			}
			if (lineRndr == null)
			{
				lineRndr = base.gameObject.AddComponent<global::UnityEngine.LineRenderer>();
			}
		}

		private void Start()
		{
			if (pathParent == null)
			{
				pathParent = base.gameObject.transform;
			}
			if (hideNodesAtRuntime)
			{
				global::UnityEngine.Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<global::UnityEngine.Renderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
			}
			GetComponent<global::UnityEngine.Renderer>().enabled = true;
		}

		public global::System.Collections.Generic.List<global::UnityEngine.Vector3> GetPathList()
		{
			global::System.Collections.Generic.List<global::UnityEngine.Vector3> list = new global::System.Collections.Generic.List<global::UnityEngine.Vector3>();
			for (int i = 0; i < pathNodes.Count; i++)
			{
				list.Add(pathNodes[i].position);
			}
			return list;
		}

		public global::UnityEngine.Vector3 GetStartPosition()
		{
			return base.transform.GetChild(0).transform.position;
		}
	}
}
