namespace Kampai.ForcedUpgrade
{
	public class ForcedUpgradeRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Awake()
		{
			context = new global::Kampai.ForcedUpgrade.ForcedUpgradeContext(this, true);
			context.Start();
		}
	}
}
