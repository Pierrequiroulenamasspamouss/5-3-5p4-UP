namespace Kampai.Splash
{
	public class SplashRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			global::Kampai.Util.TimeProfiler.Reset(global::UnityEngine.Debug.isDebugBuild);
			global::Kampai.Util.TimeProfiler.StartMonoProfiler("loading");
		}

		private void Start()
		{
			context = new global::Kampai.Splash.SplashContext(this, true);
			context.Start();
		}
	}
}
