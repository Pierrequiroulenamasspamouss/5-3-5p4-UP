namespace Facebook.Unity
{
	internal static class Utilities
	{
		public delegate void Callback<T>(T obj);

		private const string WarningMissingParameter = "Did not find expected value '{0}' in dictionary";

		private static global::System.Collections.Generic.Dictionary<string, string> commandLineArguments;

		public static global::System.Collections.Generic.Dictionary<string, string> CommandLineArguments
		{
			get
			{
				if (commandLineArguments != null)
				{
					return commandLineArguments;
				}
				global::System.Collections.Generic.Dictionary<string, string> dictionary = new global::System.Collections.Generic.Dictionary<string, string>();
				string[] commandLineArgs = global::System.Environment.GetCommandLineArgs();
				for (int i = 0; i < commandLineArgs.Length; i++)
				{
					if (commandLineArgs[i].StartsWith("/") || commandLineArgs[i].StartsWith("-"))
					{
						string value = ((i + 1 >= commandLineArgs.Length) ? null : commandLineArgs[i + 1]);
						dictionary.Add(commandLineArgs[i], value);
					}
				}
				commandLineArguments = dictionary;
				return commandLineArguments;
			}
		}

		public static bool TryGetValue<T>(this global::System.Collections.Generic.IDictionary<string, object> dictionary, string key, out T value)
		{
			object value2;
			if (dictionary.TryGetValue(key, out value2) && value2 is T)
			{
				value = (T)value2;
				return true;
			}
			value = default(T);
			return false;
		}

		public static long TotalSeconds(this global::System.DateTime dateTime)
		{
			return (long)(dateTime - new global::System.DateTime(1970, 1, 1, 0, 0, 0, global::System.DateTimeKind.Utc)).TotalSeconds;
		}

		public static T GetValueOrDefault<T>(this global::System.Collections.Generic.IDictionary<string, object> dictionary, string key, bool logWarning = true)
		{
			T value;
			if (!dictionary.TryGetValue<T>(key, out value) && logWarning)
			{
				global::Facebook.Unity.FacebookLogger.Warn("Did not find expected value '{0}' in dictionary", key);
			}
			return value;
		}

		public static string ToCommaSeparateList(this global::System.Collections.Generic.IEnumerable<string> list)
		{
			if (list == null)
			{
				return string.Empty;
			}
			return string.Join(",", global::System.Linq.Enumerable.ToArray(list));
		}

		public static string AbsoluteUrlOrEmptyString(this global::System.Uri uri)
		{
			if (uri == null)
			{
				return string.Empty;
			}
			return uri.AbsoluteUri;
		}

		public static string GetUserAgent(string productName, string productVersion)
		{
			return string.Format(global::System.Globalization.CultureInfo.InvariantCulture, "{0}/{1}", productName, productVersion);
		}

		public static string ToJson(this global::System.Collections.Generic.IDictionary<string, object> dictionary)
		{
			return global::Facebook.MiniJSON.Json.Serialize(dictionary);
		}

		public static void AddAllKVPFrom<T1, T2>(this global::System.Collections.Generic.IDictionary<T1, T2> dest, global::System.Collections.Generic.IDictionary<T1, T2> source)
		{
			foreach (T1 key in source.Keys)
			{
				dest[key] = source[key];
			}
		}

		public static global::Facebook.Unity.AccessToken ParseAccessTokenFromResult(global::System.Collections.Generic.IDictionary<string, object> resultDictionary)
		{
			string valueOrDefault = resultDictionary.GetValueOrDefault<string>(global::Facebook.Unity.LoginResult.UserIdKey);
			string valueOrDefault2 = resultDictionary.GetValueOrDefault<string>(global::Facebook.Unity.LoginResult.AccessTokenKey);
			global::System.DateTime expirationTime = ParseExpirationDateFromResult(resultDictionary);
			global::System.Collections.Generic.ICollection<string> permissions = ParsePermissionFromResult(resultDictionary);
			global::System.DateTime? lastRefresh = ParseLastRefreshFromResult(resultDictionary);
			return new global::Facebook.Unity.AccessToken(valueOrDefault2, valueOrDefault, expirationTime, permissions, lastRefresh);
		}

		public static string ToStringNullOk(this object obj)
		{
			if (obj == null)
			{
				return "null";
			}
			return obj.ToString();
		}

		public static string FormatToString(string baseString, string className, global::System.Collections.Generic.IDictionary<string, string> propertiesAndValues)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			if (baseString != null)
			{
				stringBuilder.Append(baseString);
			}
			stringBuilder.AppendFormat("\n{0}:", className);
			foreach (global::System.Collections.Generic.KeyValuePair<string, string> propertiesAndValue in propertiesAndValues)
			{
				string arg = ((propertiesAndValue.Value == null) ? "null" : propertiesAndValue.Value);
				stringBuilder.AppendFormat("\n\t{0}: {1}", propertiesAndValue.Key, arg);
			}
			return stringBuilder.ToString();
		}

		private static global::System.DateTime ParseExpirationDateFromResult(global::System.Collections.Generic.IDictionary<string, object> resultDictionary)
		{
			if (global::Facebook.Unity.Constants.IsWeb)
			{
				long valueOrDefault = resultDictionary.GetValueOrDefault<long>(global::Facebook.Unity.LoginResult.ExpirationTimestampKey);
				return global::System.DateTime.UtcNow.AddSeconds(valueOrDefault);
			}
			string valueOrDefault2 = resultDictionary.GetValueOrDefault<string>(global::Facebook.Unity.LoginResult.ExpirationTimestampKey);
			int result;
			if (int.TryParse(valueOrDefault2, out result) && result > 0)
			{
				return FromTimestamp(result);
			}
			return global::System.DateTime.MaxValue;
		}

		private static global::System.DateTime? ParseLastRefreshFromResult(global::System.Collections.Generic.IDictionary<string, object> resultDictionary)
		{
			string valueOrDefault = resultDictionary.GetValueOrDefault<string>("last_refresh", false);
			int result;
			if (int.TryParse(valueOrDefault, out result) && result > 0)
			{
				return FromTimestamp(result);
			}
			return null;
		}

		private static global::System.Collections.Generic.ICollection<string> ParsePermissionFromResult(global::System.Collections.Generic.IDictionary<string, object> resultDictionary)
		{
			string value;
			global::System.Collections.Generic.IEnumerable<object> value2;
			if (resultDictionary.TryGetValue<string>(global::Facebook.Unity.LoginResult.PermissionsKey, out value))
			{
				value2 = value.Split(',');
			}
			else if (!resultDictionary.TryGetValue<global::System.Collections.Generic.IEnumerable<object>>(global::Facebook.Unity.LoginResult.PermissionsKey, out value2))
			{
				value2 = new string[0];
				global::Facebook.Unity.FacebookLogger.Warn("Failed to find parameter '{0}' in login result", global::Facebook.Unity.LoginResult.PermissionsKey);
			}
			return global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Select(value2, (object permission) => permission.ToString()));
		}

		private static global::System.DateTime FromTimestamp(int timestamp)
		{
			return new global::System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
		}
	}
}
