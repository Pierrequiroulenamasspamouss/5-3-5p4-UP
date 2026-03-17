namespace Kampai.UI.View
{
	public class MinionUpgradeMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MinionUpgradeView>
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MoveAudioListenerSignal toggleCharacterAudioSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetHUDTokenAmountSignal setHUDTokenAmountSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetPremiumCurrencySignal setPremiumCurrencySignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshAllOfTypeArgsSignal refreshAllOfTypeArgsSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshFromIndexArgsSignal refreshFromIndexArgsSignal { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.Init(playerService, definitionService, fancyService, localService, soundFXSignal);
			base.view.refreshAllOfTypeArgsSignal = refreshAllOfTypeArgsSignal;
			base.view.refreshFromIndexArgsSignal = refreshFromIndexArgsSignal;
			soundFXSignal.Dispatch("Play_menu_popUp_01");
			toggleCharacterAudioSignal.Dispatch(false, base.view.MinionSlot);
			base.view.Skrim.ClickedSignal.AddListener(Close);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.UpgradeButton.ClickedSignal.AddListener(UpgradeClicked);
			base.view.RightArrow.ClickedSignal.AddListener(RightArrowClicked);
			base.view.LeftArrow.ClickedSignal.AddListener(LeftArrowClicked);
			base.view.RushButton.ClickedSignal.AddListener(RushClicked);
			refreshFromIndexArgsSignal.AddListener(RefreshFromIndexArgsCallback);
			setHUDTokenAmountSignal.AddListener(base.view.SetUpgradeTokenAmount);
			base.view.CloseButton.ClickedSignal.AddListener(Close);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.Skrim.ClickedSignal.RemoveListener(Close);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.RushButton.ClickedSignal.RemoveListener(RushClicked);
			base.view.UpgradeButton.ClickedSignal.RemoveListener(UpgradeClicked);
			base.view.RightArrow.ClickedSignal.RemoveListener(RightArrowClicked);
			base.view.LeftArrow.ClickedSignal.RemoveListener(LeftArrowClicked);
			refreshFromIndexArgsSignal.RemoveListener(RefreshFromIndexArgsCallback);
			setHUDTokenAmountSignal.RemoveListener(base.view.SetUpgradeTokenAmount);
			base.view.CloseButton.ClickedSignal.RemoveListener(Close);
		}

		protected override void Close()
		{
			soundFXSignal.Dispatch("Play_menu_disappear_01");
			base.view.Close();
			base.view.CleanupMinion();
		}

		private void RefreshFromIndexArgsCallback(global::System.Type type, int index, global::Kampai.UI.View.GUIArguments args)
		{
			if (type == typeof(global::Kampai.UI.View.MinionLevelSelectorView) && base.view.currentMinionLevelSelected != index)
			{
				LevelSelected(index);
			}
		}

		private void LevelSelected(int levelIndex)
		{
			base.view.LevelSelected(levelIndex);
		}

		private void RightArrowClicked()
		{
			base.view.IncrementIndex();
		}

		private void LeftArrowClicked()
		{
			base.view.DecrementIndex();
		}

		private void UpgradeClicked()
		{
			soundFXSignal.Dispatch("Play_minionUpgrade_LevelUp_01");
			uint tokensForCurrentMinion = base.view.GetTokensForCurrentMinion();
			gameContext.injectionBinder.GetInstance<global::Kampai.Game.MinionUpgradeSignal>().Dispatch(base.view.GetCurrentMinionID(), tokensForCurrentMinion);
			playerService.AlterQuantity(global::Kampai.Game.StaticItem.MINION_LEVEL_TOKEN, -base.view.tokensToLevel);
			setHUDTokenAmountSignal.Dispatch();
			base.view.SetUpgradeTokenAmount();
			base.view.LevelMinion(base.view.currentMinionLevelSelected + 1);
		}

		private void RushClicked()
		{
			playerService.ProcessRush(base.view.rushCost, true, RushTransactionCallback, base.view.GetCurrentMinionID());
		}

		private void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				soundFXSignal.Dispatch("Play_button_premium_01");
				setPremiumCurrencySignal.Dispatch();
				uint quantity = playerService.GetQuantity(global::Kampai.Game.StaticItem.MINION_LEVEL_TOKEN);
				uint tokensForCurrentMinion = base.view.GetTokensForCurrentMinion();
				uint amount = tokensForCurrentMinion - quantity;
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.MinionUpgradeSignal>().Dispatch(base.view.GetCurrentMinionID(), quantity);
				if (quantity != 0)
				{
					playerService.AlterQuantity(global::Kampai.Game.StaticItem.MINION_LEVEL_TOKEN, (int)(0 - quantity));
				}
				setHUDTokenAmountSignal.Dispatch();
				telemetryService.Send_Telemetry_EVT_IGE_RESOURCE_CRAFTABLE_EARNED((int)amount, "Minion Level Up Token", "Minion Token", "Rush", string.Empty, string.Empty);
				soundFXSignal.Dispatch("Play_minionUpgrade_LevelUp_01");
				base.view.SetUpgradeTokenAmount();
				base.view.LevelMinion(base.view.currentMinionLevelSelected + 1);
			}
		}

		private void OnMenuClose()
		{
			toggleCharacterAudioSignal.Dispatch(true, null);
			base.view.Release();
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_MinionUpgrade");
		}
	}
}
