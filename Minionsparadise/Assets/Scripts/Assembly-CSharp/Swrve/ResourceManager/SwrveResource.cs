namespace Swrve.ResourceManager
{
	public class SwrveResource
	{
		public readonly global::System.Collections.Generic.Dictionary<string, string> Attributes;

		public SwrveResource(global::System.Collections.Generic.Dictionary<string, string> attributes)
		{
			Attributes = attributes;
		}

		public T GetAttribute<T>(string attributeName, T defaultValue)
		{
			if (Attributes.ContainsKey(attributeName))
			{
				string text = Attributes[attributeName];
				if (text != null)
				{
					return (T)global::System.Convert.ChangeType(text, typeof(T));
				}
			}
			return defaultValue;
		}
	}
}
