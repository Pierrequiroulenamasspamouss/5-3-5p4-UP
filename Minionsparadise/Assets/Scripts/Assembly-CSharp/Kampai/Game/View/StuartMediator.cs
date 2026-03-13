namespace Kampai.Game.View
{
	public class StuartMediator : global::Kampai.Game.View.FrolicCharacterMediator
	{
		[Inject]
		public global::Kampai.Game.View.StuartView stuartView { get; set; }

		[Inject]
		public global::Kampai.Game.StuartAddToStageSignal addToStageSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StuartStartPerformingSignal startPerformingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StuartGetOnStageImmediateSignal getOnStageImmediateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StuartTicketFilledSignal stuartTicketFilledSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StuartTunesGuitarSignal stuartTunesGuitarSignal { get; set; }

		[Inject]
		public global::Kampai.Game.SocialEventAvailableSignal socialEventAvailableSignal { get; set; }

		[Inject]
		public global::Kampai.Game.RestoreStuartSignal restoreStuartSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StuartShowCompleteSignal stuartShowCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Main.StopLocalAudioSignal stopLocalAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StopStuartPerformanceAudioSignal stopPerformanceAudioSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StuartSpinMicSignal spinMicSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			addToStageSignal.AddListener(AddToStage);
			startPerformingSignal.AddListener(StartPerforming);
			getOnStageImmediateSignal.AddListener(GetOnStageImmediate);
			stuartTicketFilledSignal.AddListener(TicketFilled);
			stuartTunesGuitarSignal.AddListener(StuartTunesGuitar);
			stuartShowCompleteSignal.AddListener(RestoreStuart);
			stopPerformanceAudioSignal.AddListener(StopPerformanceAudio);
			stuartView.OnSpinMic = OnSpinMic;
		}

		public override void OnRemove()
		{
			base.OnRemove();
			addToStageSignal.RemoveListener(AddToStage);
			startPerformingSignal.RemoveListener(StartPerforming);
			getOnStageImmediateSignal.RemoveListener(GetOnStageImmediate);
			stuartTicketFilledSignal.RemoveListener(TicketFilled);
			stuartTunesGuitarSignal.RemoveListener(StuartTunesGuitar);
			stuartShowCompleteSignal.RemoveListener(RestoreStuart);
			stopPerformanceAudioSignal.RemoveListener(StopPerformanceAudio);
		}

		private void StuartTunesGuitar()
		{
			stuartView.GetOnStage(true);
			stuartView.TuneGuitar(delegate
			{
				socialEventAvailableSignal.Dispatch();
			});
		}

		private void OnSpinMic()
		{
			spinMicSignal.Dispatch();
		}

		private void RestoreStuart()
		{
			restoreStuartSignal.Dispatch();
		}

		private void AddToStage(global::UnityEngine.Vector3 position, global::UnityEngine.Quaternion rotation, global::Kampai.Game.StuartStageAnimationType animType)
		{
			stuartView.AddToStage(position, rotation, animType);
		}

		private void StartPerforming(global::Kampai.Util.SignalCallback<global::strange.extensions.signal.impl.Signal> finishedCallback)
		{
			stuartView.GetOnStage(true);
			stuartView.Perform(finishedCallback);
		}

		private void StopPerformanceAudio()
		{
			stopLocalAudioSignal.Dispatch(base.gameObject.GetComponent<CustomFMOD_StudioEventEmitter>());
		}

		private void GetOnStageImmediate(bool enable)
		{
			stuartView.GetOnStageImmediate(enable);
		}

		private void TicketFilled()
		{
			stuartView.StartingState(global::Kampai.Game.StuartStageAnimationType.CELEBRATE, true);
		}
	}
}
