public class RestoreSpecialEventCommand : global::strange.extensions.command.impl.Command
{
	public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("RestoreSpecialEventCommand") as global::Kampai.Util.IKampaiLogger;

	[Inject]
	public global::Kampai.Game.SpecialEventItemDefinition specialEventItemDefinition { get; set; }

	[Inject]
	public global::Kampai.Game.LoadSpecialEventPaintoverSignal loadPaintoverSignal { get; set; }

	public override void Execute()
	{
		logger.Info("Restoring special event {0}", specialEventItemDefinition.LocalizedKey);
		loadPaintoverSignal.Dispatch(specialEventItemDefinition);
	}
}
