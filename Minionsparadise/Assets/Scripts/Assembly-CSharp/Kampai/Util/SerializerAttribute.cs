namespace Kampai.Util
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Class | global::System.AttributeTargets.Property | global::System.AttributeTargets.Field | global::System.AttributeTargets.Interface)]
	public sealed class SerializerAttribute : global::System.Attribute
	{
		public string Signature { get; private set; }

		public SerializerAttribute(string signature)
		{
			Signature = signature;
		}
	}
}
