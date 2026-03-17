namespace Kampai.Tools.AnimationToolKit
{
	public class AnimationToolKitRoot : global::strange.extensions.context.impl.ContextView
	{
		private void Start()
		{
			context = new global::Kampai.Tools.AnimationToolKit.AnimationToolKitContext(this, true);
			context.Start();
		}
	}
}
