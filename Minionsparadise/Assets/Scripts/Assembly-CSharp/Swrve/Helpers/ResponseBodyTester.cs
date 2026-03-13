namespace Swrve.Helpers
{
	public class ResponseBodyTester
	{
		public static bool TestUTF8(string data, out string decodedString)
		{
			return TestUTF8(global::System.Text.Encoding.UTF8.GetBytes(data), out decodedString);
		}

		public static bool TestUTF8(byte[] bodyBytes, out string decodedString)
		{
			try
			{
				decodedString = global::System.Text.Encoding.UTF8.GetString(bodyBytes, 0, bodyBytes.Length);
				return true;
			}
			catch (global::System.Exception)
			{
				decodedString = string.Empty;
			}
			return false;
		}
	}
}
