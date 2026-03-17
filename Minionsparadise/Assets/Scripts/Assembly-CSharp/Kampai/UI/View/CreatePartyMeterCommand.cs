namespace Kampai.UI.View
{
	public class CreatePartyMeterCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("CreatePartyMeterCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.HUD)]
		public global::UnityEngine.GameObject hud { get; set; }

		public override void Execute()
		{
			logger.EventStart("CreatePartyMeterCommand.Execute");
			if (playerService.IsMinionPartyUnlocked())
			{
				global::UnityEngine.GameObject gameObject = CreatePartyMeter();
				global::Kampai.UI.View.HUDView component = hud.GetComponent<global::Kampai.UI.View.HUDView>();
				global::UnityEngine.RectTransform partyMeterPanel = component.PartyMeterPanel;
				if (partyMeterPanel != null)
				{
					gameObject.transform.SetParent(partyMeterPanel, false);
				}
			}
			logger.EventStart("CreatePartyMeterCommand.Execute");
		}

		private global::UnityEngine.GameObject CreatePartyMeter()
		{
			global::UnityEngine.GameObject gameObject = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("cmp_PartyMeterTimer");
			if (gameObject == null)
			{
				logger.Error("Invalid GUISettings.Path: {0}", "cmp_PartyMeterTimer");
				return null;
			}
			global::UnityEngine.GameObject gameObject2 = global::UnityEngine.Object.Instantiate(gameObject);
			if (gameObject2 == null)
			{
				logger.Error("Unable to create instance of {0}", "cmp_PartyMeterTimer");
				return null;
			}
			gameObject2.name = "cmp_PartyMeterTimer";
			return gameObject2;
		}
	}
}
