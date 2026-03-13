namespace Kampai.UI.View
{
	public class OrderBoardTicketMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.OrderBoardTicketView view { get; set; }

		[Inject]
		public global::Kampai.UI.View.OrderBoardTicketClickedSignal ticketClickedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.OrderBoardStartRefillTicketSignal startRefillTicket { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal globalSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ResetDoubleTapSignal resetDoubleTapSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartCurrencyBuffSignal startCurrencyBuffSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StopCurrencyBuffSignal stopCurrencyBuffSignal { get; set; }

		public override void OnRegister()
		{
			view.TicketButton.ClickedSignal.AddListener(OnTicketClicked);
			view.TicketMeter.RushButton.ClickedSignal.AddListener(OnTicketRushed);
			startRefillTicket.AddListener(StartTimer);
			startCurrencyBuffSignal.AddListener(view.UpdateReward);
			stopCurrencyBuffSignal.AddListener(view.UpdateReward);
		}

		public override void OnRemove()
		{
			view.OnRemove();
			view.TicketButton.ClickedSignal.RemoveListener(OnTicketClicked);
			view.TicketMeter.RushButton.ClickedSignal.RemoveListener(OnTicketRushed);
			startRefillTicket.RemoveListener(StartTimer);
			startCurrencyBuffSignal.RemoveListener(view.UpdateReward);
			stopCurrencyBuffSignal.RemoveListener(view.UpdateReward);
		}

		private void StartTimer(global::Kampai.Util.Tuple<int, int, float> tuple)
		{
			if (view.Index == tuple.Item1)
			{
				view.SetRootAnimation(false);
				StartCoroutine(ChangeToDeleteState(tuple.Item1, tuple.Item2, tuple.Item3));
			}
		}

		private global::System.Collections.IEnumerator ChangeToDeleteState(int index, int duration, float repopTime)
		{
			yield return new global::UnityEngine.WaitForSeconds(repopTime);
			view.SetRootAnimation(true);
			view.StartTimer(index, duration);
		}

		private void OnTicketRushed()
		{
			resetDoubleTapSignal.Dispatch(view.Index);
			if (view.TicketMeter.RushButton.isDoubleConfirmed())
			{
				playerService.ProcessRush(view.TicketMeter.rushCost, true, RushTransactionCallback, 3022);
			}
		}

		private void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				globalSFXSignal.Dispatch("Play_button_premium_01");
				timeEventService.RushEvent(-view.Index);
			}
		}

		private void OnTicketClicked()
		{
			if (!view.IsSelected)
			{
				view.HighlightTicket(false);
				ticketClickedSignal.Dispatch(view.ticketInstance, view.Title, false);
			}
		}
	}
}
