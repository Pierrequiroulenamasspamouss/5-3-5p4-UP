namespace Facebook.Unity
{
	internal class ComponentFactory
	{
		internal enum IfNotExist
		{
			AddNew = 0,
			ReturnNull = 1
		}

		public const string GameObjectName = "UnityFacebookSDKPlugin";

		private static global::UnityEngine.GameObject facebookGameObject;

		private static global::UnityEngine.GameObject FacebookGameObject
		{
			get
			{
				if (facebookGameObject == null)
				{
					facebookGameObject = new global::UnityEngine.GameObject("UnityFacebookSDKPlugin");
				}
				return facebookGameObject;
			}
		}

		public static T GetComponent<T>(global::Facebook.Unity.ComponentFactory.IfNotExist ifNotExist = global::Facebook.Unity.ComponentFactory.IfNotExist.AddNew) where T : global::UnityEngine.MonoBehaviour
		{
			global::UnityEngine.GameObject gameObject = FacebookGameObject;
			T val = gameObject.GetComponent<T>();
			if (val == null && ifNotExist == global::Facebook.Unity.ComponentFactory.IfNotExist.AddNew)
			{
				val = gameObject.AddComponent<T>();
			}
			return val;
		}

		public static T AddComponent<T>() where T : global::UnityEngine.MonoBehaviour
		{
			return FacebookGameObject.AddComponent<T>();
		}
	}
}
