public class NimbleBridge_Log
{
	public enum NimbleBridge_Log_Level
	{
		LEVEL_VERBOSE = 100,
		LEVEL_DEBUG = 200,
		LEVEL_INFO = 300,
		LEVEL_WARN = 400,
		LEVEL_ERROR = 500,
		LEVEL_FATAL = 600
	}

	private NimbleBridge_Log()
	{
	}

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Log_setThreshold(int level);

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern int NimbleBridge_Log_getThresholdLevel();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern string NimbleBridge_Log_getLogFilePath();

	[global::System.Runtime.InteropServices.DllImport("NimbleCInterface")]
	private static extern void NimbleBridge_Log_writeWithTitle(int level, string title, string message);

	public static NimbleBridge_Log GetComponent()
	{
		return new NimbleBridge_Log();
	}

	public void WriteWithSource(NimbleBridge_Log.NimbleBridge_Log_Level level, NimbleBridge_LogSource source, string message)
	{
		NimbleBridge_Log_writeWithTitle((int)level, source.GetLogSourceTitle(), message);
	}

	public void WriteWithTitle(NimbleBridge_Log.NimbleBridge_Log_Level level, string title, string message)
	{
		NimbleBridge_Log_writeWithTitle((int)level, title, message);
	}

	public string GetLogFilePath()
	{
		return NimbleBridge_Log_getLogFilePath();
	}

	public int GetThreshold()
	{
		return NimbleBridge_Log_getThresholdLevel();
	}

	public void SetThreshold(int level)
	{
		NimbleBridge_Log_setThreshold(level);
	}

	public static void LOGV(NimbleBridge_LogSource source, string message)
	{
		GetComponent().WriteWithSource(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_VERBOSE, source, message);
	}

	public static void LOGD(NimbleBridge_LogSource source, string message)
	{
		GetComponent().WriteWithSource(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_DEBUG, source, message);
	}

	public static void LOGI(NimbleBridge_LogSource source, string message)
	{
		GetComponent().WriteWithSource(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_INFO, source, message);
	}

	public static void LOGW(NimbleBridge_LogSource source, string message)
	{
		GetComponent().WriteWithSource(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_WARN, source, message);
	}

	public static void LOGE(NimbleBridge_LogSource source, string message)
	{
		GetComponent().WriteWithSource(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_ERROR, source, message);
	}

	public static void LOGF(NimbleBridge_LogSource source, string message)
	{
		GetComponent().WriteWithSource(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_FATAL, source, message);
	}

	public static void LOGVS(string title, string message)
	{
		GetComponent().WriteWithTitle(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_VERBOSE, title, message);
	}

	public static void LOGDS(string title, string message)
	{
		GetComponent().WriteWithTitle(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_DEBUG, title, message);
	}

	public static void LOGIS(string title, string message)
	{
		GetComponent().WriteWithTitle(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_INFO, title, message);
	}

	public static void LOGWS(string title, string message)
	{
		GetComponent().WriteWithTitle(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_WARN, title, message);
	}

	public static void LOGES(string title, string message)
	{
		GetComponent().WriteWithTitle(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_ERROR, title, message);
	}

	public static void LOGFS(string title, string message)
	{
		GetComponent().WriteWithTitle(NimbleBridge_Log.NimbleBridge_Log_Level.LEVEL_FATAL, title, message);
	}

	public static void LOG(NimbleBridge_Log.NimbleBridge_Log_Level level, string message)
	{
		GetComponent().WriteWithTitle(level, null, message);
	}
}
