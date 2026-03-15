namespace Kampai.Game.View
{
	public interface IStartAudio
	{
		void InitAudio(global::Kampai.Game.BuildingState creationState, global::Kampai.Main.PlayLocalAudioSignal playLocalAudioSignal);

		void NotifyBuildingState(global::Kampai.Game.BuildingState newState);
	}
}
