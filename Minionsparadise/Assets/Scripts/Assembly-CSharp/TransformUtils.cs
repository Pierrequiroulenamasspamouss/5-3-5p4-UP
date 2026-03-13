public static class TransformUtils
{
	public static global::UnityEngine.GameObject FindChild(this global::UnityEngine.GameObject gameObject, string name)
	{
		global::UnityEngine.GameObject gameObject2 = null;
		foreach (global::UnityEngine.Transform item in gameObject.transform)
		{
			if (gameObject2 != null)
			{
				break;
			}
			if (item.gameObject.name.Equals(name))
			{
				gameObject2 = item.gameObject;
			}
			else if (item.childCount > 0)
			{
				gameObject2 = item.gameObject.FindChild(name);
			}
		}
		return gameObject2;
	}

	public static T GetComponentTypeInParent<T>(this global::UnityEngine.Transform gameObject) where T : global::UnityEngine.Component
	{
		if (gameObject == null || gameObject.transform == null)
		{
			return (T)null;
		}
		global::UnityEngine.Transform parent = gameObject.transform.parent;
		T val = (T)null;
		while (parent != null)
		{
			val = parent.GetComponent<T>();
			if (val != null)
			{
				return val;
			}
			parent = parent.parent;
		}
		return val;
	}

	public static void ResetLocal(this global::UnityEngine.Transform transform)
	{
		transform.localPosition = global::UnityEngine.Vector3.zero;
		transform.localRotation = global::UnityEngine.Quaternion.identity;
		transform.localScale = global::UnityEngine.Vector3.one;
	}
}
