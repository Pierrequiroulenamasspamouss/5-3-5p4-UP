namespace Kampai.Game.Mignette.WaterSlide
{
	public class WaterSlideMignetteSetupCommand : global::Kampai.Game.Mignette.SetupMignetteManagerViewCommand
	{
		public override void Execute()
		{
			global::Kampai.Game.Mignette.WaterSlide.View.WaterSlideMignetteManagerView waterSlideMignetteManagerView = CreateManagerView<global::Kampai.Game.Mignette.WaterSlide.View.WaterSlideMignetteManagerView>("WaterSlideMignetteManagerView");
			base.contextView.transform.position = waterSlideMignetteManagerView.MignetteBuildingObject.transform.position;
			InitializeChildObjects(waterSlideMignetteManagerView);
		}
	}
}
