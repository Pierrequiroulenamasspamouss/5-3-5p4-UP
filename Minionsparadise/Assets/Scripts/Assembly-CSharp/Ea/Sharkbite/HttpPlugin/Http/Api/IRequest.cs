namespace Ea.Sharkbite.HttpPlugin.Http.Api
{
	public interface IRequest
	{
		string Uri { get; set; }

		string Method { get; set; }

		byte[] Body { get; set; }

		string Accept { get; set; }

		string ContentType { get; set; }

		string Username { get; set; }

		string Password { set; }

		int requestCount { get; set; }

		global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<string, string>> QueryParams { get; set; }

		global::System.Collections.Generic.Dictionary<string, string> Headers { get; set; }

		global::System.Collections.Generic.List<global::System.Collections.Generic.KeyValuePair<string, string>> FormParams { get; set; }

		bool CanRetry { get; set; }

		int RetryCount { get; set; }

		bool TryResume { get; set; }

		string FilePath { get; set; }

		string Md5 { get; set; }

		bool UseGZip { get; set; }

		bool UseUdp { get; set; }

		bool AvoidBackup { get; set; }

		bool RunInBackground { get; set; }

		global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> ResponseSignal { get; set; }

		global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> ProgressSignal { get; set; }

		string GetTempFilePath();

		global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress GetProgress();

		void Get(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback);

		void Head(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback);

		void Options(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback);

		void Post(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback);

		void Put(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback);

		void Delete(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback);

		void Execute(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> callback);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithContentType(string contentType);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithAccept(string accept);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithQueryParam(string key, string value);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithHeaderParam(string key, string value);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithFormParam(string key, string value);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithBasicAuth(string username, string password);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithBody(byte[] body);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithPreprocessor(global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestPreprocessor preprocessor);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithMethod(string method);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithOutputFile(string filePath);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithMd5(string md5);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithGZip(bool useGZip);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithUdp(bool useUdp);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithAvoidBackup(bool avoidBackup);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithRunInBackground(bool runInBackground);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithResponseSignal(global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse> responseSignal);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithProgressSignal(global::strange.extensions.signal.impl.Signal<global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> progressSignal);

		void RegisterNotifiable(global::System.Action<global::Ea.Sharkbite.HttpPlugin.Http.Api.DownloadProgress, global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest> notify);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithRetry(bool retry = true, int times = 3);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithResume(bool tryResume);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithEntity(object entity);

		global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest WithRequestCount(int saveCount);

		void Abort();

		bool IsAborted();

		void Restart();

		bool IsRestarted();
	}
}
