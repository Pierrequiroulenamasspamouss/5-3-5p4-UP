namespace Newtonsoft.Json.Serialization
{
	public class JsonProperty
	{
		public string PropertyName { get; set; }

		public int? Order { get; set; }

		public string UnderlyingName { get; set; }

		public global::Newtonsoft.Json.Serialization.IValueProvider ValueProvider { get; set; }

		public global::System.Type PropertyType { get; set; }

		public global::Newtonsoft.Json.JsonConverter Converter { get; set; }

		public global::Newtonsoft.Json.JsonConverter MemberConverter { get; set; }

		public bool Ignored { get; set; }

		public bool Readable { get; set; }

		public bool Writable { get; set; }

		public object DefaultValue { get; set; }

		public global::Newtonsoft.Json.Required Required { get; set; }

		public bool? IsReference { get; set; }

		public global::Newtonsoft.Json.NullValueHandling? NullValueHandling { get; set; }

		public global::Newtonsoft.Json.DefaultValueHandling? DefaultValueHandling { get; set; }

		public global::Newtonsoft.Json.ReferenceLoopHandling? ReferenceLoopHandling { get; set; }

		public global::Newtonsoft.Json.ObjectCreationHandling? ObjectCreationHandling { get; set; }

		public global::Newtonsoft.Json.TypeNameHandling? TypeNameHandling { get; set; }

		public global::System.Predicate<object> ShouldSerialize { get; set; }

		public global::System.Predicate<object> GetIsSpecified { get; set; }

		public global::System.Action<object, object> SetIsSpecified { get; set; }

		public override string ToString()
		{
			return PropertyName;
		}
	}
}
