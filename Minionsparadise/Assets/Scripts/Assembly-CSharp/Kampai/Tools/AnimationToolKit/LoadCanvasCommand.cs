namespace Kampai.Tools.AnimationToolKit
{
	public class LoadCanvasCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject ContextView { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("Canvas");
			gameObject.transform.SetParent(ContextView.transform, false);
			global::UnityEngine.Canvas canvas = gameObject.AddComponent<global::UnityEngine.Canvas>();
			canvas.renderMode = global::UnityEngine.RenderMode.ScreenSpaceOverlay;
			base.injectionBinder.Bind<global::UnityEngine.Canvas>().ToValue(canvas);
			gameObject.AddComponent<global::UnityEngine.UI.GraphicRaycaster>();
		}
	}
}
