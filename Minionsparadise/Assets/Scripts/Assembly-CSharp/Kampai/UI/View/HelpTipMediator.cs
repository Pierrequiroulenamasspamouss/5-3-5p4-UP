namespace Kampai.UI.View
{
	public class HelpTipMediator : global::Kampai.UI.View.AbstractGenericPopupMediator<global::Kampai.UI.View.HelpTipView>
	{
		[Inject(global::Kampai.Main.MainElement.UI_GLASSCANVAS)]
		public global::UnityEngine.GameObject glassCanvas { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::UnityEngine.RectTransform rectTransform = args.Get<global::UnityEngine.RectTransform>();
			if (rectTransform == null)
			{
				OnMenuClose();
				return;
			}
			base.soundFXSignal.Dispatch("Play_menu_popUp_02");
			global::Kampai.Game.HelpTipDefinition tip = args.Get<global::Kampai.Game.HelpTipDefinition>();
			global::UnityEngine.Vector3 itemCenter = args.Get<global::UnityEngine.Vector3>();
			base.view.Init(base.localizationService);
			base.view.SetTip(tip);
			base.view.SetUICanvas(glassCanvas);
			base.view.Display(itemCenter);
			base.view.gameObject.transform.parent = rectTransform.parent;
			base.view.gameObject.transform.SetAsLastSibling();
		}

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.OnMenuClose.AddListener(OnMenuClose);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
		}
	}
}
