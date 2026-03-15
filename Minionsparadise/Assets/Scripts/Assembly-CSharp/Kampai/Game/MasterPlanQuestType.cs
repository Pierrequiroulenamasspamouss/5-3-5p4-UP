namespace Kampai.Game
{
	public static class MasterPlanQuestType
	{
		public sealed class PlanDefinition : global::Kampai.Game.QuestLine
		{
			public global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> components;

			public global::Kampai.Game.MasterPlan plan;

			public global::Kampai.Game.MasterPlanComponent component;

			public override global::Kampai.Game.QuestLineState state
			{
				get
				{
					if (component == null)
					{
						for (int i = 0; i < components.Count; i++)
						{
							global::Kampai.Game.MasterPlanComponent masterPlanComponent = components[i];
							if (masterPlanComponent.State != global::Kampai.Game.MasterPlanComponentState.Complete)
							{
								return global::Kampai.Game.QuestLineState.NotStarted;
							}
						}
						return (!plan.displayCooldownAlert) ? global::Kampai.Game.QuestLineState.Started : global::Kampai.Game.QuestLineState.Finished;
					}
					if (component.State == global::Kampai.Game.MasterPlanComponentState.Complete)
					{
						return global::Kampai.Game.QuestLineState.Finished;
					}
					return (component.State >= global::Kampai.Game.MasterPlanComponentState.InProgress) ? global::Kampai.Game.QuestLineState.Started : global::Kampai.Game.QuestLineState.NotStarted;
				}
			}

			public override int QuestLineID
			{
				get
				{
					return (components != null) ? plan.Definition.ID : component.ID;
				}
			}

			public override int GivenByCharacterID
			{
				get
				{
					return plan.Definition.VillainCharacterDefID;
				}
				set
				{
				}
			}
		}

		public sealed class ComponentTaskDefinition : global::Kampai.Game.QuestStepDefinition
		{
			public global::Kampai.Game.MasterPlanComponentTaskDefinition taskDefinition;
		}

		public sealed class ComponentTask : global::Kampai.Game.QuestStep
		{
			public global::Kampai.Game.MasterPlanComponentTask task;
		}

		public sealed class Component : global::Kampai.Game.Quest
		{
			private global::System.Collections.Generic.IList<global::Kampai.Game.QuestStep> questSteps;

			public global::Kampai.Game.MasterPlanComponent component;

			public global::Kampai.Game.MasterPlan masterPlan;

			public global::System.Collections.Generic.IList<global::Kampai.Game.MasterPlanComponent> components;

			public int buildDefId;

			public int index;

			public bool isBuildQuest;

			public bool isBuildingCompete;

			public override global::System.Collections.Generic.IList<global::Kampai.Game.QuestStep> Steps
			{
				get
				{
					global::Kampai.Game.QuestStepState? questStepState = null;
					if (components != null)
					{
						questStepState = ((!isBuildingCompete) ? global::Kampai.Game.QuestStepState.Ready : global::Kampai.Game.QuestStepState.Complete);
					}
					if (isBuildQuest)
					{
						questStepState = ((component.State <= global::Kampai.Game.MasterPlanComponentState.Scaffolding) ? global::Kampai.Game.QuestStepState.Ready : global::Kampai.Game.QuestStepState.Complete);
					}
					if (questStepState.HasValue)
					{
						if (questSteps.Count != 1)
						{
							questSteps.Clear();
							questSteps.Add(new global::Kampai.Game.MasterPlanQuestType.ComponentTask());
						}
						questSteps[0].state = questStepState.Value;
						return questSteps;
					}
					int count = component.tasks.Count;
					if (questSteps.Count != count)
					{
						questSteps.Clear();
						for (int i = 0; i < count; i++)
						{
							questSteps.Add(new global::Kampai.Game.MasterPlanQuestType.ComponentTask
							{
								task = component.tasks[i]
							});
						}
					}
					foreach (global::Kampai.Game.QuestStep questStep in questSteps)
					{
						global::Kampai.Game.MasterPlanComponentTask task = (questStep as global::Kampai.Game.MasterPlanQuestType.ComponentTask).task;
						questStep.state = ((!task.isComplete) ? global::Kampai.Game.QuestStepState.Inprogress : global::Kampai.Game.QuestStepState.Complete);
						questStep.AmountCompleted = (int)task.earnedQuantity;
						questStep.AmountReady = (int)task.earnedQuantity;
					}
					return questSteps;
				}
				set
				{
					base.Steps = value;
				}
			}

			public override global::Kampai.Game.QuestState state
			{
				get
				{
					global::Kampai.Game.QuestState result = global::Kampai.Game.QuestState.Notstarted;
					if (components != null)
					{
						for (int i = 0; i < components.Count; i++)
						{
							global::Kampai.Game.MasterPlanComponent masterPlanComponent = components[i];
							if (masterPlanComponent.State != global::Kampai.Game.MasterPlanComponentState.Complete)
							{
								return result;
							}
						}
						return masterPlan.displayCooldownAlert ? global::Kampai.Game.QuestState.Complete : ((!isBuildingCompete) ? global::Kampai.Game.QuestState.RunningTasks : global::Kampai.Game.QuestState.Harvestable);
					}
					if (isBuildQuest)
					{
						if (component.State == global::Kampai.Game.MasterPlanComponentState.Built)
						{
							result = global::Kampai.Game.QuestState.Harvestable;
						}
						else if (component.State == global::Kampai.Game.MasterPlanComponentState.Complete)
						{
							result = global::Kampai.Game.QuestState.Complete;
						}
						else if (component.State == global::Kampai.Game.MasterPlanComponentState.TasksCollected || component.State == global::Kampai.Game.MasterPlanComponentState.Scaffolding)
						{
							result = global::Kampai.Game.QuestState.RunningTasks;
						}
						return result;
					}
					if (component.State == global::Kampai.Game.MasterPlanComponentState.InProgress)
					{
						result = global::Kampai.Game.QuestState.RunningTasks;
					}
					else if (component.State == global::Kampai.Game.MasterPlanComponentState.TasksCollected)
					{
						result = global::Kampai.Game.QuestState.Complete;
					}
					else if (component.State == global::Kampai.Game.MasterPlanComponentState.TasksComplete)
					{
						result = global::Kampai.Game.QuestState.Harvestable;
					}
					return result;
				}
				set
				{
					base.state = value;
				}
			}

			public override bool AutoGrantReward
			{
				get
				{
					return state == global::Kampai.Game.QuestState.Harvestable;
				}
				set
				{
					base.AutoGrantReward = value;
				}
			}

			public Component(global::Kampai.Game.MasterPlanQuestType.ComponentDefinition def)
				: base(def)
			{
				questSteps = new global::System.Collections.Generic.List<global::Kampai.Game.QuestStep>();
			}

			public override string ToString()
			{
				return string.Format("Base: {0}, component: {1}, state: {2}, step count: {3}", base.ToString(), component, state, (Steps != null) ? Steps.Count : 0);
			}
		}

		public sealed class ComponentDefinition : global::Kampai.Game.QuestDefinition
		{
			public bool isBuildQuest;

			public global::Kampai.Game.MasterPlanComponentRewardDefinition reward;

			public override int RewardDisplayCount
			{
				get
				{
					if (reward == null)
					{
						return base.RewardDisplayCount;
					}
					if (isBuildQuest)
					{
						return 1;
					}
					int num = 0;
					if (reward.grindReward != 0)
					{
						num++;
					}
					if (reward.premiumReward != 0)
					{
						num++;
					}
					return num;
				}
				set
				{
					base.RewardDisplayCount = value;
				}
			}

			public override global::Kampai.Game.Transaction.TransactionDefinition GetReward(global::Kampai.Game.IDefinitionService definitionService)
			{
				global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition;
				if (reward == null || (base.RewardTransaction != 0 && definitionService != null))
				{
					transactionDefinition = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(base.RewardTransaction);
					RewardDisplayCount = global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputCount(transactionDefinition);
					return transactionDefinition;
				}
				transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
				transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
				if (!isBuildQuest)
				{
					if (reward.grindReward != 0)
					{
						transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(0, reward.grindReward));
					}
					if (reward.premiumReward != 0)
					{
						transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(1, reward.premiumReward));
					}
					RewardDisplayCount = global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputCount(transactionDefinition);
					return transactionDefinition;
				}
				transactionDefinition.Outputs.Add(new global::Kampai.Util.QuantityItem(reward.rewardItemId, reward.rewardQuantity));
				RewardDisplayCount = global::Kampai.Game.Transaction.TransactionDataExtension.GetOutputCount(transactionDefinition);
				return transactionDefinition;
			}
		}
	}
}
