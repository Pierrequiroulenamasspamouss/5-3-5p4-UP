namespace Kampai.Game.View
{
	public class SpecialEventCharacterMediator : global::Kampai.Game.View.FrolicCharacterMediator
	{
		private bool isInParty;

		private global::System.Collections.IEnumerator m_showWithDelay;

		[Inject]
		public global::Kampai.Game.View.SpecialEventCharacterView characterView { get; set; }

		[Inject]
		public global::Kampai.Game.HideSpecialEventCharacterSignal hideSpecialEventCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowSpecialEventCharacterSignal showSpecialEventCharacterSignal { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			hideSpecialEventCharacterSignal.AddListener(HideSECCharacter);
			characterView.RemoveCharacterSignal.AddListener(RemoveSECCharacter);
			characterView.NextPartyAnimSignal.AddListener(PlayPartyAnimation);
			showSpecialEventCharacterSignal.AddListener(ShowSpecialEventCharacter);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			hideSpecialEventCharacterSignal.RemoveListener(HideSECCharacter);
			characterView.RemoveCharacterSignal.RemoveListener(RemoveSECCharacter);
			characterView.NextPartyAnimSignal.RemoveListener(PlayPartyAnimation);
			showSpecialEventCharacterSignal.RemoveListener(ShowSpecialEventCharacter);
		}

		private void ShowSpecialEventCharacter(float delayLength, int specialEventCharacterID)
		{
			if (specialEventCharacterID == -1 || characterView.eventCharacter.ID == specialEventCharacterID)
			{
				m_showWithDelay = ShowWithDelay(delayLength);
				StartCoroutine(m_showWithDelay);
			}
		}

		private void HideSECCharacter(bool completedQuest)
		{
			characterView.HideSpecialEventCharacter(completedQuest);
		}

		private void RemoveSECCharacter()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			NamedCharacterManagerView.Remove(309);
		}

		protected override void StartParty()
		{
			isInParty = true;
			PlayPartyAnimation();
		}

		protected override void EndParty()
		{
			isInParty = false;
			base.frolicSignal.Dispatch(characterView.ID);
		}

		private void PlayPartyAnimation()
		{
			if (isInParty)
			{
				int partyAnimationId = base.definitionService.Get<global::Kampai.Game.SpecialEventCharacterDefinition>(characterView.DefinitionID).PartyAnimationId;
				global::Kampai.Game.Transaction.WeightedInstance weightedInstance = base.playerService.GetWeightedInstance(partyAnimationId);
				global::Kampai.Game.MinionAnimationDefinition minionAnimationDefinition = base.definitionService.Get<global::Kampai.Game.MinionAnimationDefinition>(weightedInstance.NextPick(randomService).ID);
				if (minionAnimationDefinition != null)
				{
					characterView.PlayPartyAnimation(minionAnimationDefinition);
				}
			}
			else
			{
				global::UnityEngine.RuntimeAnimatorController animController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>("asm_unique_sales_minion_intro");
				characterView.SetAnimController(animController);
			}
		}

		internal global::System.Collections.IEnumerator ShowWithDelay(float delayLength)
		{
			yield return new global::UnityEngine.WaitForSeconds(delayLength);
			characterView.ShowSpecialEventCharacter();
		}
	}
}
