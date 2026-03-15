namespace Kampai.Game.Mignette
{
	public class ChangeCurrentMignetteScore : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int scoreDelta { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.MignetteGameModel mignetteGameModel { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.MignetteScoreUpdatedSignal mignetteScoreUpdatedSignal { get; set; }

		public override void Execute()
		{
			mignetteGameModel.CurrentGameScore += scoreDelta;
			mignetteScoreUpdatedSignal.Dispatch(mignetteGameModel.CurrentGameScore);
		}
	}
}
