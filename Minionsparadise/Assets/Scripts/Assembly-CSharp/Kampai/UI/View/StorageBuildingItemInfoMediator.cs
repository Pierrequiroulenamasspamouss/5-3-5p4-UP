namespace Kampai.UI.View
{
	public class StorageBuildingItemInfoMediator : global::Kampai.UI.View.KampaiMediator
	{
		[Inject]
		public global::Kampai.UI.View.StorageBuildingItemInfoView view { get; set; }

		[Inject]
		public global::Kampai.Main.ILocalizationService LocalizationService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RemoveStorageBuildingItemDescriptionSignal removeItemDescriptionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject(global::Kampai.UI.View.UIElement.CAMERA)]
		public global::UnityEngine.Camera uiCamera { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.GoToResourceButtonClickedSignal gotoSignal { get; set; }

		public override void OnRegister()
		{
			base.OnRegister();
			removeItemDescriptionSignal.AddListener(Close);
			view.OnMenuClose.AddListener(OnMenuClose);
			view.gotoButton.ClickedSignal.AddListener(GotoPressed);
			view.Init(LocalizationService, uiCamera);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			view.gotoButton.ClickedSignal.RemoveListener(GotoPressed);
			removeItemDescriptionSignal.RemoveListener(Close);
			view.OnMenuClose.RemoveListener(OnMenuClose);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.Game.ItemDefinition itemDefinition = args.Get<global::Kampai.Game.ItemDefinition>();
			global::UnityEngine.RectTransform itemCenter = args.Get<global::UnityEngine.RectTransform>();
			global::UnityEngine.Vector3 center = args.Get<global::UnityEngine.Vector3>();
			view.SetItem(itemDefinition, itemCenter, center, definitionService);
		}

		private void Close()
		{
			soundFXSignal.Dispatch("Play_menu_disappear_01");
			view.Close();
		}

		private void OnMenuClose()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		private void GotoPressed()
		{
			Close();
			gotoSignal.Dispatch(view.ItemDefinition.ID);
		}
	}
}
