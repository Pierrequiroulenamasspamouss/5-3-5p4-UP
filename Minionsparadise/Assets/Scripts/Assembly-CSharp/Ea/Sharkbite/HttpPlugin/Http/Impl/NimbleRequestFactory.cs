namespace Ea.Sharkbite.HttpPlugin.Http.Impl
{
	public class NimbleRequestFactory : global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultRequestFactory
	{
		protected override global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultRequest CreateRequest(string url)
		{
			return new global::Ea.Sharkbite.HttpPlugin.Http.Impl.NimbleRequest(url);
		}
	}
}
