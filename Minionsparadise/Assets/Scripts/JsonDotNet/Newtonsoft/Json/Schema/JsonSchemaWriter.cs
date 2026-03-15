namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaWriter
	{
		private readonly global::Newtonsoft.Json.JsonWriter _writer;

		private readonly global::Newtonsoft.Json.Schema.JsonSchemaResolver _resolver;

		public JsonSchemaWriter(global::Newtonsoft.Json.JsonWriter writer, global::Newtonsoft.Json.Schema.JsonSchemaResolver resolver)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(writer, "writer");
			_writer = writer;
			_resolver = resolver;
		}

		private void ReferenceOrWriteSchema(global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			if (schema.Id != null && _resolver.GetSchema(schema.Id) != null)
			{
				_writer.WriteStartObject();
				_writer.WritePropertyName("$ref");
				_writer.WriteValue(schema.Id);
				_writer.WriteEndObject();
			}
			else
			{
				WriteSchema(schema);
			}
		}

		public void WriteSchema(global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(schema, "schema");
			if (!_resolver.LoadedSchemas.Contains(schema))
			{
				_resolver.LoadedSchemas.Add(schema);
			}
			_writer.WriteStartObject();
			WritePropertyIfNotNull(_writer, "id", schema.Id);
			WritePropertyIfNotNull(_writer, "title", schema.Title);
			WritePropertyIfNotNull(_writer, "description", schema.Description);
			WritePropertyIfNotNull(_writer, "required", schema.Required);
			WritePropertyIfNotNull(_writer, "readonly", schema.ReadOnly);
			WritePropertyIfNotNull(_writer, "hidden", schema.Hidden);
			WritePropertyIfNotNull(_writer, "transient", schema.Transient);
			if (schema.Type.HasValue)
			{
				WriteType("type", _writer, schema.Type.Value);
			}
			if (!schema.AllowAdditionalProperties)
			{
				_writer.WritePropertyName("additionalProperties");
				_writer.WriteValue(schema.AllowAdditionalProperties);
			}
			else if (schema.AdditionalProperties != null)
			{
				_writer.WritePropertyName("additionalProperties");
				ReferenceOrWriteSchema(schema.AdditionalProperties);
			}
			WriteSchemaDictionaryIfNotNull(_writer, "properties", schema.Properties);
			WriteSchemaDictionaryIfNotNull(_writer, "patternProperties", schema.PatternProperties);
			WriteItems(schema);
			WritePropertyIfNotNull(_writer, "minimum", schema.Minimum);
			WritePropertyIfNotNull(_writer, "maximum", schema.Maximum);
			WritePropertyIfNotNull(_writer, "exclusiveMinimum", schema.ExclusiveMinimum);
			WritePropertyIfNotNull(_writer, "exclusiveMaximum", schema.ExclusiveMaximum);
			WritePropertyIfNotNull(_writer, "minLength", schema.MinimumLength);
			WritePropertyIfNotNull(_writer, "maxLength", schema.MaximumLength);
			WritePropertyIfNotNull(_writer, "minItems", schema.MinimumItems);
			WritePropertyIfNotNull(_writer, "maxItems", schema.MaximumItems);
			WritePropertyIfNotNull(_writer, "divisibleBy", schema.DivisibleBy);
			WritePropertyIfNotNull(_writer, "format", schema.Format);
			WritePropertyIfNotNull(_writer, "pattern", schema.Pattern);
			if (schema.Enum != null)
			{
				_writer.WritePropertyName("enum");
				_writer.WriteStartArray();
				foreach (global::Newtonsoft.Json.Linq.JToken item in schema.Enum)
				{
					item.WriteTo(_writer);
				}
				_writer.WriteEndArray();
			}
			if (schema.Default != null)
			{
				_writer.WritePropertyName("default");
				schema.Default.WriteTo(_writer);
			}
			if (schema.Options != null)
			{
				_writer.WritePropertyName("options");
				_writer.WriteStartArray();
				foreach (global::System.Collections.Generic.KeyValuePair<global::Newtonsoft.Json.Linq.JToken, string> option in schema.Options)
				{
					_writer.WriteStartObject();
					_writer.WritePropertyName("value");
					option.Key.WriteTo(_writer);
					if (option.Value != null)
					{
						_writer.WritePropertyName("label");
						_writer.WriteValue(option.Value);
					}
					_writer.WriteEndObject();
				}
				_writer.WriteEndArray();
			}
			if (schema.Disallow.HasValue)
			{
				WriteType("disallow", _writer, schema.Disallow.Value);
			}
			if (schema.Extends != null)
			{
				_writer.WritePropertyName("extends");
				ReferenceOrWriteSchema(schema.Extends);
			}
			_writer.WriteEndObject();
		}

		private void WriteSchemaDictionaryIfNotNull(global::Newtonsoft.Json.JsonWriter writer, string propertyName, global::System.Collections.Generic.IDictionary<string, global::Newtonsoft.Json.Schema.JsonSchema> properties)
		{
			if (properties == null)
			{
				return;
			}
			writer.WritePropertyName(propertyName);
			writer.WriteStartObject();
			foreach (global::System.Collections.Generic.KeyValuePair<string, global::Newtonsoft.Json.Schema.JsonSchema> property in properties)
			{
				writer.WritePropertyName(property.Key);
				ReferenceOrWriteSchema(property.Value);
			}
			writer.WriteEndObject();
		}

		private void WriteItems(global::Newtonsoft.Json.Schema.JsonSchema schema)
		{
			if (global::Newtonsoft.Json.Utilities.CollectionUtils.IsNullOrEmpty(schema.Items))
			{
				return;
			}
			_writer.WritePropertyName("items");
			if (schema.Items.Count == 1)
			{
				ReferenceOrWriteSchema(schema.Items[0]);
				return;
			}
			_writer.WriteStartArray();
			foreach (global::Newtonsoft.Json.Schema.JsonSchema item in schema.Items)
			{
				ReferenceOrWriteSchema(item);
			}
			_writer.WriteEndArray();
		}

		private void WriteType(string propertyName, global::Newtonsoft.Json.JsonWriter writer, global::Newtonsoft.Json.Schema.JsonSchemaType type)
		{
			global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchemaType> list2;
			if (global::System.Enum.IsDefined(typeof(global::Newtonsoft.Json.Schema.JsonSchemaType), type))
			{
				global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchemaType> list = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchemaType>();
				list.Add(type);
				list2 = list;
			}
			else
			{
				list2 = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(global::Newtonsoft.Json.Utilities.EnumUtils.GetFlagsValues(type), (global::Newtonsoft.Json.Schema.JsonSchemaType v) => v != global::Newtonsoft.Json.Schema.JsonSchemaType.None));
			}
			if (list2.Count == 0)
			{
				return;
			}
			writer.WritePropertyName(propertyName);
			if (list2.Count == 1)
			{
				writer.WriteValue(global::Newtonsoft.Json.Schema.JsonSchemaBuilder.MapType(list2[0]));
				return;
			}
			writer.WriteStartArray();
			foreach (global::Newtonsoft.Json.Schema.JsonSchemaType item in list2)
			{
				writer.WriteValue(global::Newtonsoft.Json.Schema.JsonSchemaBuilder.MapType(item));
			}
			writer.WriteEndArray();
		}

		private void WritePropertyIfNotNull(global::Newtonsoft.Json.JsonWriter writer, string propertyName, object value)
		{
			if (value != null)
			{
				writer.WritePropertyName(propertyName);
				writer.WriteValue(value);
			}
		}
	}
}
