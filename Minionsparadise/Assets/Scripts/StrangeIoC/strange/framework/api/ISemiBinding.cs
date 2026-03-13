namespace strange.framework.api
{
	public interface ISemiBinding : global::strange.framework.api.IManagedList
	{
		global::strange.framework.api.BindingConstraintType constraint { get; set; }

		bool uniqueValues { get; set; }
	}
}
