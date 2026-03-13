namespace Kampai.Game.View
{
	public class AndroidBackButtonCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.UIModel model { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkModel networkModel { get; set; }

		[Inject]
		public global::Kampai.Common.MinimizeAppSignal minimizeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IZoomCameraModel zoomModel { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingZoomSignal zoomSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.LevelUpBackButtonSignal levelUpBackButtonSignal { get; set; }

		public override void Execute()
		{
			if (model.DisableBack)
			{
				return;
			}
			if (model.LevelUpUIOpen)
			{
				levelUpBackButtonSignal.Dispatch();
			}
			else if (networkModel.isConnectionLost)
			{
				minimizeSignal.Dispatch();
			}
			else if (model.UIOpen)
			{
				global::System.Action action = model.RemoveTopUI();
				if (action != null)
				{
					action();
				}
			}
			else if (zoomModel.ZoomedIn)
			{
				zoomSignal.Dispatch(new global::Kampai.Game.BuildingZoomSettings(global::Kampai.Game.ZoomType.OUT, zoomModel.LastZoomBuildingType));
			}
			else
			{
				minimizeSignal.Dispatch();
			}
		}
	}
}
