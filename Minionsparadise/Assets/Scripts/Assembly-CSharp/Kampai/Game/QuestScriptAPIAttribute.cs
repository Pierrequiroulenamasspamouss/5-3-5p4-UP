namespace Kampai.Game
{
	[global::System.AttributeUsage(global::System.AttributeTargets.Method)]
	public sealed class QuestScriptAPIAttribute : global::System.Attribute
	{
		public string Name { get; private set; }

		public QuestScriptAPIAttribute(string name)
		{
			Name = name;
		}
	}
}
