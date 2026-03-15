namespace Kampai.UI.View
{
	public class VillainLairPortalEntryMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.VillainLairPortalEntryView>
	{
		private global::Kampai.Game.VillainLairEntranceBuilding thisPortal;

		[Inject]
		public global::Kampai.Game.EnterVillainLairSignal enterVillainIslandSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairAssetsLoadedSignal villainLairAssetsLoadedSignal { get; set; }

		public override void OnRegister()
		{
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.enterLair.ClickedSignal.AddListener(EnterLair);
			villainLairAssetsLoadedSignal.AddListener(SetEnterButtonActive);
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.enterLair.ClickedSignal.RemoveListener(EnterLair);
			villainLairAssetsLoadedSignal.RemoveListener(SetEnterButtonActive);
			base.OnRemove();
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			thisPortal = args.Get<global::Kampai.Game.VillainLairEntranceBuilding>();
			global::Kampai.UI.BuildingPopupPositionData buildingPopupPositionData = args.Get<global::Kampai.UI.BuildingPopupPositionData>();
			base.view.InitProgrammatic(buildingPopupPositionData);
			SetEnterButtonActive(villainLairModel.areLairAssetsLoaded);
			base.view.Open();
		}

		private void SetEnterButtonActive(bool active)
		{
			base.view.enterLair.GetComponent<global::UnityEngine.UI.Button>().interactable = active;
		}

		private void EnterLair()
		{
			Close();
			enterVillainIslandSignal.Dispatch(thisPortal.VillainLairInstanceID, false);
		}

		protected override void Close()
		{
			base.view.Close();
		}

		private void OnMenuClose()
		{
			hideSignal.Dispatch("VillainLairPortalSkrim");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_EnterLair");
		}
	}
}
