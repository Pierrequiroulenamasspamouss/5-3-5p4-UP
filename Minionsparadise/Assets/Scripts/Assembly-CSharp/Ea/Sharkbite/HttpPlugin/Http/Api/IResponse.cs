namespace Ea.Sharkbite.HttpPlugin.Http.Api
{
	public interface IResponse
	{
		string Body { get; set; }

		int Code { get; set; }

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest Request { get; set; }

		long ContentLength { get; set; }

		int DownloadTime { get; set; }

		string ContentType { get; set; }

		global::System.Collections.Generic.IDictionary<string, string> Headers { get; set; }

		bool IsConnectionLost { get; set; }

		string Error { get; set; }

		bool Success { get; }

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse WithPostprocessor(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponsePostprocessor postprocessor);
	}
}
