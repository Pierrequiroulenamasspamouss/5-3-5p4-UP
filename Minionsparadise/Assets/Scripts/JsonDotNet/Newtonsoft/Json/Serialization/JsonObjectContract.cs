namespace Newtonsoft.Json.Serialization
{
	public class JsonObjectContract : global::Newtonsoft.Json.Serialization.JsonContract
	{
		public global::Newtonsoft.Json.MemberSerialization MemberSerialization { get; set; }

		public global::Newtonsoft.Json.Serialization.JsonPropertyCollection Properties { get; private set; }

		public global::Newtonsoft.Json.Serialization.JsonPropertyCollection ConstructorParameters { get; private set; }

		public global::System.Reflection.ConstructorInfo OverrideConstructor { get; set; }

		public global::System.Reflection.ConstructorInfo ParametrizedConstructor { get; set; }

		public JsonObjectContract(global::System.Type underlyingType)
			: base(underlyingType)
		{
			Properties = new global::Newtonsoft.Json.Serialization.JsonPropertyCollection(base.UnderlyingType);
			ConstructorParameters = new global::Newtonsoft.Json.Serialization.JsonPropertyCollection(base.UnderlyingType);
		}
	}
}
