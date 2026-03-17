namespace Kampai.BuildingsSizeToolbox
{
	public class BuildingsSizeToolboxRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			context = new global::Kampai.BuildingsSizeToolbox.BuildingsSizeToolboxContext(this, true);
			context.Start();
		}
	}
}
