namespace strange.framework.api
{
	public interface IBinder
	{
		global::strange.framework.api.IBinding Bind<T>();

		global::strange.framework.api.IBinding Bind(object value);

		global::strange.framework.api.IBinding GetBinding<T>();

		global::strange.framework.api.IBinding GetBinding(object key);

		global::strange.framework.api.IBinding GetBinding<T>(object name);

		global::strange.framework.api.IBinding GetBinding(object key, object name);

		global::strange.framework.api.IBinding GetRawBinding();

		void Unbind<T>();

		void Unbind<T>(object name);

		void Unbind(object key);

		void Unbind(object key, object name);

		void Unbind(global::strange.framework.api.IBinding binding);

		void RemoveValue(global::strange.framework.api.IBinding binding, object value);

		void RemoveKey(global::strange.framework.api.IBinding binding, object value);

		void RemoveName(global::strange.framework.api.IBinding binding, object value);

		void OnRemove();

		void ResolveBinding(global::strange.framework.api.IBinding binding, object key);
	}
}
