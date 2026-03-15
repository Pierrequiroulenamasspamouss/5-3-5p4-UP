namespace Kampai.UI.View
{
	public class ShowActivitySpinnerCommand : global::strange.extensions.command.impl.Command
	{
		private global::UnityEngine.GameObject go;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public bool show { get; set; }

		[Inject]
		public global::UnityEngine.Vector3 inputPos { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		public override void Execute()
		{
			if (show)
			{
				go = guiService.Execute(global::Kampai.UI.View.GUIOperation.Load, "Spinner_group");
				go.GetComponent<global::UnityEngine.CanvasGroup>().blocksRaycasts = false;
				go.transform.position = positionService.GetPositionData(inputPos).WorldPositionInUI;
			}
			else
			{
				guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "Spinner_group");
			}
		}
	}
}
