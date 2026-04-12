namespace Newtonsoft.Json.Schema
{
	public class JsonSchema
	{
		private readonly string _internalId = global::System.Guid.NewGuid().ToString("N");

		public string Id { get; set; }

		public string Title { get; set; }

		public bool? Required { get; set; }

		public bool? ReadOnly { get; set; }

		public bool? Hidden { get; set; }

		public bool? Transient { get; set; }

		public string Description { get; set; }

		public global::Newtonsoft.Json.Schema.JsonSchemaType? Type { get; set; }

		public string Pattern { get; set; }

		public int? MinimumLength { get; set; }

		public int? MaximumLength { get; set; }

		public double? DivisibleBy { get; set; }

		public double? Minimum { get; set; }

		public double? Maximum { get; set; }

		public bool? ExclusiveMinimum { get; set; }

		public bool? ExclusiveMaximum { get; set; }

		public int? MinimumItems { get; set; }

		public int? MaximumItems { get; set; }

		public global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchema> Items { get; set; }

		public global::System.Collections.Generic.IDictionary<string, global::Newtonsoft.Json.Schema.JsonSchema> Properties { get; set; }

		public global::Newtonsoft.Json.Schema.JsonSchema AdditionalProperties { get; set; }

		public global::System.Collections.Generic.IDictionary<string, global::Newtonsoft.Json.Schema.JsonSchema> PatternProperties { get; set; }

		public bool AllowAdditionalProperties { get; set; }

		public string Requires { get; set; }

		public global::System.Collections.Generic.IList<string> Identity { get; set; }

		public global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken> Enum { get; set; }

		public global::System.Collections.Generic.IDictionary<global::Newtonsoft.Json.Linq.JToken, string> Options { get; set; }

		public global::Newtonsoft.Json.Schema.JsonSchemaType? Disallow { get; set; }

		public global::Newtonsoft.Json.Linq.JToken Default { get; set; }

		public global::Newtonsoft.Json.Schema.JsonSchema Extends { get; set; }

		public string Format { get; set; }

		internal string InternalId
		{
			get
			{
				return _internalId;
			}
		}

		public JsonSchema()
		{
			AllowAdditionalProperties = true;
		}

		public static global::Newtonsoft.Json.Schema.JsonSchema Read(global::Newtonsoft.Json.JsonReader reader)
		{
			return Read(reader, new global::Newtonsoft.Json.Schema.JsonSchemaResolver());
		}

		public static global::Newtonsoft.Json.Schema.JsonSchema Read(global::Newtonsoft.Json.JsonReader reader, global::Newtonsoft.Json.Schema.JsonSchemaResolver resolver)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(reader, "reader");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(resolver, "resolver");
			global::Newtonsoft.Json.Schema.JsonSchemaBuilder jsonSchemaBuilder = new global::Newtonsoft.Json.Schema.JsonSchemaBuilder(resolver);
			return jsonSchemaBuilder.Parse(reader);
		}

		public static global::Newtonsoft.Json.Schema.JsonSchema Parse(string json)
		{
			return Parse(json, new global::Newtonsoft.Json.Schema.JsonSchemaResolver());
		}

		public static global::Newtonsoft.Json.Schema.JsonSchema Parse(string json, global::Newtonsoft.Json.Schema.JsonSchemaResolver resolver)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(json, "json");
			global::Newtonsoft.Json.JsonReader reader = new global::Newtonsoft.Json.JsonTextReader(new global::System.IO.StringReader(json));
			return Read(reader, resolver);
		}

		public void WriteTo(global::Newtonsoft.Json.JsonWriter writer)
		{
			WriteTo(writer, new global::Newtonsoft.Json.Schema.JsonSchemaResolver());
		}

		public void WriteTo(global::Newtonsoft.Json.JsonWriter writer, global::Newtonsoft.Json.Schema.JsonSchemaResolver resolver)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(writer, "writer");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(resolver, "resolver");
			global::Newtonsoft.Json.Schema.JsonSchemaWriter jsonSchemaWriter = new global::Newtonsoft.Json.Schema.JsonSchemaWriter(writer, resolver);
			jsonSchemaWriter.WriteSchema(this);
		}

		public override string ToString()
		{
			global::System.IO.StringWriter stringWriter = new global::System.IO.StringWriter(global::System.Globalization.CultureInfo.InvariantCulture);
			global::Newtonsoft.Json.JsonTextWriter jsonTextWriter = new global::Newtonsoft.Json.JsonTextWriter(stringWriter);
			jsonTextWriter.Formatting = global::Newtonsoft.Json.Formatting.Indented;
			WriteTo(jsonTextWriter);
			return stringWriter.ToString();
		}
	}
}
