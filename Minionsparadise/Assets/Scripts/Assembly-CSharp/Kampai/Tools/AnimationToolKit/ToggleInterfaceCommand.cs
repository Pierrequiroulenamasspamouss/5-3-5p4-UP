namespace Kampai.Tools.AnimationToolKit
{
	public class ToggleInterfaceCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::UnityEngine.Canvas canvas { get; set; }

		public override void Execute()
		{
			int childCount = canvas.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				global::UnityEngine.Transform child = canvas.transform.GetChild(i);
				global::UnityEngine.GameObject gameObject = child.gameObject;
				global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonView component = child.GetComponent<global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonView>();
				if (component == null || component.ButtonType != global::Kampai.Tools.AnimationToolKit.AnimationToolKitButtonType.ToggleInterface)
				{
					child.gameObject.SetActive(!gameObject.activeSelf);
				}
			}
		}
	}
}
