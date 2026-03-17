namespace Kampai.Game
{
	public class PlayerTrainingTransactionOutputExaminationCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.Transaction.TransactionUpdateData updateData { get; set; }

		[Inject]
		public global::Kampai.Game.DisplayPlayerTrainingSignal displaySignal { get; set; }

		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		public override void Execute()
		{
			if (updateData.NewItems != null && updateData.NewItems.Count > 0)
			{
				foreach (global::Kampai.Game.Instance newItem in updateData.NewItems)
				{
					if (newItem.Definition == null)
					{
						continue;
					}
					global::Kampai.Game.DropItemDefinition dropItemDefinition = newItem.Definition as global::Kampai.Game.DropItemDefinition;
					if (dropItemDefinition != null)
					{
						int type = 0;
						switch (dropItemDefinition.dropType)
						{
						case global::Kampai.Game.DropType.DEBRIS:
							type = 19000000;
							break;
						case global::Kampai.Game.DropType.LAND_EXPAND:
							type = 19000001;
							break;
						case global::Kampai.Game.DropType.STORAGE:
							type = 19000002;
							break;
						}
						displaySignal.Dispatch(type, false, new global::strange.extensions.signal.impl.Signal<bool>());
						return;
					}
				}
			}
			if (updateData.Outputs == null)
			{
				return;
			}
			foreach (global::Kampai.Util.QuantityItem output in updateData.Outputs)
			{
				global::Kampai.Game.ItemDefinition definition;
				if (definitionService.TryGet<global::Kampai.Game.ItemDefinition>(output.ID, out definition) && definition != null && definition.PlayerTrainingDefinitionID > 0 && !updateData.IsNotForPlayerTraining)
				{
					displaySignal.Dispatch(definition.PlayerTrainingDefinitionID, false, new global::strange.extensions.signal.impl.Signal<bool>());
					break;
				}
			}
		}
	}
}
