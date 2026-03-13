namespace Kampai.Game
{
	public class LoadAudioCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Common.Service.Audio.IFMODService fmodService { get; set; }

		[Inject]
		public global::Kampai.Util.ICoroutineProgressMonitor couroutineProgressMonitor { get; set; }

		public override void Execute()
		{
			couroutineProgressMonitor.StartTask(fmodService.InitializeSystem(), "fmod");
		}
	}
}
