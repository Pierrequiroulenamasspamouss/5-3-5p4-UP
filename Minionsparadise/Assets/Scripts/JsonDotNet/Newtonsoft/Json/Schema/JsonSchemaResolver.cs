namespace Newtonsoft.Json.Schema
{
	public class JsonSchemaResolver
	{
		public global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchema> LoadedSchemas { get; protected set; }

		public JsonSchemaResolver()
		{
			LoadedSchemas = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchema>();
		}

		public virtual global::Newtonsoft.Json.Schema.JsonSchema GetSchema(string id)
		{
			return global::System.Linq.Enumerable.SingleOrDefault(LoadedSchemas, (global::Newtonsoft.Json.Schema.JsonSchema s) => s.Id == id);
		}
	}
}
