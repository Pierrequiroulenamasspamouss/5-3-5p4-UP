namespace Kampai.UI.View
{
	public class LairEntranceWayfinderMediator : global::Kampai.UI.View.AbstractWayFinderMediator
	{
		private global::Kampai.Game.MasterPlanComponentTaskUpdatedSignal taskUpdatedSignal;

		private global::Kampai.Game.SetMasterPlanWayfinderIconToCompleteSignal setCompleteIconSignal;

		[Inject]
		public global::Kampai.UI.View.LairEntranceWayfinderView view { get; set; }

		[Inject(global::Kampai.Game.GameElement.CONTEXT)]
		public global::strange.extensions.context.api.ICrossContextCapable gameContext { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService planService { get; set; }

		[Inject]
		public global::Kampai.UI.View.StopEntranceWayfinderPulseSignal stopPulseSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ResetLairWayfinderIconSignal resetIconSignal { get; set; }

		[Inject]
		public global::Kampai.Common.PickControllerModel pickControllerModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.MoveBuildMenuSignal moveBuildMenuSignal { get; set; }

		public override global::Kampai.UI.View.IWayFinderView View
		{
			get
			{
				return view;
			}
		}

		public override void OnRegister()
		{
			base.OnRegister();
			taskUpdatedSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.MasterPlanComponentTaskUpdatedSignal>();
			setCompleteIconSignal = gameContext.injectionBinder.GetInstance<global::Kampai.Game.SetMasterPlanWayfinderIconToCompleteSignal>();
			taskUpdatedSignal.AddListener(view.TaskUpdated);
			setCompleteIconSignal.AddListener(view.SetBuildReadyIcon);
			resetIconSignal.AddListener(view.ResetDefaultIcon);
			stopPulseSignal.AddListener(view.StopPulse);
			global::Kampai.Game.VillainLair firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.VillainLair>(3137);
			planService.SetWayfinderState();
			if (firstInstanceByDefinitionId != null && !firstInstanceByDefinitionId.hasVisited)
			{
				view.StartPulse();
			}
		}

		public override void OnRemove()
		{
			base.OnRemove();
			taskUpdatedSignal.RemoveListener(view.TaskUpdated);
			taskUpdatedSignal = null;
			setCompleteIconSignal.RemoveListener(view.SetBuildReadyIcon);
			setCompleteIconSignal = null;
			resetIconSignal.RemoveListener(view.ResetDefaultIcon);
			stopPulseSignal.RemoveListener(view.StopPulse);
		}

		protected override void GoToClicked()
		{
			if (!pickControllerModel.SelectedBuilding.HasValue)
			{
				moveBuildMenuSignal.Dispatch(false);
				base.GoToClicked();
			}
		}
	}
}
