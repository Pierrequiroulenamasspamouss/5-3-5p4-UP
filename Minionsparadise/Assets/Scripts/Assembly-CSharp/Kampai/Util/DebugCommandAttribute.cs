namespace Kampai.Util
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Method, AllowMultiple = true)]
	public sealed class DebugCommandAttribute : global::System.Attribute
	{
		public string Name { get; set; }

		public string[] Args { get; set; }

		public bool RequiresAllArgs { get; set; }

		public DebugCommandAttribute()
		{
			Name = null;
		}
	}
}
