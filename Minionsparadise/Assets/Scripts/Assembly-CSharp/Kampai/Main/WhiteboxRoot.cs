namespace Kampai.Main
{
	public class WhiteboxRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Start()
		{
			context = new global::Kampai.Main.WhiteboxContext(this, true);
			context.Start();
		}
	}
}
