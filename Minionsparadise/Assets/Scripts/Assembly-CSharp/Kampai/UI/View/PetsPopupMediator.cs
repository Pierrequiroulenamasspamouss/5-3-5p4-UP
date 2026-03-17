namespace Kampai.UI.View
{
	public class PetsPopupMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.PetsPopupView>
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.UI.View.OnClickSkrimSignal onClickSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			base.view.Init();
		}

		public override void OnRegister()
		{
			base.view.PlayNowButton.ClickedSignal.AddListener(PlayNowClicked);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			onClickSkrimSignal.AddListener(SkrimClose);
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.view.PlayNowButton.ClickedSignal.RemoveListener(PlayNowClicked);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			onClickSkrimSignal.RemoveListener(SkrimClose);
			base.OnRemove();
		}

		protected override void Close()
		{
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			base.view.Close();
		}

		private void SkrimClose()
		{
			telemetryService.Send_Telemetry_EVT_GAME_BUTTON_PRESSED_GENERIC(global::Kampai.Util.GameConstants.TrackedGameButton.PetsXPromo_Dismiss, string.Empty);
		}

		private void PlayNowClicked()
		{
			global::Kampai.Game.PetsXPromoDefinition petsXPromoDefinition = definitionService.Get<global::Kampai.Game.PetsXPromoDefinition>(95000);
			if (petsXPromoDefinition != null)
			{
				if (global::Kampai.Util.Native.IsAppInstalled(petsXPromoDefinition.AndroidInstallURL))
				{
					telemetryService.Send_Telemetry_EVT_GAME_XPROMO_BUTTON_PRESSED(global::Kampai.Util.GameConstants.TrackedGameButton.PetsXPromo_PlayNow, true);
					global::Kampai.Util.Native.LaunchApp(petsXPromoDefinition.AndroidInstallURL);
				}
				else
				{
					telemetryService.Send_Telemetry_EVT_GAME_XPROMO_BUTTON_PRESSED(global::Kampai.Util.GameConstants.TrackedGameButton.PetsXPromo_PlayNow, false);
					global::Kampai.Util.Native.OpenURL(petsXPromoDefinition.AndroidPetsSmartURL);
				}
				Close();
			}
		}

		private void OnMenuClose()
		{
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "popup_Pets");
			hideSkrim.Dispatch("PetsXPromo");
		}
	}
}
