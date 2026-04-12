namespace Kampai.Game.View
{
	public class TSMCharacterMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::Kampai.Game.View.NamedCharacterManagerView namedCharacterManagerView;

		private global::Kampai.Game.View.TSMCharacterHideState hideState;

		private bool isInParty;

		private bool isChestIntro;

		[Inject]
		public global::Kampai.Game.View.TSMCharacterView View { get; set; }

		[Inject]
		public global::Kampai.Game.HideTSMCharacterSignal HideTSMCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.PostMinionPartyStartSignal minionPartyStartSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PostMinionPartyEndSignal minionPartyEndSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CheckTriggersSignal checkTriggersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TSMReachedDestinationSignal tsmReachedDestinationSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject NamedCharacterManager { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playGlobalSFX { get; set; }

		[Inject]
		public global::Kampai.Game.CaptainWaveAndCallCallbackSignal waveAndCallbackSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TSMStartIntroAnimation tsmStartIntroAnimation { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeService timeService { get; set; }

		[Inject]
		public global::Kampai.Game.CheckTriggersSignal triggersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.TSMTreasureCollectedSignal treasureCollectedSignal { get; set; }

		public override void OnRegister()
		{
			namedCharacterManagerView = NamedCharacterManager.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>();
			HideTSMCharacterSignal.AddListener(HideTSMCharacter);
			View.RemoveCharacterSignal.AddListener(RemoveTSMCharacter);
			minionPartyStartSignal.AddListener(OnPartyStart);
			minionPartyEndSignal.AddListener(OnPartyEnd);
			View.NextPartyAnimSignal.AddListener(PlayPartyAnimation);
			View.DestinationReachedSignal.AddListener(DestinationReached);
			waveAndCallbackSignal.AddListener(WaveAndCallback);
			tsmStartIntroAnimation.AddListener(StartIntro);
			View.ChestReadyToOpen.AddListener(ChestReadyToOpen);
			treasureCollectedSignal.AddListener(TreasureCollected);
			Init();
		}

		public override void OnRemove()
		{
			treasureCollectedSignal.RemoveListener(TreasureCollected);
			HideTSMCharacterSignal.RemoveListener(HideTSMCharacter);
			View.RemoveCharacterSignal.RemoveListener(RemoveTSMCharacter);
			minionPartyStartSignal.RemoveListener(OnPartyStart);
			minionPartyEndSignal.RemoveListener(OnPartyEnd);
			View.NextPartyAnimSignal.RemoveListener(PlayPartyAnimation);
			View.DestinationReachedSignal.RemoveListener(DestinationReached);
			waveAndCallbackSignal.RemoveListener(WaveAndCallback);
			tsmStartIntroAnimation.RemoveListener(StartIntro);
			View.ChestReadyToOpen.RemoveListener(ChestReadyToOpen);
		}

		private void Init()
		{
			playGlobalSFX.Dispatch("Play_tsm_arrive_01");
			View.ShowTSMCharacter();
		}

		private void HideTSMCharacter(global::Kampai.Game.View.TSMCharacterHideState hideState)
		{
			this.hideState = hideState;
			View.HideTSMCharacter(hideState);
		}

		private void DestinationReached()
		{
			tsmReachedDestinationSignal.Dispatch();
		}

		private void RemoveTSMCharacter()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			namedCharacterManagerView.Remove(301);
			if (hideState == global::Kampai.Game.View.TSMCharacterHideState.CelebrateAndReturn)
			{
				checkTriggersSignal.Dispatch(301);
			}
		}

		private void OnPartyStart()
		{
			isInParty = true;
			PlayPartyAnimation();
		}

		private void OnPartyEnd()
		{
			isInParty = false;
		}

		private void PlayPartyAnimation()
		{
			if (isInParty)
			{
				int partyAnimationId = definitionService.Get<global::Kampai.Game.TSMCharacterDefinition>(View.DefinitionID).PartyAnimationId;
				global::Kampai.Game.Transaction.WeightedInstance weightedInstance = playerService.GetWeightedInstance(partyAnimationId);
				global::Kampai.Game.MinionAnimationDefinition minionAnimationDefinition = definitionService.Get<global::Kampai.Game.MinionAnimationDefinition>(weightedInstance.NextPick(randomService).ID);
				if (minionAnimationDefinition != null)
				{
					View.PlayPartyAnimation(minionAnimationDefinition);
				}
			}
			else if (isChestIntro)
			{
				View.ChestIntroPostParty();
			}
			else
			{
				global::UnityEngine.RuntimeAnimatorController animController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_unique_sales_minion_intro");
				View.SetAnimController(animController);
			}
		}

		private void StartIntro(bool isChestIntro)
		{
			this.isChestIntro = isChestIntro;
			if (isChestIntro)
			{
				View.StartChestIntro();
			}
			else
			{
				View.ShowTSMCharacter();
			}
		}

		private void WaveAndCallback(global::System.Action callback, bool isChestIntro)
		{
			this.isChestIntro = isChestIntro;
			if (!isChestIntro)
			{
				View.SayCheese(callback);
			}
			else
			{
				View.OpenChest(callback);
			}
		}

		private void ChestReadyToOpen()
		{
			tsmReachedDestinationSignal.Dispatch();
		}

		private void TreasureCollected()
		{
			HideTSMCharacter(global::Kampai.Game.View.TSMCharacterHideState.Hide);
			global::Kampai.Game.TSMCharacterDefinition tSMCharacterDefinition = definitionService.Get<global::Kampai.Game.TSMCharacterDefinition>();
			timeEventService.AddEvent(View.ID, timeService.CurrentTime(), tSMCharacterDefinition.CooldownInSeconds, triggersSignal);
		}
	}
}
