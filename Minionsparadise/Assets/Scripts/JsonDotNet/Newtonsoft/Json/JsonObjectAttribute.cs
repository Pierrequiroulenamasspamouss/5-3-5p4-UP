namespace Newtonsoft.Json
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Struct | global::System.AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class JsonObjectAttribute : global::Newtonsoft.Json.JsonContainerAttribute
	{
		private global::Newtonsoft.Json.MemberSerialization _memberSerialization;

		public global::Newtonsoft.Json.MemberSerialization MemberSerialization
		{
			get
			{
				return _memberSerialization;
			}
			set
			{
				_memberSerialization = value;
			}
		}

		public JsonObjectAttribute()
		{
		}

		public JsonObjectAttribute(global::Newtonsoft.Json.MemberSerialization memberSerialization)
		{
			MemberSerialization = memberSerialization;
		}

		public JsonObjectAttribute(string id)
			: base(id)
		{
		}
	}
}
