namespace Kampai.UI.View
{
	public class ToggleFPSGraphCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera camera { get; set; }

		public override void Execute()
		{
			FPSGraphC component = camera.GetComponent<FPSGraphC>();
			if (component != null)
			{
				component.enabled = !component.enabled;
				return;
			}
			component = camera.gameObject.AddComponent<FPSGraphC>();
			component.showFPSNumber = true;
			component.showPerformanceOnClick = false;
		}
	}
}
