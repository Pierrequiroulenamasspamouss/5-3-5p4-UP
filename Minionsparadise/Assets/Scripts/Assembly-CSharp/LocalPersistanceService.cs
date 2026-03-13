public class LocalPersistanceService : ILocalPersistanceService
{
	private struct CachedData
	{
		public string StringData;

		public int IntData;

		public CachedData(string data)
		{
			StringData = data;
			IntData = 0;
		}

		public CachedData(int data)
		{
			IntData = data;
			StringData = null;
		}
	}

	private string EnvPrefix;

	private global::System.Collections.Generic.Dictionary<string, LocalPersistanceService.CachedData> cache = new global::System.Collections.Generic.Dictionary<string, LocalPersistanceService.CachedData>();

	[PostConstruct]
	public void PostConstruct()
	{
		EnvPrefix = ".";
		global::UnityEngine.PlayerPrefs.SetString("EnvPrefix", EnvPrefix);
		global::UnityEngine.PlayerPrefs.Save();
	}

	public void PutData(string name, string data)
	{
		cache[name] = new LocalPersistanceService.CachedData(data);
		NimbleBridge_CallbackHelper.Get().RunOnMainThread(delegate
		{
			global::UnityEngine.PlayerPrefs.SetString(EnvPrefix + name, data);
			global::UnityEngine.PlayerPrefs.Save();
		});
	}

	public void PutDataInt(string name, int data)
	{
		cache[name] = new LocalPersistanceService.CachedData(data);
		NimbleBridge_CallbackHelper.Get().RunOnMainThread(delegate
		{
			global::UnityEngine.PlayerPrefs.SetInt(EnvPrefix + name, data);
			global::UnityEngine.PlayerPrefs.Save();
		});
	}

	public string GetData(string name)
	{
		if (cache.ContainsKey(name))
		{
			return cache[name].StringData;
		}
		string text = global::UnityEngine.PlayerPrefs.GetString(EnvPrefix + name);
		cache[name] = new LocalPersistanceService.CachedData(text);
		return text;
	}

	public int GetDataInt(string name)
	{
		if (cache.ContainsKey(name))
		{
			return cache[name].IntData;
		}
		int num = global::UnityEngine.PlayerPrefs.GetInt(EnvPrefix + name);
		cache[name] = new LocalPersistanceService.CachedData(num);
		return num;
	}

	public void PutDataPlayer(string name, string data)
	{
		PutData(name + GetData("UserID"), data);
	}

	public void PutDataIntPlayer(string name, int data)
	{
		PutDataInt(name + GetData("UserID"), data);
	}

	public string GetDataPlayer(string name)
	{
		return GetData(name + GetData("UserID"));
	}

	public int GetDataIntPlayer(string name)
	{
		return GetDataInt(name + GetData("UserID"));
	}

	public void DeleteAll()
	{
		cache.Clear();
		global::UnityEngine.PlayerPrefs.DeleteAll();
		global::UnityEngine.PlayerPrefs.Save();
	}

	public bool HasKey(string name)
	{
		return cache.ContainsKey(name) || global::UnityEngine.PlayerPrefs.HasKey(EnvPrefix + name);
	}

	public void DeleteKey(string name)
	{
		if (cache.ContainsKey(name))
		{
			cache.Remove(name);
		}
		global::UnityEngine.PlayerPrefs.DeleteKey(EnvPrefix + name);
		global::UnityEngine.PlayerPrefs.Save();
	}

	public bool HasKeyPlayer(string name)
	{
		return HasKey(name + GetData("UserID"));
	}

	public void DeleteKeyPlayer(string name)
	{
		DeleteKey(name + GetData("UserID"));
	}
}
