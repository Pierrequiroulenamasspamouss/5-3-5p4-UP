public class DefinitionsFetchedCommand : global::strange.extensions.command.impl.Command
{
	public override void Execute()
	{
		global::Kampai.Game.DefinitionService.DeleteBinarySerialization();
	}
}
