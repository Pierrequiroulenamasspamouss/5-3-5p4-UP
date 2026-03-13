namespace Prime31
{
	public static class Utils
	{
		private static global::System.Random _random;

		private static global::System.Random random
		{
			get
			{
				if (_random == null)
				{
					_random = new global::System.Random();
				}
				return _random;
			}
		}

		public static string randomString(int size = 38)
		{
			global::System.Text.StringBuilder stringBuilder = new global::System.Text.StringBuilder();
			for (int i = 0; i < size; i++)
			{
				char value = global::System.Convert.ToChar(global::System.Convert.ToInt32(global::System.Math.Floor(26.0 * random.NextDouble() + 65.0)));
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		public static void logObject(object obj)
		{
			string json = global::Prime31.Json.encode(obj);
			prettyPrintJson(json);
		}

		public static void prettyPrintJson(string json)
		{
			string text = string.Empty;
			if (json != null)
			{
				text = global::Prime31.JsonFormatter.prettyPrint(json);
			}
			try
			{
				global::UnityEngine.Debug.Log(text);
			}
			catch (global::System.Exception)
			{
				global::System.Console.WriteLine(text);
			}
		}
	}
}
