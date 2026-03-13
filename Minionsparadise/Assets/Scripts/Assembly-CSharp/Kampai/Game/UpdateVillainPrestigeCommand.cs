namespace Kampai.Game
{
	public class UpdateVillainPrestigeCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("UpdateVillainPrestigeCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.Prestige prestige { get; set; }

		[Inject]
		public global::Kampai.Util.Tuple<global::Kampai.Game.PrestigeState, global::Kampai.Game.PrestigeState> states { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		[Inject]
		public global::Kampai.Game.QueueCabanaSignal updateCabanaSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MoveInCabanaSignal moveInSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void Execute()
		{
			switch (states.Item2)
			{
			case global::Kampai.Game.PrestigeState.Prestige:
			case global::Kampai.Game.PrestigeState.Taskable:
				SwitchToPrestige();
				break;
			case global::Kampai.Game.PrestigeState.InQueue:
				SwitchToInQueue();
				break;
			case global::Kampai.Game.PrestigeState.Questing:
				SwitchToQuesting();
				break;
			}
		}

		private void SwitchToInQueue()
		{
			if (prestige.trackedInstanceId == 0)
			{
				global::Kampai.Game.VillainDefinition villainDefinition = definitionService.Get<global::Kampai.Game.VillainDefinition>(prestige.Definition.TrackedDefinitionID);
				if (villainDefinition == null)
				{
					logger.Error("Trying to create a villain instance, but this prestige ({0}) doesn't have a villain definition!", prestige);
					return;
				}
				global::Kampai.Game.Villain villain = new global::Kampai.Game.Villain(villainDefinition);
				playerService.Add(villain);
				prestige.trackedInstanceId = villain.ID;
			}
			else
			{
				global::Kampai.Game.Villain villain = playerService.GetByInstanceId<global::Kampai.Game.Villain>(prestige.trackedInstanceId);
			}
			telemetryService.Send_TelemetryCharacterPrestiged(prestige);
			uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShowBuddyWelcomePanelUISignal>().Dispatch(new global::Kampai.Util.Boxed<global::Kampai.Game.Prestige>(prestige), global::Kampai.UI.View.CharacterWelcomeState.Welcome, 0);
			updateCabanaSignal.Dispatch(prestige);
		}

		private void SwitchToQuesting()
		{
			moveInSignal.Dispatch(prestige);
		}

		private void SwitchToPrestige()
		{
			if (states.Item1 == global::Kampai.Game.PrestigeState.Questing)
			{
				uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShowBuddyWelcomePanelUISignal>().Dispatch(new global::Kampai.Util.Boxed<global::Kampai.Game.Prestige>(prestige), global::Kampai.UI.View.CharacterWelcomeState.Farewell, 0);
			}
		}
	}
}
