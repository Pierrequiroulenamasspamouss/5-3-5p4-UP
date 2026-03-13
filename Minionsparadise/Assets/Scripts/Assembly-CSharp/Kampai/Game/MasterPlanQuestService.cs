namespace Kampai.Game
{
	public class MasterPlanQuestService : global::Kampai.Game.IMasterPlanQuestService
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MasterPlanQuestService") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IQuestService questService { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanService masterPlanService { get; set; }

		public global::Kampai.Game.QuestLine ConvertComponentToQuestLine(global::Kampai.Game.MasterPlanComponent component)
		{
			if (component == null)
			{
				return null;
			}
			global::Kampai.Game.MasterPlan byInstanceId = playerService.GetByInstanceId<global::Kampai.Game.MasterPlan>(component.planTrackingInstance);
			global::System.Collections.Generic.List<global::Kampai.Game.QuestDefinition> list = new global::System.Collections.Generic.List<global::Kampai.Game.QuestDefinition>();
			list.Add(GetBuildComponentQuest(component, component.ID));
			list.Add(ConvertMasterPlanComponentDefToQuestDef(component, component.ID));
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestDefinition> quests = list;
			global::Kampai.Game.MasterPlanQuestType.PlanDefinition planDefinition = new global::Kampai.Game.MasterPlanQuestType.PlanDefinition();
			planDefinition.GivenByCharacterID = byInstanceId.Definition.VillainCharacterDefID;
			planDefinition.Quests = quests;
			planDefinition.component = component;
			planDefinition.plan = byInstanceId;
			return planDefinition;
		}

		public global::Kampai.Game.QuestLine ConvertMasterPlanToQuestLine(global::Kampai.Game.MasterPlan masterPlan)
		{
			if (masterPlan == null)
			{
				return null;
			}
			global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponent> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MasterPlanComponent>();
			global::Kampai.Game.MasterPlanQuestType.PlanDefinition planDefinition = new global::Kampai.Game.MasterPlanQuestType.PlanDefinition();
			planDefinition.GivenByCharacterID = masterPlan.Definition.VillainCharacterDefID;
			planDefinition.components = new global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponent>();
			planDefinition.plan = masterPlan;
			planDefinition.Quests = new global::System.Collections.Generic.List<global::Kampai.Game.QuestDefinition> { CreateMasterPlanQuestDefinition(masterPlan) };
			global::Kampai.Game.MasterPlanQuestType.PlanDefinition planDefinition2 = planDefinition;
			for (int i = 0; i < instancesByType.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent masterPlanComponent = instancesByType[i];
				if (masterPlanComponent.planTrackingInstance == masterPlan.ID)
				{
					planDefinition2.components.Add(masterPlanComponent);
				}
			}
			return planDefinition2;
		}

		public global::Kampai.Game.Quest GetQuestByInstanceId(int id)
		{
			return questService.GetQuestByInstanceId(id);
		}

		private global::Kampai.Game.MasterPlanComponent GetActiveComponent()
		{
			global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MasterPlanComponent>();
			for (int i = 0; i < instancesByType.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent masterPlanComponent = instancesByType[i];
				if (masterPlanComponent.State > global::Kampai.Game.MasterPlanComponentState.NotStarted && masterPlanComponent.State < global::Kampai.Game.MasterPlanComponentState.Complete)
				{
					return masterPlanComponent;
				}
			}
			return null;
		}

		public global::System.Collections.Generic.List<global::Kampai.Game.Quest> GetQuests()
		{
			global::System.Collections.Generic.List<global::Kampai.Game.Quest> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.Quest>();
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			if (currentMasterPlan != null)
			{
				bool flag = masterPlanService.AllComponentsAreComplete(currentMasterPlan.Definition.ID);
				global::Kampai.Game.MasterPlanComponentBuilding firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponentBuilding>(currentMasterPlan.Definition.BuildingDefID);
				if (flag && (firstInstanceByDefinitionId == null || firstInstanceByDefinitionId.State == global::Kampai.Game.BuildingState.Complete))
				{
					global::Kampai.Game.Quest questByInstanceId = GetQuestByInstanceId(currentMasterPlan.ID);
					instancesByType.Add(questByInstanceId);
					return instancesByType;
				}
			}
			global::Kampai.Game.MasterPlanComponent activeComponent = GetActiveComponent();
			if (activeComponent == null)
			{
				return instancesByType;
			}
			int id = ((activeComponent.State < global::Kampai.Game.MasterPlanComponentState.TasksCollected) ? activeComponent.ID : 711);
			global::Kampai.Game.Quest questByInstanceId2 = questService.GetQuestByInstanceId(id);
			instancesByType.Add(questByInstanceId2);
			return instancesByType;
		}

		public global::Kampai.Game.Quest ConvertMasterPlanToQuest(global::Kampai.Game.MasterPlan masterPlan)
		{
			if (masterPlan == null)
			{
				return null;
			}
			global::Kampai.Game.Building firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.Building>(masterPlan.Definition.BuildingDefID);
			global::Kampai.Game.MasterPlanQuestType.Component component = new global::Kampai.Game.MasterPlanQuestType.Component(CreateMasterPlanQuestDefinition(masterPlan));
			component.ID = masterPlan.ID;
			component.Steps = new global::System.Collections.Generic.List<global::Kampai.Game.QuestStep> { GernerateBuildMasterPlanQuestStep(masterPlan) };
			component.state = global::Kampai.Game.QuestState.RunningTasks;
			component.masterPlan = masterPlan;
			component.buildDefId = masterPlan.Definition.BuildingDefID;
			component.component = null;
			component.components = new global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponent>();
			component.isBuildingCompete = firstInstanceByDefinitionId != null && firstInstanceByDefinitionId.State == global::Kampai.Game.BuildingState.Idle;
			global::Kampai.Game.MasterPlanQuestType.Component component2 = component;
			global::System.Collections.Generic.List<global::Kampai.Game.MasterPlanComponent> instancesByType = playerService.GetInstancesByType<global::Kampai.Game.MasterPlanComponent>();
			for (int i = 0; i < instancesByType.Count; i++)
			{
				global::Kampai.Game.MasterPlanComponent masterPlanComponent = instancesByType[i];
				if (masterPlanComponent.planTrackingInstance == masterPlan.ID)
				{
					component2.components.Add(masterPlanComponent);
				}
			}
			return component2;
		}

		private global::Kampai.Game.MasterPlanQuestType.ComponentDefinition CreateMasterPlanQuestDefinition(global::Kampai.Game.MasterPlan masterPlan)
		{
			global::Kampai.Game.MasterPlanQuestType.ComponentDefinition componentDefinition = new global::Kampai.Game.MasterPlanQuestType.ComponentDefinition();
			componentDefinition.ID = masterPlan.Definition.ID;
			componentDefinition.LocalizedKey = masterPlan.Definition.LocalizedKey;
			componentDefinition.QuestSteps = new global::System.Collections.Generic.List<global::Kampai.Game.QuestStepDefinition> { GernerateBuildMasterPlanQuestStepDef(masterPlan.Definition) };
			componentDefinition.type = global::Kampai.Game.QuestType.MasterPlan;
			componentDefinition.SurfaceID = 40001;
			componentDefinition.QuestLineID = masterPlan.ID;
			componentDefinition.NarrativeOrder = 0;
			componentDefinition.SurfaceType = global::Kampai.Game.QuestSurfaceType.Character;
			componentDefinition.RewardTransaction = masterPlan.Definition.RewardTransactionID;
			return componentDefinition;
		}

		public global::Kampai.Game.Quest ConvertMasterPlanComponentToQuest(global::Kampai.Game.MasterPlanComponent component, bool buildTask)
		{
			if (component == null)
			{
				return null;
			}
			global::Kampai.Game.QuestDefinition questDefinition = ConvertMasterPlanComponentDefToQuestDef(component, component.ID);
			global::Kampai.Game.MasterPlan currentMasterPlan = masterPlanService.CurrentMasterPlan;
			global::Kampai.Game.QuestState state = global::Kampai.Game.QuestState.Notstarted;
			global::Kampai.Game.MasterPlanQuestType.Component component2;
			if (buildTask)
			{
				component2 = new global::Kampai.Game.MasterPlanQuestType.Component(GetBuildComponentQuest(component, component.ID));
				component2.ID = 711;
				component2.Steps = new global::System.Collections.Generic.List<global::Kampai.Game.QuestStep> { GernerateBuildQuestStep(component) };
				component2.component = component;
				component2.state = state;
				component2.masterPlan = currentMasterPlan;
				component2.index = currentMasterPlan.Definition.ComponentDefinitionIDs.IndexOf(component.Definition.ID);
				component2.buildDefId = component.buildingDefID;
				return component2;
			}
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestStep> list = new global::System.Collections.Generic.List<global::Kampai.Game.QuestStep>(component.tasks.Count);
			for (int i = 0; i < component.tasks.Count; i++)
			{
				list.Add(ConvertMasterPlanComponentTaskToQuestStep(component.tasks[i]));
			}
			component2 = new global::Kampai.Game.MasterPlanQuestType.Component(questDefinition as global::Kampai.Game.MasterPlanQuestType.ComponentDefinition);
			component2.ID = component.ID;
			component2.Steps = list;
			component2.state = state;
			component2.component = component;
			component2.masterPlan = currentMasterPlan;
			component2.index = currentMasterPlan.Definition.ComponentDefinitionIDs.IndexOf(component.Definition.ID);
			component2.buildDefId = component.buildingDefID;
			return component2;
		}

		public global::Kampai.Game.Quest ConvertMasterPlanComponentToQuest(global::Kampai.Game.MasterPlanComponent component)
		{
			return ConvertMasterPlanComponentToQuest(component, false);
		}

		public global::Kampai.Game.QuestDefinition ConvertMasterPlanComponentDefToQuestDef(global::Kampai.Game.MasterPlanComponent component, int questLineId)
		{
			if (component == null)
			{
				return null;
			}
			int iD = component.ID;
			global::System.Collections.Generic.IList<global::Kampai.Game.QuestStepDefinition> list = new global::System.Collections.Generic.List<global::Kampai.Game.QuestStepDefinition>();
			for (int i = 0; i < component.tasks.Count; i++)
			{
				list.Add(ConvertMasterPlanComponentTaskDefToQuestStepDef(component.tasks[i].Definition));
			}
			global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(component.buildingDefID);
			global::Kampai.Game.MasterPlanQuestType.ComponentDefinition componentDefinition = new global::Kampai.Game.MasterPlanQuestType.ComponentDefinition();
			componentDefinition.ID = iD;
			componentDefinition.LocalizedKey = masterPlanComponentBuildingDefinition.LocalizedKey;
			componentDefinition.type = global::Kampai.Game.QuestType.MasterPlan;
			componentDefinition.QuestLineID = questLineId;
			componentDefinition.isBuildQuest = false;
			componentDefinition.reward = component.reward.Definition;
			componentDefinition.QuestSteps = list;
			componentDefinition.SurfaceType = global::Kampai.Game.QuestSurfaceType.Character;
			componentDefinition.NarrativeOrder = 0;
			componentDefinition.SurfaceID = 40001;
			return componentDefinition;
		}

		private global::Kampai.Game.MasterPlanQuestType.ComponentDefinition GetBuildComponentQuest(global::Kampai.Game.MasterPlanComponent component, int questLineId)
		{
			global::Kampai.Game.MasterPlanComponentBuildingDefinition masterPlanComponentBuildingDefinition = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(component.buildingDefID);
			global::Kampai.Game.MasterPlanQuestType.ComponentDefinition componentDefinition = new global::Kampai.Game.MasterPlanQuestType.ComponentDefinition();
			componentDefinition.ID = 711;
			componentDefinition.LocalizedKey = masterPlanComponentBuildingDefinition.LocalizedKey;
			componentDefinition.type = global::Kampai.Game.QuestType.MasterPlan;
			componentDefinition.QuestLineID = questLineId;
			componentDefinition.NarrativeOrder = 1;
			componentDefinition.isBuildQuest = true;
			componentDefinition.reward = component.reward.Definition;
			componentDefinition.SurfaceType = global::Kampai.Game.QuestSurfaceType.Character;
			componentDefinition.QuestSteps = new global::System.Collections.Generic.List<global::Kampai.Game.QuestStepDefinition> { GernerateBuildQuestStepDef(component.Definition) };
			componentDefinition.SurfaceID = 40001;
			return componentDefinition;
		}

		public global::Kampai.Game.QuestStep GernerateBuildMasterPlanQuestStep(global::Kampai.Game.MasterPlan masterPlan)
		{
			if (masterPlan == null)
			{
				return null;
			}
			global::Kampai.Game.MasterPlanQuestType.ComponentTask componentTask = new global::Kampai.Game.MasterPlanQuestType.ComponentTask();
			componentTask.state = ((!masterPlan.displayCooldownAlert) ? global::Kampai.Game.QuestStepState.Inprogress : global::Kampai.Game.QuestStepState.Complete);
			componentTask.TrackedID = masterPlan.Definition.BuildingDefID;
			componentTask.AmountCompleted = 0;
			componentTask.AmountReady = 0;
			return componentTask;
		}

		public global::Kampai.Game.QuestStepDefinition GernerateBuildMasterPlanQuestStepDef(global::Kampai.Game.MasterPlanDefinition task)
		{
			if (task == null)
			{
				return null;
			}
			global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition componentTaskDefinition = new global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition();
			componentTaskDefinition.Type = global::Kampai.Game.QuestStepType.MasterPlanBuild;
			componentTaskDefinition.ItemDefinitionID = task.BuildingDefID;
			componentTaskDefinition.ItemAmount = 1;
			return componentTaskDefinition;
		}

		public global::Kampai.Game.QuestStep GernerateBuildQuestStep(global::Kampai.Game.MasterPlanComponent component)
		{
			if (component == null)
			{
				return null;
			}
			global::Kampai.Game.QuestStepState state = global::Kampai.Game.QuestStepState.Notstarted;
			switch (component.State)
			{
			case global::Kampai.Game.MasterPlanComponentState.InProgress:
				state = global::Kampai.Game.QuestStepState.Notstarted;
				break;
			case global::Kampai.Game.MasterPlanComponentState.TasksComplete:
				state = global::Kampai.Game.QuestStepState.Notstarted;
				break;
			case global::Kampai.Game.MasterPlanComponentState.TasksCollected:
				state = global::Kampai.Game.QuestStepState.Inprogress;
				break;
			case global::Kampai.Game.MasterPlanComponentState.Scaffolding:
				state = global::Kampai.Game.QuestStepState.Inprogress;
				break;
			case global::Kampai.Game.MasterPlanComponentState.Built:
				state = global::Kampai.Game.QuestStepState.Ready;
				break;
			case global::Kampai.Game.MasterPlanComponentState.Complete:
				state = global::Kampai.Game.QuestStepState.Complete;
				break;
			default:
				logger.Warning("No master plan component state defined for: {0}", component.State);
				break;
			}
			global::Kampai.Game.MasterPlanQuestType.ComponentTask componentTask = new global::Kampai.Game.MasterPlanQuestType.ComponentTask();
			componentTask.state = state;
			componentTask.TrackedID = component.buildingDefID;
			componentTask.AmountCompleted = 0;
			componentTask.AmountReady = 0;
			return componentTask;
		}

		public global::Kampai.Game.QuestStepDefinition GernerateBuildQuestStepDef(global::Kampai.Game.MasterPlanComponentDefinition task)
		{
			if (task == null)
			{
				return null;
			}
			global::Kampai.Game.MasterPlanComponent firstInstanceByDefinitionId = playerService.GetFirstInstanceByDefinitionId<global::Kampai.Game.MasterPlanComponent>(task.ID);
			global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition componentTaskDefinition = new global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition();
			componentTaskDefinition.Type = global::Kampai.Game.QuestStepType.MasterPlanComponentBuild;
			componentTaskDefinition.ItemDefinitionID = firstInstanceByDefinitionId.buildingDefID;
			componentTaskDefinition.ItemAmount = 1;
			return componentTaskDefinition;
		}

		public global::Kampai.Game.QuestStep ConvertMasterPlanComponentTaskToQuestStep(global::Kampai.Game.MasterPlanComponentTask task)
		{
			if (task == null)
			{
				return null;
			}
			global::Kampai.Game.MasterPlanQuestType.ComponentTask componentTask = new global::Kampai.Game.MasterPlanQuestType.ComponentTask();
			componentTask.AmountCompleted = (int)task.earnedQuantity;
			componentTask.state = ((!task.isComplete) ? global::Kampai.Game.QuestStepState.Inprogress : global::Kampai.Game.QuestStepState.Complete);
			componentTask.TrackedID = 0;
			componentTask.AmountReady = (int)task.earnedQuantity;
			componentTask.task = task;
			return componentTask;
		}

		public global::Kampai.Game.QuestStepDefinition ConvertMasterPlanComponentTaskDefToQuestStepDef(global::Kampai.Game.MasterPlanComponentTaskDefinition task)
		{
			if (task == null)
			{
				return null;
			}
			global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition componentTaskDefinition = new global::Kampai.Game.MasterPlanQuestType.ComponentTaskDefinition();
			componentTaskDefinition.Type = global::Kampai.Game.QuestStepType.MasterPlanTask;
			componentTaskDefinition.ItemDefinitionID = task.requiredItemId;
			componentTaskDefinition.ItemAmount = (int)task.requiredQuantity;
			componentTaskDefinition.ShowWayfinder = task.ShowWayfinder;
			componentTaskDefinition.taskDefinition = task;
			return componentTaskDefinition;
		}
	}
}
