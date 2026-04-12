namespace Kampai.UI.View
{
	public class UIRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			context = new global::Kampai.UI.View.UIContext(this, true);
			context.Start();
		}
	}
}
