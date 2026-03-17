namespace Kampai.Game.View
{
	public class FrolicCharacterMediator : global::strange.extensions.mediation.impl.Mediator
	{
		public global::Kampai.Game.View.NamedCharacterManagerView NamedCharacterManagerView;

		public global::strange.extensions.signal.impl.Signal AnimationCallbackSignal = new global::strange.extensions.signal.impl.Signal();

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.CharacterArrivedAtDestinationSignal arrivedAtDestinationSignal { get; set; }

		[Inject]
		public global::Kampai.Game.FrolicSignal frolicSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IdleInTownHallSignal idleInTownHallSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PostMinionPartyStartSignal postMinionPartyStartSignal { get; set; }

		[Inject]
		public global::Kampai.Game.PostMinionPartyEndSignal postMinionPartyEndSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject NamedCharacterManager { get; set; }

		public global::Kampai.Game.View.FrolicCharacterView view { get; set; }

		public override void OnRegister()
		{
			NamedCharacterManagerView = NamedCharacterManager.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>();
			view = GetComponent<global::Kampai.Game.View.FrolicCharacterView>();
			view.SetAnimationCallback(AnimationCallbackSignal);
			arrivedAtDestinationSignal.AddListener(ArrivedAtWayPoint);
			idleInTownHallSignal.AddListener(IdleInTownHall);
			AnimationCallbackSignal.AddListener(AnimationCallback);
			postMinionPartyStartSignal.AddListener(StartParty);
			postMinionPartyEndSignal.AddListener(EndParty);
		}

		public override void OnRemove()
		{
			arrivedAtDestinationSignal.RemoveListener(ArrivedAtWayPoint);
			idleInTownHallSignal.RemoveListener(IdleInTownHall);
			AnimationCallbackSignal.RemoveListener(AnimationCallback);
			postMinionPartyStartSignal.RemoveListener(StartParty);
			postMinionPartyEndSignal.RemoveListener(EndParty);
		}

		private void ArrivedAtWayPoint(int minionID)
		{
			if (view.ID == minionID)
			{
				view.ArrivedAtWayPoint();
			}
		}

		private void AnimationCallback()
		{
			frolicSignal.Dispatch(view.ID);
		}

		private void IdleInTownHall(int characterId, global::Kampai.Game.LocationIncidentalAnimationDefinition animationDefinition)
		{
			if (characterId == view.ID)
			{
				global::Kampai.Game.FrolicCharacter byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.FrolicCharacter>(view.ID);
				byInstanceId.CurrentFrolicLocation = animationDefinition.Location;
				global::Kampai.Game.MinionAnimationDefinition mad = definitionService.Get<global::Kampai.Game.MinionAnimationDefinition>(animationDefinition.AnimationId);
				view.IdleInTownHall(animationDefinition, mad);
			}
		}

		protected virtual void StartParty()
		{
			view.SetIsInParty(true);
		}

		protected virtual void EndParty()
		{
			view.SetIsInParty(false);
			if (view.IsWandering())
			{
				frolicSignal.Dispatch(view.ID);
			}
		}
	}
}
