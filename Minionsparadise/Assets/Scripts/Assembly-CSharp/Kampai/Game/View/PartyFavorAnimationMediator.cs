namespace Kampai.Game.View
{
	public class PartyFavorAnimationMediator : global::strange.extensions.mediation.impl.EventMediator
	{
		private global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("PartyFavorAnimationMediator") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.View.PartyFavorAnimationView view { get; set; }

		[Inject]
		public global::Kampai.Game.MinionStateChangeSignal minionStateChangeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IBuildingUtilities buildingUtilies { get; set; }

		[Inject]
		public global::Kampai.Game.PartyFavorTrackChildSignal trackChildSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PartyFavorFreeAllMinionsSignal freeMinionsSignal { get; set; }

		public override void OnRegister()
		{
			view.SetupInjections(logger, minionStateChangeSignal, buildingUtilies);
			trackChildSignal.AddListener(TrackChild);
			freeMinionsSignal.AddListener(FreeAllMinions);
		}

		public override void OnRemove()
		{
			trackChildSignal.RemoveListener(TrackChild);
			freeMinionsSignal.RemoveListener(FreeAllMinions);
		}

		private void TrackChild(int ID, global::Kampai.Game.View.MinionObject minion)
		{
			if (ID == view.PartyFavorDefinition.ID)
			{
				view.TrackChild(minion);
			}
		}

		private void FreeAllMinions(int ID)
		{
			if (ID == view.PartyFavorDefinition.ID)
			{
				view.FreeAllMinions();
			}
		}
	}
}
