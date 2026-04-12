namespace Kampai.UI.View
{
	internal sealed class MasterPlanRewardMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MasterPlanRewardView>
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MasterPlanRewardMediator") as global::Kampai.Util.IKampaiLogger;

		private global::Kampai.Game.Transaction.TransactionDefinition reward;

		private int masterPlanInstanceID;

		private bool collected;

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService fancyUIService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal tweenSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DoobersFlownSignal doobersFlownSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.EnableBuildMenuFromLairSignal setBuildMenuEnabledSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisableBuildMenuButtonSignal disableBuildMenuSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Game.GenerateNewMasterPlanSignal generateNewMasterPlanSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			soundFXSignal.Dispatch("Play_completeQuest_01");
			base.view.collectButton.ClickedSignal.AddListener(CollectButton);
			base.view.OnMenuClose.AddListener(CloseAnimationComplete);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			soundFXSignal.Dispatch("Play_menu_disappear_01");
			reward = null;
			base.view.collectButton.ClickedSignal.RemoveListener(CollectButton);
			base.view.OnMenuClose.RemoveListener(CloseAnimationComplete);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			reward = args.Get<global::Kampai.Game.Transaction.TransactionDefinition>();
			masterPlanInstanceID = args.Get<int>();
			soundFXSignal.Dispatch("Play_menu_popUp_01");
			base.view.Init(reward, localService, definitionService, playerService, fancyUIService, guiService, masterPlanInstanceID);
		}

		protected override void Close()
		{
			CollectButton();
		}

		public void CollectButton()
		{
			if (collected)
			{
				return;
			}
			collected = true;
			global::UnityEngine.UI.Button component = base.view.collectButton.GetComponent<global::UnityEngine.UI.Button>();
			component.interactable = false;
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			currentMasterPlan.displayCooldownReward = false;
			if (global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputItem(reward, 0).ID == currentMasterPlan.Definition.LeavebehindBuildingDefID)
			{
				setBuildMenuEnabledSignal.Dispatch(true);
				disableBuildMenuSignal.Dispatch(true);
			}
			if (reward != null)
			{
				if (masterPlanInstanceID != 0)
				{
					playerService.AlterQuantity(global::Kampai.Game.StaticItem.MASTER_PLAN_COMPLETION_COUNT, 1);
					currentMasterPlan.completionCount++;
				}
				playerService.RunEntireTransaction(reward, global::Kampai.Game.TransactionTarget.AUTOMATIC, CollectTransactionCallback);
			}
			else
			{
				logger.Info("Reward is null, nothing to do.");
			}
		}

		public void CollectTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (!pct.Success)
			{
				logger.Error("CollectTransactionCallback PendingCurrencyTransaction was a failure.");
			}
			generateNewMasterPlanSignal.Dispatch(new global::Kampai.Util.Boxed<global::System.Action>(null));
			DooberUtil.CheckForTween(base.view.transactionDefinition, base.view.viewList, true, uiCamera, tweenSignal, definitionService);
			doobersFlownSignal.AddOnce(delegate
			{
				if (villainLairModel.currentActiveLair != null)
				{
					setBuildMenuEnabledSignal.Dispatch(false);
				}
				disableBuildMenuSignal.Dispatch(false);
			});
			CloseMenu();
		}

		private void CloseMenu()
		{
			base.view.Close();
		}

		public void CloseAnimationComplete()
		{
			hideSignal.Dispatch("MasterPlan");
			if (masterPlanInstanceID == 0)
			{
				guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_MasterPlanReward");
			}
			else
			{
				guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_MasterPlanCooldownReward");
			}
		}
	}
}
