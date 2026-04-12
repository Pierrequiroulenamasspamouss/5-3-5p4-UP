public class ClickedVillainLairComponentBuildingCommand : global::strange.extensions.command.impl.Command
{
	[Inject]
	public global::Kampai.Game.MasterPlanComponentDefinition componentDefinition { get; set; }

	[Inject]
	public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

	[Inject]
	public global::Kampai.UI.View.DisplayMasterPlanSignal displayMasterPlanSignal { get; set; }

	public override void Execute()
	{
		if (!string.IsNullOrEmpty(componentDefinition.OnClickSound))
		{
			playSFXSignal.Dispatch(componentDefinition.OnClickSound);
		}
		displayMasterPlanSignal.Dispatch(componentDefinition.ID);
	}
}
