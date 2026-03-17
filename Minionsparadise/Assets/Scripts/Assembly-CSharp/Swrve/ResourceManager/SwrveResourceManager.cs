namespace Swrve.ResourceManager
{
	public class SwrveResourceManager
	{
		public global::System.Collections.Generic.Dictionary<string, global::Swrve.ResourceManager.SwrveResource> UserResources;

		public void SetResourcesFromJSON(global::System.Collections.Generic.Dictionary<string, global::System.Collections.Generic.Dictionary<string, string>> userResources)
		{
			global::System.Collections.Generic.Dictionary<string, global::Swrve.ResourceManager.SwrveResource> dictionary = new global::System.Collections.Generic.Dictionary<string, global::Swrve.ResourceManager.SwrveResource>();
			foreach (string key in userResources.Keys)
			{
				dictionary[key] = new global::Swrve.ResourceManager.SwrveResource(userResources[key]);
			}
			UserResources = dictionary;
		}

		public global::Swrve.ResourceManager.SwrveResource GetResource(string resourceId)
		{
			if (UserResources != null)
			{
				if (UserResources.ContainsKey(resourceId))
				{
					return UserResources[resourceId];
				}
			}
			else
			{
				SwrveLog.LogWarning(string.Format("SwrveResourceManager::GetResource('{0}'): Resources are not available yet.", resourceId));
			}
			return null;
		}

		public T GetResourceAttribute<T>(string resourceId, string attributeName, T defaultValue)
		{
			global::Swrve.ResourceManager.SwrveResource resource = GetResource(resourceId);
			if (resource != null)
			{
				return resource.GetAttribute(attributeName, defaultValue);
			}
			return defaultValue;
		}
	}
}
