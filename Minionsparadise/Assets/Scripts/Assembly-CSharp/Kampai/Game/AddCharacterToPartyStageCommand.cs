namespace Kampai.Game
{
	public class AddCharacterToPartyStageCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService defService { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject namedCharacterManager { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject]
		public global::Kampai.UI.IFancyUIService uiService { get; set; }

		private void HandleStuartOnStage()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.StuartCharacter> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.StuartCharacter>();
			if (instancesByType != null && instancesByType.Count > 0)
			{
				global::Kampai.Game.View.NamedCharacterManagerView component = namedCharacterManager.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>();
				global::Kampai.Game.View.StuartView stuartView = (global::Kampai.Game.View.StuartView)component.Get(instancesByType[0].ID);
				if (stuartView.IsOnStage())
				{
					stuartView.GetOnStageImmediate(false);
				}
			}
		}

		public override void Execute()
		{
			global::UnityEngine.GameObject gameObject = new global::UnityEngine.GameObject("StageCharacters");
			gameObject.transform.parent = contextView.transform;
			global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
			if (firstInstanceByDefinitionId == null || !firstInstanceByDefinitionId.IsBuildingRepaired())
			{
				return;
			}
			HandleStuartOnStage();
			global::Kampai.Game.MinionParty firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MinionParty>(80000);
			global::System.Collections.Generic.List<int> lastGuestsOfHonorPrestigeIDs = firstInstanceByDefinitionId2.lastGuestsOfHonorPrestigeIDs;
			foreach (int item in lastGuestsOfHonorPrestigeIDs)
			{
				if (item <= 0)
				{
					continue;
				}
				global::Kampai.Game.PrestigeDefinition prestigeDefinition = defService.Get<global::Kampai.Game.PrestigeDefinition>(item);
				global::Kampai.Game.Prestige firstInstanceByDefinitionId3 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(item);
				if (prestigeDefinition != null)
				{
					global::Kampai.Game.Character byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Character>(firstInstanceByDefinitionId3.trackedInstanceId);
					global::Kampai.Game.View.CharacterObject characterObject = null;
					if (byInstanceId is global::Kampai.Game.NamedCharacter)
					{
						global::Kampai.Game.View.NamedCharacterManagerView component = namedCharacterManager.GetComponent<global::Kampai.Game.View.NamedCharacterManagerView>();
						characterObject = component.Get(byInstanceId.ID);
					}
					else
					{
						global::Kampai.Game.View.MinionManagerView component2 = minionManager.GetComponent<global::Kampai.Game.View.MinionManagerView>();
						characterObject = component2.Get(byInstanceId.ID);
					}
					if (characterObject != null)
					{
						characterObject.transform.localScale = new global::UnityEngine.Vector3(0f, 0f, 0f);
					}
				}
				global::Kampai.UI.DummyCharacterType characterType = uiService.GetCharacterType(item);
				global::Kampai.Game.View.DummyCharacterObject dummyCharacterObject = uiService.CreateCharacter(characterType, global::Kampai.UI.DummyCharacterAnimationState.Happy, gameObject.transform, new global::UnityEngine.Vector3(1f, 1f, 1f), new global::UnityEngine.Vector3(0f, 0f, 0f), item);
				global::Kampai.Game.GuestOfHonorDefinition guestOfHonorDefinition = defService.Get<global::Kampai.Game.GuestOfHonorDefinition>(prestigeDefinition.GuestOfHonorDefinitionID);
				if (guestOfHonorDefinition != null && guestOfHonorDefinition.gohAnimationID > 0)
				{
					global::Kampai.Game.MinionAnimationDefinition minionAnimationDefinition = defService.Get<global::Kampai.Game.MinionAnimationDefinition>(guestOfHonorDefinition.gohAnimationID);
					global::UnityEngine.RuntimeAnimatorController animController = global::Kampai.Util.KampaiResources.Load<global::UnityEngine.RuntimeAnimatorController>(minionAnimationDefinition.StateMachine);
					dummyCharacterObject.SetAnimController(animController);
				}
				dummyCharacterObject.gameObject.SetLayerRecursively(0);
				moveCharacterToStage(dummyCharacterObject, firstInstanceByDefinitionId);
				break;
			}
		}

		private void moveCharacterToStage(global::Kampai.Game.View.DummyCharacterObject characterObject, global::Kampai.Game.StageBuilding stage)
		{
			global::Kampai.Game.View.BuildingManagerView component = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component.GetBuildingObject(stage.ID);
			global::Kampai.Game.View.StageBuildingObject stageBuildingObject = buildingObject as global::Kampai.Game.View.StageBuildingObject;
			if (stageBuildingObject != null)
			{
				global::UnityEngine.Transform stageTransform = stageBuildingObject.GetStageTransform();
				float num = 0.35f;
				global::UnityEngine.Vector3 position = new global::UnityEngine.Vector3(stageTransform.position.x, stageTransform.position.y + num, stageTransform.position.z);
				characterObject.transform.position = position;
				characterObject.transform.rotation = stageTransform.localRotation;
				stageBuildingObject.SetHideMic(true);
			}
		}
	}
}
