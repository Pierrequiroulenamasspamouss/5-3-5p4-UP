namespace Swrve.Storage
{
	public class SwrveFileStorage : global::Swrve.ISwrveStorage
	{
		private const string SIGNATURE_SUFFIX = "_SGT";

		protected string swrvePath;

		protected string uniqueKey;

		protected global::System.Action callback;

		public SwrveFileStorage(string swrvePath, string uniqueKey)
		{
			this.swrvePath = swrvePath;
			this.uniqueKey = uniqueKey;
		}

		public virtual void Save(string tag, string data, string userId = null)
		{
			if (!string.IsNullOrEmpty(data))
			{
				bool flag = false;
				try
				{
					string fileName = GetFileName(tag, userId);
					SwrveLog.Log("Saving: " + fileName, "storage");
					global::Swrve.CrossPlatformFile.SaveText(fileName, data);
					flag = true;
				}
				catch (global::System.Exception ex)
				{
					SwrveLog.LogError(ex.ToString(), "storage");
				}
				if (!flag)
				{
					SwrveLog.LogWarning(tag + " not saved!", "storage");
				}
			}
		}

		public virtual void SaveSecure(string tag, string data, string userId = null)
		{
			if (!string.IsNullOrEmpty(data))
			{
				bool flag = false;
				try
				{
					string fileName = GetFileName(tag, userId);
					SwrveLog.Log("Saving: " + fileName, "storage");
					global::Swrve.CrossPlatformFile.SaveText(fileName, data);
					string path = fileName + "_SGT";
					string data2 = global::Swrve.Helpers.SwrveHelper.CreateHMACMD5(data, uniqueKey);
					global::Swrve.CrossPlatformFile.SaveText(path, data2);
					flag = true;
				}
				catch (global::System.Exception ex)
				{
					SwrveLog.LogError(ex.ToString(), "storage");
				}
				if (!flag)
				{
					SwrveLog.LogWarning(tag + " not saved!", "storage");
				}
			}
		}

		public virtual string Load(string tag, string userId = null)
		{
			string result = null;
			try
			{
				string fileName = GetFileName(tag, userId);
				if (global::Swrve.CrossPlatformFile.Exists(fileName))
				{
					result = global::Swrve.CrossPlatformFile.LoadText(fileName);
				}
			}
			catch (global::System.Exception ex)
			{
				SwrveLog.LogError(ex.ToString(), "storage");
			}
			return result;
		}

		public virtual void Remove(string tag, string userId = null)
		{
			try
			{
				string fileName = GetFileName(tag, userId);
				if (global::Swrve.CrossPlatformFile.Exists(fileName))
				{
					SwrveLog.Log("Removing: " + fileName, "storage");
					global::Swrve.CrossPlatformFile.Delete(fileName);
				}
				string path = fileName + "_SGT";
				if (global::Swrve.CrossPlatformFile.Exists(path))
				{
					global::Swrve.CrossPlatformFile.Delete(path);
				}
			}
			catch (global::System.Exception ex)
			{
				SwrveLog.LogError(ex.ToString(), "storage");
			}
		}

		public void SetSecureFailedListener(global::System.Action callback)
		{
			this.callback = callback;
		}

		public virtual string LoadSecure(string tag, string userId = null)
		{
			string text = null;
			string fileName = GetFileName(tag, userId);
			string text2 = fileName + "_SGT";
			if (global::Swrve.CrossPlatformFile.Exists(fileName))
			{
				text = global::Swrve.CrossPlatformFile.LoadText(fileName);
				if (!string.IsNullOrEmpty(text))
				{
					string text3 = null;
					if (global::Swrve.CrossPlatformFile.Exists(text2))
					{
						text3 = global::Swrve.CrossPlatformFile.LoadText(text2);
					}
					else
					{
						SwrveLog.LogError("Could not read signature file: " + text2);
						text = null;
					}
					if (!string.IsNullOrEmpty(text3))
					{
						string value = global::Swrve.Helpers.SwrveHelper.CreateHMACMD5(text, uniqueKey);
						if (string.IsNullOrEmpty(value))
						{
							SwrveLog.LogError("Could not compute signature for data in file " + fileName);
							text = null;
						}
						else if (!text3.Equals(value))
						{
							if (callback != null)
							{
								callback();
							}
							SwrveLog.LogError("Signature validation failed for " + fileName);
							text = null;
						}
					}
				}
				else
				{
					SwrveLog.LogError("Could not read file " + fileName);
				}
			}
			return text;
		}

		private string GetFileName(string tag, string userId)
		{
			string text = ((swrvePath.Length <= 0) ? string.Empty : "/");
			return swrvePath + text + tag + ((userId != null) ? userId : string.Empty);
		}
	}
}
