namespace Kampai.Game
{
	public class UpdateNamedCharacterPrestigeCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Prestige prestige { get; set; }

		[Inject]
		public global::Kampai.Util.Tuple<global::Kampai.Game.PrestigeState, global::Kampai.Game.PrestigeState> states { get; set; }

		[Inject]
		public global::Kampai.Game.MinionPrestigeCompleteSignal minionPrestigeCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RemoveMinionFromTikiBarSignal removeMinionFromTikiBarSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable uiContext { get; set; }

		public override void Execute()
		{
			switch (states.Item2)
			{
			case global::Kampai.Game.PrestigeState.InQueue:
				minionPrestigeCompleteSignal.Dispatch(prestige);
				break;
			case global::Kampai.Game.PrestigeState.Taskable:
				SwitchCharacterToTaskable();
				break;
			case global::Kampai.Game.PrestigeState.TaskableWhileQuesting:
				removeMinionFromTikiBarSignal.Dispatch(prestige);
				break;
			case global::Kampai.Game.PrestigeState.Questing:
				break;
			}
		}

		private void SwitchCharacterToTaskable()
		{
			if (states.Item1 != global::Kampai.Game.PrestigeState.TaskableWhileQuesting)
			{
				if (prestige.CurrentPrestigeLevel == 0)
				{
					uiContext.injectionBinder.GetInstance<global::Kampai.UI.View.ShowBuddyWelcomePanelUISignal>().Dispatch(new global::Kampai.Util.Boxed<global::Kampai.Game.Prestige>(prestige), global::Kampai.UI.View.CharacterWelcomeState.Farewell, 0);
				}
				removeMinionFromTikiBarSignal.Dispatch(prestige);
			}
		}
	}
}
