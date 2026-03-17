namespace Kampai.Game
{
	public interface IDefinitionService
	{
		bool Has(int id);

		bool Has<T>(int id) where T : global::Kampai.Game.Definition;

		T Get<T>(int id) where T : global::Kampai.Game.Definition;

		T Get<T>(global::Kampai.Game.StaticItem staticItem) where T : global::Kampai.Game.Definition;

		T Get<T>() where T : global::Kampai.Game.Definition;

		global::Kampai.Game.Definition Get(int id);

		bool TryGet<T>(int id, out T definition) where T : global::Kampai.Game.Definition;

		global::System.Collections.Generic.IList<string> GetEnvironemtDefinition();

		void ReclaimEnfironmentDefinitions();

		global::System.Collections.Generic.List<T> GetAll<T>() where T : global::Kampai.Game.Definition;

		global::Kampai.Game.Transaction.WeightedDefinition GetGachaWeightsForNumMinions(int numMinions, bool party);

		global::System.Collections.Generic.List<global::Kampai.Game.Transaction.WeightedDefinition> GetAllGachaDefinitions();

		global::System.Collections.Generic.Dictionary<int, global::Kampai.Game.Definition> GetAllDefinitions();

		void DeserializeJson(global::System.IO.TextReader textReader, bool validateDefinitions = true);

		void DeserializeBinary(global::System.IO.BinaryReader binaryReader, bool validateDefinitions = false);

		void DeserializeEnvironmentDefinition(global::System.IO.TextReader textReader);

		int GetHarvestItemDefinitionIdFromTransactionId(int transactionId);

		string GetHarvestIconFromTransactionID(int transactionId);

		bool HasUnlockItemInTransactionOutput(int transactionID);

		int GetBuildingDefintionIDFromItemDefintionID(int itemDefinitionID);

		global::Kampai.Game.BridgeDefinition GetBridgeDefinition(int itemDefinitionID);

		int ExtractQuantityFromTransaction(int transactionID, int definitionID);

		int GetLevelItemUnlocksAt(int definitionID);

		global::Kampai.Game.TaskLevelBandDefinition GetTaskLevelBandForLevel(int level);

		int getItemTransactionID(int id);

		global::Kampai.Game.RushTimeBandDefinition GetRushTimeBandForTime(int timeRemainingInSeconds);

		string GetInitialPlayer();

		string GetBuildingFootprint(int ID);

		int GetIncrementalCost(global::Kampai.Game.Definition definition);

		void Add(global::Kampai.Game.Definition definition);

		void Remove(global::Kampai.Game.Definition definition);

		global::Kampai.Game.AchievementDefinition GetAchievementDefinitionFromDefinitionID(int defID);

		global::System.Collections.Generic.IList<global::Kampai.Game.Trigger.TriggerDefinition> GetTriggerDefinitions();

		global::System.Collections.Generic.IList<global::Kampai.Game.CurrencyStoreCategoryDefinition> GetCurrencyStoreCategoryDefinitions();

		void SetPerformanceQualityLevel(global::Kampai.Util.TargetPerformance targetPerformance);

		global::Kampai.Game.Transaction.TransactionDefinition GetPackTransaction(int transactionId);

		global::Kampai.Game.PackDefinition GetPackDefinitionFromTransactionId(int transactionId);

		global::Kampai.Game.SalePackType getSKUSalePackType(string ExternalIdentifier);

		int GetPartyFavorDefinitionIDByItemID(int itemID);

		string GetLegalURL(global::Kampai.Util.LegalDocuments.LegalType type, string language);
	}
}
