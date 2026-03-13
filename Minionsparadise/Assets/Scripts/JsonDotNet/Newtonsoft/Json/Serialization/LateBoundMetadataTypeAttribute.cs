namespace Newtonsoft.Json.Serialization
{
	internal class LateBoundMetadataTypeAttribute : global::Newtonsoft.Json.Serialization.IMetadataTypeAttribute
	{
		private static global::System.Reflection.PropertyInfo _metadataClassTypeProperty;

		private readonly object _attribute;

		public global::System.Type MetadataClassType
		{
			get
			{
				if (_metadataClassTypeProperty == null)
				{
					_metadataClassTypeProperty = _attribute.GetType().GetProperty("MetadataClassType");
				}
				return (global::System.Type)global::Newtonsoft.Json.Utilities.ReflectionUtils.GetMemberValue(_metadataClassTypeProperty, _attribute);
			}
		}

		public LateBoundMetadataTypeAttribute(object attribute)
		{
			_attribute = attribute;
		}
	}
}
