namespace Elevation.Logging
{
	public class Factory<T, U> where T : class where U : class
	{
		private readonly global::System.Collections.Generic.Dictionary<string, global::System.Func<U, T>> _dict = new global::System.Collections.Generic.Dictionary<string, global::System.Func<U, T>>();

		public T Create(string name, U arg)
		{
			global::System.Func<U, T> value = null;
			if (_dict.TryGetValue(name, out value))
			{
				return value(arg);
			}
			return (T)null;
		}

		public void Register(string name, global::System.Func<U, T> builder)
		{
			if (_dict.ContainsKey(name))
			{
				_dict[name] = builder;
			}
			else
			{
				_dict.Add(name, builder);
			}
		}
	}
}
