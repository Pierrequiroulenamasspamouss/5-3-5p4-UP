namespace Kampai.UI.View
{
	public class TSMTriggerWayFinderMediator : global::Kampai.UI.View.AbstractWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.TSMTriggerWayFinderView view { get; set; }

		[Inject]
		public global::Kampai.Game.TSMReachedDestinationSignal tsmReachedDestinationSignal { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return view;
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();
			tsmReachedDestinationSignal.AddListener(tsmReachedDestination);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			tsmReachedDestinationSignal.RemoveListener(tsmReachedDestination);
		}

		private void tsmReachedDestination()
		{
			view.SetDestinationReached();
		}

		protected override void GoToClicked()
		{
			if (!base.pickModel.PanningCameraBlocked && !base.lairModel.goingToLair)
			{
				global::Kampai.Game.TSMCharacterSelectedSignal instance = base.GameContext.injectionBinder.GetInstance<global::Kampai.Game.TSMCharacterSelectedSignal>();
				instance.Dispatch();
			}
		}
	}
}
