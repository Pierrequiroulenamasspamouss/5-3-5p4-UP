namespace Newtonsoft.Json
{
	public class JsonValidatingReader : global::Newtonsoft.Json.JsonReader, global::Newtonsoft.Json.IJsonLineInfo
	{
		private class SchemaScope
		{
			private readonly global::Newtonsoft.Json.Linq.JTokenType _tokenType;

			private readonly global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchemaModel> _schemas;

			private readonly global::System.Collections.Generic.Dictionary<string, bool> _requiredProperties;

			public string CurrentPropertyName { get; set; }

			public int ArrayItemCount { get; set; }

			public global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchemaModel> Schemas
			{
				get
				{
					return _schemas;
				}
			}

			public global::System.Collections.Generic.Dictionary<string, bool> RequiredProperties
			{
				get
				{
					return _requiredProperties;
				}
			}

			public global::Newtonsoft.Json.Linq.JTokenType TokenType
			{
				get
				{
					return _tokenType;
				}
			}

			public SchemaScope(global::Newtonsoft.Json.Linq.JTokenType tokenType, global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchemaModel> schemas)
			{
				_tokenType = tokenType;
				_schemas = schemas;
				_requiredProperties = global::System.Linq.Enumerable.ToDictionary<string, string, bool>(global::System.Linq.Enumerable.Distinct<string>(global::System.Linq.Enumerable.SelectMany<global::Newtonsoft.Json.Schema.JsonSchemaModel, string>(schemas, (global::Newtonsoft.Json.Schema.JsonSchemaModel s) => GetRequiredProperties(s))), (string p) => p, (string p) => false);
			}

			private global::System.Collections.Generic.IEnumerable<string> GetRequiredProperties(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
			{
				if (schema == null || schema.Properties == null)
				{
					return global::System.Linq.Enumerable.Empty<string>();
				}
				return global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Where(schema.Properties, (global::System.Collections.Generic.KeyValuePair<string, global::Newtonsoft.Json.Schema.JsonSchemaModel> p) => p.Value.Required), (global::System.Collections.Generic.KeyValuePair<string, global::Newtonsoft.Json.Schema.JsonSchemaModel> p) => p.Key);
			}
		}

		private readonly global::Newtonsoft.Json.JsonReader _reader;

		private readonly global::System.Collections.Generic.Stack<global::Newtonsoft.Json.JsonValidatingReader.SchemaScope> _stack;

		private global::Newtonsoft.Json.Schema.JsonSchema _schema;

		private global::Newtonsoft.Json.Schema.JsonSchemaModel _model;

		private global::Newtonsoft.Json.JsonValidatingReader.SchemaScope _currentScope;

		public override object Value
		{
			get
			{
				return _reader.Value;
			}
		}

		public override int Depth
		{
			get
			{
				return _reader.Depth;
			}
		}

		public override char QuoteChar
		{
			get
			{
				return _reader.QuoteChar;
			}
			protected internal set
			{
			}
		}

		public override global::Newtonsoft.Json.JsonToken TokenType
		{
			get
			{
				return _reader.TokenType;
			}
		}

		public override global::System.Type ValueType
		{
			get
			{
				return _reader.ValueType;
			}
		}

		private global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Schema.JsonSchemaModel> CurrentSchemas
		{
			get
			{
				return _currentScope.Schemas;
			}
		}

		private global::System.Collections.Generic.IEnumerable<global::Newtonsoft.Json.Schema.JsonSchemaModel> CurrentMemberSchemas
		{
			get
			{
				if (_currentScope == null)
				{
					return new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchemaModel>(new global::Newtonsoft.Json.Schema.JsonSchemaModel[1] { _model });
				}
				if (_currentScope.Schemas == null || _currentScope.Schemas.Count == 0)
				{
					return global::System.Linq.Enumerable.Empty<global::Newtonsoft.Json.Schema.JsonSchemaModel>();
				}
				switch (_currentScope.TokenType)
				{
				case global::Newtonsoft.Json.Linq.JTokenType.None:
					return _currentScope.Schemas;
				case global::Newtonsoft.Json.Linq.JTokenType.Object:
				{
					if (_currentScope.CurrentPropertyName == null)
					{
						throw new global::System.Exception("CurrentPropertyName has not been set on scope.");
					}
					global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchemaModel> list2 = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchemaModel>();
					{
						foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentSchema in CurrentSchemas)
						{
							global::Newtonsoft.Json.Schema.JsonSchemaModel value;
							if (currentSchema.Properties != null && currentSchema.Properties.TryGetValue(_currentScope.CurrentPropertyName, out value))
							{
								list2.Add(value);
							}
							if (currentSchema.PatternProperties != null)
							{
								foreach (global::System.Collections.Generic.KeyValuePair<string, global::Newtonsoft.Json.Schema.JsonSchemaModel> patternProperty in currentSchema.PatternProperties)
								{
									if (global::System.Text.RegularExpressions.Regex.IsMatch(_currentScope.CurrentPropertyName, patternProperty.Key))
									{
										list2.Add(patternProperty.Value);
									}
								}
							}
							if (list2.Count == 0 && currentSchema.AllowAdditionalProperties && currentSchema.AdditionalProperties != null)
							{
								list2.Add(currentSchema.AdditionalProperties);
							}
						}
						return list2;
					}
				}
				case global::Newtonsoft.Json.Linq.JTokenType.Array:
				{
					global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchemaModel> list = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchemaModel>();
					{
						foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentSchema2 in CurrentSchemas)
						{
							if (!global::Newtonsoft.Json.Utilities.CollectionUtils.IsNullOrEmpty(currentSchema2.Items))
							{
								if (currentSchema2.Items.Count == 1)
								{
									list.Add(currentSchema2.Items[0]);
								}
								if (currentSchema2.Items.Count > _currentScope.ArrayItemCount - 1)
								{
									list.Add(currentSchema2.Items[_currentScope.ArrayItemCount - 1]);
								}
							}
							if (currentSchema2.AllowAdditionalProperties && currentSchema2.AdditionalProperties != null)
							{
								list.Add(currentSchema2.AdditionalProperties);
							}
						}
						return list;
					}
				}
				case global::Newtonsoft.Json.Linq.JTokenType.Constructor:
					return global::System.Linq.Enumerable.Empty<global::Newtonsoft.Json.Schema.JsonSchemaModel>();
				default:
					throw new global::System.ArgumentOutOfRangeException("TokenType", global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected token type: {0}", global::System.Globalization.CultureInfo.InvariantCulture, _currentScope.TokenType));
				}
			}
		}

		public global::Newtonsoft.Json.Schema.JsonSchema Schema
		{
			get
			{
				return _schema;
			}
			set
			{
				if (TokenType != global::Newtonsoft.Json.JsonToken.None)
				{
					throw new global::System.Exception("Cannot change schema while validating JSON.");
				}
				_schema = value;
				_model = null;
			}
		}

		public global::Newtonsoft.Json.JsonReader Reader
		{
			get
			{
				return _reader;
			}
		}

		int global::Newtonsoft.Json.IJsonLineInfo.LineNumber
		{
			get
			{
				global::Newtonsoft.Json.IJsonLineInfo jsonLineInfo = _reader as global::Newtonsoft.Json.IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LineNumber;
			}
		}

		int global::Newtonsoft.Json.IJsonLineInfo.LinePosition
		{
			get
			{
				global::Newtonsoft.Json.IJsonLineInfo jsonLineInfo = _reader as global::Newtonsoft.Json.IJsonLineInfo;
				if (jsonLineInfo == null)
				{
					return 0;
				}
				return jsonLineInfo.LinePosition;
			}
		}

		public event global::Newtonsoft.Json.Schema.ValidationEventHandler ValidationEventHandler;

		private void Push(global::Newtonsoft.Json.JsonValidatingReader.SchemaScope scope)
		{
			_stack.Push(scope);
			_currentScope = scope;
		}

		private global::Newtonsoft.Json.JsonValidatingReader.SchemaScope Pop()
		{
			global::Newtonsoft.Json.JsonValidatingReader.SchemaScope result = _stack.Pop();
			_currentScope = ((_stack.Count != 0) ? _stack.Peek() : null);
			return result;
		}

		private void RaiseError(string message, global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			string message2 = (((global::Newtonsoft.Json.IJsonLineInfo)this).HasLineInfo() ? (message + global::Newtonsoft.Json.Utilities.StringUtils.FormatWith(" Line {0}, position {1}.", global::System.Globalization.CultureInfo.InvariantCulture, ((global::Newtonsoft.Json.IJsonLineInfo)this).LineNumber, ((global::Newtonsoft.Json.IJsonLineInfo)this).LinePosition)) : message);
			OnValidationEvent(new global::Newtonsoft.Json.Schema.JsonSchemaException(message2, null, ((global::Newtonsoft.Json.IJsonLineInfo)this).LineNumber, ((global::Newtonsoft.Json.IJsonLineInfo)this).LinePosition));
		}

		private void OnValidationEvent(global::Newtonsoft.Json.Schema.JsonSchemaException exception)
		{
			global::Newtonsoft.Json.Schema.ValidationEventHandler validationEventHandler = this.ValidationEventHandler;
			if (validationEventHandler != null)
			{
				validationEventHandler(this, new global::Newtonsoft.Json.Schema.ValidationEventArgs(exception));
				return;
			}
			throw exception;
		}

		public JsonValidatingReader(global::Newtonsoft.Json.JsonReader reader)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(reader, "reader");
			_reader = reader;
			_stack = new global::System.Collections.Generic.Stack<global::Newtonsoft.Json.JsonValidatingReader.SchemaScope>();
		}

		private void ValidateInEnumAndNotDisallowed(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			global::Newtonsoft.Json.Linq.JToken jToken = new global::Newtonsoft.Json.Linq.JValue(_reader.Value);
			if (schema.Enum != null)
			{
				global::System.IO.StringWriter stringWriter = new global::System.IO.StringWriter(global::System.Globalization.CultureInfo.InvariantCulture);
				jToken.WriteTo(new global::Newtonsoft.Json.JsonTextWriter(stringWriter));
				if (!global::Newtonsoft.Json.Utilities.CollectionUtils.ContainsValue(schema.Enum, jToken, new global::Newtonsoft.Json.Linq.JTokenEqualityComparer()))
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Value {0} is not defined in enum.", global::System.Globalization.CultureInfo.InvariantCulture, stringWriter.ToString()), schema);
				}
			}
			global::Newtonsoft.Json.Schema.JsonSchemaType? currentNodeSchemaType = GetCurrentNodeSchemaType();
			if (currentNodeSchemaType.HasValue && global::Newtonsoft.Json.Schema.JsonSchemaGenerator.HasFlag(schema.Disallow, currentNodeSchemaType.Value))
			{
				RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Type {0} is disallowed.", global::System.Globalization.CultureInfo.InvariantCulture, currentNodeSchemaType), schema);
			}
		}

		private global::Newtonsoft.Json.Schema.JsonSchemaType? GetCurrentNodeSchemaType()
		{
			switch (_reader.TokenType)
			{
			case global::Newtonsoft.Json.JsonToken.StartObject:
				return global::Newtonsoft.Json.Schema.JsonSchemaType.Object;
			case global::Newtonsoft.Json.JsonToken.StartArray:
				return global::Newtonsoft.Json.Schema.JsonSchemaType.Array;
			case global::Newtonsoft.Json.JsonToken.Integer:
				return global::Newtonsoft.Json.Schema.JsonSchemaType.Integer;
			case global::Newtonsoft.Json.JsonToken.Float:
				return global::Newtonsoft.Json.Schema.JsonSchemaType.Float;
			case global::Newtonsoft.Json.JsonToken.String:
				return global::Newtonsoft.Json.Schema.JsonSchemaType.String;
			case global::Newtonsoft.Json.JsonToken.Boolean:
				return global::Newtonsoft.Json.Schema.JsonSchemaType.Boolean;
			case global::Newtonsoft.Json.JsonToken.Null:
				return global::Newtonsoft.Json.Schema.JsonSchemaType.Null;
			default:
				return null;
			}
		}

		public override byte[] ReadAsBytes()
		{
			byte[] result = _reader.ReadAsBytes();
			ValidateCurrentToken();
			return result;
		}

		public override decimal? ReadAsDecimal()
		{
			decimal? result = _reader.ReadAsDecimal();
			ValidateCurrentToken();
			return result;
		}

		public override global::System.DateTimeOffset? ReadAsDateTimeOffset()
		{
			global::System.DateTimeOffset? result = _reader.ReadAsDateTimeOffset();
			ValidateCurrentToken();
			return result;
		}

		public override bool Read()
		{
			if (!_reader.Read())
			{
				return false;
			}
			if (_reader.TokenType == global::Newtonsoft.Json.JsonToken.Comment)
			{
				return true;
			}
			ValidateCurrentToken();
			return true;
		}

		private void ValidateCurrentToken()
		{
			if (_model == null)
			{
				global::Newtonsoft.Json.Schema.JsonSchemaModelBuilder jsonSchemaModelBuilder = new global::Newtonsoft.Json.Schema.JsonSchemaModelBuilder();
				_model = jsonSchemaModelBuilder.Build(_schema);
			}
			switch (_reader.TokenType)
			{
			case global::Newtonsoft.Json.JsonToken.StartObject:
			{
				ProcessValue();
				global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchemaModel> schemas2 = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(CurrentMemberSchemas, ValidateObject));
				Push(new global::Newtonsoft.Json.JsonValidatingReader.SchemaScope(global::Newtonsoft.Json.Linq.JTokenType.Object, schemas2));
				break;
			}
			case global::Newtonsoft.Json.JsonToken.StartArray:
			{
				ProcessValue();
				global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchemaModel> schemas = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Where(CurrentMemberSchemas, ValidateArray));
				Push(new global::Newtonsoft.Json.JsonValidatingReader.SchemaScope(global::Newtonsoft.Json.Linq.JTokenType.Array, schemas));
				break;
			}
			case global::Newtonsoft.Json.JsonToken.StartConstructor:
				Push(new global::Newtonsoft.Json.JsonValidatingReader.SchemaScope(global::Newtonsoft.Json.Linq.JTokenType.Constructor, null));
				break;
			case global::Newtonsoft.Json.JsonToken.PropertyName:
			{
				foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentSchema in CurrentSchemas)
				{
					ValidatePropertyName(currentSchema);
				}
				break;
			}
			case global::Newtonsoft.Json.JsonToken.Integer:
				ProcessValue();
				{
					foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentMemberSchema in CurrentMemberSchemas)
					{
						ValidateInteger(currentMemberSchema);
					}
					break;
				}
			case global::Newtonsoft.Json.JsonToken.Float:
				ProcessValue();
				{
					foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentMemberSchema2 in CurrentMemberSchemas)
					{
						ValidateFloat(currentMemberSchema2);
					}
					break;
				}
			case global::Newtonsoft.Json.JsonToken.String:
				ProcessValue();
				{
					foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentMemberSchema3 in CurrentMemberSchemas)
					{
						ValidateString(currentMemberSchema3);
					}
					break;
				}
			case global::Newtonsoft.Json.JsonToken.Boolean:
				ProcessValue();
				{
					foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentMemberSchema4 in CurrentMemberSchemas)
					{
						ValidateBoolean(currentMemberSchema4);
					}
					break;
				}
			case global::Newtonsoft.Json.JsonToken.Null:
				ProcessValue();
				{
					foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentMemberSchema5 in CurrentMemberSchemas)
					{
						ValidateNull(currentMemberSchema5);
					}
					break;
				}
			case global::Newtonsoft.Json.JsonToken.EndObject:
				foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentSchema2 in CurrentSchemas)
				{
					ValidateEndObject(currentSchema2);
				}
				Pop();
				break;
			case global::Newtonsoft.Json.JsonToken.EndArray:
				foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentSchema3 in CurrentSchemas)
				{
					ValidateEndArray(currentSchema3);
				}
				Pop();
				break;
			case global::Newtonsoft.Json.JsonToken.EndConstructor:
				Pop();
				break;
			default:
				throw new global::System.ArgumentOutOfRangeException();
			case global::Newtonsoft.Json.JsonToken.Raw:
			case global::Newtonsoft.Json.JsonToken.Undefined:
			case global::Newtonsoft.Json.JsonToken.Date:
				break;
			}
		}

		private void ValidateEndObject(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return;
			}
			global::System.Collections.Generic.Dictionary<string, bool> requiredProperties = _currentScope.RequiredProperties;
			if (requiredProperties != null)
			{
				global::System.Collections.Generic.List<string> list = global::System.Linq.Enumerable.ToList(global::System.Linq.Enumerable.Select(global::System.Linq.Enumerable.Where(requiredProperties, (global::System.Collections.Generic.KeyValuePair<string, bool> kv) => !kv.Value), (global::System.Collections.Generic.KeyValuePair<string, bool> kv) => kv.Key));
				if (list.Count > 0)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Required properties are missing from object: {0}.", global::System.Globalization.CultureInfo.InvariantCulture, string.Join(", ", list.ToArray())), schema);
				}
			}
		}

		private void ValidateEndArray(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema != null)
			{
				int arrayItemCount = _currentScope.ArrayItemCount;
				if (schema.MaximumItems.HasValue && arrayItemCount > schema.MaximumItems)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Array item count {0} exceeds maximum count of {1}.", global::System.Globalization.CultureInfo.InvariantCulture, arrayItemCount, schema.MaximumItems), schema);
				}
				if (schema.MinimumItems.HasValue && arrayItemCount < schema.MinimumItems)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Array item count {0} is less than minimum count of {1}.", global::System.Globalization.CultureInfo.InvariantCulture, arrayItemCount, schema.MinimumItems), schema);
				}
			}
		}

		private void ValidateNull(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema != null && TestType(schema, global::Newtonsoft.Json.Schema.JsonSchemaType.Null))
			{
				ValidateInEnumAndNotDisallowed(schema);
			}
		}

		private void ValidateBoolean(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema != null && TestType(schema, global::Newtonsoft.Json.Schema.JsonSchemaType.Boolean))
			{
				ValidateInEnumAndNotDisallowed(schema);
			}
		}

		private void ValidateString(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema == null || !TestType(schema, global::Newtonsoft.Json.Schema.JsonSchemaType.String))
			{
				return;
			}
			ValidateInEnumAndNotDisallowed(schema);
			string text = _reader.Value.ToString();
			if (schema.MaximumLength.HasValue && text.Length > schema.MaximumLength)
			{
				RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("String '{0}' exceeds maximum length of {1}.", global::System.Globalization.CultureInfo.InvariantCulture, text, schema.MaximumLength), schema);
			}
			if (schema.MinimumLength.HasValue && text.Length < schema.MinimumLength)
			{
				RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("String '{0}' is less than minimum length of {1}.", global::System.Globalization.CultureInfo.InvariantCulture, text, schema.MinimumLength), schema);
			}
			if (schema.Patterns == null)
			{
				return;
			}
			foreach (string pattern in schema.Patterns)
			{
				if (!global::System.Text.RegularExpressions.Regex.IsMatch(text, pattern))
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("String '{0}' does not match regex pattern '{1}'.", global::System.Globalization.CultureInfo.InvariantCulture, text, pattern), schema);
				}
			}
		}

		private void ValidateInteger(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema == null || !TestType(schema, global::Newtonsoft.Json.Schema.JsonSchemaType.Integer))
			{
				return;
			}
			ValidateInEnumAndNotDisallowed(schema);
			long num = global::System.Convert.ToInt64(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
			if (schema.Maximum.HasValue)
			{
				if ((double)num > schema.Maximum)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Integer {0} exceeds maximum value of {1}.", global::System.Globalization.CultureInfo.InvariantCulture, num, schema.Maximum), schema);
				}
				if (schema.ExclusiveMaximum && (double)num == schema.Maximum)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Integer {0} equals maximum value of {1} and exclusive maximum is true.", global::System.Globalization.CultureInfo.InvariantCulture, num, schema.Maximum), schema);
				}
			}
			if (schema.Minimum.HasValue)
			{
				if ((double)num < schema.Minimum)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Integer {0} is less than minimum value of {1}.", global::System.Globalization.CultureInfo.InvariantCulture, num, schema.Minimum), schema);
				}
				if (schema.ExclusiveMinimum && (double)num == schema.Minimum)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Integer {0} equals minimum value of {1} and exclusive minimum is true.", global::System.Globalization.CultureInfo.InvariantCulture, num, schema.Minimum), schema);
				}
			}
			if (schema.DivisibleBy.HasValue && !IsZero((double)num % schema.DivisibleBy.Value))
			{
				RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Integer {0} is not evenly divisible by {1}.", global::System.Globalization.CultureInfo.InvariantCulture, global::Newtonsoft.Json.JsonConvert.ToString(num), schema.DivisibleBy), schema);
			}
		}

		private void ProcessValue()
		{
			if (_currentScope == null || _currentScope.TokenType != global::Newtonsoft.Json.Linq.JTokenType.Array)
			{
				return;
			}
			_currentScope.ArrayItemCount++;
			foreach (global::Newtonsoft.Json.Schema.JsonSchemaModel currentSchema in CurrentSchemas)
			{
				if (currentSchema != null && currentSchema.Items != null && currentSchema.Items.Count > 1 && _currentScope.ArrayItemCount >= currentSchema.Items.Count)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Index {0} has not been defined and the schema does not allow additional items.", global::System.Globalization.CultureInfo.InvariantCulture, _currentScope.ArrayItemCount), currentSchema);
				}
			}
		}

		private void ValidateFloat(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema == null || !TestType(schema, global::Newtonsoft.Json.Schema.JsonSchemaType.Float))
			{
				return;
			}
			ValidateInEnumAndNotDisallowed(schema);
			double num = global::System.Convert.ToDouble(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
			if (schema.Maximum.HasValue)
			{
				if (num > schema.Maximum)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Float {0} exceeds maximum value of {1}.", global::System.Globalization.CultureInfo.InvariantCulture, global::Newtonsoft.Json.JsonConvert.ToString(num), schema.Maximum), schema);
				}
				if (schema.ExclusiveMaximum && num == schema.Maximum)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Float {0} equals maximum value of {1} and exclusive maximum is true.", global::System.Globalization.CultureInfo.InvariantCulture, global::Newtonsoft.Json.JsonConvert.ToString(num), schema.Maximum), schema);
				}
			}
			if (schema.Minimum.HasValue)
			{
				if (num < schema.Minimum)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Float {0} is less than minimum value of {1}.", global::System.Globalization.CultureInfo.InvariantCulture, global::Newtonsoft.Json.JsonConvert.ToString(num), schema.Minimum), schema);
				}
				if (schema.ExclusiveMinimum && num == schema.Minimum)
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Float {0} equals minimum value of {1} and exclusive minimum is true.", global::System.Globalization.CultureInfo.InvariantCulture, global::Newtonsoft.Json.JsonConvert.ToString(num), schema.Minimum), schema);
				}
			}
			if (schema.DivisibleBy.HasValue && !IsZero(num % schema.DivisibleBy.Value))
			{
				RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Float {0} is not evenly divisible by {1}.", global::System.Globalization.CultureInfo.InvariantCulture, global::Newtonsoft.Json.JsonConvert.ToString(num), schema.DivisibleBy), schema);
			}
		}

		private static bool IsZero(double value)
		{
			double num = 2.220446049250313E-16;
			return global::System.Math.Abs(value) < 10.0 * num;
		}

		private void ValidatePropertyName(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema != null)
			{
				string text = global::System.Convert.ToString(_reader.Value, global::System.Globalization.CultureInfo.InvariantCulture);
				if (_currentScope.RequiredProperties.ContainsKey(text))
				{
					_currentScope.RequiredProperties[text] = true;
				}
				if (!schema.AllowAdditionalProperties && !IsPropertyDefinied(schema, text))
				{
					RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Property '{0}' has not been defined and the schema does not allow additional properties.", global::System.Globalization.CultureInfo.InvariantCulture, text), schema);
				}
				_currentScope.CurrentPropertyName = text;
			}
		}

		private bool IsPropertyDefinied(global::Newtonsoft.Json.Schema.JsonSchemaModel schema, string propertyName)
		{
			if (schema.Properties != null && schema.Properties.ContainsKey(propertyName))
			{
				return true;
			}
			if (schema.PatternProperties != null)
			{
				foreach (string key in schema.PatternProperties.Keys)
				{
					if (global::System.Text.RegularExpressions.Regex.IsMatch(propertyName, key))
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool ValidateArray(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return true;
			}
			return TestType(schema, global::Newtonsoft.Json.Schema.JsonSchemaType.Array);
		}

		private bool ValidateObject(global::Newtonsoft.Json.Schema.JsonSchemaModel schema)
		{
			if (schema == null)
			{
				return true;
			}
			return TestType(schema, global::Newtonsoft.Json.Schema.JsonSchemaType.Object);
		}

		private bool TestType(global::Newtonsoft.Json.Schema.JsonSchemaModel currentSchema, global::Newtonsoft.Json.Schema.JsonSchemaType currentType)
		{
			if (!global::Newtonsoft.Json.Schema.JsonSchemaGenerator.HasFlag(currentSchema.Type, currentType))
			{
				RaiseError(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Invalid type. Expected {0} but got {1}.", global::System.Globalization.CultureInfo.InvariantCulture, currentSchema.Type, currentType), currentSchema);
				return false;
			}
			return true;
		}

		bool global::Newtonsoft.Json.IJsonLineInfo.HasLineInfo()
		{
			global::Newtonsoft.Json.IJsonLineInfo jsonLineInfo = _reader as global::Newtonsoft.Json.IJsonLineInfo;
			if (jsonLineInfo == null)
			{
				return false;
			}
			return jsonLineInfo.HasLineInfo();
		}
	}
}
