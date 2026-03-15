namespace Kampai.UI.View
{
	public class TownHallDialogView : global::Kampai.Util.KampaiView
	{
		public global::UnityEngine.RectTransform CollectionHolder;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIRemovedSignal uiRemoveSignal { get; set; }

		public void AddMignetteScoreSummary(global::Kampai.Game.MignetteBuilding mignetteBuilding)
		{
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "MignetteScoreSummary");
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			args.Add(false);
			args.Add(mignetteBuilding.ID);
			args.Add(mignetteBuilding);
			global::UnityEngine.GameObject gameObject = guiService.Execute(iGUICommand);
			global::UnityEngine.RectTransform component = gameObject.GetComponent<global::UnityEngine.RectTransform>();
			component.transform.SetParent(CollectionHolder, false);
			CollectionHolder.sizeDelta += new global::UnityEngine.Vector2(component.GetComponent<global::UnityEngine.UI.LayoutElement>().minWidth + CollectionHolder.GetComponent<global::UnityEngine.UI.HorizontalLayoutGroup>().spacing, 0f);
			StartCoroutine(RemoveFromUIStack(gameObject));
		}

		private global::System.Collections.IEnumerator RemoveFromUIStack(global::UnityEngine.GameObject scoreSummary)
		{
			yield return null;
			uiRemoveSignal.Dispatch(scoreSummary);
		}

		public void LeftAlignContent()
		{
			CollectionHolder.anchoredPosition = new global::UnityEngine.Vector2(CollectionHolder.sizeDelta.x / 2f, 0f);
		}
	}
}
