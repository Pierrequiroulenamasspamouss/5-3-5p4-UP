namespace Kampai.Game
{
	public class EndPartyBuffTimerCommand : global::strange.extensions.command.impl.Command
	{
		[Inject]
		public global::Kampai.Game.EndPartyBuffTimerWithCallbackSignal endPartyBuffTimerWithCallBackSignal { get; set; }

		[Inject]
		public global::Kampai.Game.UnloadPartyAssetsSignal unloadPartyAssetsSignal { get; set; }

		public override void Execute()
		{
			endPartyBuffTimerWithCallBackSignal.Dispatch(new global::Kampai.Util.Boxed<global::System.Action>(null));
			unloadPartyAssetsSignal.Dispatch();
		}
	}
}
