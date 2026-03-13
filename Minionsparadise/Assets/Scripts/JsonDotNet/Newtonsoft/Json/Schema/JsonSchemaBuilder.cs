namespace Newtonsoft.Json.Schema
{
	internal class JsonSchemaBuilder
	{
		private global::Newtonsoft.Json.JsonReader _reader;

		private readonly global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchema> _stack;

		private readonly global::Newtonsoft.Json.Schema.JsonSchemaResolver _resolver;

		private global::Newtonsoft.Json.Schema.JsonSchema _currentSchema;

		private global::Newtonsoft.Json.Schema.JsonSchema CurrentSchema
		{
			get
			{
				return _currentSchema;
			}
		}

		private void Push(global::Newtonsoft.Json.Schema.JsonSchema value)
		{
			_currentSchema = value;
			_stack.Add(value);
			_resolver.LoadedSchemas.Add(value);
		}

		private global::Newtonsoft.Json.Schema.JsonSchema Pop()
		{
			global::Newtonsoft.Json.Schema.JsonSchema currentSchema = _currentSchema;
			_stack.RemoveAt(_stack.Count - 1);
			_currentSchema = global::System.Linq.Enumerable.LastOrDefault(_stack);
			return currentSchema;
		}

		public JsonSchemaBuilder(global::Newtonsoft.Json.Schema.JsonSchemaResolver resolver)
		{
			_stack = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchema>();
			_resolver = resolver;
		}

		internal global::Newtonsoft.Json.Schema.JsonSchema Parse(global::Newtonsoft.Json.JsonReader reader)
		{
			_reader = reader;
			if (reader.TokenType == global::Newtonsoft.Json.JsonToken.None)
			{
				_reader.Read();
			}
			return BuildSchema();
		}

		private global::Newtonsoft.Json.Schema.JsonSchema BuildSchema()
		{
			if (_reader.TokenType != global::Newtonsoft.Json.JsonToken.StartObject)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Expected StartObject while parsing schema object, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, _reader.TokenType));
			}
			_reader.Read();
			if (_reader.TokenType == global::Newtonsoft.Json.JsonToken.EndObject)
			{
				Push(new global::Newtonsoft.Json.Schema.JsonSchema());
				return Pop();
			}
			string text = global::System.Convert.ToString(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
			_reader.Read();
			if (text == "$ref")
			{
				string text2 = (string)_reader.Value;
				while (_reader.Read() && _reader.TokenType != global::Newtonsoft.Json.JsonToken.EndObject)
				{
					if (_reader.TokenType == global::Newtonsoft.Json.JsonToken.StartObject)
					{
						throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Found StartObject within the schema reference with the Id '{0}'", global::System.Globalization.CultureInfo.InvariantCulture, text2));
					}
				}
				global::Newtonsoft.Json.Schema.JsonSchema schema = _resolver.GetSchema(text2);
				if (schema == null)
				{
					throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Could not resolve schema reference for Id '{0}'.", global::System.Globalization.CultureInfo.InvariantCulture, text2));
				}
				return schema;
			}
			Push(new global::Newtonsoft.Json.Schema.JsonSchema());
			ProcessSchemaProperty(text);
			while (_reader.Read() && _reader.TokenType != global::Newtonsoft.Json.JsonToken.EndObject)
			{
				text = global::System.Convert.ToString(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				_reader.Read();
				ProcessSchemaProperty(text);
			}
			return Pop();
		}

		private void ProcessSchemaProperty(string propertyName)
		{
			switch (propertyName)
			{
			case "type":
				CurrentSchema.Type = ProcessType();
				break;
			case "id":
				CurrentSchema.Id = (string)_reader.Value;
				break;
			case "title":
				CurrentSchema.Title = (string)_reader.Value;
				break;
			case "description":
				CurrentSchema.Description = (string)_reader.Value;
				break;
			case "properties":
				ProcessProperties();
				break;
			case "items":
				ProcessItems();
				break;
			case "additionalProperties":
				ProcessAdditionalProperties();
				break;
			case "patternProperties":
				ProcessPatternProperties();
				break;
			case "required":
				CurrentSchema.Required = (bool)_reader.Value;
				break;
			case "requires":
				CurrentSchema.Requires = (string)_reader.Value;
				break;
			case "identity":
				ProcessIdentity();
				break;
			case "minimum":
				CurrentSchema.Minimum = global::System.Convert.ToDouble(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				break;
			case "maximum":
				CurrentSchema.Maximum = global::System.Convert.ToDouble(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				break;
			case "exclusiveMinimum":
				CurrentSchema.ExclusiveMinimum = (bool)_reader.Value;
				break;
			case "exclusiveMaximum":
				CurrentSchema.ExclusiveMaximum = (bool)_reader.Value;
				break;
			case "maxLength":
				CurrentSchema.MaximumLength = global::System.Convert.ToInt32(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				break;
			case "minLength":
				CurrentSchema.MinimumLength = global::System.Convert.ToInt32(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				break;
			case "maxItems":
				CurrentSchema.MaximumItems = global::System.Convert.ToInt32(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				break;
			case "minItems":
				CurrentSchema.MinimumItems = global::System.Convert.ToInt32(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				break;
			case "divisibleBy":
				CurrentSchema.DivisibleBy = global::System.Convert.ToDouble(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				break;
			case "disallow":
				CurrentSchema.Disallow = ProcessType();
				break;
			case "default":
				ProcessDefault();
				break;
			case "hidden":
				CurrentSchema.Hidden = (bool)_reader.Value;
				break;
			case "readonly":
				CurrentSchema.ReadOnly = (bool)_reader.Value;
				break;
			case "format":
				CurrentSchema.Format = (string)_reader.Value;
				break;
			case "pattern":
				CurrentSchema.Pattern = (string)_reader.Value;
				break;
			case "options":
				ProcessOptions();
				break;
			case "enum":
				ProcessEnum();
				break;
			case "extends":
				ProcessExtends();
				break;
			default:
				_reader.Skip();
				break;
			}
		}

		private void ProcessExtends()
		{
			CurrentSchema.Extends = BuildSchema();
		}

		private void ProcessEnum()
		{
			if (_reader.TokenType != global::Newtonsoft.Json.JsonToken.StartArray)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Expected StartArray token while parsing enum values, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, _reader.TokenType));
			}
			CurrentSchema.Enum = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Linq.JToken>();
			while (_reader.Read() && _reader.TokenType != global::Newtonsoft.Json.JsonToken.EndArray)
			{
				global::Newtonsoft.Json.Linq.JToken item = global::Newtonsoft.Json.Linq.JToken.ReadFrom(_reader);
				CurrentSchema.Enum.Add(item);
			}
		}

		private void ProcessOptions()
		{
			CurrentSchema.Options = new global::System.Collections.Generic.Dictionary<global::Newtonsoft.Json.Linq.JToken, string>(new global::Newtonsoft.Json.Linq.JTokenEqualityComparer());
			global::Newtonsoft.Json.JsonToken tokenType = _reader.TokenType;
			if (tokenType == global::Newtonsoft.Json.JsonToken.StartArray)
			{
				while (_reader.Read() && _reader.TokenType != global::Newtonsoft.Json.JsonToken.EndArray)
				{
					if (_reader.TokenType != global::Newtonsoft.Json.JsonToken.StartObject)
					{
						throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Expect object token, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, _reader.TokenType));
					}
					string value = null;
					global::Newtonsoft.Json.Linq.JToken jToken = null;
					while (_reader.Read() && _reader.TokenType != global::Newtonsoft.Json.JsonToken.EndObject)
					{
						string text = global::System.Convert.ToString(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
						_reader.Read();
						switch (text)
						{
						case "value":
							jToken = global::Newtonsoft.Json.Linq.JToken.ReadFrom(_reader);
							break;
						case "label":
							value = (string)_reader.Value;
							break;
						default:
							throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected property in JSON schema option: {0}.", global::System.Globalization.CultureInfo.InvariantCulture, text));
						}
					}
					if (jToken == null)
					{
						throw new global::System.Exception("No value specified for JSON schema option.");
					}
					if (CurrentSchema.Options.ContainsKey(jToken))
					{
						throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Duplicate value in JSON schema option collection: {0}", global::System.Globalization.CultureInfo.InvariantCulture, jToken));
					}
					CurrentSchema.Options.Add(jToken, value);
				}
				return;
			}
			throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Expected array token, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, _reader.TokenType));
		}

		private void ProcessDefault()
		{
			CurrentSchema.Default = global::Newtonsoft.Json.Linq.JToken.ReadFrom(_reader);
		}

		private void ProcessIdentity()
		{
			CurrentSchema.Identity = new global::System.Collections.Generic.List<string>();
			switch (_reader.TokenType)
			{
			case global::Newtonsoft.Json.JsonToken.String:
				CurrentSchema.Identity.Add(_reader.Value.ToString());
				break;
			case global::Newtonsoft.Json.JsonToken.StartArray:
				while (_reader.Read() && _reader.TokenType != global::Newtonsoft.Json.JsonToken.EndArray)
				{
					if (_reader.TokenType != global::Newtonsoft.Json.JsonToken.String)
					{
						throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Exception JSON property name string token, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, _reader.TokenType));
					}
					CurrentSchema.Identity.Add(_reader.Value.ToString());
				}
				break;
			default:
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Expected array or JSON property name string token, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, _reader.TokenType));
			}
		}

		private void ProcessAdditionalProperties()
		{
			if (_reader.TokenType == global::Newtonsoft.Json.JsonToken.Boolean)
			{
				CurrentSchema.AllowAdditionalProperties = (bool)_reader.Value;
			}
			else
			{
				CurrentSchema.AdditionalProperties = BuildSchema();
			}
		}

		private void ProcessPatternProperties()
		{
			global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchema> dictionary = new global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchema>();
			if (_reader.TokenType != global::Newtonsoft.Json.JsonToken.StartObject)
			{
				throw new global::System.Exception("Expected start object token.");
			}
			while (_reader.Read() && _reader.TokenType != global::Newtonsoft.Json.JsonToken.EndObject)
			{
				string text = global::System.Convert.ToString(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				_reader.Read();
				if (dictionary.ContainsKey(text))
				{
					throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Property {0} has already been defined in schema.", global::System.Globalization.CultureInfo.InvariantCulture, text));
				}
				dictionary.Add(text, BuildSchema());
			}
			CurrentSchema.PatternProperties = dictionary;
		}

		private void ProcessItems()
		{
			CurrentSchema.Items = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchema>();
			switch (_reader.TokenType)
			{
			case global::Newtonsoft.Json.JsonToken.StartObject:
				CurrentSchema.Items.Add(BuildSchema());
				break;
			case global::Newtonsoft.Json.JsonToken.StartArray:
				while (_reader.Read() && _reader.TokenType != global::Newtonsoft.Json.JsonToken.EndArray)
				{
					CurrentSchema.Items.Add(BuildSchema());
				}
				break;
			default:
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Expected array or JSON schema object token, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, _reader.TokenType));
			}
		}

		private void ProcessProperties()
		{
			global::System.Collections.Generic.IDictionary<string, global::Newtonsoft.Json.Schema.JsonSchema> dictionary = new global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchema>();
			if (_reader.TokenType != global::Newtonsoft.Json.JsonToken.StartObject)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Expected StartObject token while parsing schema properties, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, _reader.TokenType));
			}
			while (_reader.Read() && _reader.TokenType != global::Newtonsoft.Json.JsonToken.EndObject)
			{
				string text = global::System.Convert.ToString(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				_reader.Read();
				if (dictionary.ContainsKey(text))
				{
					throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Property {0} has already been defined in schema.", global::System.Globalization.CultureInfo.InvariantCulture, text));
				}
				dictionary.Add(text, BuildSchema());
			}
			CurrentSchema.Properties = dictionary;
		}

		private global::Newtonsoft.Json.Schema.JsonSchemaType? ProcessType()
		{
			switch (_reader.TokenType)
			{
			case global::Newtonsoft.Json.JsonToken.String:
				return MapType(_reader.Value.ToString());
			case global::Newtonsoft.Json.JsonToken.StartArray:
			{
				global::Newtonsoft.Json.Schema.JsonSchemaType? jsonSchemaType = global::Newtonsoft.Json.Schema.JsonSchemaType.None;
				while (_reader.Read() && _reader.TokenType != global::Newtonsoft.Json.JsonToken.EndArray)
				{
					if (_reader.TokenType != global::Newtonsoft.Json.JsonToken.String)
					{
						throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Exception JSON schema type string token, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, _reader.TokenType));
					}
					jsonSchemaType |= MapType(_reader.Value.ToString());
				}
				return jsonSchemaType;
			}
			default:
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Expected array or JSON schema type string token, got {0}.", global::System.Globalization.CultureInfo.InvariantCulture, _reader.TokenType));
			}
		}

		internal static global::Newtonsoft.Json.Schema.JsonSchemaType MapType(string type)
		{
			global::Newtonsoft.Json.Schema.JsonSchemaType value;
			if (!global::Newtonsoft.Json.Schema.JsonSchemaConstants.JsonSchemaTypeMapping.TryGetValue(type, out value))
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Invalid JSON schema type: {0}", global::System.Globalization.CultureInfo.InvariantCulture, type));
			}
			return value;
		}

		internal static string MapType(global::Newtonsoft.Json.Schema.JsonSchemaType type)
		{
			return global::System.Linq.Enumerable.Single(global::Newtonsoft.Json.Schema.JsonSchemaConstants.JsonSchemaTypeMapping, (global::System.Collections.Generic.KeyValuePair<string, global::Newtonsoft.Json.Schema.JsonSchemaType> kv) => kv.Value == type).Key;
		}
	}
}
