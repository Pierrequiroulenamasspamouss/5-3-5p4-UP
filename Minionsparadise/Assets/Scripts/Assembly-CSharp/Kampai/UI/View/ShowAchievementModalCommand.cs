namespace Kampai.UI.View
{
	public class ShowAchievementModalCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject(global::Kampai.Main.MainElement.UI_GLASSCANVAS)]
		public global::UnityEngine.GameObject glassCanvas { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void Execute()
		{
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CancelBuildingMovementSignal>().Dispatch(false);
			showHUDSignal.Dispatch(true);
			
			// Load the prefab manually to avoid GUIService's automatic meditation/initialization of QuestPanel
			global::UnityEngine.GameObject prefab = global::Kampai.Util.KampaiResources.Load("screen_QuestPanel") as global::UnityEngine.GameObject;
			if (prefab == null) return;
			
			global::UnityEngine.GameObject instance = global::UnityEngine.Object.Instantiate(prefab) as global::UnityEngine.GameObject;
			instance.name = "screen_AchievementModal";
			
			// Ensure it's active and visible (replicating GUIService behavior)
			instance.SetActive(true);
			global::UnityEngine.CanvasGroup canvasGroup = instance.GetComponent<global::UnityEngine.CanvasGroup>();
			if (canvasGroup != null) canvasGroup.alpha = 1f;
			
			// Parent to UI Canvas
			if (glassCanvas != null)
			{
				instance.transform.SetParent(glassCanvas.transform, false);
			}
			
			// Find and destroy the unwanted Quest components on root or children
			global::Kampai.UI.View.QuestPanelView[] questViews = instance.GetComponentsInChildren<global::Kampai.UI.View.QuestPanelView>(true);
			foreach (var qv in questViews) global::UnityEngine.Object.DestroyImmediate(qv);
			
			// Add our view
			instance.AddComponent<AchievementModalView>();
			
			// Find the UI Root to parent it (reusing logic from other UI commands if possible, 
			// but usually just putting it in the context is enough if it's a KampaiView)
			// For now, let's just let it be and see if mediation picks it up.
			// Most KampaiViews auto-parent or are managed by GUIService.
			// To use GUIService's layering, we might need to use its API.
		}
	}
}
