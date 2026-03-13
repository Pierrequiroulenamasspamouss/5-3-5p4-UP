namespace Prime31
{
	public class JsonArray : global::System.Collections.Generic.List<object>
	{
		public JsonArray()
		{
		}

		public JsonArray(int capacity)
			: base(capacity)
		{
		}

		public override string ToString()
		{
			return global::Prime31.JsonFormatter.prettyPrint(global::Prime31.SimpleJson.encode(this)) ?? string.Empty;
		}
	}
}
