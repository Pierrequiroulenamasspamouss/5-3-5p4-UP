namespace Newtonsoft.Json
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Struct | global::System.AttributeTargets.Enum | global::System.AttributeTargets.Property | global::System.AttributeTargets.Field | global::System.AttributeTargets.Interface | global::System.AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonConverterAttribute : global::System.Attribute
	{
		private readonly global::System.Type _converterType;

		public global::System.Type ConverterType
		{
			get
			{
				return _converterType;
			}
		}

		public JsonConverterAttribute(global::System.Type converterType)
		{
			if (converterType == null)
			{
				throw new global::System.ArgumentNullException("converterType");
			}
			_converterType = converterType;
		}

		internal static global::Newtonsoft.Json.JsonConverter CreateJsonConverterInstance(global::System.Type converterType)
		{
			try
			{
				return (global::Newtonsoft.Json.JsonConverter)global::System.Activator.CreateInstance(converterType);
			}
			catch (global::System.Exception innerException)
			{
				throw new global::System.Exception(global::Newtonsoft.Json.Utilities.StringUtils.FormatWith("Error creating {0}", global::System.Globalization.CultureInfo.InvariantCulture, converterType), innerException);
			}
		}
	}
}
