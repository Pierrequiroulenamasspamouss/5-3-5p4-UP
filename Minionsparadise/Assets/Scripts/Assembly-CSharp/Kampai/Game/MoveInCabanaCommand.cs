namespace Kampai.Game
{
	public class MoveInCabanaCommand : global::strange.extensions.command.impl.Command
	{
		private int villainId;

		private global::Kampai.Game.CabanaBuilding cabana;

		private global::Kampai.Game.TikiBarBuilding tikiBar;

		[Inject]
		public global::Kampai.Game.Prestige prestige { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.UI.View.PromptReceivedSignal receivedSignal { get; set; }

		[Inject]
		public global::Kampai.Game.CameraAutoMoveToInstanceSignal cameraMoveSignal { get; set; }

		[Inject]
		public global::Kampai.Game.KevinGreetVillainSignal kevinGreetSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainPlayWelcomeSignal villainPlayWelcomeSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainGotoCabanaSignal villainGotoCabanaSignal { get; set; }

		[Inject]
		public global::Kampai.Game.ShowDialogSignal showDialogSignal { get; set; }

		[Inject]
		public global::Kampai.Game.BuildingChangeStateSignal buildingStateSignal { get; set; }

		[Inject]
		public global::Kampai.Common.RecreateBuildingSignal recreateSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IPrestigeService prestigeService { get; set; }

		[Inject]
		public global::Kampai.Game.AddMinionToTikiBarSignal tikiSignal { get; set; }

		[Inject]
		public global::Kampai.Game.VillainGotoCarpetSignal gotoCarpetSignal { get; set; }

		public override void Execute()
		{
			int trackedInstanceId = prestige.trackedInstanceId;
			global::Kampai.Game.Villain byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Villain>(trackedInstanceId);
			global::Kampai.Game.CabanaBuilding emptyCabana = prestigeService.GetEmptyCabana();
			if (emptyCabana != null)
			{
				MoveIn(emptyCabana, byInstanceId);
			}
		}

		private void MoveIn(global::Kampai.Game.CabanaBuilding building, global::Kampai.Game.Villain villain)
		{
			buildingStateSignal.Dispatch(building.ID, global::Kampai.Game.BuildingState.Working);
			recreateSignal.Dispatch(building);
			villain.CabanaBuildingId = building.ID;
			building.Occupied = true;
			gotoCarpetSignal.Dispatch(villain.ID);
			RunWelcomeFlow(villain, building);
		}

		private void RunWelcomeFlow(global::Kampai.Game.Villain villain, global::Kampai.Game.CabanaBuilding building)
		{
			kevinGreetSignal.Dispatch(true);
			villainPlayWelcomeSignal.Dispatch(villain.ID);
			global::Kampai.Game.QuestDialogSetting questDialogSetting = new global::Kampai.Game.QuestDialogSetting();
			questDialogSetting.definitionID = prestige.Definition.ID;
			questDialogSetting.type = global::Kampai.UI.View.QuestDialogType.NORMAL;
			global::Kampai.Game.QuestDialogSetting type = questDialogSetting;
			villainId = villain.ID;
			cabana = building;
			receivedSignal.AddOnce(HandleReceived);
			showDialogSignal.Dispatch(villain.Definition.WelcomeDialogKey, type, new global::Kampai.Util.Tuple<int, int>(-1, -1));
		}

		private void HandleReceived(int questId, int stepId)
		{
			kevinGreetSignal.Dispatch(false);
			villainGotoCabanaSignal.Dispatch(villainId, cabana.ID);
			global::Kampai.Game.PanInstructions panInstructions = new global::Kampai.Game.PanInstructions(cabana.ID);
			panInstructions.ZoomDistance = new global::Kampai.Util.Boxed<float>(0.4f);
			cameraMoveSignal.Dispatch(panInstructions, new global::Kampai.Util.Boxed<global::Kampai.Game.ScreenPosition>(new global::Kampai.Game.ScreenPosition()));
			TeleportToTikiBar();
		}

		private void TeleportToTikiBar()
		{
			global::Kampai.Game.Prestige firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(40004);
			if (firstInstanceByDefinitionId.state != global::Kampai.Game.PrestigeState.Questing)
			{
				return;
			}
			global::Kampai.Game.KevinCharacter firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.KevinCharacter>(70003);
			global::System.Collections.Generic.IList<global::Kampai.Game.Instance> instancesByDefinition = playerService.GetInstancesByDefinition<global::Kampai.Game.TikiBarBuildingDefinition>();
			if (instancesByDefinition != null && instancesByDefinition.Count != 0)
			{
				tikiBar = instancesByDefinition[0] as global::Kampai.Game.TikiBarBuilding;
				int minionSlotIndex = tikiBar.GetMinionSlotIndex(firstInstanceByDefinitionId.Definition.ID);
				if (minionSlotIndex != -1)
				{
					tikiSignal.Dispatch(tikiBar, firstInstanceByDefinitionId2, firstInstanceByDefinitionId, minionSlotIndex);
				}
			}
		}
	}
}
