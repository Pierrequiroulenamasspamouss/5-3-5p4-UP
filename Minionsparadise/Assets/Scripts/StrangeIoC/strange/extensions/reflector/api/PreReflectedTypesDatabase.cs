namespace strange.extensions.reflector.api
{
	public class PreReflectedTypesDatabase
	{
		public static readonly global::strange.extensions.reflector.api.PreReflectedTypesDatabase Instance;

		private global::System.Collections.Generic.Dictionary<global::System.Type, global::strange.extensions.reflector.api.IReflectedClass> lookup = new global::System.Collections.Generic.Dictionary<global::System.Type, global::strange.extensions.reflector.api.IReflectedClass>();

		static PreReflectedTypesDatabase()
		{
			Instance = new global::strange.extensions.reflector.api.PreReflectedTypesDatabase();
		}

		private PreReflectedTypesDatabase()
		{
		}

		public void RegisterClass(global::System.Type type, global::strange.extensions.reflector.api.IReflectedClass instance)
		{
			lookup[type] = instance;
		}

		public void RegisterClasses(global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<global::System.Type, global::strange.extensions.reflector.api.IReflectedClass>> classes)
		{
			foreach (global::System.Collections.Generic.KeyValuePair<global::System.Type, global::strange.extensions.reflector.api.IReflectedClass> @class in classes)
			{
				lookup[@class.Key] = @class.Value;
			}
		}

		public void RegisterClasses(global::System.Collections.Generic.KeyValuePair<global::System.Type, global::strange.extensions.reflector.api.IReflectedClass>[] classes)
		{
			for (int i = 0; i < classes.Length; i++)
			{
				global::System.Collections.Generic.KeyValuePair<global::System.Type, global::strange.extensions.reflector.api.IReflectedClass> keyValuePair = classes[i];
				lookup[keyValuePair.Key] = keyValuePair.Value;
			}
		}

		public global::strange.extensions.reflector.api.IReflectedClass GetClass(global::System.Type type)
		{
			global::strange.extensions.reflector.api.IReflectedClass value;
			if (lookup.TryGetValue(type, out value))
			{
				return value;
			}
			return null;
		}
	}
}
