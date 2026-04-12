namespace Kampai.Common
{
	public class OpenRateAppPageCommand : global::strange.extensions.command.impl.Command
	{
		[Inject("game.server.host")]
		public string ServerUrl { get; set; }

		public override void Execute()
		{
			global::UnityEngine.Application.OpenURL(ServerUrl + "/rateus");
		}
	}
}
