namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaModel
	{
		public bool Required { get; set; }

		public global::Newtonsoft.Json.Schema.JsonSchemaType Type { get; set; }

		public int? MinimumLength { get; set; }

		public int? MaximumLength { get; set; }

		public double? DivisibleBy { get; set; }

		public double? Minimum { get; set; }

		public double? Maximum { get; set; }

		public bool ExclusiveMinimum { get; set; }

		public bool ExclusiveMaximum { get; set; }

		public int? MinimumItems { get; set; }

		public int? MaximumItems { get; set; }

		public global::System.Collections.Generic.IList<string> Patterns { get; set; }

		public global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchemaModel> Items { get; set; }

		public global::System.Collections.Generic.IDictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaModel> Properties { get; set; }

		public global::System.Collections.Generic.IDictionary<string, global::Newtonsoft.Json.Schema.JsonSchemaModel> PatternProperties { get; set; }

		public global::Newtonsoft.Json.Schema.JsonSchemaModel AdditionalProperties { get; set; }

		public bool AllowAdditionalProperties { get; set; }

		public global::System.Collections.Generic.IList<global::Newtonsoft.Json.Linq.JToken> Enum { get; set; }

		public global::Newtonsoft.Json.Schema.JsonSchemaType Disallow { get; set; }

		public JsonSchemaModel()
		{
			Type = global::Newtonsoft.Json.Schema.JsonSchemaType.Any;
			AllowAdditionalProperties = true;
			Required = false;
		}

		public static global::Newtonsoft.Json.Schema.JsonSchemaModel Create(global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchema> schemata)
		{
			global::Newtonsoft.Json.Schema.JsonSchemaModel jsonSchemaModel = new global::Newtonsoft.Json.Schema.JsonSchemaModel();
			foreach (global::Newtonsoft.Json.Schema.JsonSchema schematum in schemata)
			{
				Combine(jsonSchemaModel, schematum);
			}
			return jsonSchemaModel;
		}

		private static void Combine(global::Newtonsoft.Json.Schema.JsonSchemaModel model, global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			model.Required = model.Required || (schema.Required ?? false);
			model.Type &= schema.Type ?? global::Newtonsoft.Json.Schema.JsonSchemaType.Any;
			model.MinimumLength = global::Newtonsoft.Json.Utilities.MathUtils.Max(model.MinimumLength, schema.MinimumLength);
			model.MaximumLength = global::Newtonsoft.Json.Utilities.MathUtils.Min(model.MaximumLength, schema.MaximumLength);
			model.DivisibleBy = global::Newtonsoft.Json.Utilities.MathUtils.Max(model.DivisibleBy, schema.DivisibleBy);
			model.Minimum = global::Newtonsoft.Json.Utilities.MathUtils.Max(model.Minimum, schema.Minimum);
			model.Maximum = global::Newtonsoft.Json.Utilities.MathUtils.Max(model.Maximum, schema.Maximum);
			model.ExclusiveMinimum = model.ExclusiveMinimum || (schema.ExclusiveMinimum ?? false);
			model.ExclusiveMaximum = model.ExclusiveMaximum || (schema.ExclusiveMaximum ?? false);
			model.MinimumItems = global::Newtonsoft.Json.Utilities.MathUtils.Max(model.MinimumItems, schema.MinimumItems);
			model.MaximumItems = global::Newtonsoft.Json.Utilities.MathUtils.Min(model.MaximumItems, schema.MaximumItems);
			model.AllowAdditionalProperties = model.AllowAdditionalProperties && schema.AllowAdditionalProperties;
			if (schema.Enum != null)
			{
				if (model.Enum == null)
				{
					model.Enum = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Linq.JToken>();
				}
				global::Newtonsoft.Json.Utilities.CollectionUtils.AddRangeDistinct(model.Enum, schema.Enum, new global::Newtonsoft.Json.Linq.JTokenEqualityComparer());
			}
			model.Disallow |= schema.Disallow ?? global::Newtonsoft.Json.Schema.JsonSchemaType.None;
			if (schema.Pattern != null)
			{
				if (model.Patterns == null)
				{
					model.Patterns = new global::System.Collections.Generic.List<string>();
				}
				global::Newtonsoft.Json.Utilities.CollectionUtils.AddDistinct(model.Patterns, schema.Pattern);
			}
		}
	}
}
