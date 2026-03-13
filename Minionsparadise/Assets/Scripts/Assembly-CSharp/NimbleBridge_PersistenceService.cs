public class NimbleBridge_PersistenceService
{
	private NimbleBridge_PersistenceService()
	{
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Persistence NimbleBridge_PersistenceService_getPersistence(string identifier, int storage);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_PersistenceService_migratePersistence(string sourcePersistenceId, int storage, string targetPersistenceId, int mergePolicy);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Persistence NimbleBridge_PersistenceService_getAppPersistence(int storage);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern NimbleBridge_Persistence NimbleBridge_PersistenceService_getPersistenceForNimbleComponent(string componentId, int storage);

	public static NimbleBridge_PersistenceService GetComponent()
	{
		return new NimbleBridge_PersistenceService();
	}

	public NimbleBridge_Persistence GetPersistence(string identifier, NimbleBridge_Persistence.Storage storage)
	{
		return NimbleBridge_PersistenceService_getPersistence(identifier, (int)storage);
	}

	public void MigratePersistence(string sourcePersistenceId, NimbleBridge_Persistence.Storage storage, string targetPersistenceId, NimbleBridge_Persistence.MergePolicy mergePolicy)
	{
		NimbleBridge_PersistenceService_migratePersistence(sourcePersistenceId, (int)storage, targetPersistenceId, (int)mergePolicy);
	}

	public static NimbleBridge_Persistence GetAppPersistence(NimbleBridge_Persistence.Storage storage)
	{
		return NimbleBridge_PersistenceService_getAppPersistence((int)storage);
	}

	public static NimbleBridge_Persistence GetPersistenceForNimbleComponent(string componentId, NimbleBridge_Persistence.Storage storage)
	{
		return NimbleBridge_PersistenceService_getPersistenceForNimbleComponent(componentId, (int)storage);
	}
}
