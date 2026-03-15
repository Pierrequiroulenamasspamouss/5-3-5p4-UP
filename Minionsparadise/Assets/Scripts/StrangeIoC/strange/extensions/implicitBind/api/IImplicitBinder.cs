namespace strange.extensions.implicitBind.api
{
	public interface IImplicitBinder
	{
		void ScanForAnnotatedClasses(string[] usingNamespaces);
	}
}
