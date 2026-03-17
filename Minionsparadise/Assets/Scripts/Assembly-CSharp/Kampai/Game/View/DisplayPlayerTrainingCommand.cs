namespace Kampai.Game.View
{
	public class DisplayPlayerTrainingCommand : global::strange.extensions.command.impl.Command
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("DisplayPlayerTrainingCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public int playerTrainingDefinitionId { get; set; }

		[Inject]
		public bool openedFromSettingsMenu { get; set; }

		[Inject]
		public global::strange.extensions.signal.impl.Signal<bool> callback { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.IPlayerTrainingService playerTrainingService { get; set; }

		public override void Execute()
		{
			if (playerTrainingDefinitionId <= 0)
			{
				callback.Dispatch(false);
				return;
			}
			global::Kampai.Game.PlayerTrainingDefinition playerTrainingDefinition = definitionService.Get<global::Kampai.Game.PlayerTrainingDefinition>(playerTrainingDefinitionId);
			if (playerTrainingDefinition == null)
			{
				logger.Warning("Invalid player training definition id " + playerTrainingDefinitionId);
				callback.Dispatch(false);
				return;
			}
			if (!openedFromSettingsMenu)
			{
				if (playerTrainingDefinition.disableAutomaticDisplay)
				{
					callback.Dispatch(false);
					return;
				}
				if (playerTrainingService.HasSeen(playerTrainingDefinitionId, global::Kampai.UI.PlayerTrainingVisiblityType.GAME))
				{
					callback.Dispatch(false);
					return;
				}
			}
			logger.Info("Showing player training definition id: {0}", playerTrainingDefinitionId);
			playerTrainingService.MarkSeen(playerTrainingDefinitionId, global::Kampai.UI.PlayerTrainingVisiblityType.GAME);
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Queue, "popup_PlayerTraining");
			global::Kampai.UI.View.GUIArguments args = iGUICommand.Args;
			iGUICommand.skrimScreen = "PlayerTrainingSkrim";
			iGUICommand.darkSkrim = true;
			iGUICommand.disableSkrimButton = true;
			args.Add(playerTrainingDefinitionId);
			args.Add(openedFromSettingsMenu);
			args.Add(callback);
			guiService.Execute(iGUICommand);
		}
	}
}
