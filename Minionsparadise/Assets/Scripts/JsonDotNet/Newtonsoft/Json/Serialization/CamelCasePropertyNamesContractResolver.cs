namespace Newtonsoft.Json.Serialization
{
	public class CamelCasePropertyNamesContractResolver : global::Newtonsoft.Json.Serialization.DefaultContractResolver
	{
		public CamelCasePropertyNamesContractResolver()
			: base(true)
		{
		}

		protected internal override string ResolvePropertyName(string propertyName)
		{
			return global::Newtonsoft.Json.Utilities.StringUtils.ToCamelCase(propertyName);
		}
	}
}
