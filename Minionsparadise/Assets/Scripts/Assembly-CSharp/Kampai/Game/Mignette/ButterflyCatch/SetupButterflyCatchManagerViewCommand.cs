namespace Kampai.Game.Mignette.ButterflyCatch
{
	public class SetupButterflyCatchManagerViewCommand : global::Kampai.Game.Mignette.SetupMignetteManagerViewCommand
	{
		public override void Execute()
		{
			global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView butterflyCatchMignetteManagerView = CreateManagerView<global::Kampai.Game.Mignette.ButterflyCatch.View.ButterflyCatchMignetteManagerView>("ButterflyCatchMignetteManagerView");
			base.contextView.transform.position = butterflyCatchMignetteManagerView.MignetteBuildingObject.transform.position;
			InitializeChildObjects(butterflyCatchMignetteManagerView);
		}
	}
}
