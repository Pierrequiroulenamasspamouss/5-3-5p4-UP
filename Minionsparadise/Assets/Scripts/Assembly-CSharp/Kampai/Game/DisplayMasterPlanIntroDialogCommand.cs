namespace Kampai.Game
{
	public class DisplayMasterPlanIntroDialogCommand : global::strange.extensions.command.impl.Command
	{
		private bool firstTimeInLair;

		[Inject]
		public global::Kampai.Game.VillainLairModel model { get; set; }

		[Inject]
		public global::Kampai.Game.SetVillainLairAnimationTriggerSignal setAnimTriggerSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.PromptReceivedSignal promptReceivedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayVolcanoLairVillainWayfinderSignal displayVolcanoWayfinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.HideFluxWayfinder hideFluxWayfinderSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.EnableVillainLairHudSignal enableVillainHudSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraMoveToCustomPositionSignal cameraMoveToCustomPositionSignal { get; set; }

		[Inject]
		public global::Kampai.Game.EnableAllVillainLairCollidersSignal enableAllLairCollidersSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		[Inject]
		public global::Kampai.Game.DetermineLairUISignal determineLairUISignal { get; set; }

		public override void Execute()
		{
			firstTimeInLair = !model.currentActiveLair.hasVisited;
			if (firstTimeInLair)
			{
				model.currentActiveLair.hasVisited = true;
				enableVillainHudSignal.Dispatch(true);
				cameraMoveToCustomPositionSignal.Dispatch(60017, new global::Kampai.Util.Boxed<global::System.Action>(DisplayDialog));
			}
			else
			{
				DisplayDialog();
			}
			setAnimTriggerSignal.Dispatch("OnLoop");
		}

		private void DisplayDialog()
		{
			global::Kampai.Game.QuestDialogSetting questDialogSetting = new global::Kampai.Game.QuestDialogSetting();
			questDialogSetting.type = global::Kampai.UI.View.QuestDialogType.NORMAL;
			global::Kampai.Game.QuestDialogSetting questDialogSetting2 = questDialogSetting;
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			questDialogSetting2.additionalStringParameter = currentMasterPlan.Definition.LocalizedKey;
			global::Kampai.Util.Tuple<int, int> type = new global::Kampai.Util.Tuple<int, int>(-1, -1);
			base.injectionBinder.GetInstance<global::Kampai.Game.ShowDialogSignal>().Dispatch(currentMasterPlan.Definition.IntroDialog, questDialogSetting2, type);
			masterPlanService.CurrentMasterPlan.introHasBeenDisplayed = true;
			promptReceivedSignal.AddOnce(HandlePrompt);
		}

		private void HandlePrompt(int questId, int stepId)
		{
			setAnimTriggerSignal.Dispatch("OnStop");
			displayVolcanoWayfinderSignal.Dispatch();
			hideFluxWayfinderSignal.Dispatch(false);
			enableAllLairCollidersSignal.Dispatch(true);
			if (!firstTimeInLair)
			{
				determineLairUISignal.Dispatch();
			}
		}
	}
}
