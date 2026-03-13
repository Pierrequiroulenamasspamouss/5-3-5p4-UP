namespace Kampai.UI.View
{
	public class VillainLairPortalResourcesMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.VillainLairPortalResourcesView>
	{
		private int lairDefinitionID;

		private global::Kampai.Game.VillainLair currentLair;

		private global::Kampai.Game.VillainLairEntranceBuilding thisPortal;

		private global::System.Collections.Generic.List<global::Kampai.Game.VillainLairResourcePlot> resourcePlots;

		private global::Kampai.UI.View.ModalSettings modalSettings = new global::Kampai.UI.View.ModalSettings();

		private global::Kampai.Game.EndPartyBuffTimerSignal endPartyBuffTimerSignal;

		[Inject]
		public global::Kampai.Game.EnterVillainLairSignal enterVillainLairSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService localizationService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeOtherMenusSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IGuestOfHonorService guestService { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateSliderSignal updateSliderSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.UpdateVillainLairMenuViewSignal updateVillainLairMenuViewSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayDisco3DElements displayDisco3DElements { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairAssetsLoadedSignal villainLairAssetsLoadedSignal { get; set; }

		public override void OnRegister()
		{
			villainLairModel.isPortalResourceModalOpen = true;
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.enterLair.ClickedSignal.AddListener(EnterLair);
			villainLairAssetsLoadedSignal.AddListener(SetEnterLairButtonActive);
			updateSliderSignal.AddListener(UpdateDisplay);
			updateVillainLairMenuViewSignal.AddListener(ResetSlotStates);
			endPartyBuffTimerSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.EndPartyBuffTimerSignal>();
			endPartyBuffTimerSignal.AddListener(MinionPartyEnded);
			base.OnRegister();
		}

		public override void OnRemove()
		{
			villainLairModel.isPortalResourceModalOpen = false;
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.enterLair.ClickedSignal.RemoveListener(EnterLair);
			villainLairAssetsLoadedSignal.RemoveListener(SetEnterLairButtonActive);
			updateSliderSignal.RemoveListener(UpdateDisplay);
			updateVillainLairMenuViewSignal.RemoveListener(ResetSlotStates);
			endPartyBuffTimerSignal.RemoveListener(MinionPartyEnded);
			base.OnRemove();
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			lairDefinitionID = args.Get<int>();
			thisPortal = args.Get<global::Kampai.Game.VillainLairEntranceBuilding>();
			global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData = args.Get<global::Kampai.UI.BuildingPopupPositionData>();
			currentLair = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.VillainLair>(lairDefinitionID);
			Initialize(buildingPopupPositionData);
		}

		private void Initialize(global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData)
		{
			resourcePlots = new global::System.Collections.Generic.List<global::Kampai.Game.VillainLairResourcePlot>();
			for (int i = 0; i < currentLair.resourcePlotInstanceIDs.Count; i++)
			{
				int id = currentLair.resourcePlotInstanceIDs[i];
				global::Kampai.Game.VillainLairResourcePlot byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.VillainLairResourcePlot>(id);
				resourcePlots.Add(byInstanceId);
			}
			resourcePlots.Sort(delegate(global::Kampai.Game.VillainLairResourcePlot a, global::Kampai.Game.VillainLairResourcePlot b)
			{
				int num = a.State.CompareTo(b.State);
				if (a.State != global::Kampai.Game.BuildingState.Inaccessible && b.State != global::Kampai.Game.BuildingState.Inaccessible)
				{
					if (a.State == global::Kampai.Game.BuildingState.Idle)
					{
						return 1;
					}
					if (b.State == global::Kampai.Game.BuildingState.Idle)
					{
						return -1;
					}
					if (a.State == global::Kampai.Game.BuildingState.Working)
					{
						return 1;
					}
					if (b.State == global::Kampai.Game.BuildingState.Working)
					{
						return -1;
					}
				}
				return (num == 0) ? a.ID.CompareTo(b.ID) : num;
			});
			modalSettings.enableRushButtons = true;
			modalSettings.enableHarvestButtons = true;
			modalSettings.enableCallButtons = true;
			modalSettings.enableLockedButtons = true;
			modalSettings.enableRushThrob = false;
			modalSettings.enableCallThrob = false;
			modalSettings.enableLockedThrob = false;
			closeOtherMenusSignal.Dispatch(base.gameObject);
			base.view.Init(currentLair, resourcePlots, localizationService, definitionService, playerService, modalSettings, buildingPopupPositionData);
			SetEnterLairButtonActive(villainLairModel.areLairAssetsLoaded);
			CheckPartyState();
		}

		private void SetEnterLairButtonActive(bool active)
		{
			base.view.SetEnterLairButtonActive(active);
		}

		private void EnterLair()
		{
			Close();
			enterVillainLairSignal.Dispatch(thisPortal.VillainLairInstanceID, false);
			displayDisco3DElements.Dispatch(false);
		}

		protected override void Close()
		{
			base.view.Close();
		}

		private void OnMenuClose()
		{
			villainLairModel.isPortalResourceModalOpen = false;
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_Resource_LairPortal");
			hideSignal.Dispatch("VillainLairPortalSkrim");
		}

		private void MinionPartyEnded(int duration)
		{
			if (base.view != null)
			{
				base.view.SetPartyInfo(1f, string.Empty, false);
			}
		}

		private void CheckPartyState()
		{
			float currentBuffMultiplierForBuffType = guestService.GetCurrentBuffMultiplierForBuffType(global::Kampai.Game.BuffType.PRODUCTION);
			base.view.SetPartyInfo(currentBuffMultiplierForBuffType, localizationService.GetString("partyBuffMultiplier", currentBuffMultiplierForBuffType), playerService.GetMinionPartyInstance().IsBuffHappening);
		}

		private void UpdateDisplay()
		{
			if (thisPortal != null)
			{
				base.view.UpdateDisplay();
			}
		}

		private void ResetSlotStates()
		{
			if (thisPortal != null)
			{
				base.view.SetupModalInfo(resourcePlots, modalSettings);
			}
		}
	}
}
