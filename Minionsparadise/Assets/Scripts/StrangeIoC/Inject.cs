[global::System.AttributeUsage(global::System.AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class Inject : global::System.Attribute
{
	public object name { get; set; }

	public Inject()
	{
	}

	public Inject(object n)
	{
		name = n;
	}
}
