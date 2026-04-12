namespace Kampai.UI.View
{
	public class MasterPlanSelectComponentMediator : global::Kampai.UI.View.UIStackMediator<global::Kampai.UI.View.MasterPlanSelectComponentView>
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.RefreshFromIndexSignal refreshFromIndex { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.MasterPlanSelectComponentSignal selectComponentSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideFluxWayfinder hideFluxWayfinderSignal { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal sfxSignal { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.UI.View.MoveSkrimTopLayerSignal moveSkrimTopLayerSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideSkrimSignal hideSkrimSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IGhostComponentService ghostService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel lairModel { get; set; }

		public override void Initialize(global::Kampai.UI.View.GUIArguments args)
		{
			global::Kampai.Util.Tuple<int, int> tuple = args.Get<global::Kampai.Util.Tuple<int, int>>();
			int item = tuple.Item1;
			int item2 = tuple.Item2;
			int selectedIndex;
			if (GetActiveComponent(out selectedIndex) == null)
			{
				selectedIndex = -1;
			}
			hideFluxWayfinderSignal.Dispatch(true);
			base.view.Init(item, item2, playerService, definitionService, guiService, ghostService, masterPlanService);
			moveSkrimTopLayerSignal.Dispatch("MasterPlan");
		}

		private global::Kampai.Game.MasterPlanComponent GetActiveComponent(out int selectedIndex)
		{
			selectedIndex = 0;
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MasterPlanComponent>();
			for (int i = 0; i < instancesByType.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent masterPlanComponent = instancesByType[i];
				if (masterPlanComponent.State != global::Kampai.Game.MasterPlanComponentState.NotStarted && masterPlanComponent.State != global::Kampai.Game.MasterPlanComponentState.Scaffolding && masterPlanComponent.State != global::Kampai.Game.MasterPlanComponentState.Complete)
				{
					selectedIndex = i;
					return masterPlanComponent;
				}
			}
			return null;
		}

		public override void OnRegister()
		{
			base.OnRegister();
			base.view.updateSubViewSignal.AddListener(SignalViewUpdate);
			base.view.nextButtonView.ClickedSignal.AddListener(base.view.NextComponent);
			base.view.previousButtonView.ClickedSignal.AddListener(base.view.PreviousComponent);
			base.view.actionButtonView.ClickedSignal.AddListener(ComponentSelected);
			base.view.OnMenuClose.AddListener(OnMenuClose);
			base.view.PanWithinLairSignal.AddListener(PanCameraWithinLair);
		}

		public override void OnRemove()
		{
			base.OnRemove();
			base.view.updateSubViewSignal.RemoveListener(SignalViewUpdate);
			base.view.nextButtonView.ClickedSignal.RemoveListener(base.view.NextComponent);
			base.view.previousButtonView.ClickedSignal.RemoveListener(base.view.PreviousComponent);
			base.view.actionButtonView.ClickedSignal.RemoveListener(ComponentSelected);
			base.view.OnMenuClose.RemoveListener(OnMenuClose);
			base.view.PanWithinLairSignal.RemoveListener(PanCameraWithinLair);
		}

		private void SignalViewUpdate(global::System.Type type, int index)
		{
			refreshFromIndex.Dispatch(type, index);
		}

		private void ComponentSelected()
		{
			if (base.view.selectedIndex >= 0)
			{
				selectComponentSignal.Dispatch(base.view.planDefinition, base.view.selectedIndex, false);
				Close();
			}
		}

		private void OnMenuClose()
		{
			hideFluxWayfinderSignal.Dispatch(false);
			base.view.PanToMainLairView();
			hideSkrimSignal.Dispatch("MasterPlan");
			sfxSignal.Dispatch("Play_menu_disappear_01");
			guiService.Execute(global::Kampai.UI.View.GUIOperation.Unload, "screen_MasterPlanComponentSelection");
		}

		protected override void Close()
		{
			base.view.ReleaseViews();
			base.view.PanToMainLairView();
			base.view.Close();
		}

		private void PanCameraWithinLair(int cameraPos, global::Kampai.Util.Boxed<global::System.Action> callback)
		{
			if (lairModel.currentActiveLair != null)
			{
				gameContext.injectionBinder.GetInstance<global::Kampai.Game.CameraMoveToCustomPositionSignal>().Dispatch(cameraPos, callback);
			}
		}
	}
}
