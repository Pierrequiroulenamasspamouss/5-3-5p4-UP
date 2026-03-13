namespace strange.extensions.implicitBind.impl
{
	public class ImplicitBinder : global::strange.extensions.implicitBind.api.IImplicitBinder
	{
		private sealed class ImplicitBindingVO
		{
			public global::System.Collections.Generic.List<global::System.Type> BindTypes = new global::System.Collections.Generic.List<global::System.Type>();

			public global::System.Type ToType;

			public bool IsCrossContext;

			public object Name;

			public ImplicitBindingVO(global::System.Type bindType, global::System.Type toType, bool isCrossContext, object name)
			{
				BindTypes.Add(bindType);
				ToType = toType;
				IsCrossContext = isCrossContext;
				Name = name;
			}

			public ImplicitBindingVO(global::System.Collections.Generic.List<global::System.Type> bindTypes, global::System.Type toType, bool isCrossContext, object name)
			{
				BindTypes = bindTypes;
				ToType = toType;
				IsCrossContext = isCrossContext;
				Name = name;
			}
		}

		private global::System.Reflection.Assembly assembly;

		[Inject]
		public global::strange.extensions.injector.api.IInjectionBinder injectionBinder { get; set; }

		[Inject]
		public global::strange.extensions.mediation.api.IMediationBinder mediationBinder { get; set; }

		[PostConstruct]
		public void PostConstruct()
		{
			assembly = global::System.Reflection.Assembly.GetExecutingAssembly();
		}

		public virtual void ScanForAnnotatedClasses(string[] usingNamespaces)
		{
			if (assembly != null)
			{
				global::System.Collections.Generic.IEnumerable<global::System.Type> exportedTypes = assembly.GetExportedTypes();
				global::System.Collections.Generic.List<global::System.Type> list = new global::System.Collections.Generic.List<global::System.Type>();
				int num = usingNamespaces.Length;
				int ns;
				for (ns = 0; ns < num; ns++)
				{
					list.AddRange(global::System.Linq.Enumerable.Where(exportedTypes, (global::System.Type t) => !string.IsNullOrEmpty(t.Namespace) && t.Namespace.StartsWith(usingNamespaces[ns])));
				}
				global::System.Collections.Generic.List<global::strange.extensions.implicitBind.impl.ImplicitBinder.ImplicitBindingVO> list2 = new global::System.Collections.Generic.List<global::strange.extensions.implicitBind.impl.ImplicitBinder.ImplicitBindingVO>();
				global::System.Collections.Generic.List<global::strange.extensions.implicitBind.impl.ImplicitBinder.ImplicitBindingVO> list3 = new global::System.Collections.Generic.List<global::strange.extensions.implicitBind.impl.ImplicitBinder.ImplicitBindingVO>();
				foreach (global::System.Type item2 in list)
				{
					object[] customAttributes = item2.GetCustomAttributes(typeof(Implements), true);
					object[] customAttributes2 = item2.GetCustomAttributes(typeof(ImplementedBy), true);
					object[] customAttributes3 = item2.GetCustomAttributes(typeof(MediatedBy), true);
					object[] customAttributes4 = item2.GetCustomAttributes(typeof(Mediates), true);
					if (global::System.Linq.Enumerable.Any(customAttributes2))
					{
						ImplementedBy implementedBy = (ImplementedBy)global::System.Linq.Enumerable.First(customAttributes2);
						if (!global::System.Linq.Enumerable.Contains(implementedBy.DefaultType.GetInterfaces(), item2))
						{
							throw new global::strange.extensions.injector.impl.InjectionException("Default Type: " + implementedBy.DefaultType.Name + " does not implement annotated interface " + item2.Name, global::strange.extensions.injector.api.InjectionExceptionType.IMPLICIT_BINDING_IMPLEMENTOR_DOES_NOT_IMPLEMENT_INTERFACE);
						}
						list3.Add(new global::strange.extensions.implicitBind.impl.ImplicitBinder.ImplicitBindingVO(item2, implementedBy.DefaultType, implementedBy.Scope == global::strange.extensions.injector.api.InjectionBindingScope.CROSS_CONTEXT, null));
					}
					if (global::System.Linq.Enumerable.Any(customAttributes))
					{
						global::System.Type[] interfaces = item2.GetInterfaces();
						object obj = null;
						bool flag = false;
						global::System.Collections.Generic.List<global::System.Type> list4 = new global::System.Collections.Generic.List<global::System.Type>();
						object[] array = customAttributes;
						for (int num2 = 0; num2 < array.Length; num2++)
						{
							Implements implements = (Implements)array[num2];
							if (implements.DefaultInterface != null)
							{
								if (!global::System.Linq.Enumerable.Contains(interfaces, implements.DefaultInterface) && item2 != implements.DefaultInterface)
								{
									throw new global::strange.extensions.injector.impl.InjectionException("Annotated type " + item2.Name + " does not implement Default Interface " + implements.DefaultInterface.Name, global::strange.extensions.injector.api.InjectionExceptionType.IMPLICIT_BINDING_TYPE_DOES_NOT_IMPLEMENT_DESIGNATED_INTERFACE);
								}
								list4.Add(implements.DefaultInterface);
							}
							else
							{
								list4.Add(item2);
							}
							flag = flag || implements.Scope == global::strange.extensions.injector.api.InjectionBindingScope.CROSS_CONTEXT;
							obj = obj ?? implements.Name;
						}
						global::strange.extensions.implicitBind.impl.ImplicitBinder.ImplicitBindingVO item = new global::strange.extensions.implicitBind.impl.ImplicitBinder.ImplicitBindingVO(list4, item2, flag, obj);
						list2.Add(item);
					}
					global::System.Type type = null;
					global::System.Type type2 = null;
					if (global::System.Linq.Enumerable.Any(customAttributes3))
					{
						type2 = item2;
						type = ((MediatedBy)global::System.Linq.Enumerable.First(customAttributes3)).MediatorType;
						if (type == null)
						{
							throw new global::strange.extensions.mediation.impl.MediationException("Cannot implicitly bind view of type: " + item2.Name + " due to null MediatorType", global::strange.extensions.mediation.api.MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
						}
					}
					else if (global::System.Linq.Enumerable.Any(customAttributes4))
					{
						type = item2;
						type2 = ((Mediates)global::System.Linq.Enumerable.First(customAttributes4)).ViewType;
						if (type2 == null)
						{
							throw new global::strange.extensions.mediation.impl.MediationException("Cannot implicitly bind Mediator of type: " + item2.Name + " due to null ViewType", global::strange.extensions.mediation.api.MediationExceptionType.MEDIATOR_VIEW_STACK_OVERFLOW);
						}
					}
					if (mediationBinder != null && type2 != null && type != null)
					{
						mediationBinder.Bind(type2).To(type);
					}
				}
				list3.ForEach(Bind);
				list2.ForEach(Bind);
				return;
			}
			throw new global::strange.extensions.injector.impl.InjectionException("Assembly was not initialized yet for Implicit Bindings!", global::strange.extensions.injector.api.InjectionExceptionType.UNINITIALIZED_ASSEMBLY);
		}

		private void Bind(global::strange.extensions.implicitBind.impl.ImplicitBinder.ImplicitBindingVO toBind)
		{
			global::strange.extensions.injector.api.IInjectionBinding injectionBinding = injectionBinder.Bind(global::System.Linq.Enumerable.First(toBind.BindTypes));
			injectionBinding.Weak();
			for (int i = 1; i < toBind.BindTypes.Count; i++)
			{
				global::System.Type key = global::System.Linq.Enumerable.ElementAt(toBind.BindTypes, i);
				injectionBinding.Bind(key);
			}
			injectionBinding = ((toBind.ToType != null) ? injectionBinding.To(toBind.ToType).ToName(toBind.Name).ToSingleton() : injectionBinding.ToName(toBind.Name).ToSingleton());
			if (toBind.IsCrossContext)
			{
				injectionBinding.CrossContext();
			}
		}
	}
}
