public class RestoreNamedCharacterCommand : global::strange.extensions.command.impl.Command
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RestoreNamedCharacterCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public global::Kampai.Game.NamedCharacter character { get; set; }

	[Inject]
	public global::Kampai.Game.Prestige prestige { get; set; }

	[Inject]
	public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

	[Inject]
	public global::Kampai.Game.IPlayerService playerService { get; set; }

	[Inject]
	public global::Kampai.Game.KevinGoToWelcomeHutSignal gotoWelcomeHutSignal { get; set; }

	[Inject]
	public global::Kampai.Game.CreateNamedCharacterViewSignal createSignal { get; set; }

	[Inject]
	public global::Kampai.Game.PhilSitAtBarSignal sitAtBarSignal { get; set; }

	[Inject]
	public global::Kampai.Game.RestoreMinionAtTikiBarSignal restoreMinionAtTikiBarSignal { get; set; }

	[Inject]
	public global::Kampai.Game.RestoreStuartSignal restoreStuartSignal { get; set; }

	[Inject]
	public global::Kampai.Game.FrolicSignal frolicSignal { get; set; }

	public override void Execute()
	{
		if (character.Definition.Type != global::Kampai.Game.NamedCharacterType.TSM)
		{
			logger.Info("Restoring Character: {0} with prestige state: {1}", character.Name, prestige.state);
			createSignal.Dispatch(character);
			routineRunner.StartCoroutine(WaitAFrame());
		}
	}

	private global::System.Collections.IEnumerator WaitAFrame()
	{
		yield return null;
		global::Kampai.Game.PrestigeState state = prestige.state;
		if (state == global::Kampai.Game.PrestigeState.Questing)
		{
			if (character.Definition.Type == global::Kampai.Game.NamedCharacterType.PHIL)
			{
				sitAtBarSignal.Dispatch(true);
			}
			else if (character.Definition.Type != global::Kampai.Game.NamedCharacterType.KEVIN)
			{
				restoreMinionAtTikiBarSignal.Dispatch(character);
			}
			else
			{
				global::Kampai.Game.KevinCharacter kevin = character as global::Kampai.Game.KevinCharacter;
				if (kevin != null)
				{
					global::Kampai.Game.WelcomeHutBuilding welcomeHutBuilding = playerService.GetByInstanceId<global::Kampai.Game.WelcomeHutBuilding>(372);
					if (welcomeHutBuilding.IsBuildingRepaired())
					{
						gotoWelcomeHutSignal.Dispatch(true);
					}
					else
					{
						frolicSignal.Dispatch(kevin.ID);
					}
				}
			}
		}
		if (character.Definition.Type == global::Kampai.Game.NamedCharacterType.STUART)
		{
			restoreStuartSignal.Dispatch();
		}
	}
}
