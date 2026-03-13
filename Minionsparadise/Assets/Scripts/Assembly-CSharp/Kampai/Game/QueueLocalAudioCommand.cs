namespace Kampai.Game
{
	public class QueueLocalAudioCommand
	{
		public void Execute(CustomFMOD_StudioEventEmitter emitter, string audioSource)
		{
			emitter.QueueClip(audioSource);
		}
	}
}
