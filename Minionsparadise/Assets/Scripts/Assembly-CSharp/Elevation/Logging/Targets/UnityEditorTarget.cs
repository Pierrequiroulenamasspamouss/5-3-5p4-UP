namespace Elevation.Logging.Targets
{
	public class UnityEditorTarget : global::Elevation.Logging.Targets.BaseLoggingTarget
	{
		public UnityEditorTarget(global::Elevation.Logging.LogLevel level)
			: base("UnityEngine.Debug", level)
		{
		}

		public override void WriteLogEvent(global::Elevation.Logging.LogEvent logEvent)
		{
		}

		public static global::Elevation.Logging.Targets.UnityEditorTarget Build(global::System.Collections.Generic.Dictionary<string, object> config)
		{
			global::Elevation.Logging.Targets.UnityEditorTarget unityEditorTarget = new global::Elevation.Logging.Targets.UnityEditorTarget(global::Elevation.Logging.LogLevel.None);
			unityEditorTarget.UpdateConfig(config);
			return unityEditorTarget;
		}
	}
}
