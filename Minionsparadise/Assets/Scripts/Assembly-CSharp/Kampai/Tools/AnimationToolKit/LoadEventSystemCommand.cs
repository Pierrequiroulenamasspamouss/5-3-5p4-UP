namespace Kampai.Tools.AnimationToolKit
{
	public class LoadEventSystemCommand : global::strange.extensions.command.impl.Command
	{
		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject ContextView { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("Event System");
			gameObject.transform.parent = ContextView.transform;
			global::UnityEngine.EventSystems.EventSystem o = gameObject.AddComponent<global::UnityEngine.EventSystems.EventSystem>();
			base.injectionBinder.Bind<global::UnityEngine.EventSystems.EventSystem>().ToValue(o);
			gameObject.AddComponent<global::UnityEngine.EventSystems.StandaloneInputModule>();
			gameObject.AddComponent<global::Kampai.Game.KampaiTouchInputModule>();
		}
	}
}
