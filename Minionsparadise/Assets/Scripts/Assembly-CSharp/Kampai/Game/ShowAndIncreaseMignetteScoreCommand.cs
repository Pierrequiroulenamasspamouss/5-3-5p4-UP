namespace Kampai.Game
{
	public class ShowAndIncreaseMignetteScoreCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.UI.View.IGUIService guiService { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.MignetteGameModel mignetteGameModel { get; set; }

		[Inject]
		public global::Kampai.Game.EjectAllMinionsFromBuildingSignal ejectAllMinionsFromBuildingSignal { get; set; }

		[Inject]
		public global::Kampai.Game.MignetteCollectionService collectionService { get; set; }

		[Inject]
		public global::Kampai.UI.View.ShowHUDSignal showHUDSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetHUDButtonsVisibleSignal setHUDButtonsVisibleSignal { get; set; }

		[Inject]
		public global::Kampai.Game.Mignette.DestroyMignetteContextSignal destroyMignetteContextSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IAchievementService achievementService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		public override void Execute()
		{
			collectionService.IncreaseScoreForMignetteCollection(mignetteGameModel.BuildingId, mignetteGameModel.CurrentGameScore);
			if (!playerService.HasPurchasedMinigamePack())
			{
				ejectAllMinionsFromBuildingSignal.Dispatch(mignetteGameModel.BuildingId);
			}
			destroyMignetteContextSignal.Dispatch();
			global::Kampai.Game.MignetteBuilding byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MignetteBuilding>(mignetteGameModel.BuildingId);
			if (byInstanceId != null)
			{
				achievementService.UpdateIncrementalAchievement(byInstanceId.MignetteBuildingDefinition.ID, 1);
				questService.UpdateAllQuestsWithQuestStepType(global::Kampai.Game.QuestStepType.Mignette, global::Kampai.Game.QuestTaskTransition.Complete, byInstanceId);
				UpdateMasterPlanComponentTasks(byInstanceId);
			}
			global::Kampai.UI.View.IGUICommand iGUICommand = guiService.BuildCommand(global::Kampai.UI.View.GUIOperation.Load, "MignetteScoreSummary");
			iGUICommand.Args.Add(true);
			iGUICommand.Args.Add(mignetteGameModel.BuildingId);
			iGUICommand.skrimScreen = "MignetteSkrim";
			iGUICommand.darkSkrim = true;
			iGUICommand.disableSkrimButton = true;
			guiService.Execute(iGUICommand);
			showHUDSignal.Dispatch(true);
			setHUDButtonsVisibleSignal.Dispatch(false);
		}

		private void UpdateMasterPlanComponentTasks(global::Kampai.Game.MignetteBuilding building)
		{
			uint currentGameScore = (uint)mignetteGameModel.CurrentGameScore;
			int iD = building.Definition.ID;
			uint progress = (uint)building.Definition.XPRewardFactor;
			masterPlanService.ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.PlayMiniGame, 1u, iD);
			masterPlanService.ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.MiniGameScore, currentGameScore, iD);
			masterPlanService.ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints, progress, iD);
			masterPlanService.ProcessActiveComponent(global::Kampai.Game.MasterPlanComponentTaskType.EarnMignettePartyPoints, progress, iD);
		}
	}
}
