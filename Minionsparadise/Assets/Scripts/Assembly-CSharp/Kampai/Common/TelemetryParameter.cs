namespace Kampai.Common
{
	public struct TelemetryParameter
	{
		public string keyType;

		public object value;

		public global::Kampai.Common.ParameterName name;

		public TelemetryParameter(string keyType, object value, global::Kampai.Common.ParameterName name)
		{
			this.keyType = keyType;
			this.value = value;
			this.name = name;
		}
	}
}
