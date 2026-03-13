namespace Kampai.UI.View
{
	public class RewardedAdPickRewardModalMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.RewardedAdPickRewardModalView>
	{
		private const float DOOBER_ANIMATION_TIME_SEC = 1f;

		private string prefabName;

		private global::Kampai.Game.AdPlacementInstance adPlacementInstance;

		private global::Kampai.Util.QuantityItem[] items;

		private int selectedIndex = -1;

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IRewardedAdService rewardedAdService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberSignal spawnDooberSignal { get; set; }

		[Inject(global::Kampai.Main.MainElement.UI_DOOBER_CANVAS)]
		public global::UnityEngine.GameObject dooberCanvas { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.UI.View.SpawnDooberModel spawnDooberModel { get; set; }

		[Inject]
		public global::Kampai.Util.IRoutineRunner routineRunner { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UIModel uiModel { get; set; }

		public override void OnRegister()
		{
			base.closeAllOtherMenuSignal.Dispatch(null);
			base.OnRegister();
			base.view.Init(localService, definitionService, randomService);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.CollectButton.onClick.AddListener(OnCollect);
			int num = 0;
			global::UnityEngine.UI.Button[] boxes = base.view.Boxes;
			foreach (global::UnityEngine.UI.Button button in boxes)
			{
				int index_ = num++;
				button.onClick.AddListener(delegate
				{
					OnBox(index_);
				});
			}
		}

		public override void OnRemove()
		{
			base.OnRemove();
			CleanupListeners();
		}

		private void CleanupListeners()
		{
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.CollectButton.onClick.RemoveListener(OnCollect);
			global::UnityEngine.UI.Button[] boxes = base.view.Boxes;
			foreach (global::UnityEngine.UI.Button button in boxes)
			{
				button.onClick.RemoveAllListeners();
			}
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			prefabName = args.Get<string>();
			items = args.Get<global::Kampai.Util.QuantityItem[]>();
			adPlacementInstance = args.Get<global::Kampai.Game.AdPlacementInstance>();
			uiModel.DisableBack = true;
		}

		protected override void Close()
		{
			uiModel.DisableBack = false;
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			base.view.Close();
		}

		private void OnMenuClose()
		{
			hideSkrimSignal.Dispatch("RewardedAdPickReward");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, prefabName);
		}

		private void OnCollect()
		{
			if (!base.view.IsAnimationPlaying("Close"))
			{
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
				transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem> { items[0] };
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition2 = transactionDefinition;
				rewardedAdService.RewardPlayer(transactionDefinition2, adPlacementInstance);
				telemetryService.Send_Telemetry_EVT_AD_INTERACTION(adPlacementInstance.Definition.Name, transactionDefinition2.Outputs, adPlacementInstance.RewardPerPeriodCount);
				global::Kampai.Game.ItemDefinition itemDefinition;
				int rewardAmount;
				if (global::Kampai.Game.RewardedAdUtil.GetFirstItemDefintion(transactionDefinition2.Outputs, out itemDefinition, out rewardAmount, definitionService))
				{
					SpawnRewardDoober(transactionDefinition2, itemDefinition, rewardAmount);
				}
				base.view.Close();
			}
		}

		private void OnBox(int index)
		{
			if (selectedIndex == -1)
			{
				selectedIndex = index;
				base.view.Select(selectedIndex, items);
			}
		}

		private void SpawnRewardDoober(global::Kampai.Game.Transaction.TransactionDefinition rewardTransaction, global::Kampai.Game.ItemDefinition rewardItem, int rewardAmount)
		{
			global::UnityEngine.GameObject original = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.GameObject>("rewardedAdReward");
			global::UnityEngine.GameObject gameObject = global::UnityEngine.Object.Instantiate(original);
			gameObject.transform.SetParent(dooberCanvas.transform, false);
			global::UnityEngine.Transform parent = base.view.Boxes[selectedIndex].transform.parent;
			spawnDooberModel.rewardedAdDooberSpawnLocation = parent.position;
			gameObject.transform.position = parent.position;
			global::Kampai.UI.View.RewardedAdRewardView component = gameObject.GetComponent<global::Kampai.UI.View.RewardedAdRewardView>();
			component.Init(rewardItem, rewardAmount);
			routineRunner.StartCoroutine(ProcessDoobers(component, rewardTransaction));
		}

		private global::System.Collections.IEnumerator ProcessDoobers(global::Kampai.UI.View.RewardedAdRewardView view, global::Kampai.Game.Transaction.TransactionDefinition rewardTransaction)
		{
			yield return new global::UnityEngine.WaitForSeconds(1f);
			spawnDooberModel.RewardedAdDooberMode = true;
			DooberUtil.CheckForTween(rewardTransaction, new global::System.Collections.Generic.List<global::Kampai.UI.View.KampaiImage> { view.RewardItemImage }, true, uiCamera, spawnDooberSignal, definitionService);
			spawnDooberModel.RewardedAdDooberMode = false;
			yield return new global::UnityEngine.WaitForEndOfFrame();
			global::UnityEngine.Object.Destroy(view.gameObject);
		}
	}
}
