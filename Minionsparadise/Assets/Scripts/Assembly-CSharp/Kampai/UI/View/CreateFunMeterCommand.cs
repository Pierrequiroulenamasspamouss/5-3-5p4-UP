namespace Kampai.UI.View
{
	public class CreateFunMeterCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CreateFunMeterCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.HUD)]
		public global::UnityEngine.GameObject hud { get; set; }

		[Inject]
		public global::Kampai.UI.View.FinishCreateFunMeterSignal finishCreateFunMeterSignal { get; set; }

		public override void Execute()
		{
			logger.EventStart("CreateFunMeterCommand.Execute");
			if (playerService.IsMinionPartyUnlocked())
			{
				global::UnityEngine.GameObject gameObject = CreateNewXPBar();
				global::Kampai.UI.View.HUDView component = hud.GetComponent<global::Kampai.UI.View.HUDView>();
				global::UnityEngine.RectTransform pointsPanel = component.PointsPanel;
				if (pointsPanel != null)
				{
					gameObject.transform.SetParent(pointsPanel, false);
					finishCreateFunMeterSignal.Dispatch();
				}
			}
			logger.EventStart("CreateFunMeterCommand.Execute");
		}

		private global::UnityEngine.GameObject CreateNewXPBar()
		{
			global::UnityEngine.GameObject gameObject = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("XP_FunMeter");
			if (gameObject == null)
			{
				logger.Error("Invalid GUISettings.Path: {0}", "XP_FunMeter");
				return null;
			}
			global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject);
			if (gameObject2 == null)
			{
				logger.Error("Unable to create instance of {0}", "XP_FunMeter");
				return null;
			}
			gameObject2.name = "XP_FunMeter";
			return gameObject2;
		}
	}
}
