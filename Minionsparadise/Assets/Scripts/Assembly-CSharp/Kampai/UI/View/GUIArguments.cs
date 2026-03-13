namespace Kampai.UI.View
{
	public class GUIArguments
	{
		private readonly global::Kampai.Util.IKampaiLogger logger;

		public global::System.Collections.Generic.Dictionary<global::System.Type, object> arguments = new global::System.Collections.Generic.Dictionary<global::System.Type, object>();

		public int Count
		{
			get
			{
				return arguments.Count;
			}
		}

		public GUIArguments(global::Kampai.Util.IKampaiLogger logger)
		{
			this.logger = logger;
		}

		public T Get<T>()
		{
			if (arguments.ContainsKey(typeof(T)))
			{
				return (T)arguments[typeof(T)];
			}
			return default(T);
		}

		public bool Contains<T>()
		{
			if (arguments.ContainsKey(typeof(T)))
			{
				return true;
			}
			return false;
		}

		public global::Kampai.UI.View.GUIArguments Add(object value)
		{
			return Add(value.GetType(), value);
		}

		public global::Kampai.UI.View.GUIArguments Add(global::System.Type type, object value)
		{
			if (value != null)
			{
				if (arguments.ContainsKey(type))
				{
					logger.Debug(string.Format("Overwriting previous GUIArguments value for type: {0}", type));
					arguments.Remove(type);
				}
				arguments.Add(type, value);
			}
			return this;
		}

		public void Remove(global::System.Type type)
		{
			if (arguments.ContainsKey(type))
			{
				arguments.Remove(type);
			}
		}

		public void AddArguments(global::Kampai.UI.View.GUIArguments other)
		{
			foreach (object value in other.arguments.Values)
			{
				Add(value);
			}
		}
	}
}
