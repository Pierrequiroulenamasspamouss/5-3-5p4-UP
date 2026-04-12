[global::System.AttributeUsage(global::System.AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public class ImplementedBy : global::System.Attribute
{
	public global::System.Type DefaultType { get; set; }

	public global::strange.extensions.injector.api.InjectionBindingScope Scope { get; set; }

	public ImplementedBy(global::System.Type t, global::strange.extensions.injector.api.InjectionBindingScope scope = global::strange.extensions.injector.api.InjectionBindingScope.SINGLE_CONTEXT)
	{
		DefaultType = t;
		Scope = scope;
	}
}
