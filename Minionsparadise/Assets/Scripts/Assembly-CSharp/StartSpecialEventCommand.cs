public class StartSpecialEventCommand : global::strange.extensions.command.impl.Command
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("StartSpecialEventCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public global::Kampai.Game.SpecialEventItemDefinition specialEventItemDefinition { get; set; }

	[Inject]
	public global::Kampai.Game.RestoreSpecialEventSignal restoreSpecialEventSignal { get; set; }

	[Inject]
	public global::Kampai.Game.IDefinitionService definitionService { get; set; }

	[Inject]
	public global::Kampai.Game.IPlayerService playerService { get; set; }

	public override void Execute()
	{
		logger.Info("Starting special event {0}", specialEventItemDefinition.LocalizedKey);
		global::Kampai.Game.SpecialEventCharacterDefinition specialEventCharacterDefinition = definitionService.Get<global::Kampai.Game.SpecialEventCharacterDefinition>(70009);
		if (specialEventCharacterDefinition == null)
		{
			logger.Error("Failed to find Special Event Character Definition");
			return;
		}
		global::Kampai.Game.CostumeItemDefinition costumeItemDefinition = definitionService.Get<global::Kampai.Game.CostumeItemDefinition>(specialEventItemDefinition.EventMinionCostumeId);
		if (costumeItemDefinition == null)
		{
			logger.Error("Failed to find Special Event Character Costume for CostumeID:" + specialEventItemDefinition.EventMinionCostumeId);
		}
		global::Kampai.Game.SpecialEventCharacter specialEventCharacter = new global::Kampai.Game.SpecialEventCharacter(specialEventCharacterDefinition);
		specialEventCharacter.SpecialEventID = specialEventItemDefinition.ID;
		specialEventCharacter.PrestigeDefinitionID = specialEventItemDefinition.PrestigeDefinitionID;
		playerService.Add(specialEventCharacter);
		restoreSpecialEventSignal.Dispatch(specialEventItemDefinition);
	}
}
