namespace Kampai.UI.View
{
	public class PopulationBenefitMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::UnityEngine.Coroutine dooberCoroutine;

		[Inject]
		public global::Kampai.UI.View.PopulationBenefitView view { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal spawnDooberSignal { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberModel dooberModel { get; set; }

		public override void OnRegister()
		{
			view.Init(definitionService, localizationService, playerService);
			view.updateSignal.AddListener(CheckTriggerDoober);
			CheckTriggerDoober();
		}

		public override void OnRemove()
		{
			view.updateSignal.RemoveListener(CheckTriggerDoober);
		}

		private void CheckTriggerDoober()
		{
			if (dooberCoroutine != null)
			{
				StopCoroutine(dooberCoroutine);
				dooberCoroutine = null;
			}
			dooberCoroutine = StartCoroutine(TriggerDoober());
		}

		private global::System.Collections.IEnumerator TriggerDoober()
		{
			yield return 0;
			if (view.benefitDefinitionID == dooberModel.PendingPopulationDoober)
			{
				global::Kampai.Game.PopulationBenefitDefinition benefitDefinition = definitionService.Get<global::Kampai.Game.PopulationBenefitDefinition>(view.benefitDefinitionID);
				global::Kampai.Game.Transaction.TransactionDefinition transDef = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(benefitDefinition.transactionDefinitionID);
				global::Kampai.UI.View.DestinationType destType = ((transDef.Outputs[0].ID != 335) ? global::Kampai.UI.View.DestinationType.STORAGE_POPULATION_GOAL : global::Kampai.UI.View.DestinationType.TIMER_POPULATION_GOAL);
				spawnDooberSignal.Dispatch(uiCamera.WorldToScreenPoint(view.PopulationIcon.transform.position), destType, transDef.Outputs[0].ID, false);
				dooberModel.PendingPopulationDoober = 0;
			}
		}
	}
}
