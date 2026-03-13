namespace Elevation.Logging.Targets
{
#if !UNITY_WEBPLAYER
	public abstract class BufferedTarget : global::Elevation.Logging.Targets.FileTarget
	{
		public BufferedTarget(string name, global::Elevation.Logging.LogLevel level, int timeoutMillis, int bufferSize, string logFolder = null, params global::Elevation.Logging.LogFilter[] filters)
			: base(level, string.Format("{0}.buffer", name), bufferSize, 3, logFolder, filters)
		{
			base.TimeoutMillis = timeoutMillis;
			base.Name = name;
		}

		protected override void RollLogFiles()
		{
			global::System.IO.FileInfo currentFileInfo = _currentFileInfo;
			base.RollLogFiles();
			if (currentFileInfo != null)
			{
				BatchProcess(currentFileInfo);
			}
		}

		protected abstract void BatchProcess(global::System.IO.FileInfo buffer);

		public override void Flush()
		{
			if (!base.Disposed)
			{
				base.Flush();
				RollLogFiles();
			}
		}
	}
#else
	public abstract class BufferedTarget : global::Elevation.Logging.Targets.FileTarget
	{
		public BufferedTarget(string name, global::Elevation.Logging.LogLevel level, int timeoutMillis, int bufferSize, string logFolder = null, params global::Elevation.Logging.LogFilter[] filters)
			: base(level, string.Empty, 0, 0, logFolder, filters)
		{
		}

		public override void Flush()
		{
		}

		protected abstract void BatchProcess(global::System.IO.FileInfo buffer);
	}
#endif
}
