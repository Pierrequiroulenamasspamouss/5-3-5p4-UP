namespace Kampai.Game.Mignette.EdwardMinionHands
{
	public class SetupEdwardMinionHandsManagerViewCommand : global::Kampai.Game.Mignette.SetupMignetteManagerViewCommand
	{
		public override void Execute()
		{
			global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsMignetteManagerView edwardMinionHandsMignetteManagerView = CreateManagerView<global::Kampai.Game.Mignette.EdwardMinionHands.View.EdwardMinionHandsMignetteManagerView>("EdwardMinionHandsMignetteManagerView");
			base.contextView.transform.position = edwardMinionHandsMignetteManagerView.MignetteBuildingObject.transform.position;
			InitializeChildObjects(edwardMinionHandsMignetteManagerView);
		}
	}
}
