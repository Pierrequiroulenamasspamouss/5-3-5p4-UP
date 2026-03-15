[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class Mediates : global::System.Attribute
{
	public global::System.Type ViewType { get; set; }

	public Mediates(global::System.Type t)
	{
		ViewType = t;
	}
}
