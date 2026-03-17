namespace Kampai.Game
{
	public class MasterPlanTaskCompleteCommand : global::strange.extensions.command.impl.Command
	{
		public global::Kampai.Util.IKampaiLogger logger = global::Elevation.Logging.LogManager.GetClassLogger("MasterPlanTaskCompleteCommand") as global::Kampai.Util.IKampaiLogger;

		[Inject]
		public global::Kampai.Game.MasterPlanComponent component { get; set; }

		[Inject]
		public int taskIndex { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Common.ITelemetryService telemetryService { get; set; }

		[Inject]
		public global::Kampai.Main.PlayGlobalSoundFXSignal fxSignal { get; set; }

		[Inject]
		public global::Kampai.UI.View.SetStorageCapacitySignal updateStorageItemsSignal { get; set; }

		[Inject]
		public global::Kampai.Game.IMasterPlanQuestService masterPlanQuestService { get; set; }

		[Inject]
		public global::Kampai.Game.UpdateQuestWorldIconsSignal updateQuestWorldIconsSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Game.MasterPlanComponentTask masterPlanComponentTask = component.tasks[taskIndex];
			if (masterPlanComponentTask.isHarvestable)
			{
				if (masterPlanComponentTask.Definition.Type == global::Kampai.Game.MasterPlanComponentTaskType.Deliver)
				{
					global::Kampai.Game.Transaction.TransactionDefinition transactionDefinition = new global::Kampai.Game.Transaction.TransactionDefinition();
					transactionDefinition.Inputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
					transactionDefinition.Outputs = new global::System.Collections.Generic.List<global::Kampai.Util.QuantityItem>();
					transactionDefinition.Inputs.Add(new global::Kampai.Util.QuantityItem(masterPlanComponentTask.Definition.requiredItemId, masterPlanComponentTask.Definition.requiredQuantity));
					global::Kampai.Game.TransactionArg transactionArg = new global::Kampai.Game.TransactionArg();
					transactionArg.IsFromQuestSource = 3;
					playerService.RunEntireTransaction(transactionDefinition, global::Kampai.Game.TransactionTarget.AUTOMATIC, TaskCompleteTransactionCallback, transactionArg);
				}
				else
				{
					TaskComplete();
				}
			}
		}

		private void TaskCompleteTransactionCallback(global::Kampai.Game.PendingCurrencyTransaction pct)
		{
			if (pct.Success)
			{
				TaskComplete();
			}
		}

		private void TaskComplete()
		{
			global::Kampai.Game.MasterPlanComponentTask masterPlanComponentTask = component.tasks[taskIndex];
			masterPlanComponentTask.isComplete = true;
			fxSignal.Dispatch("Play_completePartQuest_01");
			updateStorageItemsSignal.Dispatch();
			bool flag = true;
			foreach (global::Kampai.Game.MasterPlanComponentTask task in component.tasks)
			{
				if (!task.isComplete)
				{
					flag = false;
				}
			}
			if (flag && component.State == global::Kampai.Game.MasterPlanComponentState.InProgress)
			{
				component.State = global::Kampai.Game.MasterPlanComponentState.TasksComplete;
			}
			global::Kampai.Game.Quest questByInstanceId = masterPlanQuestService.GetQuestByInstanceId(component.ID);
			if (questByInstanceId != null)
			{
				updateQuestWorldIconsSignal.Dispatch(questByInstanceId);
				SendTelemetry(masterPlanComponentTask);
			}
		}

		private void SendTelemetry(global::Kampai.Game.MasterPlanComponentTask task)
		{
			string requiredItem = string.Empty;
			switch (task.Definition.Type)
			{
			case global::Kampai.Game.MasterPlanComponentTaskType.Deliver:
			case global::Kampai.Game.MasterPlanComponentTaskType.Collect:
				requiredItem = definitionService.Get<global::Kampai.Game.ItemDefinition>(task.Definition.requiredItemId).LocalizedKey;
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.CompleteOrders:
				requiredItem = "Complete Orders";
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.PlayMiniGame:
				requiredItem = ((task.Definition.requiredItemId != 0) ? definitionService.Get<global::Kampai.Game.Definition>(task.Definition.requiredItemId).LocalizedKey : "Any Mini Game");
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.MiniGameScore:
				requiredItem = definitionService.Get<global::Kampai.Game.Definition>(task.Definition.requiredItemId).LocalizedKey;
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnPartyPoints:
				requiredItem = ((task.Definition.requiredItemId != 0) ? definitionService.Get<global::Kampai.Game.Definition>(task.Definition.requiredItemId).LocalizedKey : "Any Party Points");
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnLeisurePartyPoints:
				requiredItem = ((task.Definition.requiredItemId != 0) ? definitionService.Get<global::Kampai.Game.Definition>(task.Definition.requiredItemId).LocalizedKey : "Distractivity Party Points");
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnMignettePartyPoints:
				requiredItem = ((task.Definition.requiredItemId != 0) ? definitionService.Get<global::Kampai.Game.Definition>(task.Definition.requiredItemId).LocalizedKey : "Mini Game Party Points");
				break;
			case global::Kampai.Game.MasterPlanComponentTaskType.EarnSandDollars:
				requiredItem = ((task.Definition.requiredItemId != 0) ? definitionService.Get<global::Kampai.Game.Definition>(task.Definition.requiredItemId).LocalizedKey : "Any Sand Dollars");
				break;
			default:
				logger.Warning("No task type of {0} defined for telemetry", task.Definition.Type);
				break;
			}
			string taxonomySpecific = definitionService.Get<global::Kampai.Game.MasterPlanComponentBuildingDefinition>(component.buildingDefID).TaxonomySpecific;
			telemetryService.Send_Telemetry_EVT_MASTER_PLAN_TASK_COMPLETE(taxonomySpecific, task.Definition.Type.ToString(), requiredItem, (int)task.Definition.requiredQuantity);
		}
	}
}
