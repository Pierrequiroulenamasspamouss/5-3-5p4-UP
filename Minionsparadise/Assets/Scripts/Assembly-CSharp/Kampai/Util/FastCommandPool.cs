namespace Kampai.Util
{
	public class FastCommandPool
	{
		private const int warmupSize = 8;

		private object[] warmup = new object[8];

		private static object POOL_TAG = new object();

		private global::System.Collections.Generic.Dictionary<global::System.Type, global::strange.extensions.pool.impl.Pool> pools = new global::System.Collections.Generic.Dictionary<global::System.Type, global::strange.extensions.pool.impl.Pool>();

		private global::System.Collections.Generic.List<global::Kampai.Util.Tuple<global::System.Type, global::strange.extensions.injector.api.IInjectionBinder>> poolsToWarmUp = new global::System.Collections.Generic.List<global::Kampai.Util.Tuple<global::System.Type, global::strange.extensions.injector.api.IInjectionBinder>>();

		private global::strange.extensions.pool.impl.Pool GetPoolForType(global::System.Type type, global::strange.extensions.injector.api.IInjectionBinder binder)
		{
			global::strange.extensions.pool.impl.Pool value = null;
			if (pools.TryGetValue(type, out value))
			{
				return value;
			}
			global::System.Type o = typeof(global::strange.extensions.pool.impl.Pool<>).MakeGenericType(type);
			binder.Bind(type).To(type);
			binder.Bind<global::strange.extensions.pool.impl.Pool>().To(o).ToName(POOL_TAG);
			value = binder.GetInstance<global::strange.extensions.pool.impl.Pool>(POOL_TAG);
			binder.Unbind<global::strange.extensions.pool.impl.Pool>(POOL_TAG);
			pools.Add(type, value);
			for (int i = 0; i < 8; i++)
			{
				warmup[i] = value.GetInstance();
			}
			for (int j = 0; j < 8; j++)
			{
				value.ReturnInstance(warmup[j]);
			}
			return value;
		}

		public void ReturnToPool<T>(T command)
		{
			global::strange.extensions.pool.impl.Pool value = null;
			if (pools.TryGetValue(command.GetType(), out value))
			{
				value.ReturnInstance(command);
			}
		}

		public T GetCommand<T>(global::strange.extensions.injector.api.IInjectionBinder binder) where T : class, global::Kampai.Util.IFastPooledCommandBase
		{
			T result = GetPoolForType(typeof(T), binder).GetInstance() as T;
			result.commandPool = this;
			return result;
		}

		public void WarmupPool<T>(global::strange.extensions.injector.api.IInjectionBinder binder)
		{
			poolsToWarmUp.Add(new global::Kampai.Util.Tuple<global::System.Type, global::strange.extensions.injector.api.IInjectionBinder>(typeof(T), binder));
		}

		public void Warmup()
		{
			foreach (global::Kampai.Util.Tuple<global::System.Type, global::strange.extensions.injector.api.IInjectionBinder> item in poolsToWarmUp)
			{
				GetPoolForType(item.Item1, item.Item2);
			}
			poolsToWarmUp.Clear();
		}
	}
}
