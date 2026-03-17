namespace Kampai.Game
{
	public class RemoveCharacterFromPartyStageCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService defService { get; set; }

		[Inject(global::strange.extensions.context.api.ContextKeys.CONTEXT_VIEW)]
		public global::UnityEngine.GameObject contextView { get; set; }

		[Inject(global::Kampai.Game.GameElement.NAMED_CHARACTER_MANAGER)]
		public global::UnityEngine.GameObject namedCharacterManager { get; set; }

		[Inject(global::Kampai.Game.GameElement.MINION_MANAGER)]
		public global::UnityEngine.GameObject minionManager { get; set; }

		[Inject(global::Kampai.Game.GameElement.BUILDING_MANAGER)]
		public global::UnityEngine.GameObject buildingManager { get; set; }

		[Inject]
		public global::Kampai.Game.SocialEventAvailableSignal socialEventAvailableSignal { get; set; }

		public override void Execute()
		{
			global::UnityEngine.GameObject gameObject = contextView.FindChild("StageCharacters");
			if (gameObject != null)
			{
				global::UnityEngine.Object.Destroy(gameObject);
			}
			global::Kampai.Game.StageBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StageBuilding>(3054);
			if (firstInstanceByDefinitionId == null || !firstInstanceByDefinitionId.IsBuildingRepaired())
			{
				return;
			}
			global::Kampai.Game.MinionParty firstInstanceByDefinitionId2 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MinionParty>(80000);
			global::System.Collections.Generic.List<int> lastGuestsOfHonorPrestigeIDs = firstInstanceByDefinitionId2.lastGuestsOfHonorPrestigeIDs;
			foreach (int item in lastGuestsOfHonorPrestigeIDs)
			{
				if (item == 0)
				{
					continue;
				}
				global::Kampai.Game.PrestigeDefinition prestigeDefinition = defService.Get<global::Kampai.Game.PrestigeDefinition>(item);
				global::Kampai.Game.Prestige firstInstanceByDefinitionId3 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(item);
				if (prestigeDefinition != null)
				{
					global::Kampai.Game.Character byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.Character>(firstInstanceByDefinitionId3.trackedInstanceId);
					global::Kampai.Game.View.CharacterObject characterObject;
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
						characterObject.transform.localScale = new global::UnityEngine.Vector3(1f, 1f, 1f);
					}
				}
			}
			global::Kampai.Game.View.BuildingManagerView component3 = buildingManager.GetComponent<global::Kampai.Game.View.BuildingManagerView>();
			global::Kampai.Game.View.BuildingObject buildingObject = component3.GetBuildingObject(firstInstanceByDefinitionId.ID);
			global::Kampai.Game.View.StageBuildingObject stageBuildingObject = buildingObject as global::Kampai.Game.View.StageBuildingObject;
			if (stageBuildingObject != null)
			{
				stageBuildingObject.SetHideMic(false);
			}
			global::Kampai.Game.StuartCharacter firstInstanceByDefinitionId4 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.StuartCharacter>(70001);
			if (firstInstanceByDefinitionId4 != null)
			{
				global::Kampai.Game.Prestige firstInstanceByDefinitionId5 = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Prestige>(40003);
				if (firstInstanceByDefinitionId5.CurrentPrestigeLevel >= 1)
				{
					socialEventAvailableSignal.Dispatch();
				}
			}
		}
	}
}
