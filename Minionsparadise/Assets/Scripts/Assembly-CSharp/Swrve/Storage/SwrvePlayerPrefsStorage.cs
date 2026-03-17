namespace Swrve.Storage
{
	public class SwrvePlayerPrefsStorage : global::Swrve.ISwrveStorage
	{
		public virtual void Save(string tag, string data, string userId = null)
		{
			bool flag = false;
			try
			{
				string text = tag + ((userId != null) ? userId : string.Empty);
				SwrveLog.Log("Setting " + text + " in PlayerPrefs", "storage");
				global::UnityEngine.PlayerPrefs.SetString(text, data);
				flag = true;
			}
			catch (global::UnityEngine.PlayerPrefsException ex)
			{
				SwrveLog.LogError(ex.ToString(), "storage");
			}
			if (!flag)
			{
				SwrveLog.LogWarning(tag + " not saved!", "storage");
			}
		}

		public virtual string Load(string tag, string userId = null)
		{
			string result = null;
			try
			{
				string key = tag + ((userId != null) ? userId : string.Empty);
				if (global::UnityEngine.PlayerPrefs.HasKey(key))
				{
					SwrveLog.Log("Got " + tag + " from PlayerPrefs", "storage");
					result = global::UnityEngine.PlayerPrefs.GetString(key);
				}
			}
			catch (global::UnityEngine.PlayerPrefsException ex)
			{
				SwrveLog.LogError(ex.ToString(), "storage");
			}
			return result;
		}

		public virtual void Remove(string tag, string userId = null)
		{
			try
			{
				string text = tag + ((userId != null) ? userId : string.Empty);
				SwrveLog.Log("Setting " + text + " to null", "storage");
				global::UnityEngine.PlayerPrefs.SetString(text, null);
			}
			catch (global::UnityEngine.PlayerPrefsException ex)
			{
				SwrveLog.LogError(ex.ToString());
			}
		}

		public void SetSecureFailedListener(global::System.Action callback)
		{
		}

		public virtual void SaveSecure(string tag, string data, string userId = null)
		{
			Save(tag, data, userId);
		}

		public virtual string LoadSecure(string tag, string userId = null)
		{
			return Load(tag, userId);
		}
	}
}
