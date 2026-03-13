namespace Swrve.Helpers
{
	public class UnityWwwHelper
	{
		public static global::Swrve.Helpers.WwwDeducedError DeduceWwwError(global::UnityEngine.WWW request)
		{
			if (request.responseHeaders.Count > 0)
			{
				string value = null;
				foreach (string key in request.responseHeaders.Keys)
				{
					if (string.Equals(key, "X-Swrve-Error", global::System.StringComparison.OrdinalIgnoreCase))
					{
						request.responseHeaders.TryGetValue(key, out value);
						break;
					}
				}
				if (value != null)
				{
					SwrveLog.LogError("Request response headers [\"X-Swrve-Error\"]: " + value + " at " + request.url);
					return global::Swrve.Helpers.WwwDeducedError.ApplicationErrorHeader;
				}
			}
			if (!string.IsNullOrEmpty(request.error))
			{
				SwrveLog.LogError("Request error: " + request.error + " in " + request.url);
				return global::Swrve.Helpers.WwwDeducedError.NetworkError;
			}
			return global::Swrve.Helpers.WwwDeducedError.NoError;
		}
	}
}
