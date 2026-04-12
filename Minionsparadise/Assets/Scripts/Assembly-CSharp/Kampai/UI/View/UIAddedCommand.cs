namespace Kampai.UI.View
{
	public class UIAddedCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::UnityEngine.GameObject obj { get; set; }

		[Inject]
		public global::System.Action action { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel model { get; set; }

		public override void Execute()
		{
			if (obj.activeInHierarchy)
			{
				model.AddUI(obj.GetInstanceID(), action);
			}
		}
	}
}
