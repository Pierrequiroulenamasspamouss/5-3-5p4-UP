namespace strange.extensions.signal.api
{
	public interface IBaseSignal
	{
		void Dispatch(object[] args);

		void AddListener(global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]> callback);

		void AddOnce(global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]> callback);

		void RemoveListener(global::System.Action<global::strange.extensions.signal.api.IBaseSignal, object[]> callback);

		global::System.Collections.Generic.List<global::System.Type> GetTypes();
	}
}
