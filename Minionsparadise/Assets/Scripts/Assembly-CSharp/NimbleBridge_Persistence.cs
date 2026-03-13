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
		NimbleBridge_PersistenceWrapper_Dispose(this);
		return true;
	}

	public string GetIdentifier()
	{
		return NimbleBridge_Persistence_getIdentifier(this);
	}

	public NimbleBridge_Persistence.Storage GetStorage()
	{
		return (NimbleBridge_Persistence.Storage)NimbleBridge_Persistence_getStorage(this);
	}

	public bool GetEncryption()
	{
		return NimbleBridge_Persistence_getEncryption(this);
	}

	public void SetEncryption(bool encryption)
	{
		NimbleBridge_Persistence_setEncryption(this, encryption);
	}

	public void SetValue(string key, string value)
	{
		NimbleBridge_Persistence_setValue(this, key, value);
	}

	public string GetStringValue(string key)
	{
		return NimbleBridge_Persistence_getStringValue(this, key);
	}

	public void AddEntries(global::System.Collections.Generic.Dictionary<string, string> dictionary)
	{
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
	}

	public void Clean()
	{
		NimbleBridge_Persistence_clean(this);
	}

	public void Synchronize()
	{
		NimbleBridge_Persistence_synchronize(this);
	}
}
