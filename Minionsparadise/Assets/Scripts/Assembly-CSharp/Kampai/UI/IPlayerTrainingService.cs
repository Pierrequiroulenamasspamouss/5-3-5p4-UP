namespace Kampai.UI
{
	public interface IPlayerTrainingService
	{
		bool HasSeen(int playerTrainingDefinitionId, global::Kampai.UI.PlayerTrainingVisiblityType visibilityType);

		void MarkSeen(int playerTrainingDefinitionId, global::Kampai.UI.PlayerTrainingVisiblityType visibilityType);
	}
}
