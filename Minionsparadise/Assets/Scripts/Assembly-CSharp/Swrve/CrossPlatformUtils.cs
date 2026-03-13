namespace Swrve
{
	public static class CrossPlatformUtils
	{
		public static global::UnityEngine.WWW MakeWWW(string url, byte[] encodedData, global::System.Collections.Generic.Dictionary<string, string> headers)
		{
			return new global::UnityEngine.WWW(url, encodedData, headers);
		}
	}
}
