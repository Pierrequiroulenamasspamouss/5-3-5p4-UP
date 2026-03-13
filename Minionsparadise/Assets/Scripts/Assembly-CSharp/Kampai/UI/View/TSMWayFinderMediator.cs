namespace Kampai.UI.View
{
	public class TSMWayFinderMediator : global::Kampai.UI.View.AbstractQuestWayFinderMediator
	{
		[Inject]
		public global::Kampai.UI.View.TSMWayFinderView TSMWayFinderView { get; set; }

		[Inject]
		public global::Kampai.Game.TSMReachedDestinationSignal tsmReachedDestinationSignal { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return TSMWayFinderView;
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
			TSMWayFinderView.SetDestinationReached();
		}
	}
}
