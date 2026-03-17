namespace Newtonsoft.Json
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Property | global::System.AttributeTargets.Field | global::System.AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonPropertyAttribute : global::System.Attribute
	{
		internal global::Newtonsoft.Json.NullValueHandling? _nullValueHandling;

		internal global::Newtonsoft.Json.DefaultValueHandling? _defaultValueHandling;

		internal global::Newtonsoft.Json.ReferenceLoopHandling? _referenceLoopHandling;

		internal global::Newtonsoft.Json.ObjectCreationHandling? _objectCreationHandling;

		internal global::Newtonsoft.Json.TypeNameHandling? _typeNameHandling;

		internal bool? _isReference;

		internal int? _order;

		public global::Newtonsoft.Json.NullValueHandling NullValueHandling
		{
			get
			{
				return _nullValueHandling ?? global::Newtonsoft.Json.NullValueHandling.Include;
			}
			set
			{
				_nullValueHandling = value;
			}
		}

		public global::Newtonsoft.Json.DefaultValueHandling DefaultValueHandling
		{
			get
			{
				return _defaultValueHandling ?? global::Newtonsoft.Json.DefaultValueHandling.Include;
			}
			set
			{
				_defaultValueHandling = value;
			}
		}

		public global::Newtonsoft.Json.ReferenceLoopHandling ReferenceLoopHandling
		{
			get
			{
				return _referenceLoopHandling ?? global::Newtonsoft.Json.ReferenceLoopHandling.Error;
			}
			set
			{
				_referenceLoopHandling = value;
			}
		}

		public global::Newtonsoft.Json.ObjectCreationHandling ObjectCreationHandling
		{
			get
			{
				return _objectCreationHandling ?? global::Newtonsoft.Json.ObjectCreationHandling.Auto;
			}
			set
			{
				_objectCreationHandling = value;
			}
		}

		public global::Newtonsoft.Json.TypeNameHandling TypeNameHandling
		{
			get
			{
				return _typeNameHandling ?? global::Newtonsoft.Json.TypeNameHandling.None;
			}
			set
			{
				_typeNameHandling = value;
			}
		}

		public bool IsReference
		{
			get
			{
				return _isReference ?? false;
			}
			set
			{
				_isReference = value;
			}
		}

		public int Order
		{
			get
			{
				return _order ?? 0;
			}
			set
			{
				_order = value;
			}
		}

		public string PropertyName { get; set; }

		public global::Newtonsoft.Json.Required Required { get; set; }

		public JsonPropertyAttribute()
		{
		}

		public JsonPropertyAttribute(string propertyName)
		{
			PropertyName = propertyName;
		}
	}
}
