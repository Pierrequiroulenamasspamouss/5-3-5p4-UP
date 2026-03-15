public class NimbleBridge_Persistence : global::System.Runtime.InteropServices.SafeHandle
{
	public enum Storage
	{
		STORAGE_DOCUMENT = 0,
		STORAGE_CACHE = 1,
		STORAGE_TEMP = 2
	}

	public enum RemoteSynchronization
	{
		REMOTE_LOCAL = 0,
		REMOTE_ICLOUD = 1,
		REMOTE_VAULT_SERVICE = 2
	}

	public enum MergePolicy
	{
		MERGE_OVERWRITE = 0,
		MERGE_SOURCE_FIRST = 1,
		MERGE_TARGET_FIRST = 2
	}

	public override bool IsInvalid
	{
		get
		{
			return handle == global::System.IntPtr.Zero;
		}
	}

	internal NimbleBridge_Persistence()
		: base(global::System.IntPtr.Zero, true)
	{
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_PersistenceWrapper_Dispose(NimbleBridge_Persistence persistenceWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_Persistence_getIdentifier(NimbleBridge_Persistence persistenceWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_Persistence_getStorage(NimbleBridge_Persistence persistenceWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern bool NimbleBridge_Persistence_getEncryption(NimbleBridge_Persistence persistenceWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Persistence_setEncryption(NimbleBridge_Persistence persistenceWrapper, bool encryption);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Persistence_setValue(NimbleBridge_Persistence persistenceWrapper, string key, string value);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_Persistence_getStringValue(NimbleBridge_Persistence persistenceWrapper, string key);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Persistence_addEntries(NimbleBridge_Persistence persistenceWrapper, global::System.IntPtr map);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Persistence_clean(NimbleBridge_Persistence persistenceWrapper);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Persistence_synchronize(NimbleBridge_Persistence persistenceWrapper);

	protected override bool ReleaseHandle()
	{
#if !UNITY_EDITOR
		NimbleBridge_PersistenceWrapper_Dispose(this);
#endif
		return true;
	}

	public string GetIdentifier()
	{
#if !UNITY_EDITOR
		return NimbleBridge_Persistence_getIdentifier(this);
#else
		return string.Empty;
#endif
	}

	public NimbleBridge_Persistence.Storage GetStorage()
	{
#if !UNITY_EDITOR
		return (NimbleBridge_Persistence.Storage)NimbleBridge_Persistence_getStorage(this);
#else
		return Storage.STORAGE_DOCUMENT;
#endif
	}

	public bool GetEncryption()
	{
#if !UNITY_EDITOR
		return NimbleBridge_Persistence_getEncryption(this);
#else
		return false;
#endif
	}

	public void SetEncryption(bool encryption)
	{
#if !UNITY_EDITOR
		NimbleBridge_Persistence_setEncryption(this, encryption);
#endif
	}

	public void SetValue(string key, string value)
	{
#if !UNITY_EDITOR
		NimbleBridge_Persistence_setValue(this, key, value);
#endif
	}

	public string GetStringValue(string key)
	{
#if !UNITY_EDITOR
		return NimbleBridge_Persistence_getStringValue(this, key);
#else
		return string.Empty;
#endif
	}

	public void AddEntries(global::System.Collections.Generic.Dictionary<string, string> dictionary)
	{
#if !UNITY_EDITOR
		global::System.IntPtr intPtr = global::System.IntPtr.Zero;
		try
		{
			intPtr = MarshalUtility.ConvertDictionaryToPtr(dictionary);
			NimbleBridge_Persistence_addEntries(this, intPtr);
		}
		finally
		{
			if (intPtr != global::System.IntPtr.Zero)
			{
				MarshalUtility.DisposeMapPtr(intPtr);
			}
		}
#endif
	}

	public void Clean()
	{
#if !UNITY_EDITOR
		NimbleBridge_Persistence_clean(this);
#endif
	}

	public void Synchronize()
	{
#if !UNITY_EDITOR
		NimbleBridge_Persistence_synchronize(this);
#endif
	}
}
