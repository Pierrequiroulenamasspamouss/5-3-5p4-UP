namespace System.Runtime.Serialization
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class EnumMemberAttribute : global::System.Attribute
	{
		private string value;

		public string Value
		{
			get
			{
				return value;
			}
			set
			{
				this.value = value;
			}
		}
	}
}
