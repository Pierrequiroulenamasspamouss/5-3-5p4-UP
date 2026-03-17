namespace Ea.Sharkbite.HttpPlugin.Http.Impl
{
	public class FileDownloadResponse : global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse
	{
		public override string Body
		{
			get
			{
				throw new global::System.NotImplementedException("Download requests do not have a body.  The downloaded contents were written to the specified file.");
			}
			set
			{
				throw new global::System.NotImplementedException("Download requests do not have a body.  The downloaded contents were written to the specified file.");
			}
		}
	}
}
