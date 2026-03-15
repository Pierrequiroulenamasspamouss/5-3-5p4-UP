[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class MediatedBy : global::System.Attribute
{
	public global::System.Type MediatorType { get; set; }

	public MediatedBy(global::System.Type t)
	{
		MediatorType = t;
	}
}
