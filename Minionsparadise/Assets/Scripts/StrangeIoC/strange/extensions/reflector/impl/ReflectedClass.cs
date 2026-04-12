namespace strange.extensions.reflector.impl
{
	internal sealed class ReflectedClass : global::strange.extensions.reflector.api.IReflectedClass
	{
		private static object[] emptyObjectList = new object[0];

		public global::System.Func<object[], object> Constructor { get; set; }

		public global::System.Type[] ConstructorParameters { get; set; }

		public global::System.Action<object>[] PostConstructors { get; set; }

		public global::System.Collections.Generic.KeyValuePair<global::System.Type, global::System.Action<object, object>>[] Setters { get; set; }

		public object[] SetterNames { get; set; }

		public bool[] SetterOptional { get; set; }

		public bool PreGenerated { get; set; }

		public object CreateInstance(global::strange.extensions.injector.api.IInjector injector)
		{
			int num = ConstructorParameters.Length;
			object[] array = ((num > 0) ? new object[num] : emptyObjectList);
			for (int i = 0; i < ConstructorParameters.Length; i++)
			{
				array[i] = injector.GetValueInjection(ConstructorParameters[i], null, null, false);
			}
			return Constructor(array);
		}

		public void CallPostConstructors(object instance)
		{
			for (int i = 0; i < PostConstructors.Length; i++)
			{
				PostConstructors[i](instance);
			}
		}

		public void PerformInjection(object instance, global::strange.extensions.injector.api.IInjector injector)
		{
			for (int i = 0; i < Setters.Length; i++)
			{
				global::System.Collections.Generic.KeyValuePair<global::System.Type, global::System.Action<object, object>> keyValuePair = Setters[i];
				object name = SetterNames[i];
				bool optional = SetterOptional[i];
				object valueInjection = injector.GetValueInjection(keyValuePair.Key, name, instance, optional);
				keyValuePair.Value(instance, valueInjection);
			}
		}

		public void PerformUninjection(object instance, global::strange.extensions.injector.api.IInjector injector)
		{
			for (int i = 0; i < Setters.Length; i++)
			{
				global::System.Collections.Generic.KeyValuePair<global::System.Type, global::System.Action<object, object>> keyValuePair = Setters[i];
				keyValuePair.Value(instance, null);
			}
		}

		public bool HasNonTrivialConstructor()
		{
			return ConstructorParameters.Length > 0;
		}
	}
}
