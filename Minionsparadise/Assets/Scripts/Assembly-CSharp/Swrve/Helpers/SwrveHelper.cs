namespace Swrve.Helpers
{
	public static class SwrveHelper
	{
		public static global::System.DateTime? Now = null;

		public static global::System.DateTime? UtcNow = null;

		private static global::System.Security.Cryptography.MD5CryptoServiceProvider fakeReference = new global::System.Security.Cryptography.MD5CryptoServiceProvider();

		private static global::System.Text.RegularExpressions.Regex rgxNonAlphanumeric = new global::System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");

		public static readonly global::System.DateTime UnixEpoch = new global::System.DateTime(1970, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc);

		public static global::System.DateTime GetNow()
		{
			global::System.DateTime? now = Now;
			if (now.HasValue && Now.HasValue)
			{
				return Now.Value;
			}
			return global::System.DateTime.Now;
		}

		public static global::System.DateTime GetUtcNow()
		{
			global::System.DateTime? utcNow = UtcNow;
			if (utcNow.HasValue && UtcNow.HasValue)
			{
				return UtcNow.Value;
			}
			return global::System.DateTime.UtcNow;
		}

		public static void Shuffle<T>(this global::System.Collections.Generic.IList<T> list)
		{
			int num = list.Count;
			global::System.Random random = new global::System.Random();
			while (num > 1)
			{
				int index = random.Next(0, num) % num;
				num--;
				T value = list[index];
				list[index] = list[num];
				list[num] = value;
			}
		}

		public static byte[] MD5(string str)
		{
			byte[] bytes = global::System.Text.Encoding.UTF8.GetBytes(str);
			return SwrveMD5Core.GetHash(bytes);
		}

		public static string ApplyMD5(string str)
		{
			byte[] array = MD5(str);
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		public static bool CheckBase64(string str)
		{
			string text = str.Trim();
			return text.Length % 4 == 0 && global::System.Text.RegularExpressions.Regex.IsMatch(text, "^[a-zA-Z0-9\\+/]*={0,3}$", global::System.Text.RegularExpressions.RegexOptions.None);
		}

		public static string CreateHMACMD5(string data, string key)
		{
			if (fakeReference != null)
			{
				byte[] bytes = global::System.Text.Encoding.UTF8.GetBytes(data);
				byte[] bytes2 = global::System.Text.Encoding.UTF8.GetBytes(key);
				using (global::System.Security.Cryptography.HMACMD5 hMACMD = new global::System.Security.Cryptography.HMACMD5(bytes2))
				{
					byte[] bytes3 = hMACMD.ComputeHash(bytes);
					return global::System.Text.Encoding.UTF8.GetString(bytes3);
				}
			}
			return null;
		}

		public static long GetSeconds()
		{
			return (long)(global::System.DateTime.UtcNow - UnixEpoch).TotalSeconds;
		}

		public static long GetMilliseconds()
		{
			return (long)(global::System.DateTime.UtcNow - UnixEpoch).TotalMilliseconds;
		}

		public static string GetEventName(global::System.Collections.Generic.Dictionary<string, object> eventParameters)
		{
			string result = string.Empty;
			switch ((string)eventParameters["type"])
			{
			case "session_start":
				result = "Swrve.session.start";
				break;
			case "session_end":
				result = "Swrve.session.end";
				break;
			case "buy_in":
				result = "Swrve.buy_in";
				break;
			case "iap":
				result = "Swrve.iap";
				break;
			case "event":
				result = (string)eventParameters["name"];
				break;
			case "purchase":
				result = "Swrve.user_purchase";
				break;
			case "currency_given":
				result = "Swrve.currency_given";
				break;
			case "user":
				result = "Swrve.user_properties_changed";
				break;
			}
			return result;
		}

		public static string EpochToFormat(long epochTime, string format)
		{
			return new global::System.DateTime(1970, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc).AddMilliseconds(epochTime).ToString(format);
		}

		public static string FilterNonAlphanumeric(string str)
		{
			return rgxNonAlphanumeric.Replace(str, string.Empty);
		}
	}
}
