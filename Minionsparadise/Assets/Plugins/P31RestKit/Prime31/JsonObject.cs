namespace Prime31
{
	public class JsonObject : global::System.Collections.Generic.Dictionary<string, object>
	{
		public override string ToString()
		{
			return global::Prime31.JsonFormatter.prettyPrint(global::Prime31.SimpleJson.encode(this)) ?? string.Empty;
		}
	}
}
