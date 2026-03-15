namespace Kampai.UI.View
{
	public class MinionLevelSelectorMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MinionLevelSelectorMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.PostMinionUpgradeSignal upgradeSignal;

		[Inject]
		public global::Kampai.UI.View.MinionLevelSelectorView view { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshFromIndexArgsSignal refreshFromIndexArgsSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public override void OnRegister()
		{
			view.Init(playerService, definitionService);
			view.SelectionButton.ClickedSignal.AddListener(Selected);
			refreshFromIndexArgsSignal.AddListener(view.RefreshAllOfTypeArgsCallback);
			upgradeSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.PostMinionUpgradeSignal>();
			upgradeSignal.AddListener(UpdateCount);
		}

		public override void OnRemove()
		{
			view.SelectionButton.ClickedSignal.RemoveListener(Selected);
			refreshFromIndexArgsSignal.RemoveListener(view.RefreshAllOfTypeArgsCallback);
			upgradeSignal.RemoveListener(UpdateCount);
			upgradeSignal = null;
		}

		private void Selected()
		{
			refreshFromIndexArgsSignal.Dispatch(view.GetType(), view.index, new global::Kampai.UI.View.GUIArguments(logger));
			soundFXSignal.Dispatch("Play_button_click_01");
		}

		private void UpdateCount()
		{
			view.UpdateMinionCountText();
		}
	}
}
