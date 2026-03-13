namespace Kampai.Game.View
{
	public class PoolableVfxProvider : global::strange.framework.api.IInstanceProvider
	{
		private global::UnityEngine.GameObject prefab;

		public PoolableVfxProvider(global::UnityEngine.GameObject prefab)
		{
			this.prefab = prefab;
		}

		public T GetInstance<T>()
		{
			return (T)GetInstance(typeof(T));
		}

		public object GetInstance(global::System.Type key)
		{
			return new global::Kampai.Game.View.PoolableVFX(prefab);
		}
	}
}
