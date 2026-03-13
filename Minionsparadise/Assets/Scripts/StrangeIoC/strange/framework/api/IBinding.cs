namespace strange.framework.api
{
	public interface IBinding
	{
		object key { get; }

		object name { get; }

		object value { get; }

		global::strange.framework.api.BindingConstraintType keyConstraint { get; set; }

		global::strange.framework.api.BindingConstraintType valueConstraint { get; set; }

		bool isWeak { get; }

		global::strange.framework.api.IBinding Bind<T>();

		global::strange.framework.api.IBinding Bind(object key);

		global::strange.framework.api.IBinding To<T>();

		global::strange.framework.api.IBinding To(object o);

		global::strange.framework.api.IBinding ToName<T>();

		global::strange.framework.api.IBinding ToName(object o);

		global::strange.framework.api.IBinding Named<T>();

		global::strange.framework.api.IBinding Named(object o);

		void RemoveKey(object o);

		void RemoveValue(object o);

		void RemoveName(object o);

		global::strange.framework.api.IBinding Weak();
	}
}
