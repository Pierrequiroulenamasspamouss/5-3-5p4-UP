namespace Kampai.Game.Controller
{
	public class AppFocusGainedCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.MuteVolumeSignal muteVolumeSignal { get; set; }

		[Inject]
		public global::Kampai.Common.AppFocusGainedCompletedSignal appFocusGainedCompleteSignal { get; set; }

		public override void Execute()
		{
			global::Kampai.Util.ScreenUtils.ToggleAutoRotation(true);
			appFocusGainedCompleteSignal.Dispatch();
		}
	}
}
