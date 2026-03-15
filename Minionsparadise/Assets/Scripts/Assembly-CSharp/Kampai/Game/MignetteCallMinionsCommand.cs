namespace Kampai.Game
{
	public class MignetteCallMinionsCommand : global::strange.extensions.command.impl.Command
	{
		private bool ownsMinigamePack;

		private int numMinionsToCall;

		private global::Kampai.Game.MignetteBuilding mignetteBuilding;

		private global::UnityEngine.GameObject signalSender;

		[Inject]
		public global::Kampai.Game.MignetteCallMinionsModel model { get; set; }

		[Inject]
		public global::Kampai.UI.View.EnableSkrimButtonSignal enableSkrimButtonSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideAllWayFindersSignal hideAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.GenerateTemporaryMinionSignal generateTemporaryMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TemporaryMinionsService tempMinionService { get; set; }

		[Inject]
		public global::Kampai.Game.StartMinionTaskSignal startMinionTaskSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CallMinionSignal callMinionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		private void Init()
		{
			ownsMinigamePack = playerService.HasPurchasedMinigamePack();
			numMinionsToCall = model.NumberOfMinionsToCall;
			mignetteBuilding = model.Building;
			signalSender = model.SignalSender;
		}

		public override void Execute()
		{
			Init();
			enableSkrimButtonSignal.Dispatch(false);
			hideAllWayFindersSignal.Dispatch();
			showHUDSignal.Dispatch(false);
			showStoreSignal.Dispatch(false);
			if (ownsMinigamePack)
			{
				for (int i = 0; i < numMinionsToCall; i++)
				{
					global::Kampai.Game.GenerateTemporaryMinionCommand.TemporaryMinionProperties type = new global::Kampai.Game.GenerateTemporaryMinionCommand.TemporaryMinionProperties
					{
						TempID = -100 - i,
						startX = mignetteBuilding.Location.x,
						startY = mignetteBuilding.Location.y,
						finishX = mignetteBuilding.Location.x,
						finishY = mignetteBuilding.Location.y
					};
					generateTemporaryMinionSignal.Dispatch(type);
				}
			}
			for (int j = 0; j < numMinionsToCall; j++)
			{
				if (ownsMinigamePack)
				{
					global::Kampai.Game.View.MinionObject minion = tempMinionService.GetMinion(-100 - j);
					if (mignetteBuilding.Definition.ID == 3509)
					{
						minion.transform.GetComponentInChildren<global::UnityEngine.BoxCollider>().enabled = true;
					}
					startMinionTaskSignal.Dispatch(new global::Kampai.Util.Tuple<int, global::Kampai.Game.View.MinionObject, int>(mignetteBuilding.ID, minion, timeService.CurrentTime()));
				}
				else
				{
					callMinionSignal.Dispatch(mignetteBuilding, signalSender);
				}
			}
		}
	}
}
