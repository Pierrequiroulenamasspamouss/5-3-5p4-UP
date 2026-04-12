namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaNode
	{
		public string Id { get; private set; }

		public global::System.Collections.ObjectModel.ReadOnlyCollection<global::Newtonsoft.Json.Schema.JsonSchema> Schemas { get; private set; }

		public global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaNode> Properties { get; private set; }

		public global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaNode> PatternProperties { get; private set; }

		public global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchemaNode> Items { get; private set; }

		public global::Newtonsoft.Json.Schema.JsonSchemaNode AdditionalProperties { get; set; }

		public JsonSchemaNode(global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			Schemas = new global::System.Collections.ObjectModel.ReadOnlyCollection<global::Newtonsoft.Json.Schema.JsonSchema>(new global::Newtonsoft.Json.Schema.JsonSchema[1] { schema });
			Properties = new global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaNode>();
			PatternProperties = new global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaNode>();
			Items = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchemaNode>();
			Id = GetId(Schemas);
		}

		private JsonSchemaNode(global::Newtonsoft.Json.Schema.JsonSchemaNode source, global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			Schemas = new global::System.Collections.ObjectModel.ReadOnlyCollection<global::Newtonsoft.Json.Schema.JsonSchema>(global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Union(source.Schemas, new global::Newtonsoft.Json.Schema.JsonSchema[1] { schema })));
			Properties = new global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaNode>(source.Properties);
			PatternProperties = new global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaNode>(source.PatternProperties);
			Items = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchemaNode>(source.Items);
			AdditionalProperties = source.AdditionalProperties;
			Id = GetId(Schemas);
		}

		public global::Newtonsoft.Json.Schema.JsonSchemaNode Combine(global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			return new global::Newtonsoft.Json.Schema.JsonSchemaNode(this, schema);
		}

		public static string GetId(global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Schema.JsonSchema> schemata)
		{
			return string.Join("-", global::System.Linq.Enumerable.ToArray(global::System.Linq.Enumerable.OrderBy(global::System.Linq.Enumerable.Select(schemata, (global::Newtonsoft.Json.Schema.JsonSchema s) => s.InternalId), (string id) => id, global::System.StringComparer.Ordinal)));
		}
	}
}
