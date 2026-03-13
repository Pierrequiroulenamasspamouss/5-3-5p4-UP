namespace Kampai.Game.Mignette.BalloonBarrage
{
	public class SetupBalloonBarrageManagerViewCommand : global::Kampai.Game.Mignette.SetupMignetteManagerViewCommand
	{
		public override void Execute()
		{
			global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMignetteManagerView balloonBarrageMignetteManagerView = CreateManagerView<global::Kampai.Game.Mignette.BalloonBarrage.View.BalloonBarrageMignetteManagerView>("BalloonBarrageMignetteManagerView");
			base.contextView.transform.position = balloonBarrageMignetteManagerView.MignetteBuildingObject.transform.position;
			InitializeChildObjects(balloonBarrageMignetteManagerView);
		}
	}
}
