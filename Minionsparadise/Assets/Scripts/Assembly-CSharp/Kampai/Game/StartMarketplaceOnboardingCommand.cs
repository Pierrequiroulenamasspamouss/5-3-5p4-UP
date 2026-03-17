namespace Kampai.Game
{
	public class StartMarketplaceOnboardingCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public int buildingId { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayFloatingTextSignal displayFloatingTextSignal { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		public override void Execute()
		{
			string text = localizationService.GetString("MarketplaceOnboarding");
			global::Kampai.UI.View.FloatingTextSettings type = new global::Kampai.UI.View.FloatingTextSettings(buildingId, text, 130f);
			displayFloatingTextSignal.Dispatch(type);
		}
	}
}
