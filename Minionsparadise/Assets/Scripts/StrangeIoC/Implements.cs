[global::System.AttributeUsage(global::System.AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class Implements : global::System.Attribute
{
	public object Name { get; set; }

	public global::System.Type DefaultInterface { get; set; }

	public global::strange.extensions.injector.api.InjectionBindingScope Scope { get; set; }

	public Implements()
	{
	}

	public Implements(global::strange.extensions.injector.api.InjectionBindingScope scope)
	{
		Scope = scope;
	}

	public Implements(global::System.Type t, global::strange.extensions.injector.api.InjectionBindingScope scope = global::strange.extensions.injector.api.InjectionBindingScope.SINGLE_CONTEXT)
	{
		DefaultInterface = t;
		Scope = scope;
	}

	public Implements(global::strange.extensions.injector.api.InjectionBindingScope scope, object name)
	{
		Scope = scope;
		Name = name;
	}

	public Implements(global::System.Type t, global::strange.extensions.injector.api.InjectionBindingScope scope, object name)
	{
		DefaultInterface = t;
		Name = name;
		Scope = scope;
	}
}
