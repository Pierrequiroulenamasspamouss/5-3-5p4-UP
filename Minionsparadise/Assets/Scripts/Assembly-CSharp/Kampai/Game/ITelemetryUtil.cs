namespace Kampai.Game
{
	public interface ITelemetryUtil
	{
		void DetermineTaxonomy(global::Kampai.Game.Transaction.TransactionUpdateData update, bool concatenateType, out string highLevel, out string specific, out string type, out string other);

		string GetSourceName(global::Kampai.Game.Transaction.TransactionUpdateData update);
	}
}
