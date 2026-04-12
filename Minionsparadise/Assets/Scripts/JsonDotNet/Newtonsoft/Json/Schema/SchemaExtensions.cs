namespace Newtonsoft.Json.Schema
{
	public static class SchemaExtensions
	{
		public static bool IsValid(this global::Newtonsoft.Json.Linq.JToken source, global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			bool valid = true;
			source.Validate(schema, delegate
			{
				valid = false;
			});
			return valid;
		}

		public static void Validate(this global::Newtonsoft.Json.Linq.JToken source, global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			source.Validate(schema, null);
		}

		public static void Validate(this global::Newtonsoft.Json.Linq.JToken source, global::Newtonsoft.Json.Schema.JsonSchema schema, global::Newtonsoft.Json.Schema.ValidationEventHandler validationEventHandler)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(source, "source");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(schema, "schema");
			using (global::Newtonsoft.Json.JsonValidatingReader jsonValidatingReader = new global::Newtonsoft.Json.JsonValidatingReader(source.CreateReader()))
			{
				jsonValidatingReader.Schema = schema;
				if (validationEventHandler != null)
				{
					jsonValidatingReader.ValidationEventHandler += validationEventHandler;
				}
				while (jsonValidatingReader.Read())
				{
				}
			}
		}
	}
}
