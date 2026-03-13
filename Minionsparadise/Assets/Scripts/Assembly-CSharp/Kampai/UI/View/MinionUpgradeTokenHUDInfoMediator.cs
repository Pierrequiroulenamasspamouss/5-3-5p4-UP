namespace Kampai.UI.View
{
	public class MinionUpgradeTokenHUDInfoMediator : global::strange.extensions.mediation.impl.Mediator
	{
		[Inject]
		public global::Kampai.UI.View.MinionUpgradeTokenHUDInfoView view { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.TokenDooberCompleteSignal tokenDooberCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.TokenDooberHasBeenSpawnedSignal tokenDooberHasBeenSpawnedSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetHUDTokenAmountSignal setHUDTokenAmountSignal { get; set; }

		public override void OnRegister()
		{
			view.Init();
			tokenDooberCompleteSignal.AddListener(RecievedToken);
			tokenDooberHasBeenSpawnedSignal.AddListener(SlideIn);
			setHUDTokenAmountSignal.AddListener(UpdateAmount);
			UpdateAmount();
		}

		public override void OnRemove()
		{
			tokenDooberCompleteSignal.RemoveListener(RecievedToken);
			tokenDooberHasBeenSpawnedSignal.RemoveListener(SlideIn);
			setHUDTokenAmountSignal.RemoveListener(UpdateAmount);
		}

		private void UpdateAmount()
		{
			int quantityByDefinitionId = (int)playerService.GetQuantityByDefinitionId(50);
			string text = localizationService.GetString("QuantityItemFormat", quantityByDefinitionId);
			view.SetText(text);
		}

		private void SlideIn()
		{
			view.animator.Play("Open");
		}

		private void RecievedToken()
		{
			UpdateAmount();
			SlideOut();
		}

		private void SlideOut()
		{
			view.animator.Play("Close");
		}
	}
}
