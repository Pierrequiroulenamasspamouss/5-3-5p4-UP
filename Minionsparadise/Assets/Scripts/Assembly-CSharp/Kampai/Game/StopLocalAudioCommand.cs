namespace Kampai.Game
{
	public class StopLocalAudioCommand
	{
		public void Execute(CustomFMOD_StudioEventEmitter emitter)
		{
			if (emitter != null)
			{
				emitter.Stop();
			}
		}
	}
}
