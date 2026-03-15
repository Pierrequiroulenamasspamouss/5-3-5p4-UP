namespace Kampai.UI.View
{
	public class SpawnMignetteDooberCommand : global::Kampai.UI.View.DooberCommand, global::strange.extensions.pool.api.IPoolable, global::Kampai.Util.IFastPooledCommand<global::Kampai.UI.View.MignetteHUDView, global::UnityEngine.Vector3, int, bool>, global::Kampai.Util.IFastPooledCommandBase
	{
		private const float flyTime = 1.1f;

		private const float delayTime = 0.1f;

		private int numberOnDoober;

		private global::Kampai.UI.View.MignetteHUDView mignetteHUDView;

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.MignetteGameModel mignetteModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.MignetteDooberSpawnedSignal spawnedSignal { get; set; }

		public void Execute(global::Kampai.UI.View.MignetteHUDView _mignetteHUDView, global::UnityEngine.Vector3 iconPosition, int numberOnDoober, bool fromWorldCanvas)
		{
			base.iconPosition = iconPosition;
			base.fromWorldCanvas = fromWorldCanvas;
			this.numberOnDoober = numberOnDoober;
			mignetteHUDView = _mignetteHUDView;
			global::UnityEngine.GameObject gameObject = CreateTweenObject();
			global::UnityEngine.Vector3 position = mignetteHUDView.DooberTarget.position;
			TweenToDestination(gameObject, position, 1.1f, global::Kampai.UI.View.DestinationType.MIGNETTE, 0.1f);
			spawnedSignal.Dispatch(gameObject);
		}

		private global::UnityEngine.GameObject CreateTweenObject()
		{
			global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadUntrackedInstance, "NumberedDoober");
			global::UnityEngine.GameObject gameObject = guiService.Execute(command);
			global::Kampai.UI.View.NumberedDooberViewObject component = gameObject.GetComponent<global::Kampai.UI.View.NumberedDooberViewObject>();
			component.NumberLabel.text = string.Format("x {0}", numberOnDoober);
			if (mignetteModel.CollectableImage != null)
			{
				component.IconImage.sprite = mignetteModel.CollectableImage;
			}
			if (mignetteModel.CollectableImageMask != null)
			{
				component.IconImage.maskSprite = mignetteModel.CollectableImageMask;
			}
			global::UnityEngine.RectTransform component2 = gameObject.GetComponent<global::UnityEngine.RectTransform>();
			component2.anchoredPosition = GetScreenStartPosition();
			return gameObject;
		}
	}
}
