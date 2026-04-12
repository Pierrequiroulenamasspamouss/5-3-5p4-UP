namespace Kampai.Tools.AnimationToolKit
{
	public class LoadToggleGroupCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::UnityEngine.Canvas Canvas { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("Toggle Group");
			gameObject.transform.parent = Canvas.transform;
			global::UnityEngine.RectTransform rectTransform = gameObject.AddComponent<global::UnityEngine.RectTransform>();
			rectTransform.anchoredPosition = global::UnityEngine.Vector2.zero;
			global::UnityEngine.UI.ToggleGroup toggleGroup = gameObject.AddComponent<global::UnityEngine.UI.ToggleGroup>();
			toggleGroup.transform.position = global::UnityEngine.Vector3.zero;
			base.injectionBinder.Bind<global::UnityEngine.GameObject>().ToValue(gameObject).ToName(global::Kampai.Tools.AnimationToolKit.AnimationToolKitElement.TOGGLE_GROUP);
		}
	}
}
