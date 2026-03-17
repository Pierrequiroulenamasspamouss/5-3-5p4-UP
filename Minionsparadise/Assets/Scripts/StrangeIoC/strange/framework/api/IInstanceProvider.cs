namespace strange.framework.api
{
	public interface IInstanceProvider
	{
		T GetInstance<T>();

		object GetInstance(global::System.Type key);
	}
}
