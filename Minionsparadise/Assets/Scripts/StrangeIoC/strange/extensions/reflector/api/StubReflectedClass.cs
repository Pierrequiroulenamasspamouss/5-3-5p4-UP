namespace strange.extensions.reflector.api
{
	public abstract class StubReflectedClass : global::strange.extensions.reflector.api.IReflectedClass
	{
		public object[] SetterNames { get; set; }

		public bool[] SetterOptional { get; set; }

		public bool PreGenerated
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public virtual bool HasNonTrivialConstructor()
		{
			return false;
		}

		public abstract object CreateInstance(global::strange.extensions.injector.api.IInjector injector);

		public virtual void CallPostConstructors(object instance)
		{
		}

		public virtual void PerformInjection(object instance, global::strange.extensions.injector.api.IInjector injector)
		{
		}

		public virtual void PerformUninjection(object instance, global::strange.extensions.injector.api.IInjector injector)
		{
		}
	}
}
