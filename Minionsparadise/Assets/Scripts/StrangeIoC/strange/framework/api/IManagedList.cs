namespace strange.framework.api
{
	public interface IManagedList
	{
		object value { get; }

		global::strange.framework.api.IManagedList Add(object value);

		global::strange.framework.api.IManagedList Add(object[] list);

		global::strange.framework.api.IManagedList Remove(object value);

		global::strange.framework.api.IManagedList Remove(object[] list);
	}
}
