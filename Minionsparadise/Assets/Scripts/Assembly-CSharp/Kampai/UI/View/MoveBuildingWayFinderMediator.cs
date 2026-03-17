namespace Kampai.UI.View
{
	public class MoveBuildingWayFinderMediator : global::Kampai.UI.View.AbstractWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.MoveBuildingWayFinderView moveBuildingWayFinderView { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return moveBuildingWayFinderView;
			}
		}

		protected override void GoToClicked()
		{
			global::Kampai.Game.View.BuildingDefinitionObject targetObject = moveBuildingWayFinderView.GetTargetObject();
			global::Kampai.Game.ScreenPosition value = new global::Kampai.Game.ScreenPosition();
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveSignal>().Dispatch(targetObject.ResourceIconPosition, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(value), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.None, null, null), false);
		}
	}
}
