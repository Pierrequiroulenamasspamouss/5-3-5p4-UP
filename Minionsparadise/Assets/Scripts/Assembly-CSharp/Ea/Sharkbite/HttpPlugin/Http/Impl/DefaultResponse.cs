namespace Ea.Sharkbite.HttpPlugin.Http.Impl
{
	public class DefaultResponse : global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse
	{
		public virtual string Body { get; set; }

		public virtual int Code { get; set; }

		public virtual global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest Request { get; set; }

		public virtual long ContentLength { get; set; }

		public virtual int DownloadTime { get; set; }

		public virtual string ContentType { get; set; }

		public virtual global::System.Collections.Generic.IDictionary<string, string> Headers { get; set; }

		public virtual bool IsConnectionLost { get; set; }

		public virtual bool Success
		{
			get
			{
				return Code >= 200 && Code < 300 && string.IsNullOrEmpty(Error);
			}
		}

		public virtual string Error { get; set; }

		public global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse WithError(string error)
		{
			Error = error;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse WithBody(string body)
		{
			Body = body;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse WithCode(int code)
		{
			Code = code;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse WithRequest(global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request)
		{
			Request = request;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse WithContentLength(long contentLength)
		{
			ContentLength = contentLength;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse WithDownloadTime(int downloadTime)
		{
			DownloadTime = downloadTime;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse WithContentType(string contentType)
		{
			ContentType = contentType;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse WithHeaders(global::System.Collections.Generic.IDictionary<string, string> headers)
		{
			Headers = headers;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultResponse WithConnectionLoss(bool isConnectionLost)
		{
			IsConnectionLost = isConnectionLost;
			return this;
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponse WithPostprocessor(global::Ea.Sharkbite.HttpPlugin.Http.Api.IResponsePostprocessor postprocessor)
		{
			postprocessor.postprocess(this);
			return this;
		}
	}
}
