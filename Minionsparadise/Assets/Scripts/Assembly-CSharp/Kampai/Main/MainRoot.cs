namespace Kampai.Main
{
	public class MainRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Start()
		{
			global::Kampai.Util.TimeProfiler.StartSection("main");
			context = new global::Kampai.Main.MainContext(this, true);
		}
	}
}
