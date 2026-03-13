namespace Kampai.Game.View
{
	public class ExitVillainLairCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("ExitVillainLairCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Util.Boxed<global::System.Action> callback { get; set; }

		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.VillainLairModel villainLairModel { get; set; }

		[Inject]
		public global::Kampai.UI.View.EnableVillainLairHudSignal enableVillainHudSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraMoveToCustomPositionSignal customCameraPositionSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.FadeBlackSignal fadeBlackSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowAllWayFindersSignal showAllWayFindersSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllOtherMenuSignal closeUISignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayVillainSignal displayVillainSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.CloseAllMessageDialogs closeAllDialogsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayMasterPlanCooldownAlertSignal displayCooldownAlertSignal { get; set; }

		[Inject]
		public global::Kampai.UI.IGhostComponentService ghostService { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideFluxWayfinder hideFluxWayfinder { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		public override void Execute()
		{
			villainLairModel.leavingLair = true;
			closeAllDialogsSignal.Dispatch();
			global::Kampai.UI.View.IGUICommand command = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.LoadStatic, "FadeBlack");
			global::UnityEngine.GameObject gameObject = guiService.Execute(command);
			if (gameObject == null)
			{
				logger.Warning("Trying to exit villain lair, but fade black prefab was not loaded");
			}
			closeUISignal.Dispatch(null);
			global::System.Collections.Generic.IList<global::System.Action> list = new global::System.Collections.Generic.List<global::System.Action>();
			list.Add(FadeInCallback);
			fadeBlackSignal.Dispatch(true, list);
		}

		private void FadeInCallback()
		{
			if (villainLairModel.currentActiveLair == null)
			{
				logger.Fatal(global::Kampai.Util.FatalCode.CMD_EXIT_VILLAIN_LAIR_NULL_REFERENCE);
			}
			villainLairModel.currentActiveLair = null;
			villainLairModel.leavingLair = false;
			enableVillainHudSignal.Dispatch(false);
			showAllWayFindersSignal.Dispatch();
			showHUDSignal.Dispatch(true);
			customCameraPositionSignal.Dispatch(60011, new global::Kampai.Util.Boxed<global::System.Action>(CameraCallback));
			hideFluxWayfinder.Dispatch(true);
			ghostService.ClearGhostComponentBuildings(true, true);
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			if (currentMasterPlan != null)
			{
				displayVillainSignal.Dispatch(currentMasterPlan.Definition.VillainCharacterDefID, false);
			}
		}

		private void CameraCallback()
		{
			global::System.Collections.Generic.IList<global::System.Action> list = new global::System.Collections.Generic.List<global::System.Action>();
			list.Add(callback.Value);
			list.Add(FadeOutCallback);
			fadeBlackSignal.Dispatch(false, list);
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			if (currentMasterPlan != null && currentMasterPlan.displayCooldownAlert)
			{
				displayCooldownAlertSignal.Dispatch(currentMasterPlan);
			}
		}

		private void FadeOutCallback()
		{
			foreach (global::UnityEngine.GameObject value in villainLairModel.villainLairInstances.Values)
			{
				value.SetActive(false);
			}
		}
	}
}
