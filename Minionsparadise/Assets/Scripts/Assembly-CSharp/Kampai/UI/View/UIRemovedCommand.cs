namespace Kampai.UI.View
{
	public class UIRemovedCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::UnityEngine.GameObject obj { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel model { get; set; }

		public override void Execute()
		{
			model.RemoveUI(obj.GetInstanceID());
		}
	}
}
