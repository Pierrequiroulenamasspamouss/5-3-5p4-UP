namespace Kampai.UI.View
{
	public class TownHallButtonMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.TownHallButtonView view { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void OnRegister()
		{
			view.ClickedSignal.AddListener(ButtonClicked);
		}

		public override void OnRemove()
		{
			view.ClickedSignal.RemoveListener(ButtonClicked);
		}

		private void ButtonClicked()
		{
			global::Kampai.Game.PanInstructions type = new global::Kampai.Game.PanInstructions(313);
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveToInstanceSignal>().Dispatch(type, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(new global::Kampai.Game.ScreenPosition()));
		}
	}
}
