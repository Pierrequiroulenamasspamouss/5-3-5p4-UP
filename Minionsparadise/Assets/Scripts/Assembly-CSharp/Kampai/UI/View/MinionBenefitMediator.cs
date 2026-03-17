namespace Kampai.UI.View
{
	public class MinionBenefitMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.MinionBenefitView view { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshAllOfTypeArgsSignal refreshAllOfTypeArgsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		public override void OnRegister()
		{
			view.Init(definitionService, playerService);
			refreshAllOfTypeArgsSignal.AddListener(view.RefreshAllOfTypeArgsCallback);
			view.triggerAbilityBarAudio.AddListener(PlayAbilityAudio);
		}

		public override void OnRemove()
		{
			refreshAllOfTypeArgsSignal.RemoveListener(view.RefreshAllOfTypeArgsCallback);
			view.triggerAbilityBarAudio.RemoveListener(PlayAbilityAudio);
		}

		private void PlayAbilityAudio()
		{
			soundFXSignal.Dispatch("Play_minionUpgrade_minionAbilityBarActivate_01");
		}
	}
}
