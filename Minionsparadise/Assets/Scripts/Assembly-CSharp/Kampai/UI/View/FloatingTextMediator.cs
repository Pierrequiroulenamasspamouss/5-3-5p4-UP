namespace Kampai.UI.View
{
	public class FloatingTextMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("FloatingTextMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.UI.View.ButtonView buttonView;

		[Inject]
		public global::Kampai.UI.View.FloatingTextView view { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveFloatingTextSignal removeFloatingTextSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OpenStorageBuildingSignal openStorageBuildingSignal { get; set; }

		public override void OnRegister()
		{
			view.Init(positionService, gameContext, logger, playerService, localizationService);
			buttonView = GetComponent<global::Kampai.UI.View.ButtonView>();
			buttonView.ClickedSignal.AddListener(OnClick);
			view.OnRemoveSignal.AddListener(OnRemoveFloatingText);
		}

		public override void OnRemove()
		{
			view.OnRemoveSignal.RemoveListener(OnRemoveFloatingText);
			buttonView.ClickedSignal.RemoveListener(OnClick);
		}

		private void OnClick()
		{
			if (view.TrackedId == 314)
			{
				global::Kampai.Game.StorageBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.StorageBuilding>(314);
				openStorageBuildingSignal.Dispatch(byInstanceId, true);
			}
		}

		private void OnRemoveFloatingText()
		{
			removeFloatingTextSignal.Dispatch(view.TrackedId);
		}
	}
}
