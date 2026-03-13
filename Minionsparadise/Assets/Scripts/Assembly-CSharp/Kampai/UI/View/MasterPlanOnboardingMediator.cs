namespace Kampai.UI.View
{
	public class MasterPlanOnboardingMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MasterPlanOnboardingView>
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.DisplayMasterPlanOnboardingSignal displayOnboardSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanIntroDialogSignal introDialogSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IGhostComponentService ghostService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal soundFXSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		public override void OnRegister()
		{
			base.view.definitionService = definitionService;
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.actionButtonView.ClickedSignal.AddListener(Next);
		}

		public override void OnRemove()
		{
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.actionButtonView.ClickedSignal.RemoveListener(Next);
		}

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			base.view.definition = args.Get<global::Kampai.Game.MasterPlanOnboardDefinition>();
			base.view.Initialize();
			base.view.Init();
			soundFXSignal.Dispatch("Play_menu_popUp_01");
			base.view.Open();
		}

		protected override void Close()
		{
			hideSkrimSignal.Dispatch("MasterPlanOnboarding");
			base.view.Close();
		}

		private void OnMenuClose()
		{
			soundFXSignal.Dispatch("Play_menu_disappear_01");
			global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Unload, "screen_MasterPlanOnboarding");
			guiService.Execute(command);
			if (!base.view.IsLast)
			{
				displayOnboardSignal.Dispatch(base.view.definition.nextOnboardDefinitionId);
			}
		}

		private void Next()
		{
			if (!(base.view == null))
			{
				ghostService.RunEndGhostComponentFunctionFromDefinition(base.view.definition.ghostFunction.closeType);
				Close();
				if (base.view.IsLast)
				{
					introDialogSignal.Dispatch();
				}
			}
		}
	}
}
