namespace Kampai.Util
{
	public static class FastJSONSerializer
	{
		private static global::System.Text.UTF8Encoding utf8NoBOM = new global::System.Text.UTF8Encoding(false);

		private static global::Newtonsoft.Json.JsonSerializer serializer = global::Newtonsoft.Json.JsonSerializer.Create(null);

		public static void Serialize(object obj, global::Newtonsoft.Json.JsonWriter writer)
		{
			global::Kampai.Util.IFastJSONSerializable fastJSONSerializable = obj as global::Kampai.Util.IFastJSONSerializable;
			if (fastJSONSerializable != null)
			{
				fastJSONSerializable.Serialize(writer);
			}
			else
			{
				serializer.Serialize(writer, obj);
			}
		}

		public static string Serialize(object obj)
		{
			return global::System.Text.Encoding.UTF8.GetString(SerializeUTF8(obj));
		}

		public static byte[] SerializeUTF8(object obj)
		{
			using (global::System.IO.MemoryStream memoryStream = new global::System.IO.MemoryStream(128))
			{
				using (global::System.IO.TextWriter textWriter = new global::Kampai.Util.FastTextStreamWriter(memoryStream, utf8NoBOM))
				{
					using (global::Newtonsoft.Json.JsonWriter writer = new global::Kampai.Util.FastJSONWriter(textWriter))
					{
						Serialize(obj, writer);
						textWriter.Flush();
						return memoryStream.ToArray();
					}
				}
			}
		}
	}
}
