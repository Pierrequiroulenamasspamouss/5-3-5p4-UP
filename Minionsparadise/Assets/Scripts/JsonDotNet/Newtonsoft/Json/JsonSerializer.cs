namespace Newtonsoft.Json
{
	public class JsonSerializer
	{
		private global::Newtonsoft.Json.TypeNameHandling _typeNameHandling;

		private global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle _typeNameAssemblyFormat;

		private global::Newtonsoft.Json.PreserveReferencesHandling _preserveReferencesHandling;

		private global::Newtonsoft.Json.ReferenceLoopHandling _referenceLoopHandling;

		private global::Newtonsoft.Json.MissingMemberHandling _missingMemberHandling;

		private global::Newtonsoft.Json.ObjectCreationHandling _objectCreationHandling;

		private global::Newtonsoft.Json.NullValueHandling _nullValueHandling;

		private global::Newtonsoft.Json.DefaultValueHandling _defaultValueHandling;

		private global::Newtonsoft.Json.ConstructorHandling _constructorHandling;

		private global::Newtonsoft.Json.JsonConverterCollection _converters;

		private global::Newtonsoft.Json.Serialization.IContractResolver _contractResolver;

		private global::Newtonsoft.Json.Serialization.IReferenceResolver _referenceResolver;

		private global::System.Runtime.Serialization.SerializationBinder _binder;

		private global::System.Runtime.Serialization.StreamingContext _context;

		public virtual global::Newtonsoft.Json.Serialization.IReferenceResolver ReferenceResolver
		{
			get
			{
				if (_referenceResolver == null)
				{
					_referenceResolver = new global::Newtonsoft.Json.Serialization.DefaultReferenceResolver();
				}
				return _referenceResolver;
			}
			set
			{
				if (value == null)
				{
					throw new global::System.ArgumentNullException("value", "Reference resolver cannot be null.");
				}
				_referenceResolver = value;
			}
		}

		public virtual global::System.Runtime.Serialization.SerializationBinder Binder
		{
			get
			{
				return _binder;
			}
			set
			{
				if (value == null)
				{
					throw new global::System.ArgumentNullException("value", "Serialization binder cannot be null.");
				}
				_binder = value;
			}
		}

		public virtual global::Newtonsoft.Json.TypeNameHandling TypeNameHandling
		{
			get
			{
				return _typeNameHandling;
			}
			set
			{
				if (value < global::Newtonsoft.Json.TypeNameHandling.None || value > global::Newtonsoft.Json.TypeNameHandling.Auto)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_typeNameHandling = value;
			}
		}

		public virtual global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle TypeNameAssemblyFormat
		{
			get
			{
				return _typeNameAssemblyFormat;
			}
			set
			{
				if (value < global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple || value > global::System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_typeNameAssemblyFormat = value;
			}
		}

		public virtual global::Newtonsoft.Json.PreserveReferencesHandling PreserveReferencesHandling
		{
			get
			{
				return _preserveReferencesHandling;
			}
			set
			{
				if (value < global::Newtonsoft.Json.PreserveReferencesHandling.None || value > global::Newtonsoft.Json.PreserveReferencesHandling.All)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_preserveReferencesHandling = value;
			}
		}

		public virtual global::Newtonsoft.Json.ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return _referenceLoopHandling;
			}
			set
			{
				if (value < global::Newtonsoft.Json.ReferenceLoopHandling.Error || value > global::Newtonsoft.Json.ReferenceLoopHandling.Serialize)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_referenceLoopHandling = value;
			}
		}

		public virtual global::Newtonsoft.Json.MissingMemberHandling MissingMemberHandling
		{
			get
			{
				return _missingMemberHandling;
			}
			set
			{
				if (value < global::Newtonsoft.Json.MissingMemberHandling.Ignore || value > global::Newtonsoft.Json.MissingMemberHandling.Error)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_missingMemberHandling = value;
			}
		}

		public virtual global::Newtonsoft.Json.NullValueHandling NullValueHandling
		{
			get
			{
				return _nullValueHandling;
			}
			set
			{
				if (value < global::Newtonsoft.Json.NullValueHandling.Include || value > global::Newtonsoft.Json.NullValueHandling.Ignore)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_nullValueHandling = value;
			}
		}

		public virtual global::Newtonsoft.Json.DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return _defaultValueHandling;
			}
			set
			{
				if (value < global::Newtonsoft.Json.DefaultValueHandling.Include || value > global::Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_defaultValueHandling = value;
			}
		}

		public virtual global::Newtonsoft.Json.ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return _objectCreationHandling;
			}
			set
			{
				if (value < global::Newtonsoft.Json.ObjectCreationHandling.Auto || value > global::Newtonsoft.Json.ObjectCreationHandling.Replace)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_objectCreationHandling = value;
			}
		}

		public virtual global::Newtonsoft.Json.ConstructorHandling ConstructorHandling
		{
			get
			{
				return _constructorHandling;
			}
			set
			{
				if (value < global::Newtonsoft.Json.ConstructorHandling.Default || value > global::Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor)
				{
					throw new global::System.ArgumentOutOfRangeException("value");
				}
				_constructorHandling = value;
			}
		}

		public virtual global::Newtonsoft.Json.JsonConverterCollection Converters
		{
			get
			{
				if (_converters == null)
				{
					_converters = new global::Newtonsoft.Json.JsonConverterCollection();
				}
				return _converters;
			}
		}

		public virtual global::Newtonsoft.Json.Serialization.IContractResolver ContractResolver
		{
			get
			{
				if (_contractResolver == null)
				{
					_contractResolver = global::Newtonsoft.Json.Serialization.DefaultContractResolver.Instance;
				}
				return _contractResolver;
			}
			set
			{
				_contractResolver = value;
			}
		}

		public virtual global::System.Runtime.Serialization.StreamingContext Context
		{
			get
			{
				return _context;
			}
			set
			{
				_context = value;
			}
		}

		public virtual event global::System.EventHandler<global::Newtonsoft.Json.Serialization.ErrorEventArgs> Error;

		public JsonSerializer()
		{
			_referenceLoopHandling = global::Newtonsoft.Json.ReferenceLoopHandling.Error;
			_missingMemberHandling = global::Newtonsoft.Json.MissingMemberHandling.Ignore;
			_nullValueHandling = global::Newtonsoft.Json.NullValueHandling.Include;
			_defaultValueHandling = global::Newtonsoft.Json.DefaultValueHandling.Include;
			_objectCreationHandling = global::Newtonsoft.Json.ObjectCreationHandling.Auto;
			_preserveReferencesHandling = global::Newtonsoft.Json.PreserveReferencesHandling.None;
			_constructorHandling = global::Newtonsoft.Json.ConstructorHandling.Default;
			_typeNameHandling = global::Newtonsoft.Json.TypeNameHandling.None;
			_context = global::Newtonsoft.Json.JsonSerializerSettings.DefaultContext;
			_binder = global::Newtonsoft.Json.Serialization.DefaultSerializationBinder.Instance;
		}

		public static global::Newtonsoft.Json.JsonSerializer Create(global::Newtonsoft.Json.JsonSerializerSettings settings)
		{
			global::Newtonsoft.Json.JsonSerializer jsonSerializer = new global::Newtonsoft.Json.JsonSerializer();
			if (settings != null)
			{
				if (!global::Newtonsoft.Json.Utilities.CollectionUtils.IsNullOrEmpty(settings.Converters))
				{
					global::Newtonsoft.Json.Utilities.CollectionUtils.AddRange(jsonSerializer.Converters, settings.Converters);
				}
				jsonSerializer.TypeNameHandling = settings.TypeNameHandling;
				jsonSerializer.TypeNameAssemblyFormat = settings.TypeNameAssemblyFormat;
				jsonSerializer.PreserveReferencesHandling = settings.PreserveReferencesHandling;
				jsonSerializer.ReferenceLoopHandling = settings.ReferenceLoopHandling;
				jsonSerializer.MissingMemberHandling = settings.MissingMemberHandling;
				jsonSerializer.ObjectCreationHandling = settings.ObjectCreationHandling;
				jsonSerializer.NullValueHandling = settings.NullValueHandling;
				jsonSerializer.DefaultValueHandling = settings.DefaultValueHandling;
				jsonSerializer.ConstructorHandling = settings.ConstructorHandling;
				jsonSerializer.Context = settings.Context;
				if (settings.Error != null)
				{
					jsonSerializer.Error += settings.Error;
				}
				if (settings.ContractResolver != null)
				{
					jsonSerializer.ContractResolver = settings.ContractResolver;
				}
				if (settings.ReferenceResolver != null)
				{
					jsonSerializer.ReferenceResolver = settings.ReferenceResolver;
				}
				if (settings.Binder != null)
				{
					jsonSerializer.Binder = settings.Binder;
				}
			}
			return jsonSerializer;
		}

		public void Populate(global::System.IO.TextReader reader, object target)
		{
			Populate(new global::Newtonsoft.Json.JsonTextReader(reader), target);
		}

		public void Populate(global::Newtonsoft.Json.JsonReader reader, object target)
		{
			PopulateInternal(reader, target);
		}

		internal virtual void PopulateInternal(global::Newtonsoft.Json.JsonReader reader, object target)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(reader, "reader");
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(target, "target");
			global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader jsonSerializerInternalReader = new global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader(this);
			jsonSerializerInternalReader.Populate(reader, target);
		}

		public object Deserialize(global::Newtonsoft.Json.JsonReader reader)
		{
			return Deserialize(reader, null);
		}

		public object Deserialize(global::System.IO.TextReader reader, global::System.Type objectType)
		{
			return Deserialize(new global::Newtonsoft.Json.JsonTextReader(reader), objectType);
		}

		public T Deserialize<T>(global::Newtonsoft.Json.JsonReader reader)
		{
			return (T)Deserialize(reader, typeof(T));
		}

		public object Deserialize(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType)
		{
			return DeserializeInternal(reader, objectType);
		}

		internal virtual object DeserializeInternal(global::Newtonsoft.Json.JsonReader reader, global::System.Type objectType)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(reader, "reader");
			global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader jsonSerializerInternalReader = new global::Newtonsoft.Json.Serialization.JsonSerializerInternalReader(this);
			return jsonSerializerInternalReader.Deserialize(reader, objectType);
		}

		public void Serialize(global::System.IO.TextWriter textWriter, object value)
		{
			Serialize(new global::Newtonsoft.Json.JsonTextWriter(textWriter), value);
		}

		public void Serialize(global::Newtonsoft.Json.JsonWriter jsonWriter, object value)
		{
			SerializeInternal(jsonWriter, value);
		}

		internal virtual void SerializeInternal(global::Newtonsoft.Json.JsonWriter jsonWriter, object value)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(jsonWriter, "jsonWriter");
			global::Newtonsoft.Json.Serialization.JsonSerializerInternalWriter jsonSerializerInternalWriter = new global::Newtonsoft.Json.Serialization.JsonSerializerInternalWriter(this);
			jsonSerializerInternalWriter.Serialize(jsonWriter, value);
		}

		internal global::Newtonsoft.Json.JsonConverter GetMatchingConverter(global::System.Type type)
		{
			return GetMatchingConverter(_converters, type);
		}

		internal static global::Newtonsoft.Json.JsonConverter GetMatchingConverter(global::System.Collections.Generic.IList<global::Newtonsoft.Json.JsonConverter> converters, global::System.Type objectType)
		{
			global::Newtonsoft.Json.Utilities.ValidationUtils.ArgumentNotNull(objectType, "objectType");
			if (converters != null)
			{
				for (int i = 0; i < converters.Count; i++)
				{
					global::Newtonsoft.Json.JsonConverter jsonConverter = converters[i];
					if (jsonConverter.CanConvert(objectType))
					{
						return jsonConverter;
					}
				}
			}
			return null;
		}

		internal void OnError(global::Newtonsoft.Json.Serialization.ErrorEventArgs e)
		{
			global::System.EventHandler<global::Newtonsoft.Json.Serialization.ErrorEventArgs> error = this.Error;
			if (error != null)
			{
				error(this, e);
			}
		}
	}
}
