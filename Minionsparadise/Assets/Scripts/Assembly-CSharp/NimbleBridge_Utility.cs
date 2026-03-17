public class NimbleBridge_Utility
{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_Utility_getUTCDateStringFormat(double date);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_Utility_SHA256HashString(string str);
#endif

	public static string GetUTCDateStringFormat(double date)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_Utility_getUTCDateStringFormat(date);
#else
		return global::System.DateTime.FromOADate(date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
#endif
	}

	public static string SHA256HashString(string str)
	{
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
		return NimbleBridge_Utility_SHA256HashString(str);
#else
		using (global::System.Security.Cryptography.SHA256 sha = global::System.Security.Cryptography.SHA256.Create())
		{
			byte[] bytes = sha.ComputeHash(global::System.Text.Encoding.UTF8.GetBytes(str));
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				stringBuilder.Append(bytes[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}
#endif
	}
}
