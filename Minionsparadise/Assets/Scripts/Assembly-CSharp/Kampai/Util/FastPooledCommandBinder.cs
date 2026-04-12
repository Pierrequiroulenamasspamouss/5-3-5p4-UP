namespace Kampai.Util
{
	public static class FastPooledCommandBinder
	{
		public static void Bind<Sig, Cmd>(global::strange.extensions.injector.api.IInjectionBinder binder) where Sig : global::strange.extensions.signal.impl.Signal where Cmd : class, global::Kampai.Util.IFastPooledCommand
		{
			Sig instance = binder.GetInstance<Sig>();
			global::Kampai.Util.FastCommandPool pool = binder.GetInstance<global::Kampai.Util.FastCommandPool>();
			pool.WarmupPool<Cmd>(binder);
			instance.AddListener(delegate
			{
				Cmd command = pool.GetCommand<Cmd>(binder);
				command.Execute();
				if (!command.retain)
				{
					pool.ReturnToPool(command);
				}
			});
		}
	}
	public static class FastPooledCommandBinder<T>
	{
		public static void Bind<Sig, Cmd>(global::strange.extensions.injector.api.IInjectionBinder binder) where Sig : global::strange.extensions.signal.impl.Signal<T> where Cmd : class, global::Kampai.Util.IFastPooledCommand<T>
		{
			Sig instance = binder.GetInstance<Sig>();
			global::Kampai.Util.FastCommandPool pool = binder.GetInstance<global::Kampai.Util.FastCommandPool>();
			pool.WarmupPool<Cmd>(binder);
			instance.AddListener(delegate(T arg1)
			{
				Cmd command = pool.GetCommand<Cmd>(binder);
				command.Execute(arg1);
				if (!command.retain)
				{
					pool.ReturnToPool(command);
				}
			});
		}
	}
	public static class FastPooledCommandBinder<T1, T2>
	{
		public static void Bind<Sig, Cmd>(global::strange.extensions.injector.api.IInjectionBinder binder) where Sig : global::strange.extensions.signal.impl.Signal<T1, T2> where Cmd : class, global::Kampai.Util.IFastPooledCommand<T1, T2>
		{
			Sig instance = binder.GetInstance<Sig>();
			global::Kampai.Util.FastCommandPool pool = binder.GetInstance<global::Kampai.Util.FastCommandPool>();
			pool.WarmupPool<Cmd>(binder);
			instance.AddListener(delegate(T1 arg1, T2 arg2)
			{
				Cmd command = pool.GetCommand<Cmd>(binder);
				command.Execute(arg1, arg2);
				if (!command.retain)
				{
					pool.ReturnToPool(command);
				}
			});
		}
	}
	public static class FastPooledCommandBinder<T1, T2, T3>
	{
		public static void Bind<Sig, Cmd>(global::strange.extensions.injector.api.IInjectionBinder binder) where Sig : global::strange.extensions.signal.impl.Signal<T1, T2, T3> where Cmd : class, global::Kampai.Util.IFastPooledCommand<T1, T2, T3>
		{
			Sig instance = binder.GetInstance<Sig>();
			global::Kampai.Util.FastCommandPool pool = binder.GetInstance<global::Kampai.Util.FastCommandPool>();
			pool.WarmupPool<Cmd>(binder);
			instance.AddListener(delegate(T1 arg1, T2 arg2, T3 arg3)
			{
				Cmd command = pool.GetCommand<Cmd>(binder);
				command.Execute(arg1, arg2, arg3);
				if (!command.retain)
				{
					pool.ReturnToPool(command);
				}
			});
		}
	}
	public static class FastPooledCommandBinder<T1, T2, T3, T4>
	{
		public static void Bind<Sig, Cmd>(global::strange.extensions.injector.api.IInjectionBinder binder) where Sig : global::strange.extensions.signal.impl.Signal<T1, T2, T3, T4> where Cmd : class, global::Kampai.Util.IFastPooledCommand<T1, T2, T3, T4>
		{
			Sig instance = binder.GetInstance<Sig>();
			global::Kampai.Util.FastCommandPool pool = binder.GetInstance<global::Kampai.Util.FastCommandPool>();
			pool.WarmupPool<Cmd>(binder);
			instance.AddListener(delegate(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
			{
				Cmd command = pool.GetCommand<Cmd>(binder);
				command.Execute(arg1, arg2, arg3, arg4);
				if (!command.retain)
				{
					pool.ReturnToPool(command);
				}
			});
		}
	}
}
