namespace Newtonsoft.Json.Schema
{
	public class JsonSchemaGenerator
	{
		private class TypeSchema
		{
			public global::System.Type Type { get; private set; }

			public global::Newtonsoft.Json.Schema.JsonSchema Schema { get; private set; }

			public TypeSchema(global::System.Type type, global::Newtonsoft.Json.Schema.JsonSchema schema)
			{
				global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
				global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(schema, "schema");
				Type = type;
				Schema = schema;
			}
		}

		private global::Newtonsoft.Json.Serialization.IContractResolver _contractResolver;

		private global::Newtonsoft.Json.Schema.JsonSchemaResolver _resolver;

		private global::System.Collections.Generic.IList<global::Newtonsoft.Json.Schema.JsonSchemaGenerator.TypeSchema> _stack = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchemaGenerator.TypeSchema>();

		private global::Newtonsoft.Json.Schema.JsonSchema _currentSchema;

		public global::Newtonsoft.Json.Schema.UndefinedSchemaIdHandling UndefinedSchemaIdHandling { get; set; }

		public global::Newtonsoft.Json.Serialization.IContractResolver ContractResolver
		{
			get
			{
				if (_contractResolver == null)
				{
					return global::Newtonsoft.Json.Serialization.DefaultContractResolver.Instance;
				}
				return _contractResolver;
			}
			set
			{
				_contractResolver = value;
			}
		}

		private global::Newtonsoft.Json.Schema.JsonSchema CurrentSchema
		{
			get
			{
				return _currentSchema;
			}
		}

		private void Push(global::Newtonsoft.Json.Schema.JsonSchemaGenerator.TypeSchema typeSchema)
		{
			_currentSchema = typeSchema.Schema;
			_stack.Add(typeSchema);
			_resolver.LoadedSchemas.Add(typeSchema.Schema);
		}

		private global::Newtonsoft.Json.Schema.JsonSchemaGenerator.TypeSchema Pop()
		{
			global::Newtonsoft.Json.Schema.JsonSchemaGenerator.TypeSchema result = _stack[_stack.Count - 1];
			_stack.RemoveAt(_stack.Count - 1);
			global::Newtonsoft.Json.Schema.JsonSchemaGenerator.TypeSchema typeSchema = global::System.Linq.Enumerable.LastOrDefault(_stack);
			if (typeSchema != null)
			{
				_currentSchema = typeSchema.Schema;
			}
			else
			{
				_currentSchema = null;
			}
			return result;
		}

		public global::Newtonsoft.Json.Schema.JsonSchema Generate(global::System.Type type)
		{
			return Generate(type, new global::Newtonsoft.Json.Schema.JsonSchemaResolver(), false);
		}

		public global::Newtonsoft.Json.Schema.JsonSchema Generate(global::System.Type type, global::Newtonsoft.Json.Schema.JsonSchemaResolver resolver)
		{
			return Generate(type, resolver, false);
		}

		public global::Newtonsoft.Json.Schema.JsonSchema Generate(global::System.Type type, bool rootSchemaNullable)
		{
			return Generate(type, new global::Newtonsoft.Json.Schema.JsonSchemaResolver(), rootSchemaNullable);
		}

		public global::Newtonsoft.Json.Schema.JsonSchema Generate(global::System.Type type, global::Newtonsoft.Json.Schema.JsonSchemaResolver resolver, bool rootSchemaNullable)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(resolver, "resolver");
			_resolver = resolver;
			return GenerateInternal(type, (!rootSchemaNullable) ? global::Newtonsoft.Json.Required.Always : global::Newtonsoft.Json.Required.Default, false);
		}

		private string GetTitle(global::System.Type type)
		{
			global::Newtonsoft.Json.JsonContainerAttribute jsonContainerAttribute = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Title))
			{
				return jsonContainerAttribute.Title;
			}
			return null;
		}

		private string GetDescription(global::System.Type type)
		{
			global::Newtonsoft.Json.JsonContainerAttribute jsonContainerAttribute = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Description))
			{
				return jsonContainerAttribute.Description;
			}
			global::System.ComponentModel.DescriptionAttribute attribute = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetAttribute<global::System.ComponentModel.DescriptionAttribute>(type);
			if (attribute != null)
			{
				return attribute.Description;
			}
			return null;
		}

		private string GetTypeId(global::System.Type type, bool explicitOnly)
		{
			global::Newtonsoft.Json.JsonContainerAttribute jsonContainerAttribute = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetJsonContainerAttribute(type);
			if (jsonContainerAttribute != null && !string.IsNullOrEmpty(jsonContainerAttribute.Id))
			{
				return jsonContainerAttribute.Id;
			}
			if (explicitOnly)
			{
				return null;
			}
			switch (UndefinedSchemaIdHandling)
			{
			case global::Newtonsoft.Json.Schema.UndefinedSchemaIdHandling.UseTypeName:
				return type.FullName;
			case global::Newtonsoft.Json.Schema.UndefinedSchemaIdHandling.UseAssemblyQualifiedName:
				return type.AssemblyQualifiedName;
			default:
				return null;
			}
		}

		private global::Newtonsoft.Json.Schema.JsonSchema GenerateInternal(global::System.Type type, global::Newtonsoft.Json.Required valueRequired, bool required)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(type, "type");
			string typeId = GetTypeId(type, false);
			string typeId2 = GetTypeId(type, true);
			if (!string.IsNullOrEmpty(typeId))
			{
				global::Newtonsoft.Json.Schema.JsonSchema schema = _resolver.GetSchema(typeId);
				if (schema != null)
				{
					if (valueRequired != global::Newtonsoft.Json.Required.Always && !HasFlag(schema.Type, global::Newtonsoft.Json.Schema.JsonSchemaType.Null))
					{
						schema.Type |= global::Newtonsoft.Json.Schema.JsonSchemaType.Null;
					}
					if (required && schema.Required != true)
					{
						schema.Required = true;
					}
					return schema;
				}
			}
			if (global::System.Linq.Enumerable.Any(_stack, (global::Newtonsoft.Json.Schema.JsonSchemaGenerator.TypeSchema tc) => tc.Type == type))
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unresolved circular reference for type '{0}'. Explicitly define an Id for the type using a JsonObject/JsonArray attribute or automatically generate a type Id using the UndefinedSchemaIdHandling property.", global::System.Globalization.CultureInfo.InvariantCulture, type));
			}
			global::Newtonsoft.Json.Serialization.JsonContract jsonContract = ContractResolver.ResolveContract(type);
			global::Newtonsoft.Json.JsonConverter jsonConverter;
			if ((jsonConverter = jsonContract.Converter) != null || (jsonConverter = jsonContract.InternalConverter) != null)
			{
				global::Newtonsoft.Json.Schema.JsonSchema schema2 = jsonConverter.GetSchema();
				if (schema2 != null)
				{
					return schema2;
				}
			}
			Push(new global::Newtonsoft.Json.Schema.JsonSchemaGenerator.TypeSchema(type, new global::Newtonsoft.Json.Schema.JsonSchema()));
			if (typeId2 != null)
			{
				CurrentSchema.Id = typeId2;
			}
			if (required)
			{
				CurrentSchema.Required = true;
			}
			CurrentSchema.Title = GetTitle(type);
			CurrentSchema.Description = GetDescription(type);
			if (jsonConverter != null)
			{
				CurrentSchema.Type = global::Newtonsoft.Json.Schema.JsonSchemaType.Any;
			}
			else if (jsonContract is global::Newtonsoft.Json.Serialization.JsonDictionaryContract)
			{
				CurrentSchema.Type = AddNullType(global::Newtonsoft.Json.Schema.JsonSchemaType.Object, valueRequired);
				global::System.Type keyType;
				global::System.Type valueType;
				global::Newtonsoft.Json.Utilities.ReflectionUtils.GetDictionaryKeyValueTypes(type, out keyType, out valueType);
				if (keyType != null && typeof(global::System.IConvertible).IsAssignableFrom(keyType))
				{
					CurrentSchema.AdditionalProperties = GenerateInternal(valueType, global::Newtonsoft.Json.Required.Default, false);
				}
			}
			else if (jsonContract is global::Newtonsoft.Json.Serialization.JsonArrayContract)
			{
				CurrentSchema.Type = AddNullType(global::Newtonsoft.Json.Schema.JsonSchemaType.Array, valueRequired);
				CurrentSchema.Id = GetTypeId(type, false);
				global::Newtonsoft.Json.JsonArrayAttribute jsonArrayAttribute = global::Newtonsoft.Json.Serialization.JsonTypeReflector.GetJsonContainerAttribute(type) as global::Newtonsoft.Json.JsonArrayAttribute;
				bool flag = jsonArrayAttribute == null || jsonArrayAttribute.AllowNullItems;
				global::System.Type collectionItemType = global::Newtonsoft.Json.Utilities.ReflectionUtils.GetCollectionItemType(type);
				if (collectionItemType != null)
				{
					CurrentSchema.Items = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Schema.JsonSchema>();
					CurrentSchema.Items.Add(GenerateInternal(collectionItemType, (!flag) ? global::Newtonsoft.Json.Required.Always : global::Newtonsoft.Json.Required.Default, false));
				}
			}
			else if (jsonContract is global::Newtonsoft.Json.Serialization.JsonPrimitiveContract)
			{
				CurrentSchema.Type = GetJsonSchemaType(type, valueRequired);
				global::Newtonsoft.Json.Schema.JsonSchemaType? type2 = CurrentSchema.Type;
				if (type2 == global::Newtonsoft.Json.Schema.JsonSchemaType.Integer && type2.HasValue && type.IsEnum && !type.IsDefined(typeof(global::System.FlagsAttribute), true))
				{
					CurrentSchema.Enum = new global::System.Collections.Generic.List<global::Newtonsoft.Json.Linq.JToken>();
					CurrentSchema.Options = new global::System.Collections.Generic.Dictionary<global::Newtonsoft.Json.Linq.JToken, string>();
					global::Newtonsoft.Json.Utilities.EnumValues<long> namesAndValues = global::Newtonsoft.Json.Utilities.EnumUtils.GetNamesAndValues<long>(type);
					foreach (global::Newtonsoft.Json.Utilities.EnumValue<long> item in namesAndValues)
					{
						global::Newtonsoft.Json.Linq.JToken jToken = global::Newtonsoft.Json.Linq.JToken.FromObject(item.Value);
						CurrentSchema.Enum.Add(jToken);
						CurrentSchema.Options.Add(jToken, item.Name);
					}
				}
			}
			else if (jsonContract is global::Newtonsoft.Json.Serialization.JsonObjectContract)
			{
				CurrentSchema.Type = AddNullType(global::Newtonsoft.Json.Schema.JsonSchemaType.Object, valueRequired);
				CurrentSchema.Id = GetTypeId(type, false);
				GenerateObjectSchema(type, (global::Newtonsoft.Json.Serialization.JsonObjectContract)jsonContract);
			}
			else if (jsonContract is global::Newtonsoft.Json.Serialization.JsonISerializableContract)
			{
				CurrentSchema.Type = AddNullType(global::Newtonsoft.Json.Schema.JsonSchemaType.Object, valueRequired);
				CurrentSchema.Id = GetTypeId(type, false);
				GenerateISerializableContract(type, (global::Newtonsoft.Json.Serialization.JsonISerializableContract)jsonContract);
			}
			else if (jsonContract is global::Newtonsoft.Json.Serialization.JsonStringContract)
			{
				global::Newtonsoft.Json.Schema.JsonSchemaType value = ((!global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullable(jsonContract.UnderlyingType)) ? global::Newtonsoft.Json.Schema.JsonSchemaType.String : AddNullType(global::Newtonsoft.Json.Schema.JsonSchemaType.String, valueRequired));
				CurrentSchema.Type = value;
			}
			else
			{
				if (!(jsonContract is global::Newtonsoft.Json.Serialization.JsonLinqContract))
				{
					throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected contract type: {0}", global::System.Globalization.CultureInfo.InvariantCulture, jsonContract));
				}
				CurrentSchema.Type = global::Newtonsoft.Json.Schema.JsonSchemaType.Any;
			}
			return Pop().Schema;
		}

		private global::Newtonsoft.Json.Schema.JsonSchemaType AddNullType(global::Newtonsoft.Json.Schema.JsonSchemaType type, global::Newtonsoft.Json.Required valueRequired)
		{
			if (valueRequired != global::Newtonsoft.Json.Required.Always)
			{
				return type | global::Newtonsoft.Json.Schema.JsonSchemaType.Null;
			}
			return type;
		}

		private bool HasFlag(global::Newtonsoft.Json.DefaultValueHandling value, global::Newtonsoft.Json.DefaultValueHandling flag)
		{
			return (value & flag) == flag;
		}

		private void GenerateObjectSchema(global::System.Type type, global::Newtonsoft.Json.Serialization.JsonObjectContract contract)
		{
			CurrentSchema.Properties = new global::System.Collections.Generic.Dictionary<string, global::Newtonsoft.Json.Schema.JsonSchema>();
			foreach (global::Newtonsoft.Json.Serialization.JsonProperty property in contract.Properties)
			{
				if (!property.Ignored)
				{
					bool flag = property.NullValueHandling == global::Newtonsoft.Json.NullValueHandling.Ignore || HasFlag(property.DefaultValueHandling.GetValueOrDefault(), global::Newtonsoft.Json.DefaultValueHandling.Ignore) || property.ShouldSerialize != null || property.GetIsSpecified != null;
					global::Newtonsoft.Json.Schema.JsonSchema jsonSchema = GenerateInternal(property.PropertyType, property.Required, !flag);
					if (property.DefaultValue != null)
					{
						jsonSchema.Default = global::Newtonsoft.Json.Linq.JToken.FromObject(property.DefaultValue);
					}
					CurrentSchema.Properties.Add(property.PropertyName, jsonSchema);
				}
			}
			if (type.IsSealed)
			{
				CurrentSchema.AllowAdditionalProperties = false;
			}
		}

		private void GenerateISerializableContract(global::System.Type type, global::Newtonsoft.Json.Serialization.JsonISerializableContract contract)
		{
			CurrentSchema.AllowAdditionalProperties = true;
		}

		internal static bool HasFlag(global::Newtonsoft.Json.Schema.JsonSchemaType? value, global::Newtonsoft.Json.Schema.JsonSchemaType flag)
		{
			if (!value.HasValue)
			{
				return true;
			}
			return (value & flag) == flag;
		}

		private global::Newtonsoft.Json.Schema.JsonSchemaType GetJsonSchemaType(global::System.Type type, global::Newtonsoft.Json.Required valueRequired)
		{
			global::Newtonsoft.Json.Schema.JsonSchemaType jsonSchemaType = global::Newtonsoft.Json.Schema.JsonSchemaType.None;
			if (valueRequired != global::Newtonsoft.Json.Required.Always && global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullable(type))
			{
				jsonSchemaType = global::Newtonsoft.Json.Schema.JsonSchemaType.Null;
				if (global::Newtonsoft.Json.Utilities.ReflectionUtils.IsNullableType(type))
				{
					type = global::System.Nullable.GetUnderlyingType(type);
				}
			}
			global::System.TypeCode typeCode = global::System.Type.GetTypeCode(type);
			switch (typeCode)
			{
			case global::System.TypeCode.Empty:
			case global::System.TypeCode.Object:
				return jsonSchemaType | global::Newtonsoft.Json.Schema.JsonSchemaType.String;
			case global::System.TypeCode.DBNull:
				return jsonSchemaType | global::Newtonsoft.Json.Schema.JsonSchemaType.Null;
			case global::System.TypeCode.Boolean:
				return jsonSchemaType | global::Newtonsoft.Json.Schema.JsonSchemaType.Boolean;
			case global::System.TypeCode.Char:
				return jsonSchemaType | global::Newtonsoft.Json.Schema.JsonSchemaType.String;
			case global::System.TypeCode.SByte:
			case global::System.TypeCode.Byte:
			case global::System.TypeCode.Int16:
			case global::System.TypeCode.UInt16:
			case global::System.TypeCode.Int32:
			case global::System.TypeCode.UInt32:
			case global::System.TypeCode.Int64:
			case global::System.TypeCode.UInt64:
				return jsonSchemaType | global::Newtonsoft.Json.Schema.JsonSchemaType.Integer;
			case global::System.TypeCode.Single:
			case global::System.TypeCode.Double:
			case global::System.TypeCode.Decimal:
				return jsonSchemaType | global::Newtonsoft.Json.Schema.JsonSchemaType.Float;
			case global::System.TypeCode.DateTime:
				return jsonSchemaType | global::Newtonsoft.Json.Schema.JsonSchemaType.String;
			case global::System.TypeCode.String:
				return jsonSchemaType | global::Newtonsoft.Json.Schema.JsonSchemaType.String;
			default:
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Unexpected type code '{0}' for type '{1}'.", global::System.Globalization.CultureInfo.InvariantCulture, typeCode, type));
			}
		}
	}
}
