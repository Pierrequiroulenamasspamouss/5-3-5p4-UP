namespace Kampai.UI.View
{
	public class MignettePlayConfirmMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MignettePlayConfirmView>
	{
		private global::Kampai.Game.MignetteBuilding mignetteBuilding;

		private global::Kampai.Game.View.MignetteBuildingObject mignetteBuildingObject;

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.EjectAllMinionsFromBuildingSignal ejectAllMinionsFromBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.StartMignetteSignal startMignetteSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowStoreSignal showStoreSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			closeSignal.Dispatch(base.view.gameObject);
			base.view.CloseButton.ClickedSignal.AddListener(OnCloseButtonClicked);
			base.view.PlayButton.ClickedSignal.AddListener(OnPlayButtonClicked);
			base.view.gameObject.SetActive(false);
			SetMainHUDVisible(false);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.Game.MignetteBuilding building = args.Get<global::Kampai.Game.MignetteBuilding>();
			Init(building);
		}

		private void SetMainHUDVisible(bool visible)
		{
			pickControllerModel.ForceDisabled = !visible;
			showHUDSignal.Dispatch(visible);
			showStoreSignal.Dispatch(visible);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.CloseButton.ClickedSignal.RemoveListener(OnCloseButtonClicked);
			base.view.PlayButton.ClickedSignal.RemoveListener(OnPlayButtonClicked);
		}

		protected override void Update()
		{
			if (mignetteBuildingObject != null)
			{
				if (mignetteBuildingObject.GetMignetteMinionCount() == mignetteBuilding.GetMinionSlotsOwned())
				{
					base.view.PlayButton.gameObject.SetActive(true);
				}
				else
				{
					base.view.PlayButton.gameObject.SetActive(false);
				}
			}
		}

		private void Init(global::Kampai.Game.MignetteBuilding building)
		{
			if (building != null)
			{
				mignetteBuilding = building;
				base.view.gameObject.SetActive(true);
				global::UnityEngine.GameObject instance = gameContext.injectionBinder.GetInstance<global::UnityEngine.GameObject>(global::Kampai.Game.GameElement.BUILDING_MANAGER);
				global::Kampai.Game.View.BuildingManagerMediator component = instance.GetComponent<global::Kampai.Game.View.BuildingManagerMediator>();
				mignetteBuildingObject = component.view.GetBuildingObject(mignetteBuilding.ID) as global::Kampai.Game.View.MignetteBuildingObject;
			}
		}

		private void CancelMignette()
		{
			ejectAllMinionsFromBuildingSignal.Dispatch(mignetteBuilding.ID);
			SetMainHUDVisible(true);
			HideConfirmUI();
		}

		private void HideConfirmUI()
		{
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "MignettePlayConfirmMenu");
		}

		protected override void Close()
		{
			CancelMignette();
		}

		private void OnCloseButtonClicked()
		{
			CancelMignette();
		}

		private void OnPlayButtonClicked()
		{
			if (mignetteBuildingObject.GetMignetteMinionCount() == mignetteBuilding.GetMinionSlotsOwned())
			{
				startMignetteSignal.Dispatch(mignetteBuilding.ID);
				HideConfirmUI();
			}
		}
	}
}
