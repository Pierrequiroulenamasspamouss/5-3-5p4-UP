namespace Kampai.Game
{
	public class MIBService : global::Kampai.Game.IMIBService
	{
		[Inject]
		public global::Kampai.Game.IDefinitionService definitionService { get; set; }

		[Inject]
		public global::Kampai.Game.IPlayerService playerService { get; set; }

		[Inject]
		public global::Kampai.Common.IRandomService randomService { get; set; }

		[Inject]
		public ILocalPersistanceService localPersistanceService { get; set; }

		public global::Kampai.Game.Transaction.TransactionDefinition PickWeightedTransaction(int weightedDefId)
		{
			global::Kampai.Game.Transaction.WeightedDefinition definition = null;
			if (!definitionService.TryGet<global::Kampai.Game.Transaction.WeightedDefinition>(weightedDefId, out definition))
			{
				return null;
			}
			global::Kampai.Game.Transaction.WeightedInstance weightedInstance = playerService.GetWeightedInstance(weightedDefId, definition);
			int iD = weightedInstance.NextPick(randomService).ID;
			return definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(iD);
		}

		public global::Kampai.Game.ItemDefinition GetItemDefinition(global::Kampai.Game.Transaction.TransactionDefinition transactionDef)
		{
			if (transactionDef == null || transactionDef.Outputs.Count <= 0)
			{
				return null;
			}
			return definitionService.Get<global::Kampai.Game.ItemDefinition>(transactionDef.Outputs[0].ID);
		}

		public global::Kampai.Game.ItemDefinition[] GetItemDefinitions(int weightedDefId)
		{
			global::Kampai.Game.Transaction.WeightedDefinition weightedDefinition = definitionService.Get<global::Kampai.Game.Transaction.WeightedDefinition>(weightedDefId);
			global::Kampai.Game.ItemDefinition[] array = new global::Kampai.Game.ItemDefinition[weightedDefinition.Entities.Count];
			for (int i = 0; i < weightedDefinition.Entities.Count; i++)
			{
				global::Kampai.Game.Transaction.TransactionDefinition transactionDef = definitionService.Get<global::Kampai.Game.Transaction.TransactionDefinition>(weightedDefinition.Entities[i].ID);
				array[i] = GetItemDefinition(transactionDef);
			}
			return array;
		}

		public bool IsUserReturning()
		{
			return localPersistanceService.HasKeyPlayer("MIBPlacementSelected") && localPersistanceService.GetDataIntPlayer("MIBPlacementSelected") == 1;
		}

		public void SetReturningKey()
		{
			localPersistanceService.PutDataIntPlayer("MIBPlacementSelected", 1);
		}

		public void ClearReturningKey()
		{
			localPersistanceService.DeleteKeyPlayer("MIBPlacementSelected");
		}
	}
}
