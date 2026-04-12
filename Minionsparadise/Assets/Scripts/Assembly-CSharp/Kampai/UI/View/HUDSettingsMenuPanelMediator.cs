namespace Kampai.UI.View
{
	public class HUDSettingsMenuPanelMediator : global::strange.extensions.mediation.impl.Mediator
	{
		private global::UnityEngine.GameObject settingsMenuPanelGO;

		[Inject]
		public global::Kampai.UI.View.HUDSettingsMenuPanelView view { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject(global::Kampai.Game.SocialServices.FACEBOOK)]
		public global::Kampai.Game.ISocialService facebookService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateFacebookStateSignal updateFacebookDialogState { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeAllMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.StopAutopanSignal stopAutopanSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel model { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplaySettingsMenuSignal displaySettingsMenuSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDebugButtonSignal displayDebugButtonSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDisco3DElements displayDisco3DElements { get; set; }

		[Inject]
		public global::Kampai.Common.NetworkConnectionLostSignal networkConnectionLostSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ResumeNetworkOperationSignal resumeNetworkOperationSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.TempHideSettingsMenuSignal tempHideSettingsMenuSignal { get; set; }

		[Inject(global::Kampai.Main.MainElement.UI_OVERLAY_CANVAS)]
		public global::UnityEngine.GameObject overlayCanvas { get; set; }

		public override void OnRegister()
		{
			settingsMenuPanelGO = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("screen_HUD_Panel_Settings_Menu"));
			
			if (overlayCanvas != null)
			{
				settingsMenuPanelGO.transform.SetParent(overlayCanvas.transform, false);
			}
			else
			{
				global::UnityEngine.Debug.LogError("[HUDSettingsMenuPanelMediator] overlayCanvas is null! Settings menu could not be parented correctly.");
				settingsMenuPanelGO.transform.SetParent(base.transform, false);
			}

			global::UnityEngine.RectTransform rect = settingsMenuPanelGO.GetComponent<global::UnityEngine.RectTransform>();
			if (rect != null)
			{
				rect.anchorMin = global::UnityEngine.Vector2.zero;
				rect.anchorMax = global::UnityEngine.Vector2.one;
				rect.offsetMin = global::UnityEngine.Vector2.zero;
				rect.offsetMax = global::UnityEngine.Vector2.zero;
			}

			settingsMenuPanelGO.SetActive(false);
			if (global::Kampai.Util.GameConstants.StaticConfig.DEBUG_ENABLED || global::UnityEngine.PlayerPrefs.GetInt("DebugConsoleEnabled", 0) == 1)
			{
				global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("DebugConsoleButton"));
				gameObject.transform.SetParent(base.transform, false);
			}
			displaySettingsMenuSignal.AddListener(Display);
			view.ButtonToListenTo.ClickedSignal.AddListener(ButtonClicked);
			networkConnectionLostSignal.AddListener(OnNetworkLost);
		}

		public override void OnRemove()
		{
			view.ButtonToListenTo.ClickedSignal.RemoveListener(ButtonClicked);
			displaySettingsMenuSignal.RemoveListener(Display);
			networkConnectionLostSignal.RemoveListener(OnNetworkLost);
		}

		private void Display(bool display)
		{
			displayDisco3DElements.Dispatch(!display);
			if (!display && settingsMenuPanelGO.activeInHierarchy)
			{
				closeAllMenuSignal.Dispatch(null);
				displayDebugButtonSignal.Dispatch(false);
				return;
			}
			model.ForceDisabled = true;
			stopAutopanSignal.Dispatch();
			soundFXSignal.Dispatch("Play_menu_popUp_01");
			updateFacebookDialogState.Dispatch(facebookService.isLoggedIn);
			closeAllMenuSignal.Dispatch(settingsMenuPanelGO);
			settingsMenuPanelGO.SetActive(true);
			settingsMenuPanelGO.transform.SetAsLastSibling();
		}

		private void ButtonClicked()
		{
			global::Kampai.Game.MinionParty minionPartyInstance = playerService.GetMinionPartyInstance();
			if (minionPartyInstance == null || (!minionPartyInstance.CharacterUnlocking && !minionPartyInstance.IsPartyHappening))
			{
				Display(true);
				displayDebugButtonSignal.Dispatch(true);
			}
		}

		private void OnNetworkLost()
		{
			if (settingsMenuPanelGO.activeInHierarchy)
			{
				tempHideSettingsMenuSignal.Dispatch();
				Display(false);
				resumeNetworkOperationSignal.AddOnce(delegate
				{
					Display(true);
				});
			}
		}
	}
}
