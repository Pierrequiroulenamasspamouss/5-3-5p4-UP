namespace Ea.Sharkbite.HttpPlugin.Http.Impl
{
	public class DefaultRequestFactory : global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequestFactory
	{
		protected virtual global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultRequest CreateRequest(string url)
		{
			return new global::Ea.Sharkbite.HttpPlugin.Http.Impl.DefaultRequest(url);
		}

		public global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest Resource(string uri)
		{
			if (uri == null)
			{
				throw new global::System.ArgumentNullException();
			}
			string[] array = uri.Split('?');
			global::Ea.Sharkbite.HttpPlugin.Http.Api.IRequest request = CreateRequest(array[0]);
			if (array.Length > 1)
			{
				string text = array[1];
				for (int i = 2; i < array.Length; i++)
				{
					text = text + "?" + array[i];
				}
				array = text.Split('&');
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					string[] array3 = text2.Split('=');
					string key = array3[0];
					string text3 = array3[1];
					if (array3.Length > 2)
					{
						for (int k = 2; k < array3.Length; k++)
						{
							text3 = text3 + "=" + array3[k];
						}
					}
					request = request.WithQueryParam(key, text3);
				}
			}
			return request;
		}
	}
}
