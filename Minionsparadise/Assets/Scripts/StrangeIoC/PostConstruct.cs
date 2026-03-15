[global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class PostConstruct : global::System.Attribute
{
	public int priority { get; set; }

	public PostConstruct()
	{
	}

	public PostConstruct(int p)
	{
		priority = p;
	}
}
