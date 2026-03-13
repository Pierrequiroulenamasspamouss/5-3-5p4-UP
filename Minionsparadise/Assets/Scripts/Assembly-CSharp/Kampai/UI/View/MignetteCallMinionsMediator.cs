namespace Kampai.UI.View
{
	public class MignetteCallMinionsMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MignetteCallMinionsView>
	{
		private global::Kampai.Game.MignetteBuilding mignetteBuilding;

		private bool isAutoPanning;

		private bool callMinionsOnPanComplete;

		private int currentIndex;

		private int mignetteBuildingCount;

		private int rushCost;

		private bool ownsMinigamePack;

		private global::System.Collections.IEnumerator updateTimeCoroutine;

		private global::Kampai.Game.CameraAutoPanCompleteSignal cameraAutoPanCompleteSignal;

		private global::System.Collections.Generic.List<global::Kampai.Game.MignetteBuilding> validMignetteBuildingList = new global::System.Collections.Generic.List<global::Kampai.Game.MignetteBuilding>();

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrim { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingStateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingCooldownCompleteSignal onCooldownCompleteSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ITimeEventService timeEventService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal playSFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowNeedXMinionsSignal ShowNeedXMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Main.MignetteCallMinionsSignal mignetteCallMinionsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MignetteCallMinionsModel model { get; set; }

		public override void OnRegister()
		{
			isAutoPanning = false;
			callMinionsOnPanComplete = false;
			base.OnRegister();
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.callMinionsButton.ClickedSignal.AddListener(OnCallMinions);
			base.view.modal.rushButton.ClickedSignal.AddListener(Rush);
			onCooldownCompleteSignal.AddListener(OnBuildingCooldownComplete);
			base.view.leftArrow.ClickedSignal.AddListener(MoveToPreviousBuilding);
			base.view.rightArrow.ClickedSignal.AddListener(MoveToNextBuilding);
			cameraAutoPanCompleteSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraAutoPanCompleteSignal>();
			cameraAutoPanCompleteSignal.AddListener(PanComplete);
			ownsMinigamePack = playerService.HasPurchasedMinigamePack();
			if (!ownsMinigamePack)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.IdleMinionSignal>().AddListener(base.view.UpdateView);
			}
		}

		public override void OnRemove()
		{
			base.OnRemove();
			StopCoroutine();
			Go.killAllTweensWithTarget(base.view.callMinionsButton.transform);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.callMinionsButton.ClickedSignal.RemoveListener(OnCallMinions);
			base.view.modal.rushButton.ClickedSignal.RemoveListener(Rush);
			onCooldownCompleteSignal.RemoveListener(OnBuildingCooldownComplete);
			base.view.leftArrow.ClickedSignal.RemoveListener(MoveToPreviousBuilding);
			base.view.rightArrow.ClickedSignal.RemoveListener(MoveToNextBuilding);
			cameraAutoPanCompleteSignal.RemoveListener(PanComplete);
			if (!ownsMinigamePack)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.IdleMinionSignal>().RemoveListener(base.view.UpdateView);
			}
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.Game.MignetteBuilding mignetteBuilding = args.Get<global::Kampai.Game.MignetteBuilding>();
			if (mignetteBuilding != null)
			{
				this.mignetteBuilding = mignetteBuilding;
				global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData = args.Get<global::Kampai.UI.BuildingPopupPositionData>();
				base.view.Init(mignetteBuilding, localService, playerService, gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.MINION_MANAGER), buildingPopupPositionData);
				InitMignetteBuildingList(this.mignetteBuilding);
				LoadMenu();
			}
		}

		private void LoadMenu()
		{
			if (mignetteBuilding.State == global::Kampai.Game.BuildingState.Cooldown)
			{
				base.view.StartTime(mignetteBuilding.StateStartTime, mignetteBuilding.StateStartTime + mignetteBuilding.GetCooldown());
				StopCoroutine();
				StartCoroutine();
			}
			else
			{
				StopCoroutine();
			}
		}

		private void StopCoroutine()
		{
			if (updateTimeCoroutine != null)
			{
				StopCoroutine(updateTimeCoroutine);
				updateTimeCoroutine = null;
			}
		}

		private void StartCoroutine()
		{
			updateTimeCoroutine = UpdateTime();
			StartCoroutine(updateTimeCoroutine);
		}

		private global::System.Collections.IEnumerator UpdateTime()
		{
			while (true)
			{
				int timeRemaining = timeEventService.GetTimeRemaining(mignetteBuilding.ID);
				base.view.UpdateTime(timeRemaining);
				rushCost = timeEventService.CalculateRushCostForTimer(timeRemaining, global::Kampai.Game.RushActionType.COOLDOWN);
				base.view.SetRushCost(rushCost);
				yield return new global::UnityEngine.WaitForSeconds(1f);
			}
		}

		private void Rush()
		{
			if (base.view.modal.rushButton.isDoubleConfirmed())
			{
				playerService.ProcessRush(rushCost, true, RushTransactionCallback, mignetteBuilding.ID);
			}
		}

		private void RushTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				timeEventService.RushEvent(mignetteBuilding.ID);
				playSFXSignal.Dispatch("Play_button_premium_01");
			}
		}

		private void OnBuildingCooldownComplete(int instanceId)
		{
			if (mignetteBuilding != null && instanceId == mignetteBuilding.ID)
			{
				buildingStateSignal.Dispatch(instanceId, global::Kampai.Game.BuildingState.Idle);
				base.view.RecreateModal(mignetteBuilding);
				LoadMenu();
			}
		}

		private void MoveToPreviousBuilding()
		{
			if (currentIndex <= 0)
			{
				currentIndex = mignetteBuildingCount - 1;
			}
			else
			{
				currentIndex--;
			}
			mignetteBuilding = validMignetteBuildingList[currentIndex];
			ReloadMenu();
		}

		private void MoveToNextBuilding()
		{
			if (currentIndex >= mignetteBuildingCount - 1)
			{
				currentIndex = 0;
			}
			else
			{
				currentIndex++;
			}
			mignetteBuilding = validMignetteBuildingList[currentIndex];
			ReloadMenu();
		}

		private void ReloadMenu()
		{
			base.view.SetArrowButtonsState(false);
			base.view.RecreateModal(mignetteBuilding);
			PanAndShowBuildingMenu(mignetteBuilding);
			LoadMenu();
		}

		private void PanComplete()
		{
			isAutoPanning = false;
			base.view.SetArrowButtonsState(true);
			if (callMinionsOnPanComplete)
			{
				callMinionsOnPanComplete = false;
				OnCallMinions();
			}
		}

		private void PanAndShowBuildingMenu(global::Kampai.Game.Building building)
		{
			global::strange.extensions.injector.api.ICrossContextInjectionBinder injectionBinder = gameContext.injectionBinder;
			global::UnityEngine.GameObject instance = injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER);
			global::Kampai.Game.View.BuildingManagerView component = instance.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(building.ID);
			global::UnityEngine.Vector3 position = buildingObject.transform.position;
			global::Kampai.Game.ScreenPosition screenPosition = building.Definition.ScreenPosition;
			isAutoPanning = true;
			injectionBinder.GetInstance<global::Kampai.Game.CameraAutoMoveSignal>().Dispatch(position, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(screenPosition), new global::Kampai.Game.CameraMovementSettings(global::Kampai.Game.CameraMovementSettings.Settings.KeepUIOpen, building, null), false);
			injectionBinder.GetInstance<global::Kampai.Game.ShowHiddenBuildingsSignal>().Dispatch();
		}

		private void InitMignetteBuildingList(global::Kampai.Game.MignetteBuilding currentMignetteBuilding)
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MignetteBuilding> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MignetteBuilding>();
			for (int i = 0; i < instancesByType.Count; i++)
			{
				global::Kampai.Game.MignetteBuilding item = instancesByType[i];
				validMignetteBuildingList.Add(item);
			}
			currentIndex = validMignetteBuildingList.IndexOf(currentMignetteBuilding);
			mignetteBuildingCount = validMignetteBuildingList.Count;
			if (mignetteBuildingCount <= 1)
			{
				base.view.SetArrowButtonsVisibleAndActive(false);
			}
		}

		private void OnCallMinions()
		{
			if (!ownsMinigamePack && !global::Kampai.Game.OpenBuildingMenuCommand.HasEnoughFreeMinionsToAssignToBuilding(playerService, mignetteBuilding))
			{
				ShowNeedXMinionsSignal.Dispatch(mignetteBuilding.GetMinionSlotsOwned());
				return;
			}
			if (isAutoPanning)
			{
				callMinionsOnPanComplete = true;
				return;
			}
			model.NumberOfMinionsToCall = mignetteBuilding.MinionSlotsOwned;
			model.Building = mignetteBuilding;
			model.SignalSender = base.view.gameObject;
			mignetteCallMinionsSignal.Dispatch();
			base.view.Close();
		}

		protected override void Close()
		{
			playSFXSignal.Dispatch("Play_menu_disappear_01");
			base.view.Close();
		}

		private void OnMenuClose()
		{
			hideSkrim.Dispatch("MignetteSkrim");
			string prefab = (ownsMinigamePack ? "MignetteCallMinionsMenu" : "MignetteCallMinionsRequiredMenu");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, prefab);
		}
	}
}
