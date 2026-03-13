namespace Kampai.Game
{
	public class PlayLocalAudioOneShotCommand
	{
		[Inject]
		public global::Kampai.Common.Service.Audio.IFMODService fmodService { get; set; }

		public void Execute(CustomFMOD_StudioEventEmitter emitter, string audioClip)
		{
			if (emitter != null)
			{
				emitter.Stop();
				if (emitter.evt != null)
				{
					emitter.evt.release();
					emitter.evt = null;
				}
			}
			emitter.path = fmodService.GetGuid(audioClip);
			emitter.StartEvent();
		}
	}
}
