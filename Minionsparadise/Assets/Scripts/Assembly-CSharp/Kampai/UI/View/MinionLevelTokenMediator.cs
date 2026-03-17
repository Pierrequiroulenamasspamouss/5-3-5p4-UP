namespace Kampai.UI.View
{
	public class MinionLevelTokenMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MinionLevelTokenMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.UI.View.MinionLevelTokenView view { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal spawnDooberSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.IPositionService positionService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		public override void OnRegister()
		{
			global::Kampai.Game.VillainLairEntranceBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(374);
			global::Kampai.UI.View.WorldToGlassUIModal component = view.GetComponent<global::Kampai.UI.View.WorldToGlassUIModal>();
			component.Settings = new global::Kampai.UI.View.WorldToGlassUISettings(byInstanceId.ID);
			view.Init(positionService, gameContext, logger, playerService, localizationService);
			view.HarvestButton.ClickedSignal.AddListener(HarvestPendingToken);
			UpdateView();
		}

		public override void OnRemove()
		{
			view.HarvestButton.ClickedSignal.RemoveListener(HarvestPendingToken);
		}

		private void UpdateView()
		{
			uint quantity = playerService.GetQuantity(global::Kampai.Game.StaticItem.PENDING_MINION_LEVEL_TOKEN);
			view.SetTokenCount(quantity);
		}

		private void HarvestPendingToken()
		{
			if (playerService.GetQuantity(global::Kampai.Game.StaticItem.PENDING_MINION_LEVEL_TOKEN) != 0)
			{
				playerService.AlterQuantity(global::Kampai.Game.StaticItem.PENDING_MINION_LEVEL_TOKEN, -1);
			}
			playerService.AlterQuantity(global::Kampai.Game.StaticItem.MINION_LEVEL_TOKEN, 1);
			global::Kampai.Game.VillainLairEntranceBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairEntranceBuilding>(374);
			global::UnityEngine.Vector3 type = new global::UnityEngine.Vector3(byInstanceId.Location.x, 0f, byInstanceId.Location.y);
			spawnDooberSignal.Dispatch(type, global::Kampai.UI.View.DestinationType.MINION_LEVEL_TOKEN, 31, true);
			global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Unload, "cmp_MinionLevelToken");
			guiService.Execute(command);
		}
	}
}
