namespace Kampai.Util
{
	public class ResourceInstanceProvider : global::strange.framework.api.IInstanceProvider
	{
		private global::UnityEngine.GameObject prototype;

		private string resourceName;

		public ResourceInstanceProvider(string name)
		{
			resourceName = name;
		}

		public T GetInstance<T>()
		{
			object instance = GetInstance(typeof(T));
			return (T)instance;
		}

		public object GetInstance(global::System.Type key)
		{
			if (prototype == null)
			{
				prototype = global::UnityEngine.Resources.Load<global::UnityEngine.GameObject>(resourceName);
			}
			return global::UnityEngine.Object.Instantiate(prototype);
		}
	}
}
