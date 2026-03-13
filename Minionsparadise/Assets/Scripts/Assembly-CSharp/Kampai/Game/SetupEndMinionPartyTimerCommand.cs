namespace Kampai.Game
{
	public class SetupEndMinionPartyTimerCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("SetupEndMinionPartyTimerCommand") as global::Kampai.Util.IKampaiLogger;

		private global::System.Collections.IEnumerator EndPartyCoroutine;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.EndMinionPartySignal endMinionPartySignal { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestOfHonorService { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			int partyDuration = minionPartyInstance.Definition.GetPartyDuration(guestOfHonorService.PartyShouldProduceBuff());
			EndPartyCoroutine = EndMinionParty(partyDuration);
			routineRunner.StartCoroutine(EndPartyCoroutine);
			endMinionPartySignal.AddListener(EndMinionPartyThroughSkip);
		}

		private void EndMinionPartyThroughSkip(bool isSkipping)
		{
			endMinionPartySignal.RemoveListener(EndMinionPartyThroughSkip);
			routineRunner.StopCoroutine(EndPartyCoroutine);
			EndPartyCoroutine = null;
		}

		private global::System.Collections.IEnumerator EndMinionParty(int sequenceLength)
		{
			yield return new global::UnityEngine.WaitForSeconds(sequenceLength);
			endMinionPartySignal.RemoveListener(EndMinionPartyThroughSkip);
			logger.Debug("Dispatching the EndMinionPartySignal at the end of the party coroutine");
			endMinionPartySignal.Dispatch(false);
			EndPartyCoroutine = null;
		}
	}
}
